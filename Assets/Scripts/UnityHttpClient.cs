using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class UnityHttpClient : IHttpClient
{
    public async UniTask<string> PostAsync(string url, string json)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, json))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            var operation = webRequest.SendWebRequest();
            await operation;

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                return webRequest.downloadHandler.text;
            }
            else
            {
                throw new System.Exception(webRequest.error);
            }
        }
    }
}
