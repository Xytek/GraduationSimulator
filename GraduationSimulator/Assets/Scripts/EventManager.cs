using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, Action<EventParams>> _eventDictionary;
    private static EventManager _eventManager;

    // make sure that there is an EventManager instance in the scene
    public static EventManager instance
    {
        get
        {
            if (!_eventManager)
            {
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!_eventManager)
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                else
                    _eventManager.Init();
            }
            return _eventManager;
        }
    }

    void Init()
    {
        if (_eventDictionary == null)
            _eventDictionary = new Dictionary<string, Action<EventParams>>();
    }

    // subscription to an event
    public static void StartListening(string eventName, Action<EventParams> callback)
    {
        Action<EventParams> thisEvent;
        if (instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            // add another callback to the existing event
            thisEvent += callback;

            //Update the Dictionary
            instance._eventDictionary[eventName] = thisEvent;
        }
        else
        {
            // add callback to the new event
            thisEvent += callback;

            // add event to the Dictionary for the first time
            instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    // unsubscribe from an event
    public static void StopListening(string eventName, Action<EventParams> callback)
    {
        if (_eventManager == null)
            return;

        Action<EventParams> thisEvent;
        if (instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            // remove callback from the existing event
            thisEvent -= callback;

            //Update the Dictionary
            instance._eventDictionary[eventName] = thisEvent;
        }
        else
            Debug.Log("The event you are trying to unsubscribe from doesn't exist");
    }

    // trigger an event from the dictionary
    public static void TriggerEvent(string eventName, EventParams eventParam)
    {
        Action<EventParams> thisEvent = null;
        if (instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.Invoke(eventParam);
        else
            Debug.Log("The event: "+ eventName + ", does not have any listeners.");
    }
}

public struct EventParams
{
    public int intNr;
    public float floatNr;
    public CourseTypes courseType;
    public Color color;
    public string text;
}

