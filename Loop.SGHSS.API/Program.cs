using Loop.SGHSS.Data;
using Loop.SGHSS.Extensions.File;
using Loop.SGHSS.Model._Configuration;
using Loop.SGHSS.Services.Administrador;
using Loop.SGHSS.Services.Funcionarios;
using Loop.SGHSS.Services.Identidade;
using Loop.SGHSS.Services.Instituicoes;
using Loop.SGHSS.Services.Leitos;
using Loop.SGHSS.Services.Monitoramento;
using Loop.SGHSS.Services.Pacientes;
using Loop.SGHSS.Services.Permissoes;
using Loop.SGHSS.Services.Profissionais_Saude;
using Loop.SGHSS.Services.Servicos_Prestados.Consultas;
using Loop.SGHSS.Services.Servicos_Prestados.Especializacoes;
using Loop.SGHSS.Services.Servicos_Prestados.Exames;
using Loop.SGHSS.Services.Servicos_Prestados.Laboratorio;
using Loop.SGHSS.Services.Suprimentos;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --== Vincula configurações da appsettings.json à classe de configuração.
builder.Services.Configure<LoopSGHSSConfiguration>(builder.Configuration);

// --== Lê as configurações do JWT no appsettings.json.
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("A chave JWT (Jwt:Key) não está configurada.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("O Issuer JWT (Jwt:Issuer) não está configurado.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("O Audience JWT (Jwt:Audience) não está configurado.");

// --== Configura autenticação com JWT.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero 
    };

    // --== Personalização de mensagens para erros de autenticação/autorização.
    options.Events = new JwtBearerEvents
    {
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                mensagem = "Você não tem permissão para executar esta ação (policy requerida)."
            });
            return context.Response.WriteAsync(result);
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                mensagem = "Token inválido ou ausente. Acesso negado."
            });
            return context.Response.WriteAsync(result);
        }
    };
});

// --== Configura autorização com base em policies
builder.Services.AddAuthorization(options =>
{
    // --== Lista de permissões utilizadas no sistema
    var allSystemPermissions = new List<string>
    {
        "A08", "B08", "E01", "E05", "C02", "C03",
        "C08", "A02", "A03", "A04", "A05", "A06", "A07", "A09",
        "B01", "B02", "B03", "B04", "B05", "B06", "B07", "B09", "B10",
        "C01", "C04", "C05", "C06", "C07", "C09",
        "D01", "D02", "D03", "D04", "D05", "D06", "D07", "D08", "D09",
        "E02", "E03", "E04", "E06", "E07", "E08", "A01",
        "USUARIO_RESETAR_SENHA_TERCEIRO",
        "USUARIO_VISUALIZAR_PERFIL_PROPRIO",
        "USUARIO_MUDAR_SENHA_PROPRIA",
        "USUARIO_EDITAR_PERFIL_PROPRIO"
    };

    // --== Cria uma policy para cada permissão
    foreach (var permission in allSystemPermissions)
    {
        options.AddPolicy(permission, policy => policy.RequireClaim("Permission", permission));
    }
});

// --== Configura Swagger (documentação da API)
builder.Services.AddSwaggerGen(c =>
{
    // --== Inclui comentários XML no Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // --== Define informações da API
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Loop SGHSS API - VidaPlus",
        Description = "API RESTful desenvolvida para gestão hospitalar do SGHSS " +
        "(Sistema de Gestão Hospitalar e Saúde Suplementar). Esta API oferece funcionalidades " +
        "completas para gerenciamento de instituições, pacientes, profissionais de saúde, consultas " +
        "(presenciais e teleconsultas), leitos, suprimentos, permissões, exames e serviços laboratoriais. " +
        "Inclui recursos como videochamadas para teleconsultas, controle de estoque de suprimentos, administração " +
        "de leitos, agenda médica e emissão de documentos clínicos. A API foi desenvolvida com .NET 8, Entity Framework Core, " +
        "MySQL, Mapster e Swagger para documentação, garantindo escalabilidade, segurança e alta performance para operações de saúde."
    });

    // --== Adiciona segurança via token Bearer ao Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Insira 'Bearer' [espaço] e seu token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // --== Requer o token em todas as operações do Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    // --== Suporte a upload de arquivos
    c.OperationFilter<FileUploadOperationFilter>();

    // --== Suporte ao tipo IFormFile no Swagger
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    // --== Suporte a tipos não anuláveis
    c.SupportNonNullableReferenceTypes();
});

// --== Recupera configurações personalizadas (ex: strings de conexão, etc.)
var _config = builder.Configuration.Get<LoopSGHSSConfiguration>();

// --== Configura o contexto do Entity Framework com MySQL
builder.Services.AddDbContext<LoopSGHSSDataContext>(options =>
{
    var connectionString = builder.Configuration.GetSection("ConnectionStrings:ConnectionMySQL").Value
                           ?? throw new InvalidOperationException("A string de conexão MySQL não está configurada.");

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// --== Configura o Mapster para mapeamento automático entre modelos/DTOs
TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMapster();

// --== Registro dos serviços da aplicação (injeção de dependência)
builder.Services.AddScoped<IInstituicaoService, InstituicaoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IMonitoramentoService, MonitoramentoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IProfissionalSaudeService, ProfissionalSaudeService>();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<ILeitoService, LeitoService>();
builder.Services.AddScoped<ISuprimentoService, SuprimentoService>();
builder.Services.AddScoped<IEspecializacaoService, EspecializacaoService>();
builder.Services.AddScoped<ILaboratorioService, LaboratorioService>();
builder.Services.AddScoped<IExameService, ExameService>();
builder.Services.AddScoped<ITeleConsultaService, TeleConsultaService>();
builder.Services.AddScoped<IPermissaoService, PermissaoService>();
builder.Services.AddScoped<IIdentidadeService, IdentidadeService>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();

// --== Suporte para chamadas HTTP externas e contexto da requisição atual
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// --== Testes de integração
public partial class Program { }
