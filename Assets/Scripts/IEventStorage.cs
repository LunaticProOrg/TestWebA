using System.Collections.Generic;

public interface IEventStorage
{
    void SavePendingEvents(List<EventService.EventData> events);
    List<EventService.EventData> LoadPendingEvents();
}
