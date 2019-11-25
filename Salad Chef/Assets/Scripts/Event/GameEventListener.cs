using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameEventData
{
    public GameEvent eventObject;
    public CallbackEvent callBack;

    public void Attach()
    {
        eventObject.Subscribe(callBack);
    }
    public void Detach()
    {
        eventObject.UnSubscribe(callBack);
    }
}


public class GameEventListener : MonoBehaviour
{
    [SerializeField] List<GameEventData> callBacks;

    private void OnEnable()
    {
        foreach (GameEventData eventData in callBacks)
            eventData.Attach();
    }

    private void OnDisable()
    {
        foreach (GameEventData eventData in callBacks)
            eventData.Detach();
    }

    private void OnDestroy()
    {

    }
}