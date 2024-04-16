using WebAPI.Domains;

namespace WebAPI.Interfaces
{
    public interface IExameRepository
    {
        Task Cadastrar(Exame exame);

        public List<Exame> BuscarPorIdConsulta(Guid idConsulta);
    }
}