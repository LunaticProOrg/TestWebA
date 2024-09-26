using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EventService : IEventService
{
    private readonly string serverUrl;
    private readonly float cooldownBeforeSend;
    private readonly IEventStorage eventStorage;
    private readonly IHttpClient httpClient;

    private List<EventData> eventsQueue;
    private bool isCooldownActive;

    public EventService(string serverUrl, IEventStorage eventStorage, IHttpClient httpClient, float cooldownBeforeSend = 2f)
    {
        this.serverUrl = serverUrl;
        this.cooldownBeforeSend = cooldownBeforeSend;
        this.eventStorage = eventStorage;
        this.httpClient = httpClient;
        this.eventsQueue = eventStorage.LoadPendingEvents();
    }

    public async UniTask TrackEventAsync(string type, string data)
    {
        var newEvent = new EventData(type, data);
        eventsQueue.Add(newEvent);

        if (!isCooldownActive)
        {
            isCooldownActive = true;
            await UniTask.Delay((int)(cooldownBeforeSend * 1000));
            await SendEventsAsync();
            isCooldownActive = false;
        }
    }

    private async UniTask SendEventsAsync()
    {
        if (eventsQueue.Count == 0) return;

        var jsonData = new { events = eventsQueue };
        string json = JsonUtility.ToJson(jsonData);
        eventsQueue.Clear();

        try
        {
            string response = await httpClient.PostAsync(serverUrl, json);
            // Обработка ответа, если необходимо
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error sending events: {ex.Message}");
            HandleFailedSend(jsonData.events);
        }
    }

    private void HandleFailedSend(List<EventData> failedEvents)
    {
        foreach (var ev in failedEvents)
        {
            eventsQueue.Add(ev);
        }
        eventStorage.SavePendingEvents(eventsQueue);
    }

    [System.Serializable]
    public class EventData
    {
        public string type;
        public string data;

        public EventData(string type, string data)
        {
            this.type = type;
            this.data = data;
        }
    }
}
