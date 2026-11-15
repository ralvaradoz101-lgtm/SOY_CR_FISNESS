namespace SoyCrFisness.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> ObtenerTodos();
        void GuardarTodos(List<T> elementos);
    }
}