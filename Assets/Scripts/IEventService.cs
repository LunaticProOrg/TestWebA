using Cysharp.Threading.Tasks;

public interface IEventService
{
    UniTask TrackEventAsync(string type, string data);
}
