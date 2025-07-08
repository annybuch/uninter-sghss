using Loop.SGHSS.Data.Entities.Suprimento_Entity;
using Loop.SGHSS.Domain.Entities.Adm_Entity;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.Consulta_Entity;
using Loop.SGHSS.Domain.Entities.Exame_Entity;
using Loop.SGHSS.Domain.Entities.Funcionario_Entity;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Leito_Entity;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Domain.Entities.Servicos_Entity;
using Loop.SGHSS.Domain.Entities.Suprimento_Entity;
using Loop.SGHSS.Model.Adm;
using Loop.SGHSS.Model.Agenda;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Funcionarios;
using Loop.SGHSS.Model.Instituicoes;
using Loop.SGHSS.Model.Leitos;
using Loop.SGHSS.Model.Pacientes;
using Loop.SGHSS.Model.ProfissionaisSaude;
using Loop.SGHSS.Model.ServicosPrestados;
using Loop.SGHSS.Model.Suprimentos;
using Mapster;

namespace Loop.SGHSS.Domain._Mapper
{
    public class LoopSGHSSMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {


            config.NewConfig<Administrador, AdministradorModel>();
            config.NewConfig<AdministradorModel, Administrador>();
            config.NewConfig<Administrador, AdministradorViewModel>();
            config.NewConfig<AdministradorViewModel, Administrador>();


            // --== Instituições
            config.NewConfig<Instituicao, InstituicaoModel>();
            config.NewConfig<InstituicaoModel, Instituicao>();

            config.NewConfig<Instituicao_ServicosLaboratorio, InstituicaoServicosLaboratorioModel>();
            config.NewConfig<InstituicaoServicosLaboratorioModel, Instituicao_ServicosLaboratorio>();

            config.NewConfig<Instituicao_Especializacao, InstituicaoEspecializacaoModel>();
            config.NewConfig<InstituicaoEspecializacaoModel, Instituicao_Especializacao>();

            config.NewConfig<Instituicao, EnderecoModel>();
            config.NewConfig<EnderecoModel, Instituicao>();

            config.NewConfig<Instituicao_Agenda, InstituicaoAgendaModel>();
            config.NewConfig<InstituicaoAgendaModel, Instituicao_Agenda>();


            // --== Pacientes
            config.NewConfig<Paciente, PacientesModel>();
            config.NewConfig<PacientesModel, Paciente>();

            config.NewConfig<Paciente, EnderecoModel>();
            config.NewConfig<EnderecoModel, Paciente>();

            config.NewConfig<Paciente, PacientesViewModel>();
            config.NewConfig<PacientesViewModel, Paciente>();


            // --== Funcionários
            config.NewConfig<Funcionario, FuncionarioModel>();
            config.NewConfig<FuncionarioModel, Funcionario>();

            config.NewConfig<Funcionario, EnderecoModel>();
            config.NewConfig<EnderecoModel, Funcionario>();

            config.NewConfig<Funcionario, FuncionarioViewModel>();
            config.NewConfig<FuncionarioViewModel, Funcionario>();


            // --== Profissionais de saúde
            config.NewConfig<ProfissionalSaude, ProfissionalSaudeModel>();
            config.NewConfig<ProfissionalSaudeModel, ProfissionalSaude>();

            config.NewConfig<ProfissionalSaude, EnderecoModel>();
            config.NewConfig<EnderecoModel, ProfissionalSaude>();

            config.NewConfig<ProfissionalSaude, ProfissionalSaudeViewModel>();
            config.NewConfig<ProfissionalSaudeViewModel, ProfissionalSaude>();

            config.NewConfig<ProfissionalSaude_Especializacao, ProfissionalSaudeEspecializacaoModel>();
            config.NewConfig<ProfissionalSaudeEspecializacaoModel, ProfissionalSaude_Especializacao>();

            config.NewConfig<ProfissionalSaude_Instituicao, ProfissionalSaudeInstituicaoModel>();
            config.NewConfig<ProfissionalSaudeInstituicaoModel, ProfissionalSaude_Instituicao>();

            config.NewConfig<ProfissionalSaude_ServicoLaboratorio, ProfissionalSaudeServicoLaboratorioModel>();
            config.NewConfig<ProfissionalSaudeServicoLaboratorioModel, ProfissionalSaude_ServicoLaboratorio>();

            config.NewConfig<ProfissionalSaude_Agenda, ProfissionalSaudeAgendaModel>();
            config.NewConfig<ProfissionalSaudeAgendaModel, ProfissionalSaude_Agenda>();



            // --== Consultas
            config.NewConfig<Consulta, ConsultaModel>();
            config.NewConfig<ConsultaModel, Consulta>();
            config.NewConfig<Consulta, MarcarConsultaModel>();
            config.NewConfig<MarcarConsultaModel, Consulta>();


            // --== Exames
            config.NewConfig<Exame, ExameModel>();
            config.NewConfig<ExameModel, Exame>();


            // --== Serviços prestados
            config.NewConfig<Especializacoes, EspecializacoesModel>();
            config.NewConfig<EspecializacoesModel, Especializacoes>();

            config.NewConfig<ServicosLaboratorio, ServicosLaboratorioModel>();
            config.NewConfig<ServicosLaboratorioModel, ServicosLaboratorio>();


            // --== Recursos
            config.NewConfig<Leito, LeitosModel>();
            config.NewConfig<LeitosModel, Leito>();

            config.NewConfig<Leito_Paciente, LeitosPacientesModel>();
            config.NewConfig<LeitosPacientesModel, Leito_Paciente>();

            config.NewConfig<Leito, TicketCadastrarLeitoViewModel>();
            config.NewConfig<TicketCadastrarLeitoViewModel, Leito>();

            config.NewConfig<Leito_PacienteLog, LeitosPacientesLogModel>();
            config.NewConfig<LeitosPacientesLogModel, Leito_PacienteLog>();

            config.NewConfig<Leito, LeitosMassa>();
            config.NewConfig<LeitosMassa, Leito>();

            config.NewConfig<Leito, LeitoStatus>();
            config.NewConfig<LeitoStatus, Leito>();

            config.NewConfig<LeitoPacienteObservacao, LeitosPacientesObservacaoModel>();
            config.NewConfig<LeitosPacientesObservacaoModel, LeitoPacienteObservacao>();

            config.NewConfig<Suprimento, SuprimentosModel>();
            config.NewConfig<SuprimentosModel, Suprimento>();

            config.NewConfig<Suprimento_Compra, SuprimentosCompraModel>();
            config.NewConfig<SuprimentosCompraModel, Suprimento_Compra>();

            config.NewConfig<CategoriaSuprimento, CategoriaSuprimentosModel>();
            config.NewConfig<CategoriaSuprimentosModel, CategoriaSuprimento>();
        }
    }
}
