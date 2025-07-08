using Loop.SGHSS.Data; 
using Loop.SGHSS.Extensions.Seguranca; 
using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model.Identidade; 
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Identidade
{
    public class IdentidadeService : IIdentidadeService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IConfiguration _configuration;

        public IdentidadeService(LoopSGHSSDataContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Tenta autenticar um usuário com um identificador e senha genéricos e gera um token JWT.
        /// </summary>
        public async Task<SGHSSAuthenticationTokenModel?> AuthenticateAndGenerateTokenAsync(SGHSSCredentialsModel credentials)
        {
            // --== Verifica se o identificador e a senha foram fornecidos,
            if (string.IsNullOrWhiteSpace(credentials.Identificacao) || string.IsNullOrWhiteSpace(credentials.Password))
                return null;

            SGHSSBaseUserTokenModel? userTokenData = null;

            // --==  1. Tentar autenticar como Administrador (geralmente por Email) --==
            var administrador = await _dbContext.Administrador
                .Include(a => a.PermissoesAdministrador!)
                    .ThenInclude(pa => pa.Permissao)
                .FirstOrDefaultAsync(a => a.Email == credentials.Identificacao && !a.SysIsDeleted);

            // --== Verifica se o administrador existe e se a senha está correta
            if (administrador != null && PasswordHelper.VerificarSenha(credentials.Password!, administrador.PasswordHash!))
            {
                userTokenData = new SGHSSUserTokenAdministradorModel
                {
                    UserId = administrador.Id,
                    Nome = administrador.Nome,
                    Email = administrador.Email,
                    UserType = "Administrador",
                    InstituicaoId = administrador.InstituicaoId,
                    Permissions = administrador.PermissoesAdministrador!.Select(pa => pa.Permissao!.Codigo!).ToList()
                };
            }
            else
            {
                // --== 2. Tentar autenticar como Funcionário (por Email) --==
                var funcionario = await _dbContext.Funcionarios
                    .Include(f => f.PermissoesFuncionario)
                        .ThenInclude(pf => pf.Permissao)
                    .FirstOrDefaultAsync(f => f.Email == credentials.Identificacao && !f.SysIsDeleted);

                // --== Verifica se o funcionário existe e se a senha está correta
                if (funcionario != null && PasswordHelper.VerificarSenha(credentials.Password!, funcionario.PasswordHash!))
                {
                    userTokenData = new SGHSSUserTokenFuncionarioModel
                    {
                        UserId = funcionario.Id,
                        Nome = funcionario.Nome,
                        Email = funcionario.Email,
                        UserType = "Funcionario",
                        Cargo = (CargoFuncionario)funcionario.CargoFuncionario, 
                        InstituicaoId = funcionario.InstituicaoId,
                        Permissions = funcionario.PermissoesFuncionario.Select(pf => pf.Permissao.Codigo).ToList()
                    };
                }
                else
                {
                    // --== 3. Tentar autenticar como Profissional de Saúde (por Email) --==
                    var profissional = await _dbContext.ProfissionaisSaude
                        .Include(ps => ps.PermissoesProfissionalSaude)
                            .ThenInclude(pps => pps.Permissao)
                        .Include(ps => ps.ProfissionalSaudeInstituicoes) // Inclui vínculo com instituição, se aplicável
                        .FirstOrDefaultAsync(ps => ps.Email == credentials.Identificacao && !ps.SysIsDeleted);

                    // --== Verifica se o profissional existe e se a senha está correta
                    if (profissional != null && PasswordHelper.VerificarSenha(credentials.Password!, profissional.PasswordHash!))
                    {
                        userTokenData = new SGHSSUserTokenProfissionalSaudeModel
                        {
                            UserId = profissional.Id,
                            Nome = profissional.Nome,
                            Email = profissional.Email,
                            UserType = "ProfissionalSaude",
                            Cargo = (CargoProfissionalSaude)profissional.CargoProfissionalSaude, 
                            Permissions = profissional.PermissoesProfissionalSaude.Select(pps => pps.Permissao.Codigo).ToList()
                        };
                    }
                    else
                    {
                        // --== 4. Tentar autenticar como Paciente (por CPF) --==
                        var paciente = await _dbContext.Pacientes
                            .Include(p => p.PermissoesPaciente)
                                .ThenInclude(pp => pp.Permissao)
                            .FirstOrDefaultAsync(p => p.CPF == credentials.Identificacao && !p.SysIsDeleted);

                        // --== Verifica se o paciente existe e se a senha está correta
                        if (paciente != null && PasswordHelper.VerificarSenha(credentials.Password!, paciente.PasswordHash!))
                        {
                            userTokenData = new SGHSSUserTokenPacienteModel
                            {
                                UserId = paciente.Id,
                                Nome = paciente.Nome,
                                Email = paciente.Email,
                                UserType = "Paciente",
                                CPF = paciente.CPF,
                                Permissions = paciente.PermissoesPaciente.Select(pp => pp.Permissao.Codigo).ToList()
                            };
                        }
                    }
                }
            }

            // --== Se nenhum tipo de usuário foi autenticado com sucesso
            if (userTokenData == null)
                return null;

            // --== 5. Gerar o JWT com as Claims corretas --==
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("A chave JWT não está configurada.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userTokenData.UserId.ToString()),
                new Claim(ClaimTypes.Name, userTokenData.Nome ?? string.Empty),
                new Claim("UserType", userTokenData.UserType ?? string.Empty) 
            };

            // --== Adiciona Email se disponível
            if (!string.IsNullOrWhiteSpace(userTokenData.Email))
                claims.Add(new Claim(ClaimTypes.Email, userTokenData.Email));

            // --== Adiciona claims específicas de cada tipo de usuário
            if (userTokenData is SGHSSUserTokenFuncionarioModel funcionarioToken)
            {
                if (funcionarioToken.Cargo.HasValue)
                    claims.Add(new Claim("Cargo", funcionarioToken.Cargo.ToString()!));
                
                if (funcionarioToken.InstituicaoId.HasValue && funcionarioToken.InstituicaoId != Guid.Empty)
                    claims.Add(new Claim("InstituicaoId", funcionarioToken.InstituicaoId.ToString()!));
                
            }
            else if (userTokenData is SGHSSUserTokenProfissionalSaudeModel profissionalToken)
            {
                if (profissionalToken.Cargo.HasValue)
                    claims.Add(new Claim("Cargo", profissionalToken.Cargo.ToString()!));          
            }
            else if (userTokenData is SGHSSUserTokenPacienteModel pacienteToken)
            {
                if (!string.IsNullOrWhiteSpace(pacienteToken.CPF))
                    claims.Add(new Claim("CPF", pacienteToken.CPF));
                
            }
            else if (userTokenData is SGHSSUserTokenAdministradorModel administradorToken)
            {
                if (administradorToken.InstituicaoId.HasValue && administradorToken.InstituicaoId != Guid.Empty)
                {
                    claims.Add(new Claim("InstituicaoId", administradorToken.InstituicaoId.ToString()!));
                    claims.Add(new Claim("AdminType", "Local")); 
                }
                else
                {
                    claims.Add(new Claim("AdminType", "Geral")); 
                }
            }

            // --== Adiciona as permissões como claims individuais
            if (userTokenData.Permissions != null)
            {
                foreach (var permCode in userTokenData.Permissions)
                {
                    claims.Add(new Claim("Permission", permCode));
                }
            }

            // --== Configura o descritor do token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:ExpiresHours"] ?? "1")),
                SigningCredentials = jwtKey.ToSymmetricSecurity(),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            // --== Cria e serializa o token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            // --== Retorna o modelo de token de autenticação
            return new SGHSSAuthenticationTokenModel(userTokenData.UserId, accessToken, userTokenData.Nome ?? "Usuário");
        }
    }
}