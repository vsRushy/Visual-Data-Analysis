using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGetter : MonoBehaviour
{
    public GameObject events;
    private GameObject player;

    private float position_event_counter = 0.0f;
    private float position_event_limiter = 0.5f;

    void Start()
    {
        player = GameObject.Find("Ellen");
    }

    void Update()
    {
        GetPositionEvent();
    }

    private void GetPositionEvent()
    {
        position_event_counter += Time.deltaTime;
        if (position_event_counter >= position_event_limiter)
        {
            position_event_counter = 0.0f;

            CustomPositionEvent pos_event = new CustomPositionEvent();
            pos_event.Set((int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z);

            Events custom_events = events.GetComponent<Events>();
            custom_events.custom_events.Enqueue(pos_event);
        }
    }
}
