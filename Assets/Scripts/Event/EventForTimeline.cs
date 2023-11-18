using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TankGame
{
    /// <summary>
    /// EventForTimeline
    /// </summary>
    public class EventForTimeline : MonoBehaviour
    {
        public EventForTimeline()
        {

        }
        public void OnEvent(string timeStr)
        {
            if(GameManager.GetSingleton() == null || GameManager.GetSingleton().isEditor)
                Debug.Log("OnEvent " + timeStr);
            else
            {
                Event evt = EventManager.GetSingleton().CreateEvent(EEventName.EN_TIMELINE_EVENT);
                evt.AddParam("String", timeStr);
                EventManager.GetSingleton().PostEvent(evt);
            }
        }

    }
}