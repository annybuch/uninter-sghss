using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Consulta_Entity;
using Loop.SGHSS.Domain.Entities.Exame_Entity;
using Loop.SGHSS.Domain.Entities.Leito_Entity;
using Loop.SGHSS.Domain.Entities.Permissao_Entity;
using Loop.SGHSS.Model._Enums.Estados;
using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Pacientes;
using Loop.SGHSS.Model.PassWord;

namespace Loop.SGHSS.Domain.Entities.Patient_Entity
{
    public class Paciente : DefaultEntityIdModel
    {
        public Paciente() { }

        /// <summary>
        /// Nome completo do paciente.
        /// </summary>
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }

        /// <summary>
        /// E-mail do paciente.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Hash da senha de acesso.
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Número do telefone paciente.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento do paciente.
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// CPF do paciente.
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// Genero do paciente.
        /// </summary>
        public GeneroEnum? Genero { get; set; }

        /// <summary>
        /// foto do paciente.
        /// </summary>
        public string? Foto { get; set; }

        /// <summary>
        /// Endereço do paciente
        /// </summary>
        public string? Cidade { get; set; }
        public EstadosEnum? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? CEP { get; set; }
        public string? Numero { get; set; }


        // --== Relacionamentos
        public virtual ICollection<Consulta>? Consultas { get; set; }
        public virtual ICollection<Exame>? Exames { get; set; }
        public virtual ICollection<Leito_Paciente>? LeitoPaciente { get; set; }
        public virtual ICollection<Permissao_Paciente>? PermissoesPaciente { get; set; }


        // --== Atualiza o endereço da instituilção.
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

        // --== Atualiza informações Gerais do funcionário.
        public void AtualizarGeral(PacientesModel model)
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
                if (model.Foto != null)
                    this.Foto = model.Foto;
            }
        }

        // --== Atualiza a senha do funcionário.
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