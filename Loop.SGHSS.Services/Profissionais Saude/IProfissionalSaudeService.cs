using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Agenda;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Model.ProfissionaisSaude;

namespace Loop.SGHSS.Services.Profissionais_Saude
{
    public interface IProfissionalSaudeService
    {
        Task<PassModel> EditarSenhaProfissional(PassModel model);
        Task<EnderecoModel> EditarProfissionalEndereco(EnderecoModel model);
        Task<ProfissionalSaudeModel> EditarProfissionalGeral(ProfissionalSaudeModel model);
        Task<ProfissionalSaudeModel> BuscarProfissionalPorId(Guid id);
        Task<PagedResult<ProfissionalSaudeViewModel>> ObterProfissionaisPaginados(ProfissionaisSaudeQueryFilter filter);
        Task CadastrarProfissional(ProfissionalSaudeModel model);
        Task<PagedResult<ConsultaGradeProfissionalSaudeModel>> ObterConsultasPorProfissional(ConsultaProfissionalQueryFilter filter);
        Task<PagedResult<ExameGradeProfissionalSaudeModel>> ObterExamesPorProfissionalSaude(ExameGradeQueryFilter filter);
        Task CadastrarAgendaProfissional(ProfissionalSaudeAgendaModel model);
        Task DesvincularEspecializacao(Guid profissionalId, Guid especializacaoId);
        Task VincularEspecializacao(Guid profissionalId, Guid especializacaoId);
        Task DesvincularServicoLaboratorio(Guid profissionalId, Guid servicoLaboratorioId);
        Task VincularServicoLaboratorio(Guid profissionalId, Guid servicoLaboratorioId);
    }
}
