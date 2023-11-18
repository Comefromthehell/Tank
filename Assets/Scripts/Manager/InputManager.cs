using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TankGame
{
    public class InputManager : Singleton<InputManager>
    {
        public Vector2 mMoveDir = new Vector2();
        public ThumbController thumbController { get; private set; }
        public void Init()
        {

        }
        public void UnInit()
        {

        }
        public void Update(float fDeltaTime)
        {
            //float h = (Time.timeScale < 0.01 ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal")) + (thumbController != null ? thumbController.joystickAxis.x : 0.0f);
            //float v = (Time.timeScale < 0.01 ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical")) + (thumbController != null ? thumbController.joystickAxis.y : 0.0f);

            float h = Input.GetAxis("Player1Horizontal");
            float v = Input.GetAxis("Player1Vertical");

            //摇杆节点，后期可能会用
            //if (thumbController != null)
            //    thumbController.SetThumbPos(h, v);

            Vector2 moveDir = new Vector2(h, v);
            if (moveDir != Vector2.zero /*&& thumbController != null && thumbController.isEnable*/)
            {
                Event evt = EventManager.GetSingleton().CreateEvent(EEventName.EN_INPUT_MOVE_CMD);
                mMoveDir = moveDir;
                evt.AddParam("rightMove", moveDir.x);
                evt.AddParam("forwardMove", moveDir.y);

                Debug.LogWarning($"当前的h值为{h} v值为{v}");
                EventManager.GetSingleton().SendEvent(evt);
            }
            else
            {
                if (mMoveDir != moveDir)
                {
                    Event evt = EventManager.GetSingleton().CreateEvent(EEventName.EN_INPUT_STOP_MOVE_CMD);
                    EventManager.GetSingleton().PostEvent(evt);
                }
                mMoveDir = moveDir;
            }
        }
    }
}
