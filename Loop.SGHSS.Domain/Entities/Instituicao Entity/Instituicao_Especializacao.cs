using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Servicos_Entity;

namespace Loop.SGHSS.Domain.Entities.Instituicao_Entity
{
    public class Instituicao_Especializacao : DefaultEntityIdModel
    {
        public Instituicao_Especializacao() { }
        public Instituicao_Especializacao(Guid? instituicaoId, Guid? especializacaoId, Instituicao? instituicao, Especializacoes? especializacao)
        {
            InstituicaoId = instituicaoId;
            EspecializacaoId = especializacaoId;
            Instituicao = instituicao;
            Especializacao = especializacao;
        }

        public Guid? InstituicaoId { get; set; }
        public Guid? EspecializacaoId { get; set; }

        // --== Relacionamentos
        public Instituicao? Instituicao { get; set; }
        public Especializacoes? Especializacao { get; set; }
    }
}
