using Loop.SGHSS.Services.Servicos_Prestados.Especializacoes;
using Loop.SGHSS.Services.Servicos_Prestados.Consultas;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model.Consultas;
using Microsoft.Extensions.DependencyInjection;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Tests.Factories;
using Loop.SGHSS.Services.Pacientes;
using Loop.SGHSS.Model.Pacientes;
using Loop.SGHSS.Extensions.Exceptions;

namespace Loop.SGHSS.Tests.Consulta_Tests
{
    public class ConsultaTests : IClassFixture<LoopTestFactory>
    {
        private readonly IEspecializacaoService _especializacaoService;
        private readonly IConsultaService _consultaService;
        private readonly IPacienteService _pacienteService;

        public ConsultaTests(LoopTestFactory factory)
        {
            // --== Cria escopo de dependências
            var scope = factory.Services.CreateScope();

            // --== Instancia serviços
            _consultaService = scope.ServiceProvider.GetRequiredService<IConsultaService>();
            _pacienteService = scope.ServiceProvider.GetRequiredService<IPacienteService>();
            _especializacaoService = scope.ServiceProvider.GetRequiredService<IEspecializacaoService>();
        }

        [Fact(DisplayName = "Fluxo feliz de agendamento de consulta e tentativa de conflito")]
        public async Task Deve_AgendarConsulta_E_NaoPermitirConflito()
        {
            // --== Busca especializações cadastradas
            var especializacoes = await _especializacaoService
                .ObterEspecializacoesPaginadas(new EspecializacoesQueryFilter());

            Assert.NotNull(especializacoes);
            Assert.True(especializacoes.List?.Any() == true, "Não há especializações cadastradas");

            // --== Seleciona a primeira especialização válida
            var esp = especializacoes.List!.First();

            // --== Busca profissionais com horários para TeleConsulta
            var disponiveis = await _consultaService
                .ObterProfissionaisComHorarios((Guid)esp.Id!, TipoConsultaEnum.TeleConsulta);

            Assert.NotNull(disponiveis);
            Assert.True(disponiveis.Any(), "Nenhum profissional disponível para consulta");

            // --== Seleciona o primeiro profissional e horário livre
            var profissional = disponiveis.First();
            var dia = profissional.HorariosDisponiveisPorData.Keys.First();
            var horario = profissional.HorariosDisponiveisPorData[dia].First();

            // --== Cria paciente novo para o teste
            var pacienteId = await CriarPacienteAsync();

            // --== Monta modelo para agendamento
            var model = new MarcarConsultaModel
            {
                PacienteId = pacienteId,
                ProfissionalSaudeId = profissional.Id,
                EspecializacaoId = esp.Id,
                DataMarcada = dia.Date + horario,
                TipoConsulta = TipoConsultaEnum.TeleConsulta
            };

            // --== Agenda a consulta
            var consulta = await _consultaService.MarcarConsulta(model);

            Assert.NotNull(consulta);
            Assert.Equal(StatusConsultaEnum.Pendente, consulta.StatusConsulta);

            // --== Tenta marcar consulta no mesmo horário/profissional/paciente.
            await Assert.ThrowsAsync<SGHSSBadRequestException>(async () =>
            {
                await _consultaService.MarcarConsulta(model);
            });
        }


        // --== Cria paciente fake
        private async Task<Guid> CriarPacienteAsync()
        {
            var paciente = new PacientesModel
            {
                Nome = "Paciente Teste",
                Sobrenome = "Unitário",
                DataNascimento = DateTime.Today.AddYears(-25),
                Genero = Model._Enums.Genero.GeneroEnum.NaoDefinido,
                Telefone = "11999999999",
                Email = $"teste{Guid.NewGuid()}@teste.com",
                PasswordHash = "abc123**",
                CPF = GerarCpfFake() 
            };

            var criado = await _pacienteService.CadastrarPaciente(paciente);

            return criado.Id;
        }

        // --==  Cria consulta válida
        private async Task<ConsultaModel> CriarConsultaValidaAsync()
        {
            var especializacoes = await _especializacaoService
                .ObterEspecializacoesPaginadas(new EspecializacoesQueryFilter());

            var esp = especializacoes.List!.First();
            var disponiveis = await _consultaService
                .ObterProfissionaisComHorarios((Guid)esp.Id!, TipoConsultaEnum.TeleConsulta);

            var profissional = disponiveis.First();
            var dia = profissional.HorariosDisponiveisPorData.Keys.First();
            var horario = profissional.HorariosDisponiveisPorData[dia].First();

            var pacienteId = await CriarPacienteAsync();

            var model = new MarcarConsultaModel
            {
                PacienteId = pacienteId,
                ProfissionalSaudeId = profissional.Id,
                EspecializacaoId = esp.Id,
                DataMarcada = dia.Date + horario,
                TipoConsulta = TipoConsultaEnum.TeleConsulta
            };

            return await _consultaService.MarcarConsulta(model);
        }


        [Fact(DisplayName = "Iniciar consulta com sucesso")]
        public async Task Deve_IniciarConsulta_ComSucesso()
        {
            var consulta = await CriarConsultaValidaAsync();

            var iniciada = await _consultaService.IniciarConsulta((Guid)consulta.Id!);

            Assert.Equal(StatusConsultaEnum.EmAtendimento, iniciada.StatusConsulta);
            Assert.NotNull(iniciada.DataInicio);
        }


        [Fact(DisplayName = "Finalizar consulta com sucesso")]
        public async Task Deve_FinalizarConsulta_ComSucesso()
        {
            var consulta = await CriarConsultaValidaAsync();
            await _consultaService.IniciarConsulta((Guid)consulta.Id!);

            var finalizada = await _consultaService.FinalizarConsulta((Guid)consulta.Id!, "Finalizado via teste");

            Assert.Equal(StatusConsultaEnum.Finalizada, finalizada.StatusConsulta);
            Assert.NotNull(finalizada.DataFim);
            Assert.Equal("Finalizado via teste", finalizada.Anotacoes);
        }


        [Fact(DisplayName = "Cancelar consulta com sucesso")]
        public async Task Deve_CancelarConsulta_ComSucesso()
        {
            var consulta = await CriarConsultaValidaAsync();

            var cancelada = await _consultaService.CancelarConsulta((Guid)consulta.Id!);

            Assert.Equal(StatusConsultaEnum.Cancelada, cancelada.StatusConsulta);
        }

        private string GerarCpfFake()
        {
            var random = new Random();
            var cpf = new char[11];
            for (int i = 0; i < 11; i++)
            {
                cpf[i] = (char)('0' + random.Next(0, 10));
            }
            return new string(cpf);
        }

    }
}
