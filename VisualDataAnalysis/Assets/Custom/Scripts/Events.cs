using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent
{

}

public class CustomPositionEvent : CustomEvent
{
    public int pos_x, pos_y, pos_z;
}

// -------------------------------------------------------

public class Events : MonoBehaviour
{
    public Queue<CustomEvent> custom_events;

    public GameObject heat_map;

    void Start()
    {
        custom_events = new Queue<CustomEvent>();
    }

    void Update()
    {
        foreach (CustomEvent custom_event in custom_events)
        {
            DispatchCustomEvent(custom_event);
            custom_events.Dequeue();
        }
    }

    private void DispatchCustomEvent(CustomEvent custom_event)
    {
        if(custom_event is CustomPositionEvent)
        {
            CustomPositionEvent ev = (CustomPositionEvent)custom_event;
            
        }
    }
}
