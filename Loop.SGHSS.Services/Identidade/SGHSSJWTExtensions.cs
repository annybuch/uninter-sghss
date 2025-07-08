using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model.Identidade;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

public static class SGHSSJWTExtensions
{
    
    /// <summary>
    /// Desserializa as claims de um JwtSecurityToken para uma instância
    /// de um tipo específico de SGHSSUserTokenModel.
    /// </summary>
    public static T DeserializeToken<T>(this JwtSecurityToken token) where T : SGHSSBaseUserTokenModel, new()
    {
        var resultado = new T();

        // --== Mapeamento de Claims padrão e customizadas comuns a todos os tipos de usuário.
        resultado.UserId = Guid.Parse(token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        resultado.Nome = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        resultado.Email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        resultado.UserType = token.Claims.FirstOrDefault(c => c.Type == "UserType")?.Value;

        // --== Coleta todas as claims de "Permission".
        resultado.Permissions = token.Claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

        // --== Preenche outras propriedades específicas dos modelos derivados, tratando tipos customizados e enums.
        foreach (var prop in typeof(T).GetProperties())
        {
            // --== Ignora propriedades já tratadas explicitamente ou a lista de Permissões
            if (prop.Name == nameof(SGHSSBaseUserTokenModel.UserId) ||
                prop.Name == nameof(SGHSSBaseUserTokenModel.Nome) ||
                prop.Name == nameof(SGHSSBaseUserTokenModel.Email) ||
                prop.Name == nameof(SGHSSBaseUserTokenModel.UserType) ||
                prop.Name == nameof(SGHSSBaseUserTokenModel.Permissions))
            {
                continue;
            }

            // --== Tenta encontrar uma claim que corresponda ao nome da propriedade.
            var claim = token.Claims.FirstOrDefault(c => c.Type.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));

            // --== Tratamento especial para nomes de claims que podem ser diferentes do nome da propriedade.
            if (claim == null)
            {
                if (prop.Name.Equals("InstituicaoId", StringComparison.OrdinalIgnoreCase) &&
                    (resultado is SGHSSUserTokenFuncionarioModel || resultado is SGHSSUserTokenProfissionalSaudeModel || resultado is SGHSSUserTokenAdministradorModel))
                {
                    claim = token.Claims.FirstOrDefault(c => c.Type.Equals("InstituicaoId", StringComparison.OrdinalIgnoreCase));
                }
                else if (prop.Name.Equals("Cargo", StringComparison.OrdinalIgnoreCase) &&
                         (resultado is SGHSSUserTokenFuncionarioModel || resultado is SGHSSUserTokenProfissionalSaudeModel))
                {
                    claim = token.Claims.FirstOrDefault(c => c.Type.Equals("Cargo", StringComparison.OrdinalIgnoreCase));
                }
                else if (prop.Name.Equals("CPF", StringComparison.OrdinalIgnoreCase) && resultado is SGHSSUserTokenPacienteModel)
                {
                    claim = token.Claims.FirstOrDefault(c => c.Type.Equals("CPF", StringComparison.OrdinalIgnoreCase));
                }
            }

            if (claim is not null)
            {
                object? valor = null;
                Type tipoAlvo = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType; 

                if (tipoAlvo == typeof(string))
                {
                    valor = claim.Value;
                }
                else if (tipoAlvo == typeof(Guid))
                {
                    if (Guid.TryParse(claim.Value, out var valorGuid))
                        valor = valorGuid;
                }
                else if (tipoAlvo.IsEnum)
                {
                    try
                    {
                        valor = Enum.Parse(tipoAlvo, claim.Value);
                    }
                    catch (ArgumentException)
                    {
                        valor = null;
                    }
                }
                else if (tipoAlvo == typeof(int))
                {
                    if (int.TryParse(claim.Value, out var valorInt))
                        valor = valorInt;
                }
                else if (tipoAlvo == typeof(double))
                {
                    if (double.TryParse(claim.Value, out var valorDouble))
                        valor = valorDouble;
                }
                else if (tipoAlvo.IsClass) 
                {
                    try
                    {
                        valor = JsonSerializer.Deserialize(claim.Value, prop.PropertyType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch (JsonException)
                    {
                        valor = null;
                    }
                }
                else
                {
                    throw new NotImplementedException($"A desserialização para o tipo {prop.PropertyType.Name} não está implementada.");
                }

                if (valor is not null)
                {
                    prop.SetValue(resultado, valor);
                }
            }
        }
        return resultado;
    }

    /// <summary>
    /// Converte uma string secreta em credenciais de segurança simétricas para a assinatura JWT.
    /// </summary>
    /// <param name="chaveSecreta">A string da chave secreta.</param>
    /// <returns>Uma instância de SigningCredentials.</returns>
    public static SigningCredentials ToSymmetricSecurity(this string chaveSecreta)
    {
        return new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(chaveSecreta)), SecurityAlgorithms.HmacSha256Signature);
    }

    /// <summary>
    /// Converte um ClaimsPrincipal (usuário autenticado) para um modelo SGHSSBaseUserTokenModel específico,
    /// baseado nas claims presentes no token.
    /// </summary>
    public static SGHSSBaseUserTokenModel? ToSGHSSUserTokenModel(this ClaimsPrincipal principal)
    {
        if (principal?.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            return null;
        }

        var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var nome = principal.FindFirst(ClaimTypes.Name)?.Value;
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var userType = principal.FindFirst("UserType")?.Value;
        var permissoes = principal.FindAll("Permission").Select(c => c.Value).ToList();

        SGHSSBaseUserTokenModel? dadosUsuario = null;

        switch (userType)
        {
            case "Funcionario":
                var cargoFuncionarioStr = principal.FindFirst("Cargo")?.Value;
                var instituicaoIdFuncStr = principal.FindFirst("InstituicaoId")?.Value;
                dadosUsuario = new SGHSSUserTokenFuncionarioModel
                {
                    UserId = userId,
                    Nome = nome,
                    Email = email,
                    UserType = userType,
                    Permissions = permissoes,
                    Cargo = Enum.TryParse<CargoFuncionario>(cargoFuncionarioStr, out var cargoFunc) ? cargoFunc : (CargoFuncionario?)null,
                    InstituicaoId = Guid.TryParse(instituicaoIdFuncStr, out var instIdFunc) ? instIdFunc : (Guid?)null
                };
                break;
            case "ProfissionalSaude":
                var cargoProfissionalStr = principal.FindFirst("Cargo")?.Value;
                var instituicaoIdProfStr = principal.FindFirst("InstituicaoId")?.Value;
                dadosUsuario = new SGHSSUserTokenProfissionalSaudeModel
                {
                    UserId = userId,
                    Nome = nome,
                    Email = email,
                    UserType = userType,
                    Permissions = permissoes,
                    Cargo = Enum.TryParse<CargoProfissionalSaude>(cargoProfissionalStr, out var cargoProf) ? cargoProf : (CargoProfissionalSaude?)null,
                };
                break;
            case "Paciente":
                var cpf = principal.FindFirst("CPF")?.Value;
                dadosUsuario = new SGHSSUserTokenPacienteModel
                {
                    UserId = userId,
                    Nome = nome,
                    Email = email,
                    UserType = userType,
                    Permissions = permissoes,
                    CPF = cpf
                };
                break;
            case "Administrador":
                var instituicaoIdAdmStr = principal.FindFirst("InstituicaoId")?.Value;
                dadosUsuario = new SGHSSUserTokenAdministradorModel
                {
                    UserId = userId,
                    Nome = nome,
                    Email = email,
                    UserType = userType,
                    Permissions = permissoes,
                    InstituicaoId = Guid.TryParse(instituicaoIdAdmStr, out var instIdAdm) ? instIdAdm : (Guid?)null
                };
                break;
            default:
                return null;
        }

        return dadosUsuario;
    }
}