using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Leito_Entity;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Instituicao;
using Loop.SGHSS.Model._Enums.Leitos.Loop.SGHSS.Model.Enums;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Leitos;
using Loop.SGHSS.Model.Pacientes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Leitos
{
    public class LeitoService : ILeitoService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;

        public LeitoService(LoopSGHSSDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Cadastrar uma novo leito.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarLeito(LeitosModel model)
        {
            // --== Validando se já existe um leito com o mesmo número.
            bool jaExiste = _dbContext.Leitos.Any(item => item.NumeroLeito == model.NumeroLeito);

            if (jaExiste)
                throw new Exception("Um leito com o mesmo número já foi cadastrado no sistema");

            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            var entidade = _mapper.Map<Leito>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Cadastrar leitos em massa.
        /// </summary>
        public async Task Cadastrar_Leito_Em_Massa(LeitosMassa model)
        {
            List<LeitosModel> leitosCadastrados = new();

            // --== Obtendo a instituição.
            var instituicao = _dbContext.Instituicoes
                .FirstOrDefault(x => x.Id == model.InstituicaoId)
                ?? throw new Exception("Instituição informada não encontrada.");

            // --== Buscando leitos já existentes na instituição.
            var leitosExistentes = _dbContext.Leitos
                .Where(x => x.InstutuicaoId == model.InstituicaoId)
                .ToList();

            // --== Definindo valores padrão (se não vier, aplica padrão).
            var tipoLeito = model.tipoLeitoEnum ?? TipoLeitoEnum.SemIdentificacao;
            var statusLeito = model.StatusLeito ?? statusLeitoEnum.EmManutencao;
            var andar = model.Andar;

            for (int numeroLeito = model.NumeroInicial!.Value; numeroLeito <= model.NumeroFinal!.Value; numeroLeito++)
            {
                string numeroLeitoStr = numeroLeito.ToString();

                // --== Pula se estiver na lista de números cancelados.
                if (model.NumerosCancelados != null && model.NumerosCancelados.Contains(numeroLeito))
                    continue;

                // --== Verifica se já existe um leito com esse número.
                if (leitosExistentes.Any(x => x.NumeroLeito == numeroLeitoStr))
                    continue;

                // --== Montando o ViewModel para cadastro.
                var novoLeito = new TicketCadastrarLeitoViewModel(
                    numeroLeito: numeroLeitoStr,
                    instutuicaoId: model.InstituicaoId,
                    andar: model.Andar,
                    tipoLeitoEnum: tipoLeito,
                    statusLeitoEnum: statusLeito
                );

                // --== Criando a entidade Leito.
                var leito = instituicao.Adicionar_Leito(novoLeito);

                // --== Mapeando para retorno (opcional).
                var leitoModel = _mapper.Map<LeitosModel>(leito);
                leitosCadastrados.Add(leitoModel);

                // --== Adicionando ao contexto.
                _dbContext.Add(leito);
            }

            // --== Salvando no banco.
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todos os leitos paginados ou filtrando por tipo, andar ou status.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<LeitosModel>> ObterLeitosPaginados(LeitoQueryFilter filter)
        {
            var query = _dbContext.Leitos.AsQueryable();

            if (filter.HasFilters)
            {
                if (filter.InstituicaoId.HasValue)
                    query = query.Where(x => x.InstutuicaoId == filter.InstituicaoId);

                if (filter.Andar.HasValue)
                    query = query.Where(x => x.Andar == filter.Andar);

                if (filter.TipoLeito.HasValue)
                    query = query.Where(x => x.TipoLeito == filter.TipoLeito);

                if (filter.Status.HasValue)
                    query = query.Where(x => x.StatusLeito == filter.Status);
            }

            var leitos = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<LeitosModel>(x))
                .ToPaged(filter);

            return leitos;
        }

        /// <summary>
        /// Obter leito por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LeitosModel?> ObterLeitoPorId(Guid id)
        {
            var leito = await _dbContext.Leitos.FindAsync(id);

            if (leito == null)
                return null;

            return _mapper.Map<LeitosModel>(leito);
        }

        /// <summary>
        /// Editar informações gerais do leito.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<LeitosModel> EditarLeitosGeral(LeitosModel model)
        {
            // --== Obtendo leito.
            Leito leito = _dbContext.Leitos.Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new Exception("Leito informado não encontrado");

            // --== Atualizando leito.
            leito.AtualizarGeral(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Atualizar status do leito.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<LeitoStatus> AtualizarStatusLeito(LeitoStatus model)
        {
            // --== Obtendo leito.
            Leito leito = _dbContext.Leitos.Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new Exception("Leito informado não encontrado");

            // --== Atualizando leito.
            leito.AtualizarStatus(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Adiciona um paciente ao leito.
        /// </summary>
        public async Task<LeitosPacientesModel> AdicionarPacienteLeito(AddPacienteLeitoModel model)
        {
            // --== Verificar se o leito existe e está liberado.
            var leito = await _dbContext.Leitos
                .FirstOrDefaultAsync(item => item.Id == model.LeitoId && item.StatusLeito == statusLeitoEnum.Liberado)
                ?? throw new Exception("Leito informado não encontrado e/ou leito não disponível para uso no momento.");

            // --==  Mapear o modelo recebido.
            var leitoPaciente = _mapper.Map<LeitosPacientesModel>(model);

            // --== Gerar ID.
            leitoPaciente.Id = Guid.NewGuid();

            // --== Definir data de entrada do paciente no leito.
            leitoPaciente.DataEntrada = DateTime.Now;

            // --== Criar a primeira observação (se houver).
            if (!string.IsNullOrWhiteSpace(model.Observacao))
            {
                var novaObservacao = new LeitosPacientesObservacaoModel
                {
                    Id = Guid.NewGuid(),
                    LeitosPacientesId = leitoPaciente.Id,
                    Observacao = model.Observacao,
                    DataCriacao = DateTime.Now
                };

                leitoPaciente.Observacoes.Add(novaObservacao);
            }

            //--==  Mapear para a entidade do banco.
            var entidade = _mapper.Map<Leito_Paciente>(leitoPaciente);

            await _dbContext.LeitosPacientes.AddAsync(entidade);

            // --== Atualizar status do leito para EmUso.
            leito.AtualizarStatus(new LeitoStatus
            {
                Id = leito.Id,
                StatusLeito = statusLeitoEnum.EmUso
            });

            await _dbContext.SaveChangesAsync();

            return leitoPaciente;
        }

        /// <summary>
        /// Adiciona uma nova observação ao paciente no leito.
        /// </summary>
        public async Task<LeitosPacientesModel> AdicionarObservacoesPacienteLeito(AddPacienteLeitoModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Observacao))
                throw new Exception("Não é possível adicionar uma observação vazia.");

            // --== Localizar o relacionamento paciente-leito.
            var leitoPaciente = await _dbContext.LeitosPacientes
                .FirstOrDefaultAsync(x => x.LeitoId == model.LeitoId && x.PacienteId == model.PacienteId)
                ?? throw new Exception("Este paciente não foi encontrado neste leito.");

            // --== Criar nova observação.
            var novaObservacao = new LeitosPacientesObservacaoModel
            {
                Id = Guid.NewGuid(),
                LeitosPacientesId = leitoPaciente.Id,
                Observacao = model.Observacao,
                DataCriacao = DateTime.Now
            };

            var obs = _mapper.Map<LeitoPacienteObservacao>(novaObservacao);

            await _dbContext.LeitoPacienteObservacao.AddAsync(obs);

            // --== Adiciona também no objeto em memória, para refletir no retorno
            leitoPaciente.Observacoes.Add(obs);

            await _dbContext.SaveChangesAsync();

            // --== Faz o mapeamento para o model de retorno
            var entidade = _mapper.Map<LeitosPacientesModel>(leitoPaciente);

            return entidade;
        }

        /// <summary>
        /// Remover um paciente do leito.
        /// </summary>
        public async Task RemoverPacienteLeito(Guid? leitoId, Guid? pacienteId)
        {
            if (leitoId == null || pacienteId == null)
                throw new Exception("LeitoId e PacienteId são obrigatórios.");

            // --== Obter o leito.
            var leito = _dbContext.Leitos
                .FirstOrDefault(x => x.Id == leitoId)
                ?? throw new Exception("Leito informado não encontrado.");

            // --== Buscar o relacionamento Leito-Paciente.
            var leitoPaciente = _dbContext.LeitosPacientes
                .FirstOrDefault(x => x.LeitoId == leitoId && x.PacienteId == pacienteId)
                ?? throw new Exception("O paciente informado não está neste leito.");

            // --== Registrar no histórico (log) a saída do paciente para liberar o leito.
            var log = new LeitosPacientesLogModel
            {
                Id = Guid.NewGuid(),
                IdOriginal = leitoPaciente.Id,
                LeitoId = leitoPaciente.LeitoId,
                PacienteId = leitoPaciente.PacienteId,
                DataEntrada = leitoPaciente.DataEntrada,
                DataSaida = DateTime.Now,
                Observacoes = leitoPaciente.Observacoes
                .Select(o => new LeitosPacientesObservacaoModel
                {
                    Id = o.Id,
                    LeitosPacientesId = o.LeitosPacientesId,
                    Observacao = o.Observacao,
                    DataCriacao = o.DataCriacao
                }).ToList()
            };

            await _dbContext.LeitoPacienteLog.AddAsync(_mapper.Map<Leito_PacienteLog>(log));

            // --== Remover o relacionamento Leito-Paciente
            _dbContext.LeitosPacientes.Remove(leitoPaciente);

            // --==  Salvar alterações
            await _dbContext.SaveChangesAsync();

            // --== Atualizar status do leito para Liberado.
            leito.AtualizarStatus(new LeitoStatus
            {
                Id = leitoId,
                StatusLeito = statusLeitoEnum.Liberado
            });
        }
        public async Task<LeitoComPacienteModel?> ObterLeitoComPacienteAtual(Guid leitoId)
        {
            // --== Buscar o leito pelo ID
            var leito = await _dbContext.Leitos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == leitoId);

            if (leito == null)
                return null;

            // --== Preparar objeto de retorno
            var resultado = new LeitoComPacienteModel
            {
                Leito = _mapper.Map<LeitosModel>(leito),
                Paciente = null,
                Observacoes = null
            };

            // --== Verifica se o leito está em uso
            if (leito.StatusLeito == statusLeitoEnum.EmUso)
            {
                var leitoPaciente = await _dbContext.LeitosPacientes
                    .Include(lp => lp.Paciente)
                    .Include(lp => lp.Observacoes)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(lp => lp.LeitoId == leitoId);

                if (leitoPaciente != null)
                {
                    resultado.Paciente = _mapper.Map<PacientesViewModel>(leitoPaciente.Paciente!);
                    resultado.Observacoes = leitoPaciente.Observacoes?
                        .Select(o => new LeitoPacienteObservacaoModel
                        {
                            Id = o.Id,
                            LeitosPacientesId = o.LeitosPacientesId,
                            Observacao = o.Observacao,
                            DataCriacao = o.DataCriacao
                        }).ToList();
                }
            }

            return resultado;
        }
    }
}
