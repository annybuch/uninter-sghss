namespace Loop.SGHSS.Services.Servicos_Prestados.Consultas
{
    public interface ITeleConsultaService
    {
        Task EncerrarSala(string nomeSala);
        string GerarTokenAcesso(string nomeSala, string nomeUsuario, string role = "user");
        Task<string> CriarSala(string nomeSala);
    }
}
