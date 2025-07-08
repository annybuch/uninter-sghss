using Loop.SGHSS.Data;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._Enums.Financas;
using Loop.SGHSS.Model._Enums.Instituicao;
using Loop.SGHSS.Model.Monitoramento.Financas;
using Loop.SGHSS.Model.Monitoramento.Recursos;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Monitoramento
{
    public class MonitoramentoService : IMonitoramentoService
    {
        private readonly LoopSGHSSDataContext _dbContext;

        public MonitoramentoService(LoopSGHSSDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtém dados financeiros agregados para monitoramento de hospitais.
        /// </summary>
        public async Task<MonitoramentoFinancasModel> ObterFinancasDashboard(DateTime dataInicio, DateTime dataFim, Guid? instituicaoId = null)
        {
                    var consultasPago = _dbContext.Consultas
            .Where(c => c.StatusPagamento == StatusPagamentoEnum.Aprovado &&
                        c.DataMarcada.Date >= dataInicio.Date && c.DataMarcada.Date <= dataFim.Date &&
                        (!instituicaoId.HasValue || c.InstituicaoId == instituicaoId.Value))
            .Select(c => new
            {
                Data = c.DataMarcada.Date,
                c.Valor,
                TipoConsulta = (TipoConsultaEnum?)c.TipoConsulta,
                FormaDePagamento = (FormaDePagamentoEnum?)c.FormaDePagamento,
                c.StatusPagamento
            });

            var examesPago = _dbContext.Exames
                .Where(e => e.StatusPagamento == StatusPagamentoEnum.Aprovado &&
                            e.DataMarcada.Date >= dataInicio.Date && e.DataMarcada.Date <= dataFim.Date &&
                            (!instituicaoId.HasValue || e.InstituicaoId == instituicaoId.Value))
                .Select(e => new
                {
                    Data = e.DataMarcada.Date,
                    e.Valor,
                    TipoConsulta = (TipoConsultaEnum?)null,
                    FormaDePagamento = (FormaDePagamentoEnum?)null,
                    e.StatusPagamento
                });

            var servicos = await consultasPago.Concat(examesPago).ToListAsync();


            var receitaTotal = servicos.Sum(s => s.Valor);
            var totalServicos = servicos.Count;
            var totalDias = (dataFim.Date - dataInicio.Date).Days + 1;
            var receitaMediaDiaria = totalDias > 0 ? receitaTotal / totalDias : 0;
            var ticketMedio = totalServicos > 0 ? receitaTotal / totalServicos : 0;

            var receitaConsultas = servicos.Where(s => s.TipoConsulta.HasValue).Sum(s => s.Valor);
            var receitaExames = servicos.Where(s => !s.TipoConsulta.HasValue).Sum(s => s.Valor);
            var totalConsultas = servicos.Count(s => s.TipoConsulta.HasValue);
            var totalExames = totalServicos - totalConsultas;

            var queryGasto = _dbContext.SuprimentosCompras
                .Where(sc => sc.DataComprada.HasValue &&
                             sc.DataComprada.Value.Date >= dataInicio.Date &&
                             sc.DataComprada.Value.Date <= dataFim.Date &&
                             (!instituicaoId.HasValue || sc.InstituicaoId == instituicaoId.Value))
                .Include(sc => sc.Suprimento)
                    .ThenInclude(s => s.Categoria);

            var gastosList = await queryGasto.ToListAsync();

            var totalGastosSuprimentos = gastosList.Sum(sc => sc.QuantidadeSaida.GetValueOrDefault() * (sc.ValorPago.GetValueOrDefault() / sc.QuantidadeComprada.GetValueOrDefault(1)));

            var gastosPorCategoria = gastosList
                .GroupBy(sc => sc.Suprimento?.Categoria?.Titulo ?? "Sem Categoria")
                .Select(g => new GastoPorCategoria
                {
                    Categoria = g.Key!,
                    Valor = g.Sum(sc => sc.QuantidadeSaida.GetValueOrDefault() * (sc.ValorPago.GetValueOrDefault() / sc.QuantidadeComprada.GetValueOrDefault(1)))
                }).ToList();

            var receitaDiaria = servicos
                .GroupBy(s => s.Data)
                .Select(g => new ReceitaPorDia { Data = g.Key, Valor = g.Sum(s => s.Valor) })
                .OrderBy(r => r.Data)
                .ToList();

            var gastosPorTipoConsulta = consultasPago
                .GroupBy(c => c.TipoConsulta)
                .Select(g => new GastoPorTipoConsulta
                {
                    TipoConsulta = g.Key.Value,
                    Valor = g.Sum(c => c.Valor)
                }).ToList();

            var receitaPorStatusPagamento = await consultasPago
                .Concat(examesPago)
                .GroupBy(s => s.StatusPagamento)
                .Select(g => new ReceitaPorStatusPagamento
                {
                    StatusPagamento = g.Key,
                    Valor = g.Sum(s => s.Valor)
                }).ToListAsync();

            var receitaPorFormaPagamento = consultasPago
                .GroupBy(c => c.FormaDePagamento)
                .Select(g => new ReceitaPorFormaPagamento
                {
                    FormaPagamento = g.Key.Value,
                    Valor = g.Sum(c => c.Valor)
                }).ToList();

            return new MonitoramentoFinancasModel
            {
                HospitalId = instituicaoId,
                ReceitaTotal = Math.Round(receitaTotal, 2),
                ReceitaConsultas = Math.Round(receitaConsultas, 2),
                ReceitaExames = Math.Round(receitaExames, 2),
                TotalConsultas = totalConsultas,
                TotalExames = totalExames,
                ReceitaMediaDiaria = Math.Round(receitaMediaDiaria, 2),
                TicketMedio = Math.Round(ticketMedio, 2),
                TotalGastosSuprimentos = Math.Round(totalGastosSuprimentos, 2),
                GastosPorCategoria = gastosPorCategoria,
                ReceitaDiaria = receitaDiaria,
                GastosPorTipoConsulta = gastosPorTipoConsulta,
                ReceitaPorStatusPagamento = receitaPorStatusPagamento,
                ReceitaPorFormaPagamento = receitaPorFormaPagamento
            };
        }


        /// <summary>
        /// Obtém dados de monitoramento de leitos das instituições.
        /// </summary>
        /// <param name="instituicaoId"></param>
        /// <returns></returns>
        public async Task<MonitoramentoLeitosModel> ObterMonitoramentoLeitos(Guid? instituicaoId = null)
        {
            var queryLeitos = _dbContext.Leitos.AsQueryable();
            var queryLeitosPaciente = _dbContext.LeitosPacientes.AsQueryable();

            if (instituicaoId.HasValue)
            {
                queryLeitos = queryLeitos.Where(l => l.InstutuicaoId == instituicaoId.Value);
                queryLeitosPaciente = queryLeitosPaciente.Where(lp => lp.Leito!.InstutuicaoId == instituicaoId.Value);
            }

            var totalLeitos = await queryLeitos.CountAsync();

            var leitosDisponiveis = await queryLeitos.CountAsync(l => l.StatusLeito == statusLeitoEnum.Liberado);

            var leitosEmUso = await queryLeitosPaciente.CountAsync();

            var leitosEmManutencao = await queryLeitos.CountAsync(l => l.StatusLeito == statusLeitoEnum.EmManutencao);

            var leitosPorTipo = await queryLeitos
                .GroupBy(l => l.TipoLeito)
                .Select(g => new LeitosPorTipoModel
                {
                    TipoLeito = g.Key.HasValue ? g.Key.ToString() : "Desconhecido",
                    Total = g.Count(),
                    Disponiveis = g.Count(l => l.StatusLeito == statusLeitoEnum.Liberado),
                    EmUso = queryLeitosPaciente
                                .Where(lp => lp.Leito!.TipoLeito == g.Key)
                                .Count(),
                    EmManutencao = g.Count(l => l.StatusLeito == statusLeitoEnum.EmManutencao)
                })
                .ToListAsync();

            return new MonitoramentoLeitosModel
            {
                InstituicaoId = instituicaoId,
                TotalLeitos = totalLeitos,
                LeitosDisponiveis = leitosDisponiveis,
                LeitosEmUso = leitosEmUso,
                LeitosEmManutencao = leitosEmManutencao,
                LeitosPorTipo = leitosPorTipo
            };
        }

        /// <summary>
        /// Obtém dados de monitoramento de suprimentos das instituições.
        /// </summary>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <param name="instituicaoId"></param>
        /// <returns></returns>
        public async Task<MonitoramentoSuprimentosModel> ObterMonitoramentoSuprimentos(DateTime dataInicio, DateTime dataFim, Guid? instituicaoId = null)
        {
            var inicio = dataInicio.Date;
            var fim = dataFim.Date.AddDays(1);

            var queryCompras = _dbContext.SuprimentosCompras
                .Where(sc =>
                    sc.DataComprada.HasValue &&
                    sc.DataComprada.Value >= inicio &&
                    sc.DataComprada.Value < fim &&
                    (!instituicaoId.HasValue || sc.InstituicaoId == instituicaoId.Value))
                .Include(sc => sc.Suprimento)
                    .ThenInclude(s => s.Categoria);

            var comprasList = await queryCompras.ToListAsync();

            var custoTotal = comprasList.Sum(sc => sc.ValorPago.GetValueOrDefault(0M));
            var totalUnidadesCompradas = comprasList.Sum(sc => sc.QuantidadeComprada.GetValueOrDefault(0));
            var totalUnidadesUsadas = comprasList.Sum(sc => sc.QuantidadeSaida.GetValueOrDefault(0));

            var suprimentosList = await _dbContext.Suprimentos
                .Where(s => !instituicaoId.HasValue || s.InstituicaoId == instituicaoId.Value)
                .Include(s => s.Categoria)
                .ToListAsync();

            var comprasPorSuprimento = comprasList
                .GroupBy(c => c.SuprimentoId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Comprado = g.Sum(x => x.QuantidadeComprada.GetValueOrDefault(0)),
                        Usado = g.Sum(x => x.QuantidadeSaida.GetValueOrDefault(0))
                    }
                );

            int estoqueCriticoNivel = 10;
            int itensEstoqueCritico = 0;

            var suprimentosPorProduto = suprimentosList.Select(s =>
            {
                comprasPorSuprimento.TryGetValue(s.Id, out var stats);

                int comprado = stats?.Comprado ?? 0;
                int usado = stats?.Usado ?? 0;
                int estoqueAtual = comprado - usado;

                if (estoqueAtual < estoqueCriticoNivel)
                    itensEstoqueCritico++;

                decimal custoTotalComprado = stats != null
                    ? comprasList.Where(c => c.SuprimentoId == s.Id).Sum(c => c.ValorPago.GetValueOrDefault(0M))
                    : 0M;

                decimal custoPorUnidade = comprado > 0
                    ? Math.Round(custoTotalComprado / comprado, 2)
                    : 0M;

                return new SuprimentoPorProdutoModel
                {
                    NomeProduto = s.Titulo ?? string.Empty,
                    Categoria = s.Categoria?.Titulo ?? string.Empty,
                    QuantidadeComprada = comprado,
                    QuantidadeUsada = usado,
                    QuantidadeEmEstoque = estoqueAtual,
                    CustoTotalComprado = Math.Round(custoTotalComprado, 2),
                    CustoPorUnidade = custoPorUnidade,
                    TotalItensEstoque = estoqueAtual
                };
            }).ToList();

            return new MonitoramentoSuprimentosModel
            {
                InstituicaoId = instituicaoId,
                CustoTotal = Math.Round(custoTotal, 2),
                ItensEstoqueCritico = itensEstoqueCritico,
                TotalUnidadesCompradas = totalUnidadesCompradas,
                TotalUnidadesUsadas = totalUnidadesUsadas,
                SuprimentosPorProduto = suprimentosPorProduto
            };
        }
    }
}