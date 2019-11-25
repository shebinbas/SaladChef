using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CustomAppEvent", menuName = "ScriptableObjects/CustomAppEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    public List<CallbackEvent> listeners;

    public void Subscribe(CallbackEvent listener)
    {
        listeners.Add(listener);
    }

    public void UnSubscribe(CallbackEvent listener)
    {
        listeners.Remove(listener);
    }

    public void InvokeEvent(object sender)
    {
        foreach (CallbackEvent listener in listeners.ToArray())
            listener.Invoke(sender);
    }

    public void InvokeEvent(string arg)
    {
        foreach (CallbackEvent listener in listeners.ToArray())
            listener.Invoke(arg);
    }

    public void InvokeEvent(GameObject arg = null)
    {
        foreach (CallbackEvent listener in listeners.ToArray())
            listener.Invoke(arg);
    }
}

