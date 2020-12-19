using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

public class CustomEvent : MonoBehaviour
{
    // Seconds since the application is running.
    float seconds;
    // Timespan from seconds.
    TimeSpan time_span;
    // String storing the time in date format.
    String time;

    // Type of event.
    CUSTOM_EVENT_TYPE type;

    // Player id
    Int64 player_id;

    // Stage
    int stage;

    // TODO
    public CustomEvent()
    {
        seconds = Time.realtimeSinceStartup;
        time = String.Format("{0:D2}:{1:D2}:{2:D2}", time_span.Hours, time_span.Minutes, time_span.Seconds);
        
        type = CUSTOM_EVENT_TYPE.NONE;
    }
}

public class CustomEventPosition : CustomEvent
{

}

public class CustomEventAttack : CustomEvent
{

}

public class CustomEventJump : CustomEvent
{

}

public class CustomEventDeath : CustomEvent
{

}

public class CustomEventReceiveDamage : CustomEvent
{

}

public class CustomEventEnemyKilled : CustomEvent
{

}

public class CustomEventActivateSwitch : CustomEvent
{

}

// Might be irrelevant, as position will be very similar.
public class CustomEventWinGame : CustomEvent
{

}

// Might be irrelevant, as position will be very similar.
public class CustomEventEnterGame : CustomEvent
{

}
