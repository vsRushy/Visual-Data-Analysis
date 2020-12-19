using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;

    public List<Eventinfo> events;

    public static List<Eventinfo> carlosEvents = new List<Eventinfo>();
    public static List<Eventinfo> sebiEvents = new List<Eventinfo>();
    public static List<Eventinfo> marcEvents = new List<Eventinfo>();

    public static EventManager GetInstance()
    {
        return eventManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(eventManager == null)
        {
            eventManager = this;
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Eventinfo> GetListByUser(VisualizationManager.User user_filter)
    {
        List<Eventinfo> ret = new List<Eventinfo>();
        switch (user_filter)
        {
            case VisualizationManager.User.All:
                {
                    ret.AddRange(carlosEvents);
                    ret.AddRange(sebiEvents);
                    ret.AddRange(marcEvents);
                    //ret.AddRange(peterEvents);
                    //ret.AddRange(gerardEvents);
                }
                break;
            case VisualizationManager.User.Carlos:
                {
                    ret = carlosEvents;
                }
                break;
            case VisualizationManager.User.Gerard:
                {
                    //ret = gerardEvents;
                    Debug.Log("User Not ready yet");
                }
                break;
            case VisualizationManager.User.Marc:
                {
                    ret = marcEvents;
                }
                break;
            case VisualizationManager.User.Peter:
                {
                    //ret = peterEvents;
                    Debug.Log("User Not ready yet");
                }
                break;
            case VisualizationManager.User.Sebi:
                {
                    ret = sebiEvents;
                }
                break;
            default:           
                break;
        }

        return ret;
    }
}

public struct Eventinfo
{
    public string name;
    public string type;
    public DateTime timestamp;
    public Vector3 position;
    public uint stage;


    public Eventinfo(string _name, string _type, DateTime _timestamp, Vector3 _position, uint _stage)
    {
        name = _name;
        type = _type;
        timestamp = _timestamp;
        position = _position;
        stage = _stage;
    }


    //public string[] SerializeBody() {  }
    //public string[] SerializeHeader() { };


}