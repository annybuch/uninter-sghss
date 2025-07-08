using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Loop.SGHSS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ultima : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sobrenome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CPF = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Genero = table.Column<int>(type: "int", nullable: true),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logradouro = table.Column<int>(type: "int", nullable: true),
                    Bairro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CEP = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CargoAdm = table.Column<int>(type: "int", nullable: true),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoriasSuprimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasSuprimentos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Especializacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especializacoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NomeFantasia = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazaoSocial = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CNPJ = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IntervaloMinutos = table.Column<int>(type: "int", nullable: true),
                    TipoInstituicao = table.Column<int>(type: "int", nullable: true),
                    Cidade = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logradouro = table.Column<int>(type: "int", nullable: true),
                    Bairro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CEP = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sobrenome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CPF = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Genero = table.Column<int>(type: "int", nullable: true),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logradouro = table.Column<int>(type: "int", nullable: true),
                    Bairro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CEP = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permissoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codigo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Titulo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfissionaisSaude",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sobrenome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroRegistro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CPF = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Genero = table.Column<int>(type: "int", nullable: true),
                    CargoProfissionalSaude = table.Column<int>(type: "int", nullable: true),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logradouro = table.Column<int>(type: "int", nullable: true),
                    Bairro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CEP = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionaisSaude", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServicosLaboratorios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Recomendacao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicosLaboratorios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sobrenome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CPF = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Genero = table.Column<int>(type: "int", nullable: true),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CargoFuncionario = table.Column<int>(type: "int", nullable: true),
                    Cidade = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logradouro = table.Column<int>(type: "int", nullable: true),
                    Bairro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CEP = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InstituicaoAgenda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    HoraFim = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituicaoAgenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstituicaoAgenda_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InstituicoesEspecializacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EspecializacaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituicoesEspecializacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstituicoesEspecializacoes_Especializacoes_EspecializacaoId",
                        column: x => x.EspecializacaoId,
                        principalTable: "Especializacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstituicoesEspecializacoes_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Leitos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observacao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Andar = table.Column<int>(type: "int", nullable: true),
                    Sala = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroLeito = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusLeito = table.Column<int>(type: "int", nullable: true),
                    TipoLeito = table.Column<int>(type: "int", nullable: true),
                    InstutuicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leitos_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Suprimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CategoriaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suprimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suprimentos_CategoriasSuprimentos_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasSuprimentos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suprimentos_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissoesAdministrador",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PermissaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AdministradorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesAdministrador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissoesAdministrador_Administrador_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "Administrador",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissoesAdministrador_Permissoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissoesPaciente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PermissaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PacienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesPaciente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissoesPaciente_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissoesPaciente_Permissoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Anotacoes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataMarcada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Prescricao = table.Column<byte[]>(type: "longblob", nullable: true),
                    Receita = table.Column<byte[]>(type: "longblob", nullable: true),
                    GuiaMedico = table.Column<byte[]>(type: "longblob", nullable: true),
                    TipoConsulta = table.Column<int>(type: "int", nullable: false),
                    UrlSalaVideo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusConsulta = table.Column<int>(type: "int", nullable: false),
                    Notificacao2horas = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Notificacao24horas = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    StatusPagamento = table.Column<int>(type: "int", nullable: false),
                    FormaDePagamento = table.Column<int>(type: "int", nullable: false),
                    EspecializacaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ProfissionalSaudeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PacienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InstituicaoId1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultas_Especializacoes_EspecializacaoId",
                        column: x => x.EspecializacaoId,
                        principalTable: "Especializacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Consultas_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Consultas_Instituicoes_InstituicaoId1",
                        column: x => x.InstituicaoId1,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Consultas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Consultas_ProfissionaisSaude_ProfissionalSaudeId",
                        column: x => x.ProfissionalSaudeId,
                        principalTable: "ProfissionaisSaude",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissoesProfissionaisSaude",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PermissaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ProfissionalSaudeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesProfissionaisSaude", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissoesProfissionaisSaude_Permissoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissoesProfissionaisSaude_ProfissionaisSaude_Profissional~",
                        column: x => x.ProfissionalSaudeId,
                        principalTable: "ProfissionaisSaude",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfissionaisSaudeEspecializacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EspecializacaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ProfissionalSaudeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EspecializacoesId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionaisSaudeEspecializacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfissionaisSaudeEspecializacao_Especializacoes_Especializa~",
                        column: x => x.EspecializacoesId,
                        principalTable: "Especializacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfissionaisSaudeEspecializacao_ProfissionaisSaude_Profissi~",
                        column: x => x.ProfissionalSaudeId,
                        principalTable: "ProfissionaisSaude",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfissionaisSaudeInstituicoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProfissionalSaudeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionaisSaudeInstituicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfissionaisSaudeInstituicoes_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfissionaisSaudeInstituicoes_ProfissionaisSaude_Profission~",
                        column: x => x.ProfissionalSaudeId,
                        principalTable: "ProfissionaisSaude",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfissionalSaudeAgenda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProfissionalSaudeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TipoConsulta = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    HoraFim = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    HoraInicioAlmoco = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    HoraFimAlmoco = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalSaudeAgenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfissionalSaudeAgenda_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfissionalSaudeAgenda_ProfissionaisSaude_ProfissionalSaude~",
                        column: x => x.ProfissionalSaudeId,
                        principalTable: "ProfissionaisSaude",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Anotacoes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataMarcada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    GuiaMedico = table.Column<byte[]>(type: "longblob", nullable: true),
                    Resultado = table.Column<byte[]>(type: "longblob", nullable: true),
                    StatusExame = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    StatusPagamento = table.Column<int>(type: "int", nullable: false),
                    FormaDePagamento = table.Column<int>(type: "int", nullable: false),
                    servicoLaboratorioId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ProfissionalSaudeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PacienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exames_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exames_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exames_ProfissionaisSaude_ProfissionalSaudeId",
                        column: x => x.ProfissionalSaudeId,
                        principalTable: "ProfissionaisSaude",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exames_ServicosLaboratorios_servicoLaboratorioId",
                        column: x => x.servicoLaboratorioId,
                        principalTable: "ServicosLaboratorios",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InstituicoesServicosLaboratorio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ServicosLaboratorioId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituicoesServicosLaboratorio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstituicoesServicosLaboratorio_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstituicoesServicosLaboratorio_ServicosLaboratorios_Servico~",
                        column: x => x.ServicosLaboratorioId,
                        principalTable: "ServicosLaboratorios",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProfissionaisSaudeServicosLaboratorio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProfissionalSaudeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ServicosLaboratorioId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionaisSaudeServicosLaboratorio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfissionaisSaudeServicosLaboratorio_ProfissionaisSaude_Pro~",
                        column: x => x.ProfissionalSaudeId,
                        principalTable: "ProfissionaisSaude",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfissionaisSaudeServicosLaboratorio_ServicosLaboratorios_S~",
                        column: x => x.ServicosLaboratorioId,
                        principalTable: "ServicosLaboratorios",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissoesFuncionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PermissaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FuncionarioId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesFuncionarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissoesFuncionarios_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissoesFuncionarios_Permissoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LeitoPacienteLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdOriginal = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DataEntrada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataSaida = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LeitoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PacienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeitoPacienteLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeitoPacienteLog_Leitos_LeitoId",
                        column: x => x.LeitoId,
                        principalTable: "Leitos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeitoPacienteLog_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LeitosPacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataEntrada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LeitoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PacienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeitosPacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeitosPacientes_Leitos_LeitoId",
                        column: x => x.LeitoId,
                        principalTable: "Leitos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeitosPacientes_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SuprimentosCompras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marca = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorPago = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    QuantidadeComprada = table.Column<int>(type: "int", nullable: true),
                    QuantidadeSaida = table.Column<int>(type: "int", nullable: true),
                    DataComprada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SuprimentoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InstituicaoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuprimentosCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuprimentosCompras_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuprimentosCompras_Suprimentos_SuprimentoId",
                        column: x => x.SuprimentoId,
                        principalTable: "Suprimentos",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LeitoPacienteObservacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LeitosPacientesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Observacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Leito_PacienteLogId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SysIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SysUserInsert = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysUserUpdate = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SysDInsert = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SysDUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeitoPacienteObservacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeitoPacienteObservacao_LeitoPacienteLog_Leito_PacienteLogId",
                        column: x => x.Leito_PacienteLogId,
                        principalTable: "LeitoPacienteLog",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeitoPacienteObservacao_LeitosPacientes_LeitosPacientesId",
                        column: x => x.LeitosPacientesId,
                        principalTable: "LeitosPacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Codigo", "Descricao", "SysDInsert", "SysDUpdate", "SysIsDeleted", "SysUserInsert", "SysUserUpdate", "Titulo" },
                values: new object[,]
                {
                    { new Guid("0a125da8-1110-41d5-a48e-eb2a9830ccf7"), "C09", "Permite ao profissional gerenciar sua própria disponibilidade e horários de atendimento.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar dias e horários de atendimento" },
                    { new Guid("0a2d493b-d0ae-4e37-8771-b0e9e0ef38f6"), "A08", "Permite criar, editar e excluir especializações médicas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar especializações" },
                    { new Guid("0c4b4bd8-70ed-4103-a97a-f402e9cbd396"), "E02", "Permite ao paciente cancelar suas próprias consultas agendadas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Cancelar consulta" },
                    { new Guid("0f3905f2-c752-4889-8ad9-9cf7c7eab1e1"), "A01", "Permite visualizar detalhes de todas as instituições no sistema.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar todas as instituições" },
                    { new Guid("164092f9-7031-4c1e-821c-762441c2e1bc"), "C02", "Permite realizar consultas presenciais.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Atender consulta presencial" },
                    { new Guid("203f2184-0c76-454d-8072-825203621475"), "C06", "Permite solicitar exames laboratoriais ou outros procedimentos.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Solicitar exame" },
                    { new Guid("244d7fa7-25fa-4004-aa27-acd4e026018d"), "A04", "Permite a exclusão de instituições (uso restrito).", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Excluir instituição" },
                    { new Guid("2c033328-2a48-4214-885c-297c766f3009"), "E08", "Permite ao paciente visualizar e baixar suas próprias prescrições e resultados de exames.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar e baixar prescrições e exames" },
                    { new Guid("33adc69b-87c5-4939-861f-1dc9c9fee789"), "D02", "Permite agendar exames para pacientes.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Agendar exame para paciente" },
                    { new Guid("37dd6ec0-9f78-429a-8e88-ab0eb27f9c70"), "E05", "Permite visualizar o histórico de agendamentos de consultas e exames.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar histórico de agendamentos" },
                    { new Guid("3dc0c70a-035f-4a00-99dc-9382c0080df0"), "A06", "Permite visualizar todos os usuários e suas permissões atribuídas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar todos os usuários e suas permissões" },
                    { new Guid("4180ef67-0bca-4516-8a8f-ac9e4f1cb72a"), "D06", "Permite editar informações demográficas básicas de um paciente.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Editar dados do paciente" },
                    { new Guid("422ee0be-2675-4383-bd3f-ac8f1f0b7d7c"), "E07", "Permite ao paciente receber notificações do sistema por e-mail.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Receber notificações por e-mail" },
                    { new Guid("448b9609-4c30-4275-84fa-7594921ccef6"), "A07", "Permite acessar dashboards e análises de todo o sistema.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Acessar dashboard global (todas as instituições)" },
                    { new Guid("465664b9-608b-4920-b9df-2d41fcad2bf6"), "B05", "Permite modificar registros de profissionais de saúde.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Editar profissional de saúde" },
                    { new Guid("4b145763-a908-4075-b3fc-3e69af86f874"), "E03", "Permite ao paciente agendar seus próprios exames.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Agendar exame" },
                    { new Guid("566c3f4e-4a85-4ef2-8d98-a06e9128d616"), "A02", "Permite criar novas instituições.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Cadastrar nova instituição" },
                    { new Guid("59eb9ffd-8c72-49da-b85d-567ad6fc7a63"), "D05", "Permite registrar novos pacientes.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Cadastrar paciente" },
                    { new Guid("5a02cd5c-f09e-4942-b580-1585e0a39381"), "B09", "Permite gerenciar (registrar, editar, liberar) leitos hospitalares na instituição.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar leitos (cadastrar, editar, liberar)" },
                    { new Guid("5b2f9c7c-e70c-4381-a90e-6f7ede9fcc81"), "B03", "Permite modificar ou excluir registros de funcionários.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Editar/Excluir funcionário" },
                    { new Guid("5dedb2a0-00f1-40c1-9ca0-ba9d7f578f03"), "E01", "Permite ao paciente agendar suas próprias consultas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Agendar consulta" },
                    { new Guid("6549472f-ed67-4ecb-a7fe-2d9095cd2ac8"), "D04", "Permite visualizar o histórico geral de pacientes (informações básicas).", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar histórico de pacientes" },
                    { new Guid("79fe4c03-fa6b-4551-bb7b-e6cd3f676612"), "D09", "Permite atribuir pacientes a leitos e gerenciar a ocupação de leitos.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar leitos (cadastrar, associar paciente)" },
                    { new Guid("805c3ae1-df4e-4e0b-9edb-45b9dda392b4"), "B10", "Permite visualizar a agenda de agendamentos completa da instituição.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar agenda da instituição" },
                    { new Guid("82b40e9c-d00f-4fc4-b0cf-6b6b468b2f18"), "D01", "Permite agendar consultas para pacientes.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Agendar consulta para paciente" },
                    { new Guid("860e8e66-7b42-4f17-b991-0e4ac9667da7"), "C07", "Permite visualizar o histórico médico de pacientes atendidos pelo profissional.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar histórico de pacientes atendidos" },
                    { new Guid("8a015a50-04b4-4fcb-b29a-da37da226811"), "USUARIO_EDITAR_PERFIL_PROPRIO", "Permite ao usuário editar suas próprias informações de perfil (nome, telefone, etc.).", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Editar Meu Perfil" },
                    { new Guid("8b2ba859-e8ed-4b0c-bc5e-c84ddb8d3b4e"), "B04", "Permite registrar novos profissionais de saúde para a instituição.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Cadastrar profissional de saúde" },
                    { new Guid("8b375021-91d7-4ba6-bee2-df20467e435d"), "B08", "Permite acessar dashboards relevantes para a instituição específica.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar dashboard institucional" },
                    { new Guid("8c176c7f-c208-400a-b4ea-39fdb9540b82"), "C08", "Permite anexar arquivos PDF a registros de pacientes ou outras seções relevantes.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Anexar arquivos PDF" },
                    { new Guid("9cd9c283-37e0-4e45-a485-687eb338b32d"), "C05", "Permite emitir prescrições médicas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Emitir prescrição" },
                    { new Guid("9e727c46-d3ed-41f4-8cf3-9115963fa47c"), "A03", "Permite modificar detalhes de instituições existentes.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Editar instituição" },
                    { new Guid("a54628f4-a99d-4aa8-8a52-36b59cbc9a0a"), "C04", "Permite adicionar novas entradas no prontuário médico do paciente.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Inserir prontuário" },
                    { new Guid("a994973d-20e9-4358-9cf9-2a8d022572fd"), "C03", "Permite realizar teleconsultas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Atender teleconsulta" },
                    { new Guid("aab4f2d2-2a09-41f9-8c51-0ce7879ce007"), "E04", "Permite ao paciente cancelar seus próprios exames agendados.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Cancelar exame" },
                    { new Guid("ad7c3f83-b2e3-4a17-b68d-8e2ae55a7655"), "B02", "Permite registrar novos funcionários na instituição.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Cadastrar funcionário" },
                    { new Guid("b4611b48-939e-4b5f-843c-3846c979ba9f"), "B06", "Permite vincular profissionais a especializações ou instituições específicas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Associar profissionais a especializações/instituições" },
                    { new Guid("b68a0499-dc85-4fc9-8d7d-b1c40ccaf079"), "C01", "Permite ao profissional visualizar sua própria agenda de agendamentos.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar agenda pessoal" },
                    { new Guid("b7b5e90a-4a9e-4dcd-8200-125c564d8f13"), "B07", "Permite o gerenciamento completo dos suprimentos da instituição.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar suprimentos da instituição" },
                    { new Guid("c33313ce-0bbc-49c5-8387-d7cec7ccbf5b"), "D03", "Permite visualizar a agenda de agendamentos completa da instituição.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar agenda da instituição" },
                    { new Guid("c5ffb0ce-bcd0-4912-a908-d7af75a5a865"), "B01", "Permite visualizar detalhes da instituição do próprio administrador.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar dados da própria instituição" },
                    { new Guid("d3e4c6c8-70f1-4a24-8da8-8cfbf3efb70d"), "D08", "Permite acessar dashboards relevantes para a instituição.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar dashboards da instituição" },
                    { new Guid("e8e4f82f-7230-4186-9d4a-041bf8b38abe"), "A05", "Permite gerenciar permissões para qualquer tipo de usuário.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar permissões de usuários" },
                    { new Guid("e91e6715-6e9f-41b3-98c3-5694a3b47c16"), "USUARIO_MUDAR_SENHA_PROPRIA", "Permite ao usuário alterar a sua própria senha de acesso.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Mudar Minha Senha" },
                    { new Guid("f5de3541-d0f2-406b-a8c2-c6552b9d8121"), "USUARIO_RESETAR_SENHA_TERCEIRO", "Permite a redefinição de senha para outros usuários (Funcionários, Profissionais, Pacientes, etc.).", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Resetar Senha de Outro Usuário" },
                    { new Guid("f7236c11-e963-49b5-b457-9202385e7cc8"), "E06", "Permite ao paciente acessar teleconsultas agendadas.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Acessar teleconsulta" },
                    { new Guid("f8b5febc-d326-4c2e-9573-a6edb0eee675"), "USUARIO_VISUALIZAR_PERFIL_PROPRIO", "Permite ao usuário visualizar suas próprias informações de perfil.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Visualizar Meu Perfil" },
                    { new Guid("f9cd7840-871e-4508-a3b3-a96f3ee4d6ce"), "A09", "Permite gerenciar (adicionar, editar, excluir) serviços de laboratório oferecidos.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar serviços laboratoriais" },
                    { new Guid("fd948e86-9220-4202-99a4-bcf37974df13"), "D07", "Permite gerenciar compras e registrar o consumo de suprimentos.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), "Gerenciar suprimentos (cadastro de compra, consumo)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_EspecializacaoId",
                table: "Consultas",
                column: "EspecializacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_InstituicaoId",
                table: "Consultas",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_InstituicaoId1",
                table: "Consultas",
                column: "InstituicaoId1");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_PacienteId",
                table: "Consultas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_ProfissionalSaudeId",
                table: "Consultas",
                column: "ProfissionalSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_InstituicaoId",
                table: "Exames",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_PacienteId",
                table: "Exames",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_ProfissionalSaudeId",
                table: "Exames",
                column: "ProfissionalSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_servicoLaboratorioId",
                table: "Exames",
                column: "servicoLaboratorioId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_InstituicaoId",
                table: "Funcionarios",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituicaoAgenda_InstituicaoId",
                table: "InstituicaoAgenda",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituicoesEspecializacoes_EspecializacaoId",
                table: "InstituicoesEspecializacoes",
                column: "EspecializacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituicoesEspecializacoes_InstituicaoId",
                table: "InstituicoesEspecializacoes",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituicoesServicosLaboratorio_InstituicaoId",
                table: "InstituicoesServicosLaboratorio",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituicoesServicosLaboratorio_ServicosLaboratorioId",
                table: "InstituicoesServicosLaboratorio",
                column: "ServicosLaboratorioId");

            migrationBuilder.CreateIndex(
                name: "IX_LeitoPacienteLog_LeitoId",
                table: "LeitoPacienteLog",
                column: "LeitoId");

            migrationBuilder.CreateIndex(
                name: "IX_LeitoPacienteLog_PacienteId",
                table: "LeitoPacienteLog",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_LeitoPacienteObservacao_Leito_PacienteLogId",
                table: "LeitoPacienteObservacao",
                column: "Leito_PacienteLogId");

            migrationBuilder.CreateIndex(
                name: "IX_LeitoPacienteObservacao_LeitosPacientesId",
                table: "LeitoPacienteObservacao",
                column: "LeitosPacientesId");

            migrationBuilder.CreateIndex(
                name: "IX_Leitos_InstituicaoId",
                table: "Leitos",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_LeitosPacientes_LeitoId",
                table: "LeitosPacientes",
                column: "LeitoId");

            migrationBuilder.CreateIndex(
                name: "IX_LeitosPacientes_PacienteId",
                table: "LeitosPacientes",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesAdministrador_AdministradorId",
                table: "PermissoesAdministrador",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesAdministrador_PermissaoId",
                table: "PermissoesAdministrador",
                column: "PermissaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesFuncionarios_FuncionarioId",
                table: "PermissoesFuncionarios",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesFuncionarios_PermissaoId",
                table: "PermissoesFuncionarios",
                column: "PermissaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesPaciente_PacienteId",
                table: "PermissoesPaciente",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesPaciente_PermissaoId",
                table: "PermissoesPaciente",
                column: "PermissaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesProfissionaisSaude_PermissaoId",
                table: "PermissoesProfissionaisSaude",
                column: "PermissaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesProfissionaisSaude_ProfissionalSaudeId",
                table: "PermissoesProfissionaisSaude",
                column: "ProfissionalSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionaisSaudeEspecializacao_EspecializacoesId",
                table: "ProfissionaisSaudeEspecializacao",
                column: "EspecializacoesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionaisSaudeEspecializacao_ProfissionalSaudeId",
                table: "ProfissionaisSaudeEspecializacao",
                column: "ProfissionalSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionaisSaudeInstituicoes_InstituicaoId",
                table: "ProfissionaisSaudeInstituicoes",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionaisSaudeInstituicoes_ProfissionalSaudeId",
                table: "ProfissionaisSaudeInstituicoes",
                column: "ProfissionalSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionaisSaudeServicosLaboratorio_ProfissionalSaudeId",
                table: "ProfissionaisSaudeServicosLaboratorio",
                column: "ProfissionalSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionaisSaudeServicosLaboratorio_ServicosLaboratorioId",
                table: "ProfissionaisSaudeServicosLaboratorio",
                column: "ServicosLaboratorioId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalSaudeAgenda_InstituicaoId",
                table: "ProfissionalSaudeAgenda",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalSaudeAgenda_ProfissionalSaudeId",
                table: "ProfissionalSaudeAgenda",
                column: "ProfissionalSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_Suprimentos_CategoriaId",
                table: "Suprimentos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Suprimentos_InstituicaoId",
                table: "Suprimentos",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SuprimentosCompras_InstituicaoId",
                table: "SuprimentosCompras",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SuprimentosCompras_SuprimentoId",
                table: "SuprimentosCompras",
                column: "SuprimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Exames");

            migrationBuilder.DropTable(
                name: "InstituicaoAgenda");

            migrationBuilder.DropTable(
                name: "InstituicoesEspecializacoes");

            migrationBuilder.DropTable(
                name: "InstituicoesServicosLaboratorio");

            migrationBuilder.DropTable(
                name: "LeitoPacienteObservacao");

            migrationBuilder.DropTable(
                name: "PermissoesAdministrador");

            migrationBuilder.DropTable(
                name: "PermissoesFuncionarios");

            migrationBuilder.DropTable(
                name: "PermissoesPaciente");

            migrationBuilder.DropTable(
                name: "PermissoesProfissionaisSaude");

            migrationBuilder.DropTable(
                name: "ProfissionaisSaudeEspecializacao");

            migrationBuilder.DropTable(
                name: "ProfissionaisSaudeInstituicoes");

            migrationBuilder.DropTable(
                name: "ProfissionaisSaudeServicosLaboratorio");

            migrationBuilder.DropTable(
                name: "ProfissionalSaudeAgenda");

            migrationBuilder.DropTable(
                name: "SuprimentosCompras");

            migrationBuilder.DropTable(
                name: "LeitoPacienteLog");

            migrationBuilder.DropTable(
                name: "LeitosPacientes");

            migrationBuilder.DropTable(
                name: "Administrador");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.DropTable(
                name: "Especializacoes");

            migrationBuilder.DropTable(
                name: "ServicosLaboratorios");

            migrationBuilder.DropTable(
                name: "ProfissionaisSaude");

            migrationBuilder.DropTable(
                name: "Suprimentos");

            migrationBuilder.DropTable(
                name: "Leitos");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "CategoriasSuprimentos");

            migrationBuilder.DropTable(
                name: "Instituicoes");
        }
    }
}
