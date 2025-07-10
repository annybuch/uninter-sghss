using Loop.SGHSS.Data;
using Loop.SGHSS.Extensions.Exceptions;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Extensions.Seguranca;
using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Adm;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Services.Permissoes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Administrador
{
    public class AdministradorService : IAdministradorService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPermissaoService _permissaoService;

        public AdministradorService(LoopSGHSSDataContext dbContext, IMapper mapper, IPermissaoService permissaoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _permissaoService = permissaoService;
        }

        /// <summary>
        /// Cadastrar um novo administrador, seja local (de uma instituição) ou geral (da VidaPlus)
        /// </summary>
        public async Task CadastrarAdministrador(AdministradorModel model)
        {
            // --== Verifica se já existe com esse CPF.
            bool jaExiste = await _dbContext.Administrador
                .AnyAsync(x => x.CPF == model.CPF);
            if (jaExiste)
                throw new SGHSSBadRequestException("Adm já cadastrado no sistema.");

            // --== Gerar novo ID.
            model.Id = Guid.NewGuid();

            // --== Validar e gerar hash da senha
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                throw new SGHSSBadRequestException("Senha é obrigatória para o adm.");

            model.PasswordHash = PasswordHelper.GerarHashSenha(model.PasswordHash!);

            // --== Mapear e persistir
            var entidade = _mapper.Map<Domain.Entities.Adm_Entity.Administrador>(model);

            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();

            bool isGlobalAdmin = (model.CargoAdm == CargoFuncionario.AdmGeral);

            // --== ATRIBUIR PERMISSÕES PADRÃO AO NOVO ADMINISTRADOR
            await _permissaoService.AtribuirPemissaoPadraoAdministrador(entidade.Id, isGlobalAdmin);
        }

        /// <summary>
        /// Obter administradores paginados.
        /// </summary>
        public async Task<PagedResult<AdministradorViewModel>> ObterAdmPaginados(AdministradorQueryFilter filter)
        {
            var query = _dbContext.Administrador.Where(x => !x.SysIsDeleted).AsQueryable();

            var resultado = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<AdministradorViewModel>(x))
                .ToPaged(filter);

            return resultado;
        }

        /// <summary>
        /// Buscar Adm por Id.
        /// </summary>
        public async Task<AdministradorModel> BuscarAdmPorId(Guid id)
        {
            var entidade = await _dbContext.Administrador.FindAsync(id)
                ?? throw new SGHSSBadRequestException("Adm não encontrado.");

            return _mapper.Map<AdministradorModel>(entidade);
        }

        /// <summary>
        /// Editar dados gerais do adm.
        /// </summary>
        public async Task<AdministradorModel> EditarAdmGeral(AdministradorModel model)
        {
            var entidade = _dbContext.Administrador
                .FirstOrDefault(x => x.Id == model.Id)
                ?? throw new SGHSSBadRequestException("Adm não encontrado.");

            entidade.AtualizarGeral(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar endereço do adm.
        /// </summary>
        public async Task<EnderecoModel> EditarAdmEndereco(EnderecoModel model)
        {
            var entidade = _dbContext.Administrador
                .FirstOrDefault(x => x.Id == model.Id)
                ?? throw new SGHSSBadRequestException("Adm não encontrado.");

            entidade.AtualizarEndereco(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar senha do adm.
        /// </summary>
        public async Task<PassModel> EditarSenhaAdm(PassModel model)
        {
            var entidade = _dbContext.Administrador
                .FirstOrDefault(x => x.Id == model.Id)
                ?? throw new SGHSSBadRequestException("Adm não encontrado.");

            if (string.IsNullOrWhiteSpace(model.PasswordHash) || model.PasswordHash.Length < 6)
                throw new SGHSSBadRequestException("A senha deve ter pelo menos 6 caracteres.");

            if (PasswordHelper.VerificarSenha(model.PasswordHash, entidade.PasswordHash))
                throw new SGHSSBadRequestException("A nova senha não pode ser igual à senha anterior.");

            var novaSenhaHash = PasswordHelper.GerarHashSenha(model.PasswordHash);

            model.PasswordHash = novaSenhaHash;

            entidade.AtualizarSenha(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
