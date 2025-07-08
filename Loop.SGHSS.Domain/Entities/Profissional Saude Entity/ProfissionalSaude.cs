using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.Consulta_Entity;
using Loop.SGHSS.Domain.Entities.Exame_Entity;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Permissao_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._Enums.Estados;
using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Model.ProfissionaisSaude;

namespace Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity
{
    public class ProfissionalSaude : DefaultEntityIdModel
    {
        public ProfissionalSaude() { }

        /// <summary>
        /// Nome completo do profissional.
        /// </summary>
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }

        /// <summary>
        /// Número de registro profissional (CRM, COREN, etc).
        /// </summary>
        public string? NumeroRegistro { get; set; }

        /// <summary>
        /// E-mail institucional do profissional.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Hash da senha de acesso.
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Número do telefone profissional.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento do profissional.
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// CPF do profissional.
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// Genero do profissional.
        /// </summary>
        public GeneroEnum? Genero { get; set; }

        /// <summary>
        /// Cargo do profissional (médico, enfermeiro, técnico)
        /// </summary>
        public CargoProfissionalSaude? CargoProfissionalSaude { get; set; }

        /// <summary>
        /// foto do profissional.
        /// </summary>
        public string? Foto { get; set; }

        /// <summary>
        /// Endereço do profissional
        /// </summary>
        public string? Cidade { get; set; }
        public EstadosEnum? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? CEP { get; set; }
        public string? Numero { get; set; }



        // --== Relacionamentos
        public virtual ICollection<Consulta>? Consultas { get; set; }
        public virtual ICollection<Exame>? Exames { get; set; }
        public virtual ICollection<ProfissionalSaude_Instituicao>? ProfissionalSaudeInstituicoes { get; set; }
        public virtual ICollection<ProfissionalSaude_Especializacao>? ProfissionalSaudeEspecializacoes { get; set; }
        public virtual ICollection<ProfissionalSaude_ServicoLaboratorio>? ProfissionalSaudeServicosLaboratorio { get; set; }
        public virtual ICollection<ProfissionalSaude_Agenda>? ProfissionalSaudeAgenda { get; set; }
        public virtual ICollection<Permissao_ProfissionalSaude>? PermissoesProfissionalSaude { get; set; }


        // --== Atualiza o endereço do profissional.
        public void AtualizarEndereco(EnderecoModel model)
        {
            if (model != null)
            {
                if (model.Cidade != null)
                    this.Cidade = model.Cidade;
                if (model.CEP != null)
                    this.CEP = model.CEP;
                if (model.Logradouro != null)
                    this.Logradouro = model.Logradouro;
                if (model.Bairro != null)
                    this.Bairro = model.Bairro;
                if (model.Numero != null)
                    this.Numero = model.Numero;
            }
        }

        // --== Atualiza informações Gerais do profissional.
        public void AtualizarGeral(ProfissionalSaudeModel model)
        {
            if (model != null)
            {
                if (model.Nome != null)
                    this.Nome = model.Nome;
                if (model.Sobrenome != null)
                    this.Sobrenome = model.Sobrenome;
                if (model.CPF != null)
                    this.CPF = model.CPF;
                if (model.Email != null)
                    this.Email = model.Email;
                if (model.Telefone != null)
                    this.Telefone = model.Telefone;
                if (model.DataNascimento != null)
                    this.DataNascimento = model.DataNascimento;
                if (model.Genero != null)
                    this.Genero = model.Genero;
                if (model.CargoProfissionalSaude != null)
                    this.CargoProfissionalSaude = model.CargoProfissionalSaude;
                if (model.Foto != null)
                    this.Foto = model.Foto;
            }
        }

        // --== Atualiza a senha do profissional.
        public void AtualizarSenha(PassModel model)
        {
            if (model != null)
            {
                if (model.PasswordHash != null)
                    this.PasswordHash = model.PasswordHash;
            }
        }
    }
} 