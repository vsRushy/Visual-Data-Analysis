using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    // name of the person playing the game
    public string playerName = "Unnamed";

    // player id
    uint playerId = 0;

    // to keep track of the stage
    uint stage = 0;

    // polling rate so as not to crate events every frame
    public float eventIntervalSecTime = 0.5f;
    float currentEventIntervalSecTime = 0.0f;

    // reference to the player gameObject
    public GameObject player;


    // event queue
    public static Queue<Eventinfo> events = new Queue<Eventinfo>();

    // what this
    public static List<Eventinfo> carlosEvents = new List<Eventinfo>();
    public static List<Eventinfo> sebiEvents = new List<Eventinfo>();
    public static List<Eventinfo> marcEvents = new List<Eventinfo>();

    // singleton

    public static EventManager eventManager;

    public static EventManager GetInstance()
    {
        return eventManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (eventManager == null)
        {
            eventManager = this;
        }

        events = new Queue<Eventinfo>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only used for position right now since its the only "contiunous" event
        if ((currentEventIntervalSecTime += Time.deltaTime) >= eventIntervalSecTime)
        {
            currentEventIntervalSecTime = 0.0f;

            // add new position event
            AddEventByType(CUSTOM_EVENT_TYPE.POSITION);
        }

        if (events.Count == 0)
            return;

        // dispatch enqueued events
        foreach (var e in events)
        {
            DispatchEvent(e);

        }
        events.Dequeue();
    }

    public void AddEventByType(CUSTOM_EVENT_TYPE type)
    {
        if (player == null)
            return;
        Eventinfo e = new Eventinfo(playerName, playerId, type, player.transform.position, stage);
        events.Enqueue(e);
    }

    public void AddEventByType(string type)
    {
        AddEventByType(GetEventTypeFromString(type));
    }

    CUSTOM_EVENT_TYPE GetEventTypeFromString(string type)
    {
        return (CUSTOM_EVENT_TYPE)Enum.Parse(typeof(CUSTOM_EVENT_TYPE), type);
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

    // TODO: Visualization, do stuff with this to generate cubes and other magical visual data

    void DispatchEvent(Eventinfo e)
    {
        switch (e.type)
        {
            case CUSTOM_EVENT_TYPE.NONE:
                break;
            case CUSTOM_EVENT_TYPE.POSITION:
                break;
            case CUSTOM_EVENT_TYPE.ATTACK:
                break;
            case CUSTOM_EVENT_TYPE.JUMP:
                break;
            case CUSTOM_EVENT_TYPE.DEATH:
                break;
            case CUSTOM_EVENT_TYPE.RECEIVE_DAMAGE:
                break;
            case CUSTOM_EVENT_TYPE.ENEMY_KILLED:
                break;
            case CUSTOM_EVENT_TYPE.ACTIVATE_SWITCH:
                break;
            default:
                break;
        }
    }
}

public enum CUSTOM_EVENT_TYPE
{
    NONE = -1,
    POSITION,
    ATTACK,
    JUMP,
    DEATH,
    RECEIVE_DAMAGE,
    ENEMY_KILLED,
    ACTIVATE_SWITCH
}

public struct Eventinfo
{
    // Seconds since the application is running.
    float seconds;
    // Timespan from seconds.
    TimeSpan time_span;
    // String storing the time in date format.
    public String time;

    // Type of event.
    public CUSTOM_EVENT_TYPE type;

    // Player name.
    public String player_name;

    // Player id.
    public uint player_id;

    // Stage.
    public uint stage;

    // Position at which occurs the event.
    public Vector3 position;

    // TODO
    public Eventinfo(String player_name, uint player_id, CUSTOM_EVENT_TYPE type, Vector3 position, uint stage)
    {
        this.seconds = Time.realtimeSinceStartup;
        this.time_span = TimeSpan.FromSeconds(seconds);
        this.time = String.Format("{0:D2}:{1:D2}:{2:D2}", time_span.Hours, time_span.Minutes, time_span.Seconds);

        this.player_name = player_name;
        this.player_id = player_id;
        this.type = type;
        this.position = position;
        this.stage = stage;
    }
}