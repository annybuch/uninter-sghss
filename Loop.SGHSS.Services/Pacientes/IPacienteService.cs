using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Pacientes;
using Loop.SGHSS.Model.PassWord;

namespace Loop.SGHSS.Services.Pacientes
{
    public interface IPacienteService
    {
        Task<PagedResult<ConsultaGradePacienteModel>> ObterConsultasPorPaciente(ConsultaPacienteQueryFilter filter);
        Task<PagedResult<ExameGradePacienteModel>> ObterExamesPorPaciente(ExameGradeQueryFilter filter);
        Task<PassModel> EditarSenhaPaciente(PassModel model);
        Task<EnderecoModel> EditarPacienteEndereco(EnderecoModel model);
        Task<PacientesModel> EditarPacienteGeral(PacientesModel model);
        Task<PacientesModel> BuscarPacientePorId(Guid id);
        Task<PagedResult<PacientesViewModel>> ObterPacientesPaginados(PacienteQueryFilter filter);
        Task<PacientesModel> CadastrarPaciente(PacientesModel model);
    }
}
