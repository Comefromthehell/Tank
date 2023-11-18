using System;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    /// <summary>
    /// EventManager
    /// </summary>
    public class EventManager : Singleton<EventManager>
    {
        public delegate void CSharpToLuaNofication(Event evt);
        public static CSharpToLuaNofication onSendLuaNotification;

        Dictionary<EEventName, List<EventReceiver>> eventReceiverList;
        List<Event> eventList_Add;
        List<Event> eventList_Update;

        List<Event> eventPool;
        List<Event> notUsePools;
        const int poolIncreaseCount = 50;

        List<List<EventReceiver>> poolProcessReceiverList;
        const int receiverPoolIncreaseCount = 20;

        Dictionary<string, EEventName> mAllEEventName = new Dictionary<string, EEventName>();
        public Dictionary<EEventName, string> mEnumStringMap = new Dictionary<EEventName, string>();

        public EventManager()
        {
            eventReceiverList       = new Dictionary<EEventName, List<EventReceiver>>();
            eventList_Add           = new List<Event>();
            eventList_Update        = new List<Event>();
            eventPool   = new List<Event>();
            notUsePools = new List<Event>();
            poolProcessReceiverList = new List<List<EventReceiver>>();

            foreach (EEventName item in Enum.GetValues(typeof(EEventName)))
            {
                mAllEEventName[item.ToString()] = item;
                mEnumStringMap[item] = item.ToString();
            }
        }

        public void Init()
        {
            increasePool(EEventName.EN_NONE);
            increaseProcessReceiverPool();
        }

        void increasePool(EEventName name)
        {
            for (int i = 0; i < poolIncreaseCount; ++i)
            {
                eventPool.Add(new Event(name));
            }
        }

        void increaseProcessReceiverPool()
        {
            for (int i = 0; i < receiverPoolIncreaseCount; ++i)
            {
                poolProcessReceiverList.Add(new List<EventReceiver>());
            }
        }

        public List<EventReceiver> GetProcessReceiverList()
        {
            int count = poolProcessReceiverList.Count;
            for (int i = 0; i < count; ++i)
            {
                if (poolProcessReceiverList[i].Count == 0)
                {
                    return poolProcessReceiverList[i];
                }
            }

            increaseProcessReceiverPool();
            return GetProcessReceiverList();
        }

        public void RetreiveProcessReceiverList(List<EventReceiver> list)
        {
            int count = poolProcessReceiverList.Count;
            list.Clear();
        }

        public Event CreateEvent(EEventName name)
        {
            if (name == EEventName.EN_NONE)
                return null;

            int count = eventPool.Count;
            for (int i = 0; i < count; ++i)
            {
                if (eventPool[i].Used == false)
                {
                    eventPool[i].SetName(name);
                    eventPool[i].Used = true;
                    return eventPool[i];
                }
            }

            increasePool(name);
            return CreateEvent(name);
        }

        public void RetreiveEvent(Event evt)
        {
            if (evt != null)
                evt.Reset();
            else
                Debug.Log("123123");
        }

        public void Update(float fDeltaTime)
        {
            foreach (Event evt in eventList_Add)
            {
                eventList_Update.Add(evt);
            }
            eventList_Add.Clear();
           
            foreach (Event evt in eventList_Update)
            {
                SendEvent(evt, evt.GetReceiver());
            }
            eventList_Update.Clear();
            int count = eventPool.Count;
            if(count > 210)
            {
                notUsePools.Clear();
                foreach (var value in eventPool)
                {
                    if (!value.Used)
                        notUsePools.Add(value);
                }
                if(notUsePools.Count > 50)
                {
                    foreach(var value in notUsePools)
                    {
                        eventPool.Remove(value);
                    }
                }
            }
        }

        //同步发送事件
        public void SendEvent(Event evt)
        {
            EEventName eventName = evt.GetName();
            List<EventReceiver> listRecr = null;
            if (eventReceiverList.TryGetValue(eventName, out listRecr))
            {
                List<EventReceiver> recevierList = GetProcessReceiverList();
                recevierList.AddRange(listRecr);
                foreach (EventReceiver r in recevierList)
                {
                    if(r != null)
                        r.OnEvent(evt);
                }
                RetreiveProcessReceiverList(recevierList);
            }
            if (evt.sendLua)
            {
                onSendLuaNotification?.Invoke(evt);
            }

            RetreiveEvent(evt);
        }

        //对指定的接受者同步发送事件
        public void SendEvent(Event evt, EventReceiver receiver)
        {
            if (receiver != null)
            {
                receiver.OnEvent(evt);
                RetreiveEvent(evt);
            }
            else
            {
                SendEvent(evt);
            }
        }

        public void PostEvent(Event evt)
        {
            eventList_Add.Add(evt);
        }

        public void PostEvent(Event evt, EventReceiver receiver)
        {
            if (receiver != null)
                evt.SetReceiver(receiver);

            PostEvent(evt);
        }

        public void RegisterEvent(EventReceiver receiver, EEventName eventName)
        {
			List<EventReceiver> list;
			if (eventReceiverList.TryGetValue(eventName, out list)) 
			{
                foreach (EventReceiver r in list)
                {
                    if (r == receiver)
                        return;
                }
			}
			else
			{
				list = new List<EventReceiver>();
				eventReceiverList[eventName] = list;
			}

            list.Add(receiver);
        }

        public void UnregisterEvent(EventReceiver receiver, EEventName eventName)
        {
            if (eventReceiverList.ContainsKey(eventName))
            {
                foreach (EventReceiver r in eventReceiverList[eventName])
                {
                    if (r == receiver)
                    {
                        eventReceiverList[eventName].Remove(r);
                        break;
                    }
                }
            }
        }

        public void UnregisterRecevier(EventReceiver receiver)
        {
            foreach (EEventName key in eventReceiverList.Keys)
            {
                foreach (EventReceiver r in eventReceiverList[key])
                {
                    if (r == receiver)
                    {
                        eventReceiverList[key].Remove(r);
                        break;
                    }
                }
            }
        }

        public void UnregisterAllRecevier()
        {
            eventReceiverList.Clear();
        }

        public void DebugLog()
        {
            int usedCount = 0;
            int count = eventPool.Count;
            for (int i = 0; i < count; ++i)
            {
                if (eventPool[i].Used == true)
                {
                    usedCount++;
                }
            }
            Debug.LogError("Event used Count : " + usedCount);
        }

        public EEventName GetEEventName(string name)
        {
            EEventName _type;
            if (mAllEEventName.TryGetValue(name, out _type))
                return _type;
            return EEventName.EN_NONE;
        }

        //todo 以后有需要再写
        #region Lua调用区
        /// <summary>
        /// 供Lua调用，从Lua中可以发事件给C#
        /// </summary>
        /// <param name="name">事件的名字，字符串形式</param>
        /// <param name="luaEvt">Lua中的Event类</param>
//         public void SendEventFromLua(string name, XLua.LuaTable luaEvt)
//         {
//             if (string.IsNullOrEmpty(name))
//             {
//                 return;
//             }
// 
//             Event evt = CreateEvent(GetEEventName(name));
//             if (evt != null)
//             {
//                 var dict = luaEvt.GetInPath<Dictionary<string, object>>("mParamList");
//                 var keyList = luaEvt.GetInPath<List<string>>("mParamList.keyList");
//                 if (dict == null || keyList == null)
//                 {
//                     Debug.LogError($"{name} 事件异常");
//                     return;
//                 }
// 
//                 foreach (var key in keyList)
//                 {
//                     evt.AddParam(key, dict[key]);
//                 }
//                 // 这个方法本来就是从lua中发送到C#中，所以在这里就不需要再发给Lua了
//                 evt.sendLua = false;
//                 SendEvent(evt);
//             }
//         }
        #endregion

    }
}