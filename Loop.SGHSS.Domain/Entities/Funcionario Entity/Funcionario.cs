using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Permissao_Entity;
using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model._Enums.Estados;
using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Funcionarios;
using Loop.SGHSS.Model.PassWord;

namespace Loop.SGHSS.Domain.Entities.Funcionario_Entity
{
    public class Funcionario : DefaultEntityIdModel
    {
        public Funcionario() { }

        /// <summary>
        /// Nome completo do funcionario.
        /// </summary>
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }

        /// <summary>
        /// E-mail do funcionario.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Hash da senha de acesso.
        /// </summary>
        public string? PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Número do telefone funcionario.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento do funcionario.
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// CPF do funcionario.
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// Genero do funcionario.
        /// </summary>
        public GeneroEnum? Genero { get; set; }  

        /// <summary>
        /// foto do funcionario.
        /// </summary>
        public string? Foto { get; set; }

        /// <summary>
        /// Cargo do funcionario (Recepcionista, gerente etc).
        /// </summary>
        public CargoFuncionario? CargoFuncionario { get; set; }

        /// <summary>
        /// Endereço do funcionario
        /// </summary>
        public string? Cidade { get; set; }
        public EstadosEnum? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? CEP { get; set; }
        public string? Numero { get; set; }


        // --== Qual instituição o funcionário trabalha.
        public Guid? InstituicaoId { get; set; }
        public Instituicao? Instituicao { get; set; }


        public virtual ICollection<Permissao_Funcionario>? PermissoesFuncionario { get; set; }

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
        public void AtualizarGeral(FuncionarioModel model)
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
                if (model.CargoFuncionario != null)
                    this.CargoFuncionario = model.CargoFuncionario;
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
