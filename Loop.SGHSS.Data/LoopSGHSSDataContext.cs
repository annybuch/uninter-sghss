using Loop.SGHSS.Data.Entities.Suprimento_Entity;
using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Adm_Entity;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.Consulta_Entity;
using Loop.SGHSS.Domain.Entities.Exame_Entity;
using Loop.SGHSS.Domain.Entities.Funcionario_Entity;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Leito_Entity;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Domain.Entities.Permissao_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Domain.Entities.Servicos_Entity;
using Loop.SGHSS.Domain.Entities.Suprimento_Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Loop.SGHSS.Data
{
    public class LoopSGHSSDataContext : DbContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoopSGHSSDataContext(DbContextOptions<LoopSGHSSDataContext> opt, IHttpContextAccessor httpContextAccessor) : base(opt)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // DbSets
        public DbSet<Administrador> Administrador { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        // Profissionais de saúde
        public DbSet<ProfissionalSaude> ProfissionaisSaude { get; set; }
        public DbSet<ProfissionalSaude_Especializacao> ProfissionaisSaudeEspecializacao { get; set; }
        public DbSet<ProfissionalSaude_ServicoLaboratorio> ProfissionaisSaudeServicosLaboratorio { get; set; }
        public DbSet<ProfissionalSaude_Instituicao> ProfissionaisSaudeInstituicoes { get; set; }
        public DbSet<ProfissionalSaude_Agenda> ProfissionalSaudeAgenda { get; set; }

        // Serviços prestados
        public DbSet<Especializacoes> Especializacoes { get; set; }
        public DbSet<ServicosLaboratorio> ServicosLaboratorios { get; set; }

        // Instituições
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Instituicao_Especializacao> InstituicoesEspecializacoes { get; set; }
        public DbSet<Instituicao_ServicosLaboratorio> InstituicoesServicosLaboratorio { get; set; }
        public DbSet<Instituicao_Agenda> InstituicaoAgenda { get; set; }

        // Consultas e Exames
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Exame> Exames { get; set; }

        // Leitos e Suprimentos
        public DbSet<Leito> Leitos { get; set; }
        public DbSet<Leito_Paciente> LeitosPacientes { get; set; }
        public DbSet<Leito_PacienteLog> LeitoPacienteLog { get; set; }
        public DbSet<LeitoPacienteObservacao> LeitoPacienteObservacao { get; set; }
        public DbSet<Suprimento> Suprimentos { get; set; }
        public DbSet<Suprimento_Compra> SuprimentosCompras { get; set; }
        public DbSet<CategoriaSuprimento> CategoriasSuprimentos { get; set; }

        // Permissões e Segurança
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<Permissao_Funcionario> PermissoesFuncionarios { get; set; }
        public DbSet<Permissao_ProfissionalSaude> PermissoesProfissionaisSaude { get; set; }
        public DbSet<Permissao_Administrador> PermissoesAdministrador { get; set; }
        public DbSet<Permissao_Paciente> PermissoesPaciente { get; set; }

        // -- Configurações de base de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connstr = "Server=82.29.60.8;port=3306;Database=dbloop_vidaplus;Uid=one;Pwd=qIH3v7aJEpZxnugYY5c9zMEqxwyAYSu@N(JB0NQ^";
                optionsBuilder.UseMySql(connstr, ServerVersion.AutoDetect(connstr));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Consulta>()
            .HasOne(x => x.Instituicao)
            .WithMany()
            .HasForeignKey(x => x.InstituicaoId)
            .IsRequired(false);

            modelBuilder.Entity<ProfissionalSaude_Agenda>()
           .HasOne(x => x.Instituicao)
           .WithMany()
           .HasForeignKey(x => x.InstituicaoId)
           .IsRequired(false);

            modelBuilder.Entity<Permissao>().HasData(


                // --== Comentado pois já foi realizado a Migration ==-- //

                // A. Permissões de Administração Geral (A01 - A99)
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A01", Titulo = "Visualizar todas as instituições", Descricao = "Permite visualizar detalhes de todas as instituições no sistema." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A02", Titulo = "Cadastrar nova instituição", Descricao = "Permite criar novas instituições." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A03", Titulo = "Editar instituição", Descricao = "Permite modificar detalhes de instituições existentes." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A04", Titulo = "Excluir instituição", Descricao = "Permite a exclusão de instituições (uso restrito)." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A05", Titulo = "Gerenciar permissões de usuários", Descricao = "Permite gerenciar permissões para qualquer tipo de usuário." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A06", Titulo = "Visualizar todos os usuários e suas permissões", Descricao = "Permite visualizar todos os usuários e suas permissões atribuídas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A07", Titulo = "Acessar dashboard global (todas as instituições)", Descricao = "Permite acessar dashboards e análises de todo o sistema." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A08", Titulo = "Gerenciar especializações", Descricao = "Permite criar, editar e excluir especializações médicas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "A09", Titulo = "Gerenciar serviços laboratoriais", Descricao = "Permite gerenciar (adicionar, editar, excluir) serviços de laboratório oferecidos." },

                // B. Administração Local (Instituição) (B01 - B99)
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B01", Titulo = "Visualizar dados da própria instituição", Descricao = "Permite visualizar detalhes da instituição do próprio administrador." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B02", Titulo = "Cadastrar funcionário", Descricao = "Permite registrar novos funcionários na instituição." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B03", Titulo = "Editar/Excluir funcionário", Descricao = "Permite modificar ou excluir registros de funcionários." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B04", Titulo = "Cadastrar profissional de saúde", Descricao = "Permite registrar novos profissionais de saúde para a instituição." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B05", Titulo = "Editar profissional de saúde", Descricao = "Permite modificar registros de profissionais de saúde." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B06", Titulo = "Associar profissionais a especializações/instituições", Descricao = "Permite vincular profissionais a especializações ou instituições específicas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B07", Titulo = "Gerenciar suprimentos da instituição", Descricao = "Permite o gerenciamento completo dos suprimentos da instituição." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B08", Titulo = "Visualizar dashboard institucional", Descricao = "Permite acessar dashboards relevantes para a instituição específica." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B09", Titulo = "Gerenciar leitos (cadastrar, editar, liberar)", Descricao = "Permite gerenciar (registrar, editar, liberar) leitos hospitalares na instituição." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "B10", Titulo = "Visualizar agenda da instituição", Descricao = "Permite visualizar a agenda de agendamentos completa da instituição." },

                // C. Profissionais de Saúde (C01 - C99)
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C01", Titulo = "Visualizar agenda pessoal", Descricao = "Permite ao profissional visualizar sua própria agenda de agendamentos." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C02", Titulo = "Atender consulta presencial", Descricao = "Permite realizar consultas presenciais." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C03", Titulo = "Atender teleconsulta", Descricao = "Permite realizar teleconsultas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C04", Titulo = "Inserir prontuário", Descricao = "Permite adicionar novas entradas no prontuário médico do paciente." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C05", Titulo = "Emitir prescrição", Descricao = "Permite emitir prescrições médicas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C06", Titulo = "Solicitar exame", Descricao = "Permite solicitar exames laboratoriais ou outros procedimentos." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C07", Titulo = "Visualizar histórico de pacientes atendidos", Descricao = "Permite visualizar o histórico médico de pacientes atendidos pelo profissional." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C08", Titulo = "Anexar arquivos PDF", Descricao = "Permite anexar arquivos PDF a registros de pacientes ou outras seções relevantes." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "C09", Titulo = "Gerenciar dias e horários de atendimento", Descricao = "Permite ao profissional gerenciar sua própria disponibilidade e horários de atendimento." },

                // D. Funcionários Administrativos (D01 - D99)
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D01", Titulo = "Agendar consulta para paciente", Descricao = "Permite agendar consultas para pacientes." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D02", Titulo = "Agendar exame para paciente", Descricao = "Permite agendar exames para pacientes." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D03", Titulo = "Visualizar agenda da instituição", Descricao = "Permite visualizar a agenda de agendamentos completa da instituição." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D04", Titulo = "Visualizar histórico de pacientes", Descricao = "Permite visualizar o histórico geral de pacientes (informações básicas)." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D05", Titulo = "Cadastrar paciente", Descricao = "Permite registrar novos pacientes." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D06", Titulo = "Editar dados do paciente", Descricao = "Permite editar informações demográficas básicas de um paciente." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D07", Titulo = "Gerenciar suprimentos (cadastro de compra, consumo)", Descricao = "Permite gerenciar compras e registrar o consumo de suprimentos." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D08", Titulo = "Visualizar dashboards da instituição", Descricao = "Permite acessar dashboards relevantes para a instituição." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "D09", Titulo = "Gerenciar leitos (cadastrar, associar paciente)", Descricao = "Permite atribuir pacientes a leitos e gerenciar a ocupação de leitos." },

                // E. Pacientes (E01 - E99)
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E01", Titulo = "Agendar consulta", Descricao = "Permite ao paciente agendar suas próprias consultas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E02", Titulo = "Cancelar consulta", Descricao = "Permite ao paciente cancelar suas próprias consultas agendadas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E03", Titulo = "Agendar exame", Descricao = "Permite ao paciente agendar seus próprios exames." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E04", Titulo = "Cancelar exame", Descricao = "Permite ao paciente cancelar seus próprios exames agendados." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E05", Titulo = "Visualizar histórico de agendamentos", Descricao = "Permite visualizar o histórico de agendamentos de consultas e exames." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E06", Titulo = "Acessar teleconsulta", Descricao = "Permite ao paciente acessar teleconsultas agendadas." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E07", Titulo = "Receber notificações por e-mail", Descricao = "Permite ao paciente receber notificações do sistema por e-mail." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "E08", Titulo = "Visualizar e baixar prescrições e exames", Descricao = "Permite ao paciente visualizar e baixar suas próprias prescrições e resultados de exames." },

                // Permissões de Segurança e Perfil do Usuário (gerais)
                //new Permissao { Id = Guid.NewGuid(), Codigo = "USUARIO_MUDAR_SENHA_PROPRIA", Titulo = "Mudar Minha Senha", Descricao = "Permite ao usuário alterar a sua própria senha de acesso." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "USUARIO_RESETAR_SENHA_TERCEIRO", Titulo = "Resetar Senha de Outro Usuário", Descricao = "Permite a redefinição de senha para outros usuários (Funcionários, Profissionais, Pacientes, etc.)." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "USUARIO_EDITAR_PERFIL_PROPRIO", Titulo = "Editar Meu Perfil", Descricao = "Permite ao usuário editar suas próprias informações de perfil (nome, telefone, etc.)." },
                //new Permissao { Id = Guid.NewGuid(), Codigo = "USUARIO_VISUALIZAR_PERFIL_PROPRIO", Titulo = "Visualizar Meu Perfil", Descricao = "Permite ao usuário visualizar suas próprias informações de perfil." }
            );
        }

        private void AplicarInformacoes()
        {
            var now = DateTime.UtcNow;
            Guid? userId = null;

            if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity claimsIdentity &&
                claimsIdentity.IsAuthenticated)
            {
                var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid parsedUserId))
                {
                    userId = parsedUserId;
                }
            }

            foreach (var entry in ChangeTracker.Entries<DefaultEntityIdModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(nameof(DefaultEntityIdModel.SysUserInsert)).CurrentValue = userId ?? Guid.Empty;
                        entry.Property(nameof(DefaultEntityIdModel.SysDInsert)).CurrentValue = now;
                        entry.Property(nameof(DefaultEntityIdModel.SysUserUpdate)).CurrentValue = userId ?? Guid.Empty;
                        entry.Property(nameof(DefaultEntityIdModel.SysDUpdate)).CurrentValue = now;
                        entry.Property(nameof(DefaultEntityIdModel.SysIsDeleted)).CurrentValue = false;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(DefaultEntityIdModel.SysUserUpdate)).CurrentValue = userId ?? Guid.Empty;
                        entry.Property(nameof(DefaultEntityIdModel.SysDUpdate)).CurrentValue = now;
                        entry.Property(nameof(DefaultEntityIdModel.SysUserInsert)).IsModified = false;
                        entry.Property(nameof(DefaultEntityIdModel.SysDInsert)).IsModified = false;
                        entry.Property(nameof(DefaultEntityIdModel.SysIsDeleted)).IsModified = false;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            AplicarInformacoes();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AplicarInformacoes();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AplicarInformacoes();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AplicarInformacoes();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
