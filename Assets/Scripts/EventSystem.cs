using System.Collections;
using System.Collections.Generic;

public class EventSystem
{
    private static EventSystem instance;
    public static EventSystem Instance
    {
        get
        {
            if (instance == null) instance = new EventSystem();
            return instance;
        }
    }

    public delegate void EventListener(string eventName, object param = null);
    private Dictionary<string, List<EventListener>> eventListener;

    EventSystem()
    {
        eventListener = new Dictionary<string, List<EventListener>>();
    }

    public void AddEventListener(string eventId, EventListener listener)
    {
        if (!eventListener.ContainsKey(eventId)) eventListener.Add(eventId, new List<EventListener>());
        eventListener[eventId].Add(listener);
    }

    public void RemoveEventListener(string eventId, EventListener listener)
    {
        if (eventListener.ContainsKey(eventId))
            eventListener[eventId].Remove(listener);
    }

    public void Fire(string eventId, string eventName, object param = null)
    {
        if (eventListener.ContainsKey(eventId))
            for (int i = eventListener[eventId].Count - 1; i >= 0; i--)
                eventListener[eventId][i](eventName, param);

        System.Diagnostics.Debug.WriteLine(string.Format("Event {0} fired: {1}, {2}",
            eventId, eventName, param == null ? "" : param.ToString()));
    }
}
