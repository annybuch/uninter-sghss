using Loop.SGHSS.Domain.Entities.Consulta_Entity;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Instituicoes;

namespace Loop.SGHSS.Services.Servicos_Prestados.Consultas
{
    public interface IConsultaService 
    {
        Task<ConsultaModel> MarcarConsulta(MarcarConsultaModel model);
        Task<List<ProfissionalComHorariosModel>> ObterProfissionaisComHorarios(Guid especializacaoId, TipoConsultaEnum tipoConsulta, Guid? instituicaoId = null);
        Task<List<InstituicaoModel>> BuscarInstituicoesComDisponibilidade(Guid especializacaoId, TipoConsultaEnum tipoConsulta);
        Task AnexarPrescricao(Guid consultaId, byte[] prescricao);
        Task AnexarReceita(Guid consultaId, byte[] receita);
        Task<ConsultaModel> FinalizarConsulta(Guid consultaId, string? anotacoes = null);
        Task<ConsultaModel> IniciarConsulta(Guid consultaId);
        Task<string> GerarLinkDeAcessoPaciente(Guid consultaId, Guid pacienteId);
        Task AnexarGuiaMedico(Guid consultaId, byte[] guiaMedico);
        Task<ConsultaModel> CancelarConsulta(Guid consultaId);
        Task<PagedResult<ConsultaGradeGeralModel>> ObterConsultasPorPacienteEProfissional(ConsultaPacienteQueryFilter filter);
        Task<List<PacienteComConsultasModel>> ObterPacientesComConsultas(ConsultaPacienteQueryFilter filter);
        Task<ConsultaModel?> BuscarConsultaPorId(Guid id);
    }
}
