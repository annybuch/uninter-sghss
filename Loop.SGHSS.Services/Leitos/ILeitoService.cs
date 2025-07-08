using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Leitos;

namespace Loop.SGHSS.Services.Leitos
{
    public interface ILeitoService
    {
        Task<LeitoStatus> AtualizarStatusLeito(LeitoStatus model);
        Task<LeitosModel> EditarLeitosGeral(LeitosModel model);
        Task<LeitosModel?> ObterLeitoPorId(Guid id);
        Task<PagedResult<LeitosModel>> ObterLeitosPaginados(LeitoQueryFilter filter);
        Task CadastrarLeito(LeitosModel model);
        Task Cadastrar_Leito_Em_Massa(LeitosMassa model);
        Task<LeitosPacientesModel> AdicionarPacienteLeito(AddPacienteLeitoModel model);
        Task RemoverPacienteLeito(Guid? leitoId, Guid? pacienteId);
        Task<LeitosPacientesModel> AdicionarObservacoesPacienteLeito(AddPacienteLeitoModel model);
        Task<LeitoComPacienteModel?> ObterLeitoComPacienteAtual(Guid leitoId);
    }
}
