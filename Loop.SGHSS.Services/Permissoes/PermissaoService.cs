using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Permissao_Entity;
using Loop.SGHSS.Extensions.Exceptions;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Permissao;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Permissoes
{
    public class PermissaoService : IPermissaoService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;

        public PermissaoService(LoopSGHSSDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region CRUD Permissões

        /// <summary>
        /// Cadastrar uma nova permissão.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarPermissao(PermissaoModel model)
        {
            // --== Verifica se já existe uma permissão com este código.
            bool existe = await _dbContext.Permissoes
                .AnyAsync(item => item.Codigo == model.Codigo);

            if (existe)
                throw new SGHSSBadRequestException("Código da permissão informada já existe no sistema.");

            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            var entidade = _mapper.Map<Permissao>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todas as permissões paginadas.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<PermissaoModel>> ObterPermissoesPaginadas(PermissaoQueryFilter filter)
        {
            var query = _dbContext.Permissoes.Where(x => !x.SysIsDeleted).AsQueryable();

            var permissoes = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<PermissaoModel>(x))
            .ToPaged(filter);

            return permissoes;
        }

        /// <summary>
        /// Buscar instituição por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PermissaoModel?> BuscarPermissaoPorId(Guid id)
        {
            var entidade = await _dbContext.Permissoes.FindAsync(id);

            if (entidade == null)
                throw new SGHSSBadRequestException("Permissão não encontrada.");

            return _mapper.Map<PermissaoModel>(entidade);
        }

        #endregion

        #region Dar permissões

        /// <summary>
        /// Vincular permissão a um profissional de saúde.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularPermissaoProfissional(Guid permissaoId, Guid profissionalId)
        {
            Permissao permissao = await _dbContext.Permissoes
                .FirstOrDefaultAsync(x => x.Id == permissaoId)
                ?? throw new SGHSSBadRequestException("Permissão informada não encontrada.");

            var relacionamento = await _dbContext.PermissoesProfissionaisSaude
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.ProfissionalSaudeId == profissionalId);

            if (relacionamento != null)
                throw new SGHSSBadRequestException("Este profissional já possui esta permissão.");

            var entidade = new Permissao_ProfissionalSaude
            {
                Id = Guid.NewGuid(),
                ProfissionalSaudeId = profissionalId,
                PermissaoId = permissaoId,
            };

            _dbContext.PermissoesProfissionaisSaude.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular um profissional de saúde de uma permissão.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SGHSSBadRequestException"></exception>
        public async Task DesvincularPermissaoProfissional(Guid permissaoId, Guid profissionalId)
        {
            var entidade = await _dbContext.PermissoesProfissionaisSaude
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.ProfissionalSaudeId == profissionalId)
                ?? throw new SGHSSBadRequestException("Este profissional não possui esta permissão.");

            _dbContext.PermissoesProfissionaisSaude.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vincular permissão a um funcionário de uma instituição.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularPermissaoFuncionario(Guid permissaoId, Guid funcionarioId)
        {
            Permissao permissao = await _dbContext.Permissoes
                .FirstOrDefaultAsync(x => x.Id == permissaoId)
                ?? throw new SGHSSBadRequestException("Permissão informada não encontrada.");

            var relacionamento = await _dbContext.PermissoesFuncionarios
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.FuncionarioId == funcionarioId);

            if (relacionamento != null)
                throw new SGHSSBadRequestException("Este funcionário já possui esta permissão.");

            var entidade = new Permissao_Funcionario
            {
                Id = Guid.NewGuid(),
                FuncionarioId = funcionarioId,
                PermissaoId = permissaoId,
            };

            _dbContext.PermissoesFuncionarios.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular permissão de um funcionário de uma instituição
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SGHSSBadRequestException"></exception>
        public async Task DesvincularPermissaoFuncionario(Guid permissaoId, Guid funcionarioId)
        {
            var entidade = await _dbContext.PermissoesFuncionarios
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.FuncionarioId == funcionarioId)
                ?? throw new SGHSSBadRequestException("Este funcionário não possui esta permissão.");

            _dbContext.PermissoesFuncionarios.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vincular permissão a um paciente.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SGHSSBadRequestException"></exception>
        public async Task VincularPermissaoPaciente(Guid permissaoId, Guid pacienteId)
        {
            Permissao permissao = await _dbContext.Permissoes
                .FirstOrDefaultAsync(x => x.Id == permissaoId)
                ?? throw new SGHSSBadRequestException("Permissão informada não encontrada.");

            var relacionamento = await _dbContext.PermissoesPaciente
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.PacienteId == pacienteId);

            if (relacionamento != null)
                throw new SGHSSBadRequestException("Este paciente já possui esta permissão.");

            var entidade = new Permissao_Paciente
            {
                Id = Guid.NewGuid(),
                PacienteId = pacienteId,
                PermissaoId = permissaoId,
            };

            _dbContext.PermissoesPaciente.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular permissão de um paciente.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SGHSSBadRequestException"></exception>
        public async Task DesvincularPermissaoPaciente(Guid permissaoId, Guid pacienteId)
        {
            var entidade = await _dbContext.PermissoesPaciente
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.PacienteId == pacienteId)
                ?? throw new SGHSSBadRequestException("Este paciente não possui esta permissão.");

            _dbContext.PermissoesPaciente.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Vincular permissão a um paciente.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularPermissaoAdm(Guid permissaoId, Guid admId)
        {
            Permissao permissao = await _dbContext.Permissoes
                .FirstOrDefaultAsync(x => x.Id == permissaoId)
                ?? throw new SGHSSBadRequestException("Permissão informada não encontrada.");

            var relacionamento = await _dbContext.PermissoesAdministrador
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.AdministradorId == admId);

            if (relacionamento != null)
                throw new SGHSSBadRequestException("Este administrador já possui esta permissão.");

            var entidade = new Permissao_Administrador
            {
                Id = Guid.NewGuid(),
                AdministradorId = admId,
                PermissaoId = permissaoId,
            };

            _dbContext.PermissoesAdministrador.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular permissão de um paciente.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SGHSSBadRequestException"></exception>
        public async Task DesvincularPermissaoAdm(Guid permissaoId, Guid admId)
        {
            var entidade = await _dbContext.PermissoesPaciente
                .FirstOrDefaultAsync(x => x.PermissaoId == permissaoId && x.PacienteId == admId)
                ?? throw new SGHSSBadRequestException("Este paciente não possui esta permissão.");

            _dbContext.PermissoesPaciente.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Permissões padrão

        /// <summary>
        /// Atribui as permissões padrão a um paciente recém-criado.
        /// </summary>
        /// <param name="pacienteId">O ID do paciente.</param>
        private async Task AtribuirPermissoes<TPermissaoEntity>(
            Guid userId,
            List<string> codigosPermissoes,
            Func<Guid, Guid, TPermissaoEntity> createPermissionEntity)
            where TPermissaoEntity : class
        {
            var permissoesExistentes = await _dbContext.Permissoes
                .Where(p => codigosPermissoes.Contains(p.Codigo!))
                .ToListAsync();

            // Buscar as permissões já atribuídas ao usuário (paciente)
            var permissoesJaAtribuidas = await _dbContext.Set<TPermissaoEntity>()
                .Where(p => EF.Property<Guid>(p, typeof(TPermissaoEntity) == typeof(Permissao_Paciente) ? "PacienteId" : "UserId") == userId)
                .Select(p => EF.Property<Guid>(p, "PermissaoId"))
                .ToListAsync();

            var permissoesAtribuir = new List<TPermissaoEntity>();

            foreach (var perm in permissoesExistentes)
            {
                if (!permissoesJaAtribuidas.Contains(perm.Id))
                {
                    permissoesAtribuir.Add(createPermissionEntity(userId, perm.Id));
                }
            }

            if (permissoesAtribuir.Any())
            {
                if (typeof(TPermissaoEntity) == typeof(Permissao_Paciente))
                    await _dbContext.Set<Permissao_Paciente>().AddRangeAsync(permissoesAtribuir as IEnumerable<Permissao_Paciente>);
                else if (typeof(TPermissaoEntity) == typeof(Permissao_Funcionario))
                    await _dbContext.Set<Permissao_Funcionario>().AddRangeAsync(permissoesAtribuir as IEnumerable<Permissao_Funcionario>);
                else if (typeof(TPermissaoEntity) == typeof(Permissao_ProfissionalSaude))
                    await _dbContext.Set<Permissao_ProfissionalSaude>().AddRangeAsync(permissoesAtribuir as IEnumerable<Permissao_ProfissionalSaude>);
                else if (typeof(TPermissaoEntity) == typeof(Permissao_Administrador))
                    await _dbContext.Set<Permissao_Administrador>().AddRangeAsync(permissoesAtribuir as IEnumerable<Permissao_Administrador>);

                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Atribui as permissões padrão a um paciente recém-criado.
        /// </summary>
        /// <param name="pacienteId">O ID do paciente.</param>
        public async Task AtribuirPemissaoPadraoPaciente(Guid pacienteId)
        {
            var codigosPermissoesPadraoPaciente = new List<string>
            {
                "E01", // Agendar consulta
                "E02", // Cancelar consulta
                "E03", // Agendar exame
                "E04", // Cancelar exame
                "E05", // Visualizar histórico de agendamentos
                "E06", // Acessar teleconsulta
                "E07", // Receber notificações por e-mail
                "E08", // Visualizar e baixar prescrições e exames
                "USUARIO_MUDAR_SENHA_PROPRIA",
                "USUARIO_EDITAR_PERFIL_PROPRIO",
                "USUARIO_VISUALIZAR_PERFIL_PROPRIO"
            };

            await AtribuirPermissoes<Permissao_Paciente>(
                pacienteId,
                codigosPermissoesPadraoPaciente,
                (userId, permId) => new Permissao_Paciente { PacienteId = userId, PermissaoId = permId }
            );
        }


        /// <summary>
        /// Atribui as permissões padrão a um funcionário recém-criado.
        /// </summary>
        /// <param name="funcionarioId">O ID do funcionário.</param>
        public async Task AtribuirPemissaoPadraoFuncionario(Guid funcionarioId)
        {
            var codigosPermissoesPadraoFuncionario = new List<string>
            {
                "D01", // Agendar consulta para paciente
                "D02", // Agendar exame para paciente
                "D03", // Visualizar agenda da instituição
                "D04", // Visualizar histórico de pacientes
                "D05", // Cadastrar paciente
                "D06", // Editar dados do paciente
                "D07", // Gerenciar suprimentos (cadastro de compra, consumo)
                "D08", // Visualizar dashboards da instituição
                "D09", // Gerenciar leitos (cadastrar, associar paciente)
                "USUARIO_MUDAR_SENHA_PROPRIA",
                "USUARIO_EDITAR_PERFIL_PROPRIO",
                "USUARIO_VISUALIZAR_PERFIL_PROPRIO"
            };

            await AtribuirPermissoes<Permissao_Funcionario>(
                funcionarioId,
                codigosPermissoesPadraoFuncionario,
                (userId, permId) => new Permissao_Funcionario { FuncionarioId = userId, PermissaoId = permId }
            );
        }


        /// <summary>
        /// Atribui as permissões padrão a um administrador recém-criado.
        /// </summary>
        /// <param name="administradorId">O ID do administrador.</param>
        /// <param name="isGlobalAdmin">Define se o administrador é global (true) ou local (false).</param>
        public async Task AtribuirPemissaoPadraoAdministrador(Guid administradorId, bool isGlobalAdmin)
        {
            List<string> codigosPermissoesPadraoAdministrador;

            if (isGlobalAdmin)
            {
                // --== Se for um administrador global, atribua TODAS as permissões disponíveis (A, B, C, D, E e gerais).
                codigosPermissoesPadraoAdministrador = await _dbContext.Permissoes
                                                                    .Select(p => p.Codigo!)
                                                                    .ToListAsync();
            }
            else
            {
                // --== Se for um administrador local, atribua permissões específicas da categoria B e outras relevantes.
                codigosPermissoesPadraoAdministrador = new List<string>
                {
                    "B01", // Visualizar dados da própria instituição
                    "B02", // Cadastrar funcionário
                    "B03", // Editar/Excluir funcionário
                    "B04", // Cadastrar profissional de saúde
                    "B05", // Editar profissional de saúde
                    "B06", // Associar profissionais a especializações/instituições
                    "B07", // Gerenciar suprimentos da instituição
                    "B08", // Visualizar dashboard institucional
                    "B09", // Gerenciar leitos (cadastrar, editar, liberar)
                    "B10", // Visualizar agenda da instituição
                    "D01", // Agendar consulta para paciente
                    "D02", // Agendar exame para paciente
                    "D03", // Visualizar agenda da instituição (já coberto por B10, mas manter por clareza se houver diferença)
                    "D04", // Visualizar histórico de pacientes
                    "D05", // Cadastrar paciente
                    "D06", // Editar dados do paciente
                    "D07", // Gerenciar suprimentos (cadastro de compra, consumo) - já coberto por B07, mas reforça se houver subdivisão
                    "D08", // Visualizar dashboards da instituição (já coberto por B08)
                    "D09", // Gerenciar leitos (cadastrar, associar paciente) - já coberto por B09

                    "USUARIO_MUDAR_SENHA_PROPRIA",
                    "USUARIO_RESETAR_SENHA_TERCEIRO",
                    "USUARIO_EDITAR_PERFIL_PROPRIO",
                    "USUARIO_VISUALIZAR_PERFIL_PROPRIO"
                };
            }

            await AtribuirPermissoes<Permissao_Administrador>(
                administradorId,
                codigosPermissoesPadraoAdministrador,
                (userId, permId) => new Permissao_Administrador { AdministradorId = userId, PermissaoId = permId }
            );
        }


        /// <summary>
        /// Atribui as permissões padrão a um profissional de saúde recém-criado (Médico, Enfermeiro, etc.).
        /// </summary>
        /// <param name="profissionalSaudeId">O ID do profissional de saúde.</param>
        public async Task AtribuirPemissaoPadraoProfissionalSaude(Guid profissionalSaudeId)
        {
            var codigosPermissoesPadraoProfissionalSaude = new List<string>
            {
                "C01", // Visualizar agenda pessoal
                "C02", // Atender consulta presencial
                "C03", // Atender teleconsulta
                "C04", // Inserir prontuário
                "C05", // Emitir prescrição
                "C06", // Solicitar exame
                "C07", // Visualizar histórico de pacientes atendidos
                "C08", // Anexar arquivos PDF
                "C09", // Gerenciar dias e horários de atendimento
                "USUARIO_MUDAR_SENHA_PROPRIA",
                "USUARIO_EDITAR_PERFIL_PROPRIO",
                "USUARIO_VISUALIZAR_PERFIL_PROPRIO"
            };

            await AtribuirPermissoes<Permissao_ProfissionalSaude>(
                profissionalSaudeId,
                codigosPermissoesPadraoProfissionalSaude,
                (userId, permId) => new Permissao_ProfissionalSaude { ProfissionalSaudeId = userId, PermissaoId = permId }
            );
        }

        #endregion
    }
}
