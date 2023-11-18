using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TankGame
{
    /// <summary>
    /// EventReceiver
    /// </summary>

    public delegate bool LogicEventHandler(Event evt);

    public class EventReceiver
#if USE_BASECLASS
 : BaseClass
#endif
    {
        public LogicEventHandler handler;

        public EventReceiver(LogicEventHandler Handler)
        {
#if USE_BASECLASS
            BaseName = "EventReceiver";
#endif

            this.handler = Handler;
        }

        internal bool OnEvent(Event evt)
        {
            return handler(evt);
        }

        public virtual void RegisterEvent(EEventName eventName)
        {
            EventManager.GetSingleton().RegisterEvent(this, eventName);
        }

        public virtual void UnregisterEvent(EEventName eventName)
        {
            EventManager.GetSingleton().UnregisterEvent(this, eventName);
        }
    }
}