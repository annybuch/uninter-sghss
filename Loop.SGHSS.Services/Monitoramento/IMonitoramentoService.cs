using Loop.SGHSS.Model.Monitoramento.Financas;
using Loop.SGHSS.Model.Monitoramento.Recursos;

namespace Loop.SGHSS.Services.Monitoramento
{
    public interface IMonitoramentoService
    {
        Task<MonitoramentoFinancasModel> ObterFinancasDashboard(DateTime dataInicio, DateTime dataFim, Guid? InstituicaoId = null);
        Task<MonitoramentoSuprimentosModel> ObterMonitoramentoSuprimentos(DateTime dataInicio, DateTime dataFim, Guid? instituicaoId = null);
        Task<MonitoramentoLeitosModel> ObterMonitoramentoLeitos(Guid? instituicaoId = null);
    }
}
