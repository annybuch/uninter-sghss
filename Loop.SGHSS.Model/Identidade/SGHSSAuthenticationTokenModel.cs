using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loop.SGHSS.Model.Identidade
{
    public class SGHSSAuthenticationTokenModel
    {
        public Guid Id { get; set; }
        public string? AccessToken { get; set; }
        public string? Nome { get; set; }

        public SGHSSAuthenticationTokenModel(Guid id, string accessToken, string nome)
        {
            Id = id;
            AccessToken = accessToken;
            Nome = nome;
        }
    }
}
