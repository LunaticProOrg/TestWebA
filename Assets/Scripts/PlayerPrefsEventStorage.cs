using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsEventStorage : IEventStorage
{
    private const string EventKey = "PendingEvents";

    public void SavePendingEvents(List<EventService.EventData> events)
    {
        string json = JsonUtility.ToJson(new { events });
        PlayerPrefs.SetString(EventKey, json);
        PlayerPrefs.Save();
    }

    public List<EventService.EventData> LoadPendingEvents()
    {
        if (PlayerPrefs.HasKey(EventKey))
        {
            string json = PlayerPrefs.GetString(EventKey);
            var data = JsonUtility.FromJson<EventList>(json);
            return new List<EventService.EventData>(data.events);
        }
        return new List<EventService.EventData>();
    }

    [System.Serializable]
    private class EventList
    {
        public EventService.EventData[] events;
    }
}
