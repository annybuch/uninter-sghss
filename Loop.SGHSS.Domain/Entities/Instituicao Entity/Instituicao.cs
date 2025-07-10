using Loop.SGHSS.Data.Entities.Suprimento_Entity;
using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.Consulta_Entity;
using Loop.SGHSS.Domain.Entities.Exame_Entity;
using Loop.SGHSS.Domain.Entities.Funcionario_Entity;
using Loop.SGHSS.Domain.Entities.Leito_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Model._Enums.Estados;
using Loop.SGHSS.Model._Enums.Instituicao;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Instituicoes;
using Loop.SGHSS.Model.Leitos;

namespace Loop.SGHSS.Domain.Entities.Instituicao_Entity
{
    public class Instituicao : DefaultEntityIdModel
    {
        
        public Instituicao() { }

        /// <summary>
        /// Nome da Instituição
        /// </summary>
        public string? NomeFantasia { get; set; }
        public string? RazaoSocial { get; set; }

        /// <summary>
        /// CNPJ da instituição
        /// </summary>
        public string? CNPJ { get; set; }

        /// <summary>
        /// E-mail institucional.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Número do telefone da instituição.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Intervalo de atendimento em minutos.
        /// </summary>
        public int? IntervaloMinutos { get; set; }


        /// <summary>
        /// Cargo do profissional (médico, enfermeiro, técnico)
        /// </summary>
        public TipoInstituicaoEnum? TipoInstituicao { get; set; }

        /// <summary>
        /// Endereço da instituição
        /// </summary>
        public string? Cidade { get; set; }
        public EstadosEnum? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? CEP { get; set; }
        public string? Numero { get; set; }



        // --== Relacionamentos
        public virtual ICollection<Consulta>? Consultas { get; set; }
        public virtual ICollection<Exame>? Exames { get; set; }
        public virtual ICollection<Funcionario>? Funcionarios { get; set; }
        public virtual ICollection<Leito>? Leitos { get; set; }
        public virtual ICollection<Suprimento_Compra>? SuprimentosCompra { get; set; }
        public virtual ICollection<ProfissionalSaude_Instituicao>? ProfissionaisSaudeInstituicao { get; set; }
        public virtual ICollection<Instituicao_Especializacao>? InstituicaoEspecializacoes { get; set; } 
        public virtual ICollection<Instituicao_ServicosLaboratorio>? InstituicaoServicosLaboratorio { get; set; } 
        public virtual ICollection<Instituicao_Agenda>? InstituicaoAgenda { get; set; }


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

        // --== Atualiza Instituicao Geral.
        public void AtualizarGeral(InstituicaoModel model)
        {
            if (model != null)
            {
                if (model.RazaoSocial != null)
                    this.RazaoSocial = model.RazaoSocial;
                if (model.NomeFantasia != null)
                    this.NomeFantasia = model.NomeFantasia;
                if (model.CNPJ != null)
                    this.CNPJ = model.CNPJ;
                if (model.Email != null)
                    this.Email = model.Email;
                if (model.Telefone != null)
                    this.Telefone = model.Telefone;
                if (model.TipoInstituicao != null)
                    this.TipoInstituicao = model.TipoInstituicao;
            }
        }

        // --== Adicionar leito.
        public Leito Adicionar_Leito(TicketCadastrarLeitoViewModel viewModel)
        {
            this.Leitos ??= new List<Leito>();
            var leitoNovo = new Leito()
            {
                InstutuicaoId = this.Id,
                NumeroLeito = viewModel.NumeroLeito,
                StatusLeito = viewModel.StatusLeito,
                TipoLeito = viewModel.tipoLeitoEnum,
                Andar = viewModel.Andar,
                Titulo = viewModel.Titulo,
                Id = Guid.NewGuid(),
            };

            this.Leitos.Add(leitoNovo);

            return leitoNovo;
        }
    }
}
