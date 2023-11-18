using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TankGame
{
    /// <summary>
    /// EventName
    /// </summary>

    public enum EEventName
    {
        EN_NONE = 0,
        EN_INPUT_MOVE_CMD = 1,
        EN_INPUT_STOP_MOVE_CMD = 2,
        EN_TIMELINE_EVENT = 3,
    }


    /// <summary>
    /// Event
    /// </summary>

    [System.Serializable]
    public class Event
#if USE_BASECLASS
 : BaseClass
#endif
    {
        private EEventName eventName;
        private EventReceiver receiver = null;
        public Dictionary<int, object> paramList;
        public bool Used = false;
        public bool sendLua = true;


        public Event()
        {
            paramList = new Dictionary<int, object>();
        }
        public Event(EEventName name)
        {
            eventName = name;
            paramList = new Dictionary<int, object>();
        }

        public EEventName GetName()
        {
            return eventName;
        }

        public void SetName(EEventName name)
        {
            eventName = name;
        }

        public void SetReceiver(EventReceiver r)
        {
            receiver = r;
        }

        public EventReceiver GetReceiver()
        {
            return receiver;
        }

        public void AddParam(string name, object value)
        {
            paramList[name.GetHashCode()] = value;
        }

        public object GetParam(string name)
        {
            object returnObj = null;
            paramList.TryGetValue(name.GetHashCode(), out returnObj);
            return returnObj;
        }

        public bool HasParam(string name)
        {
            if (paramList.ContainsKey(name.GetHashCode()))
            {
                return true;
            }
            return false;
        }

        public int GetParamCount()
        {
            return paramList.Count;
        }

        public Dictionary<int, object> GetParamList()
        {
            return paramList;
        }

        public void Reset()
        {
            eventName = EEventName.EN_NONE;
            Used = false;
            paramList.Clear();
            receiver = null;
            sendLua = true;
        }

        public string[] GetKeyArray()
        {
            return null;
        }

    }
}