using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    // name of the person playing the game
   // public string playerName = "Unnamed";
    
    public VisualizationManager.User userName = VisualizationManager.User.All;
   public static System.Globalization.CultureInfo culture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
    
    // to keep track of the stage
    uint stage = 0;

    // polling rate so as not to crate events every frame
    public float eventIntervalSecTime = 0.5f;
    float currentEventIntervalSecTime = 0.0f;

    // reference to the player gameObject
    public GameObject player;


    // events --> those that have been dispatched 
    public static List<Eventinfo> events = new List<Eventinfo>();

    // events --> those that have not been dispatched 
    public static List<Eventinfo> pendingEvents = new List<Eventinfo>();

    // what this
    public static List<Eventinfo> carlosEvents = new List<Eventinfo>();
    public static List<Eventinfo> sebiEvents = new List<Eventinfo>();
    public static List<Eventinfo> marcEvents = new List<Eventinfo>();
    public static List<Eventinfo> gerardEvents = new List<Eventinfo>();
    public static List<Eventinfo> joseEvents = new List<Eventinfo>();

    // singleton

    public static EventManager eventManager;

    public static EventManager GetInstance()
    {
        return eventManager;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (eventManager != null && eventManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            eventManager = this;
        }

        events = new List<Eventinfo>();

        culture.NumberFormat.NumberDecimalSeparator = ".";
        // culture.NumberFormat.NumberGroupSeparator = ",";
        culture.NumberFormat.NumberDecimalDigits = 6;
    }

    private void Start()
    {
        ReaderCSV.ReadData(VisualizationManager.User.Carlos);
        ReaderCSV.ReadData(VisualizationManager.User.Sebi);
        ReaderCSV.ReadData(VisualizationManager.User.Marc);
        ReaderCSV.ReadData(VisualizationManager.User.Peter);
        ReaderCSV.ReadData(VisualizationManager.User.Gerard);
    }

    void OnApplicationQuit()
    {
       
        Debug.Log("Writting data... ");
        WriterCSV.WriterData(events);

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

        if (pendingEvents.Count == 0)
            return;

        // dispatch enqueued events that have not been dispatched already
        foreach (var e in pendingEvents)
            DispatchEvent(e);

        // add the dispatched events to the closed list
        events.AddRange(pendingEvents);

        // clear pending events
        pendingEvents.Clear();
        
    }

    public void AddEventByType(CUSTOM_EVENT_TYPE type)
    {
        if (player == null)
            return;
        Eventinfo e = new Eventinfo(userName.ToString(), type, player.transform.position, stage);
        pendingEvents.Add(e);
    }

    public void AddEventByType(string type)
    {
        AddEventByType(GetEventTypeFromString(type));
    }

   public static CUSTOM_EVENT_TYPE GetEventTypeFromString(string type)
    {
        return (CUSTOM_EVENT_TYPE)Enum.Parse(typeof(CUSTOM_EVENT_TYPE), type);
    }

    public static VisualizationManager.User GetUserTypeFromString(string userName)
    {
        return (VisualizationManager.User)Enum.Parse(typeof(VisualizationManager.User), userName);
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
                    ret.AddRange(joseEvents);
                    ret.AddRange(gerardEvents);
                }
                break;
            case VisualizationManager.User.Carlos:
                {
                    ret = carlosEvents;
                }
                break;
            case VisualizationManager.User.Gerard:
                {
                    ret = gerardEvents;
                }
                break;
            case VisualizationManager.User.Marc:
                {
                    ret = marcEvents;
                }
                break;
            case VisualizationManager.User.Peter:
                {
                    ret = joseEvents;
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
            case CUSTOM_EVENT_TYPE.LEVEL_ENTER:
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
    ACTIVATE_SWITCH,
    LEVEL_ENTER
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

    // Stage.
    public uint stage;

    // Position at which occurs the event.
    public Vector3 position;

    // TODO
    public Eventinfo(String player_name, CUSTOM_EVENT_TYPE type, Vector3 position, uint stage)
    {
        this.seconds = Time.realtimeSinceStartup;
        this.time_span = TimeSpan.FromSeconds(seconds);
        this.time = String.Format("{0:D2}:{1:D2}:{2:D2}", time_span.Hours, time_span.Minutes, time_span.Seconds);

        this.player_name = player_name;
        this.type = type;
        this.position = position;
        this.stage = stage;
    }
}