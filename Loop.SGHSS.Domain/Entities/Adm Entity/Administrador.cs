using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Permissao_Entity;
using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model._Enums.Estados;
using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Adm;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.PassWord;

namespace Loop.SGHSS.Domain.Entities.Adm_Entity
{
    public class Administrador : DefaultEntityIdModel
    {
        public Administrador() { }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CPF { get; set; }
        public GeneroEnum? Genero { get; set; }
        public string? Foto { get; set; }
        public string? Cidade { get; set; }
        public EstadosEnum? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? CEP { get; set; }
        public string? Numero { get; set; }
        public CargoFuncionario? CargoAdm { get; set; }
        public Guid? InstituicaoId { get; set; } = null;


        public virtual ICollection<Permissao_Administrador>? PermissoesAdministrador { get; set; }


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

        public void AtualizarGeral(AdministradorModel model)
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
                if (model.CargoAdm != null)
                    this.CargoAdm = model.CargoAdm;
            }
        }

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

