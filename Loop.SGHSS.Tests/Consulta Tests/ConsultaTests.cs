using Loop.SGHSS.Services.Servicos_Prestados.Especializacoes;
using Loop.SGHSS.Services.Servicos_Prestados.Consultas;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model.Consultas;
using Microsoft.Extensions.DependencyInjection;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Tests.Factories;
using Loop.SGHSS.Services.Pacientes;

namespace Loop.SGHSS.Tests.Consulta_Tests
{
    public class ConsultaTests : IClassFixture<LoopTestFactory>
    {
        private readonly IEspecializacaoService _especializacaoService;
        private readonly IConsultaService _consultaService;
        private readonly IPacienteService _pacienteService;

        public ConsultaTests(LoopTestFactory factory)
        {
            // -- Cria escopo de dependências
            var scope = factory.Services.CreateScope();

            // -- Instancia serviços
            _consultaService = scope.ServiceProvider.GetRequiredService<IConsultaService>();
            _pacienteService = scope.ServiceProvider.GetRequiredService<IPacienteService>();
            _especializacaoService = scope.ServiceProvider.GetRequiredService<IEspecializacaoService>();
        }

        [Fact(DisplayName = "Fluxo feliz de agendamento de consulta e tentativa de conflito")]
        public async Task Deve_AgendarConsulta_E_NaoPermitirConflito()
        {
            // -- Busca especializações cadastradas
            var especializacoes = await _especializacaoService
                .ObterEspecializacoesPaginadas(new EspecializacoesQueryFilter());

            Assert.NotNull(especializacoes);
            Assert.True(especializacoes.List?.Any() == true, "Não há especializações cadastradas");

            // -- Seleciona a primeira especialização válida
            var esp = especializacoes.List!.First();

            // -- Busca profissionais com horários para TeleConsulta
            var disponiveis = await _consultaService
                .ObterProfissionaisComHorarios((Guid)esp.Id!, TipoConsultaEnum.TeleConsulta);

            Assert.NotNull(disponiveis);
            Assert.True(disponiveis.Any(), "Nenhum profissional disponível para consulta");

            // -- Seleciona o primeiro profissional e horário livre
            var profissional = disponiveis.First();
            var dia = profissional.HorariosDisponiveisPorData.Keys.First();
            var horario = profissional.HorariosDisponiveisPorData[dia].First();

            // -- Pega pacienteId de algum seed
            var pacienteId = Guid.NewGuid();

            // -- Monta modelo para agendamento
            var model = new MarcarConsultaModel
            {
                PacienteId = pacienteId,
                ProfissionalSaudeId = profissional.Id,
                EspecializacaoId = esp.Id,
                DataMarcada = dia.Date + horario,
                TipoConsulta = TipoConsultaEnum.TeleConsulta
            };

            // -- Agenda a consulta
            var consulta = await _consultaService.MarcarConsulta(model);

            Assert.NotNull(consulta);
            Assert.Equal(StatusConsultaEnum.Pendente, consulta.StatusConsulta);

            // -- Tenta marcar consulta no mesmo horário/profissional/paciente (espera Exception)
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _consultaService.MarcarConsulta(model);
            });
        }
    }
}
