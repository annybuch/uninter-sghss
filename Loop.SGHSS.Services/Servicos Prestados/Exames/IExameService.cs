using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Instituicoes;

namespace Loop.SGHSS.Services.Servicos_Prestados.Exames
{
    public interface IExameService
    {
        Task<List<InstituicaoModel>> BuscarInstituicoesComDisponibilidade(Guid servicoLaboratorioId);
        Task<List<ProfissionalComHorariosModel>> ObterProfissionaisComHorarios(Guid instituicaoId, Guid servicoLaboratorioId);
        Task<ExameModel> MarcarExame(ExameModel model);
        Task<ExameModel> IniciarExame(Guid exameId);
        Task<ExameModel> FinalizarExame(Guid exameId, string? anotacoes = null);
        Task AnexarResultado(Guid exameId, byte[] resultado);
        Task AnexarGuiaMedico(Guid exameId, byte[] guiaMedico);
        Task<ExameModel> CancelarExame(Guid exameId);
        Task<PagedResult<ExameGradeGeralModel>> ObterExamesGrade(ExameGradeQueryFilter filter);
        Task<List<PacienteComExamesModel>> ObterPacientesComExames(ExameGradeQueryFilter filter);
        Task<ExameModel?> BuscarExamePorId(Guid id);
    }
}
