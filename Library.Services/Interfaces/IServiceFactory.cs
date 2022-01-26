namespace Library.Services.Interfaces
{
    public interface IServiceFactory
    {
        T GetServices<T>() where T : class;
    }
}
