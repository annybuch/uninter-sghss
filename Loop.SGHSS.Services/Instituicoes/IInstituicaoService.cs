using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Agenda;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Instituicoes;

namespace Loop.SGHSS.Services.Instituicoes
{
    public interface IInstituicaoService
    {
        Task CadastrarInstituicao(InstituicaoModel model);
        Task CadastrarAgendaInstituicao(InstituicaoAgendaModel model);
        Task<PagedResult<InstituicaoModel>> ObterInstituicoesPaginadas(InstituicaoQueryFilter filter);
        Task<InstituicaoModel?> BuscarInstituicaoPorId(Guid id);
        Task<EnderecoModel> EditarInstituicaoEndereco(EnderecoModel model);
        Task<InstituicaoModel> EditarInstituicaoGeral(InstituicaoModel model);
        Task DesvincularProfissional(Guid instituicaoId, Guid profissionalId);
        Task VincularProfissional(Guid instituicaoId, Guid profissionalId);
        Task DesvincularEspecializacao(Guid instituicaoId, Guid especializacaoId);
        Task VincularEspecializacao(Guid instituicaoId, Guid especializacaoId);
        Task DesvincularServicoLaboratorio(Guid instituicaoId, Guid servicoLaboratorioId);
        Task VincularServicoLaboratorio(Guid instituicaoId, Guid servicoLaboratorioId);
    }
}
