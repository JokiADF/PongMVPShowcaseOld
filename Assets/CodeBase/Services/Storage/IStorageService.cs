namespace CodeBase.Services.Storage
{
    public interface IStorageService
    {
        void Save<T>(string key, T obj);
        T Load<T>(string key);
    }
}
