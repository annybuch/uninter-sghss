using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Extensions.Seguranca;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Pacientes;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Services.Permissoes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Pacientes
{
    public class PacienteService : IPacienteService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPermissaoService _permissaoService;

        public PacienteService(LoopSGHSSDataContext dbContext, IMapper mapper, IPermissaoService permissaoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _permissaoService = permissaoService;
        }

        /// <summary>
        /// Cadastrar um novo paciente.
        /// </summary>
        public async Task CadastrarPaciente(PacientesModel model)
        {
            // --== Verificar se já existe um paciente com o mesmo CPF.
            var pacienteExistente = await _dbContext.Pacientes
                .FirstOrDefaultAsync(x => x.CPF == model.CPF);

            if (pacienteExistente != null)
                throw new Exception("Paciente informado já cadastrado no sistema.");

            // --== Gerar novo ID.
            model.Id = Guid.NewGuid();

            // --== Validar e hash da senha
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                throw new Exception("Senha é obrigatória para o paciente.");

            // Usa o PasswordHelper para gerar o hash
            model.PasswordHash = PasswordHelper.GerarHashSenha(model.PasswordHash!);

            // --== Mapear e salvar no banco.
            var entidade = _mapper.Map<Paciente>(model);
            await _dbContext.Pacientes.AddAsync(entidade);
            await _dbContext.SaveChangesAsync();

            // --== ATRIBUIR PERMISSÕES PADRÃO AO NOVO PACIENTE.
            await _permissaoService.AtribuirPemissaoPadraoPaciente(entidade.Id);
        }

        /// <summary>
        /// Listar todos os pacientes com paginação.
        /// </summary>
        public async Task<PagedResult<PacientesViewModel>> ObterPacientesPaginados(PacienteQueryFilter filter)
        {
            var query = _dbContext.Pacientes.AsQueryable();

            var pacientes = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<PacientesViewModel>(x))
                .ToPaged(filter);

            return pacientes;
        }

        /// <summary>
        /// Buscar paciente por ID.
        /// </summary>
        public async Task<PacientesModel> BuscarPacientePorId(Guid id)
        {
            var entidade = await _dbContext.Pacientes.FindAsync(id)
                ?? throw new Exception("Paciente não encontrado no sistema.");

            return _mapper.Map<PacientesModel>(entidade);
        }

        /// <summary>
        /// Editar dados gerais do paciente.
        /// </summary>
        public async Task<PacientesModel> EditarPacienteGeral(PacientesModel model)
        {
            var paciente = await _dbContext.Pacientes
                .FirstOrDefaultAsync(x => x.Id == model.Id)
                ?? throw new Exception("Paciente informado não encontrado.");

            paciente.AtualizarGeral(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar apenas o endereço do paciente.
        /// </summary>
        public async Task<EnderecoModel> EditarPacienteEndereco(EnderecoModel model)
        {
            var paciente = await _dbContext.Pacientes
                .FirstOrDefaultAsync(x => x.Id == model.Id)
                ?? throw new Exception("Paciente informado não encontrado.");

            paciente.AtualizarEndereco(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar senha do paciente.
        /// </summary>
        public async Task<PassModel> EditarSenhaPaciente(PassModel model)
        {
            var paciente = await _dbContext.Pacientes
                .FirstOrDefaultAsync(x => x.Id == model.Id)
                ?? throw new Exception("Paciente informado não encontrado.");

            if (string.IsNullOrWhiteSpace(model.PasswordHash) || model.PasswordHash.Length < 6)
                throw new Exception("A senha deve ter no mínimo 6 caracteres.");

            if (PasswordHelper.VerificarSenha(model.PasswordHash, paciente.PasswordHash!))
                throw new Exception("A nova senha não pode ser igual à anterior.");

            var novaSenhaHash = PasswordHelper.GerarHashSenha(model.PasswordHash);

            model.PasswordHash = novaSenhaHash;

            paciente.AtualizarSenha(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Obter todas as consultas de um paciente com filtros e paginação, para ser consumida no calendário do paciente.
        /// </summary>
        public async Task<PagedResult<ConsultaGradePacienteModel>> ObterConsultasPorPaciente(ConsultaPacienteQueryFilter filter)
        {
            var query = _dbContext.Consultas
                .Include(x => x.Especializacao)
                .Include(x => x.ProfissionalSaude)
                .Include(x => x.Instituicao) 
                .Where(x => x.PacienteId == filter.PacienteId)
                .AsQueryable();

            if (filter.StatusConsulta.HasValue)
                query = query.Where(x => x.StatusConsulta == filter.StatusConsulta);

            if (filter.TipoConsulta.HasValue)
                query = query.Where(x => x.TipoConsulta == filter.TipoConsulta);

            if (filter.EspecializacaoId.HasValue)
                query = query.Where(x => x.EspecializacaoId == filter.EspecializacaoId);

            if (filter.InstituicaoId.HasValue)
                query = query.Where(x => x.InstituicaoId == filter.InstituicaoId); 

            if (filter.DataInicial.HasValue)
                query = query.Where(x => x.DataMarcada >= filter.DataInicial.Value);

            if (filter.DataFinal.HasValue)
                query = query.Where(x => x.DataMarcada <= filter.DataFinal.Value);

            query = query.OrderByDescending(x => x.DataMarcada);

            var resultado = await query
                .Select(x => new ConsultaGradePacienteModel
                {
                    Id = x.Id,
                    DataMarcada = x.DataMarcada,
                    TipoConsulta = x.TipoConsulta,
                    StatusConsulta = x.StatusConsulta,
                    NomeEspecializacao = x.Especializacao!.Titulo,
                    NomeProfissionalSaude = x.ProfissionalSaude!.Nome + " " + x.ProfissionalSaude.Sobrenome,
                    NomeInstituicao = x.Instituicao != null ? x.Instituicao.RazaoSocial : null 
                })
                .ToPaged(filter);

            return resultado;
        }

        /// <summary>
        /// Obter todos os exames de um paciente com filtros e paginação, para ser consumida no calendário do paciente.
        /// </summary>
        public async Task<PagedResult<ExameGradePacienteModel>> ObterExamesPorPaciente(ExameGradeQueryFilter filter)
        {
            var query = _dbContext.Exames
               .Include(x => x.servicoLaboratorio)
               .Include(x => x.ProfissionalSaude)
               .Include(x => x.Instituicao)
               .Where(x => x.PacienteId == filter.PacienteId)
               .AsQueryable();

            if (filter.StatusExame.HasValue)
                query = query.Where(x => x.StatusExame == filter.StatusExame);

            if (filter.ServicoLaboratorioId.HasValue)
                query = query.Where(x => x.servicoLaboratorioId == filter.ServicoLaboratorioId);

            if (filter.InstituicaoId.HasValue)
                query = query.Where(x => x.InstituicaoId == filter.InstituicaoId);

            if (filter.DataInicial.HasValue)
                query = query.Where(x => x.DataMarcada >= filter.DataInicial.Value);

            if (filter.DataFinal.HasValue)
                query = query.Where(x => x.DataMarcada <= filter.DataFinal.Value);

            query = query.OrderByDescending(x => x.DataMarcada);

            var resultado = await query
                .Select(x => new ExameGradePacienteModel
                {
                    Id = x.Id,
                    DataMarcada = x.DataMarcada,
                    StatusExame = x.StatusExame,
                    NomeServicoLaboratorio = x.servicoLaboratorio!.Titulo,
                    NomeProfissionalSaude = x.ProfissionalSaude!.Nome + " " + x.ProfissionalSaude.Sobrenome,
                    NomeInstituicao = x.Instituicao != null ? x.Instituicao.RazaoSocial : null
                })
                .ToPaged(filter);

            return resultado;
        }
    }
}
