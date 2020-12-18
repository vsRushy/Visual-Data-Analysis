using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent
{
    
}

public class CustomPositionEvent : CustomEvent
{
    public Vector3 position;

    public void Set(int pos_x, int pos_y, int pos_z)
    {
        this.position.x = pos_x;
        this.position.y = pos_y;
        this.position.z = pos_z;
    }
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
            Ray ray = new Ray(ev.position, Vector3.up);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100000) && hit.transform.tag == "HeatMap")
            {
                GameObject cube = hit.transform.gameObject;
                cube.SetActive(true);
                Material material;
                
                material = cube.GetComponent<Material>();
                material.color = new Color(10, 10, 10);
            }
        }
    }
}
