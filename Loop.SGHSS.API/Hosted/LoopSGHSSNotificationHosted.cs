using Loop.SGHSS.Data;
using Loop.SGHSS.Model._Enums.Consulta;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Loop.SGHSS.Model._Configuration;

namespace Loop.SGHSS.API.Hosted
{
    public class LoopSGHSSNotificationHosted : BackgroundService
    {
        private readonly ILogger<LoopSGHSSNotificationHosted> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly LoopSGHSSConfiguration _config;

        public LoopSGHSSNotificationHosted(
            ILogger<LoopSGHSSNotificationHosted> logger,
            IServiceProvider serviceProvider,
            IOptions<LoopSGHSSConfiguration> config)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("✅ Serviço de notificação de consultas iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessarNotificacoes();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erro ao processar notificações.");
                }

                // --== Executa a cada 1 hora
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        private async Task ProcessarNotificacoes()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LoopSGHSSDataContext>();

            var agora = DateTime.Now;

            var consultas = await dbContext.Consultas
                .Include(c => c.Paciente)
                .Where(c =>
                    c.StatusConsulta == StatusConsultaEnum.Pendente &&
                    c.DataMarcada > agora
                )
                .ToListAsync();

            foreach (var consulta in consultas)
            {
                if (consulta.Paciente == null || string.IsNullOrWhiteSpace(consulta.Paciente.Email))
                {
                    _logger.LogWarning($"⚠️ Consulta {consulta.Id} sem e-mail do paciente.");
                    continue;
                }

                var faltamHoras = (consulta.DataMarcada - agora).TotalHours;

                // --== Notificação de 24 horas
                if (faltamHoras <= 24 && !consulta.Notificacao24horas.GetValueOrDefault())
                {
                    await EnviarNotificacao(consulta, "24 horas");
                    consulta.Notificacao24horas = true;
                    _logger.LogInformation($"📧 Notificação 24h enviada para {consulta.Paciente.Email}");
                }

                // --== Notificação de 2 horas
                if (faltamHoras <= 2 && !consulta.Notificacao2horas.GetValueOrDefault())
                {
                    await EnviarNotificacao(consulta, "2 horas");
                    consulta.Notificacao2horas = true;
                    _logger.LogInformation($"📧 Notificação 2h enviada para {consulta.Paciente.Email}");
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task EnviarNotificacao(dynamic consulta, string tipo)
        {
            var assunto = $"🔔 Lembrete de Consulta - {tipo} antes";
            var mensagem = MontarMensagemHtml(consulta, tipo);

            await EnviarEmailAsync(consulta.Paciente.Email, assunto, mensagem);
        }

        private async Task EnviarEmailAsync(string destinatario, string assunto, string mensagemHtml)
        {
            var smtp = new SmtpClient(_config.Email!.ServidorSmtp)
            {
                Port = _config.Email.Porta,
                Credentials = new NetworkCredential(_config.Email.Usuario, _config.Email.Senha),
                EnableSsl = true,
            };

            var mensagem = new MailMessage
            {
                From = new MailAddress(_config.Email.EmailRemetente, _config.Email.NomeRemetente),
                Subject = assunto,
                Body = mensagemHtml,
                IsBodyHtml = true,
            };

            mensagem.To.Add(destinatario);

            await smtp.SendMailAsync(mensagem);
        }

        private string MontarMensagemHtml(dynamic consulta, string tipo)
        {
            return $@"
                <h2>🔔 Lembrete de Consulta</h2>
                <p>Olá {consulta.Paciente?.Nome},</p>
                <p>Sua consulta está agendada para <b>{consulta.DataMarcada:dd/MM/yyyy HH:mm}</b>.</p>
                <p>Este é um lembrete enviado <b>{tipo}</b> antes da sua consulta.</p>
                <br/>
                <p><b>Equipe Vida+</b></p>
            ";
        }
    }
}
