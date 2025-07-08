using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Funcionario_Entity;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Extensions.Seguranca;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Funcionarios;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Services.Permissoes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Funcionarios
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPermissaoService _permissaoService; 

        public FuncionarioService(LoopSGHSSDataContext dbContext, IMapper mapper, IPermissaoService permissaoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _permissaoService = permissaoService; 
        }

        /// <summary>
        /// Cadastrar uma novo funcionário.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarFuncionario(FuncionarioModel model)
        {
            // --== Verifica se já existe um funcionário com esse CPF
            bool existe = await _dbContext.Funcionarios.AnyAsync(item => item.CPF == model.CPF);

            if (existe)
                throw new Exception("Funcionário informado já está cadastrado no sistema.");

            if (model.InstituicaoId is null)
                throw new Exception("O funcionário precisa estar vinculado a uma instituição.");

            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            // --== Hash da senha
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                throw new Exception("Senha é obrigatória.");

            // Aqui usamos o PasswordHelper para gerar o hash
            model.PasswordHash = PasswordHelper.GerarHashSenha(model.PasswordHash!);

            var entidade = _mapper.Map<Funcionario>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();

            // --== ATRIBUIR PERMISSÕES PADRÃO AO NOVO FUNCIONÁRIO
            await _permissaoService.AtribuirPemissaoPadraoFuncionario(entidade.Id);
        }

        /// <summary>
        /// Listar todas os funcionários de forma paginada, podendo filtrar também por Instituição.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<FuncionarioViewModel>> ObterFuncionariosPaginadas(FuncionarioQueryFilter filter)
        {
            // --== Iniciando a query.
            var query = _dbContext.Funcionarios.AsQueryable();

            if (filter.HasFilters)
            {
                // --== Caso seja passado, executando os filtros.
                if (filter.InstituicaoId.HasValue)
                    query = query.Where(x => x.InstituicaoId == filter.InstituicaoId);
            }

            // --== Executando a query e paginação.
            var funcionarios = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<FuncionarioViewModel>(x))
                .ToPaged(filter);

            return funcionarios;
        }

        /// <summary>
        /// Buscar funcionário por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncionarioModel> BuscarFuncionarioPorId(Guid id)
        {
            var entidade = await _dbContext.Funcionarios.FindAsync(id)
                ?? throw new Exception("Funcionário não encontrado no sistema");

            return _mapper.Map<FuncionarioModel>(entidade);
        }

        /// <summary>
        /// Editar informações gerais do funcionário.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<FuncionarioModel> EditarFuncionarioGeral(FuncionarioModel model)
        {
            // --== Validando funcionário.
            Funcionario funcionario = _dbContext.Funcionarios.Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new Exception("Funcionário informadao não encontrado");

            // --== Atualizando entidade.
            funcionario.AtualizarGeral(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar apenas o endereço do funcionário.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EnderecoModel> EditarFuncionarioEndereco(EnderecoModel model)
        {
            // --== Obtendo funcionário.
            Funcionario funcionario = _dbContext.Funcionarios.Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new Exception("Funcionário informado não encontrado");

            // --== Atualizando entidade.
            funcionario.AtualizarEndereco(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <exception cref="Exception"></exception>
        /// <summary>
        /// Atualizar a senha de um funcionário
        /// </summary>
        public async Task<PassModel> EditarSenhaFuncionario(PassModel model)
        {
            // --== Buscar funcionário no banco.
            var funcionario = _dbContext.Funcionarios
                .FirstOrDefault(x => x.Id == model.Id)
                ?? throw new Exception("Funcionário informado não encontrado");

            // --== Verificar força da senha.
            if (string.IsNullOrWhiteSpace(model.PasswordHash) || model.PasswordHash.Length < 6)
                throw new Exception("A senha deve ter pelo menos 6 caracteres.");

            // --== Verificar se a nova senha é igual à atual.
            if (PasswordHelper.VerificarSenha(model.PasswordHash, funcionario.PasswordHash))
                throw new Exception("A nova senha não pode ser igual à senha anterior.");

            // --== Gerar novo hash seguro.
            var novaSenhaHash = PasswordHelper.GerarHashSenha(model.PasswordHash);

            // --== Atualiza o PasswordHash.
            model.PasswordHash = novaSenhaHash;

            // --== Atualizar senha na entidade.
            funcionario.AtualizarSenha(model);

            // --== Salvar no banco.
            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
 