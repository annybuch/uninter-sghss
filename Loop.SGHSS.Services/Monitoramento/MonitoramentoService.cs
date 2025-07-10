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
            // --== Filtra as consultas com pagamento aprovado dentro do intervalo de datas e da instituição.
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

            // --== Filtra os exames com pagamento aprovado dentro do intervalo de datas e da instituição.
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

            // --== Junta as duas listas (consultas + exames).
            var servicos = await consultasPago.Concat(examesPago).ToListAsync();

            // --== Calcula os totais e médias.
            var receitaTotal = servicos.Sum(s => s.Valor);
            var totalServicos = servicos.Count;
            var totalDias = (dataFim.Date - dataInicio.Date).Days + 1;
            var receitaMediaDiaria = totalDias > 0 ? receitaTotal / totalDias : 0;
            var ticketMedio = totalServicos > 0 ? receitaTotal / totalServicos : 0;

            // --== Separa receita de consultas e exames.
            var receitaConsultas = servicos.Where(s => s.TipoConsulta.HasValue).Sum(s => s.Valor);
            var receitaExames = servicos.Where(s => !s.TipoConsulta.HasValue).Sum(s => s.Valor);
            var totalConsultas = servicos.Count(s => s.TipoConsulta.HasValue);
            var totalExames = totalServicos - totalConsultas;

            // --== Filtra compras de suprimentos feitas no período
            var queryGasto = _dbContext.SuprimentosCompras
                .Where(sc => sc.DataComprada.HasValue &&
                             sc.DataComprada.Value.Date >= dataInicio.Date &&
                             sc.DataComprada.Value.Date <= dataFim.Date &&
                             (!instituicaoId.HasValue || sc.InstituicaoId == instituicaoId.Value))
                .Include(sc => sc.Suprimento)
                    .ThenInclude(s => s.Categoria);

            var gastosList = await queryGasto.ToListAsync();

            // --== Calcula o total de gastos com suprimentos, considerando a quantidade de saída.
            var totalGastosSuprimentos = gastosList.Sum(sc => sc.QuantidadeSaida.GetValueOrDefault() * (sc.ValorPago.GetValueOrDefault() / sc.QuantidadeComprada.GetValueOrDefault(1)));

            // --== Agrupa os gastos por categoria do suprimento.
            var gastosPorCategoria = gastosList
                .GroupBy(sc => sc.Suprimento?.Categoria?.Titulo ?? "Sem Categoria")
                .Select(g => new GastoPorCategoria
                {
                    Categoria = g.Key!,
                    Valor = g.Sum(sc => sc.QuantidadeSaida.GetValueOrDefault() * (sc.ValorPago.GetValueOrDefault() / sc.QuantidadeComprada.GetValueOrDefault(1)))
                }).ToList();

            // --== Agrupa receita por dia.
            var receitaDiaria = servicos
                .GroupBy(s => s.Data)
                .Select(g => new ReceitaPorDia { Data = g.Key, Valor = g.Sum(s => s.Valor) })
                .OrderBy(r => r.Data)
                .ToList();

            // --== Agrupa receita por tipo de consulta (consultas apenas).
            var gastosPorTipoConsulta = consultasPago
                .GroupBy(c => c.TipoConsulta)
                .Select(g => new GastoPorTipoConsulta
                {
                    TipoConsulta = g.Key!.Value,
                    Valor = g.Sum(c => c.Valor)
                }).ToList();

            // --== Agrupa receita por status de pagamento (consultas + exames).
            var receitaPorStatusPagamento = await consultasPago
                .Concat(examesPago)
                .GroupBy(s => s.StatusPagamento)
                .Select(g => new ReceitaPorStatusPagamento
                {
                    StatusPagamento = g.Key,
                    Valor = g.Sum(s => s.Valor)
                }).ToListAsync();

            // --== Agrupa receita por forma de pagamento (apenas consultas, pois exames não têm essa informação).
            var receitaPorFormaPagamento = consultasPago
                .GroupBy(c => c.FormaDePagamento)
                .Select(g => new ReceitaPorFormaPagamento
                {
                    FormaPagamento = g.Key!.Value,
                    Valor = g.Sum(c => c.Valor)
                }).ToList();

            // --== Retorna o modelo consolidado com todas as métricas calculadas.
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
            // --== Cria queries base para Leitos e LeitosPacientes.
            var queryLeitos = _dbContext.Leitos.AsQueryable();
            var queryLeitosPaciente = _dbContext.LeitosPacientes.AsQueryable();

            // --== Aplica filtro por instituição, se informado.
            if (instituicaoId.HasValue)
            {
                queryLeitos = queryLeitos.Where(l => l.InstutuicaoId == instituicaoId.Value);
                queryLeitosPaciente = queryLeitosPaciente.Where(lp => lp.Leito!.InstutuicaoId == instituicaoId.Value);
            }

            // --== Conta total de leitos.
            var totalLeitos = await queryLeitos.CountAsync();

            // --== Conta leitos com status "Liberado".
            var leitosDisponiveis = await queryLeitos.CountAsync(l => l.StatusLeito == statusLeitoEnum.Liberado);

            // --== Conta total de leitos em uso (associados a pacientes).
            var leitosEmUso = await queryLeitosPaciente.CountAsync();

            // --== Conta leitos com status "Em Manutenção".
            var leitosEmManutencao = await queryLeitos.CountAsync(l => l.StatusLeito == statusLeitoEnum.EmManutencao);

            // --== Agrupa leitos por tipo e calcula métricas para cada tipo.
            var leitosPorTipo = await queryLeitos
                .GroupBy(l => l.TipoLeito)
                .Select(g => new LeitosPorTipoModel
                {
                    // --== Trata tipo nulo como "Desconhecido".
                    TipoLeito = g.Key.HasValue ? g.Key.ToString() : "Desconhecido",
                    Total = g.Count(),
                    Disponiveis = g.Count(l => l.StatusLeito == statusLeitoEnum.Liberado),
                    EmUso = queryLeitosPaciente
                                .Where(lp => lp.Leito!.TipoLeito == g.Key)
                                .Count(),
                    EmManutencao = g.Count(l => l.StatusLeito == statusLeitoEnum.EmManutencao)
                })
                .ToListAsync();

            // --== Monta e retorna o modelo consolidado.
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
            // --== Define o intervalo de datas: início incluso, fim exclusivo (fim do dia).
            var inicio = dataInicio.Date;
            var fim = dataFim.Date.AddDays(1);

            // --== Define o intervalo de datas: início incluso, fim exclusivo (fim do dia).
            var queryCompras = _dbContext.SuprimentosCompras
                .Where(sc =>
                    sc.DataComprada.HasValue &&
                    sc.DataComprada.Value >= inicio &&
                    sc.DataComprada.Value < fim &&
                    (!instituicaoId.HasValue || sc.InstituicaoId == instituicaoId.Value))
                .Include(sc => sc.Suprimento)
                    .ThenInclude(s => s.Categoria);

            // --== Executa a query e carrega os dados na memória.
            var comprasList = await queryCompras.ToListAsync();

            // --== Cálculo total de valores e quantidades.
            var custoTotal = comprasList.Sum(sc => sc.ValorPago.GetValueOrDefault(0M));
            var totalUnidadesCompradas = comprasList.Sum(sc => sc.QuantidadeComprada.GetValueOrDefault(0));
            var totalUnidadesUsadas = comprasList.Sum(sc => sc.QuantidadeSaida.GetValueOrDefault(0));

            // --== Carrega todos os suprimentos (filtrando por instituição, se necessário).
            var suprimentosList = await _dbContext.Suprimentos
                .Where(s => !instituicaoId.HasValue || s.InstituicaoId == instituicaoId.Value)
                .Include(s => s.Categoria)
                .ToListAsync();

            // --== Agrupa as compras por suprimento, somando comprados e usados.
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

            // --== Monta a lista com os dados por produto.
            var suprimentosPorProduto = suprimentosList.Select(s =>
            {
                // --== Tenta obter estatísticas de compra/uso para o suprimento atual.
                comprasPorSuprimento.TryGetValue(s.Id, out var stats);

                int comprado = stats?.Comprado ?? 0;
                int usado = stats?.Usado ?? 0;
                int estoqueAtual = comprado - usado;

                // --== Verifica se está em nível crítico.
                if (estoqueAtual < estoqueCriticoNivel)
                    itensEstoqueCritico++;

                // --== Soma do custo total para o produto.
                decimal custoTotalComprado = stats != null
                    ? comprasList.Where(c => c.SuprimentoId == s.Id).Sum(c => c.ValorPago.GetValueOrDefault(0M))
                    : 0M;

                // --== Cálculo do custo médio por unidade.
                decimal custoPorUnidade = comprado > 0
                    ? Math.Round(custoTotalComprado / comprado, 2)
                    : 0M;

                // --== Monta o modelo do produto.
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

            // --== Retorna o modelo final com todos os dados consolidados.
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