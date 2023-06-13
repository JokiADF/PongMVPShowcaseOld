using Cysharp.Threading.Tasks;

namespace CodeBase.Services.AssetManagement
{
    public interface IAssetService
    {
        UniTask Load<T>(string key);
        T Get<T>(string key);
    }
}
