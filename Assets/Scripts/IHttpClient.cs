using Cysharp.Threading.Tasks;

public interface IHttpClient
{
    UniTask<string> PostAsync(string url, string json);
}
