using UnityEngine;

namespace TankGame
{

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class ThumbController : MonoBehaviour
    {
        public enum Mode
        {
            /// <summary>
            /// 固定位置
            /// </summary>
            Fixed,
            /// <summary>
            /// 随手指移动
            /// </summary>
            Flexible
        }

        #region 位置和样式
        // 序列化用
        [HideInInspector]
        [SerializeField]
        Vector2 m_Offset;
        public Vector2 offset
        {
            get { return m_Offset; }
            set
            {
                m_Offset = value; // MARK 可以在这里更新UI 
            }
        }
        [HideInInspector]
        [SerializeField]
        Vector2 m_Scale = Vector2.one;
        public Vector2 scale
        {
            get { return m_Scale; }
            set
            {
                m_Scale = value; // MARK 可以在这里更新UI 
            }
        }
        [HideInInspector]
        [SerializeField]
        Vector2Int m_Size = Vector2Int.one;
        public Vector2Int size
        {
            get { return m_Size; }
            set
            {
                m_Size = value; // MARK 可以在这里更新UI 
            }
        }
        [HideInInspector]
        [SerializeField]
        Mode m_Mode = Mode.Fixed;
        public Mode mode
        {
            get { return m_Mode; }
            set
            {
                m_Mode = value; // MARK 可以在这里更新UI 
            }
        }
//         [HideInInspector]
//         [SerializeField]
//         UIAtlas m_ThumbAtlas;
//         [HideInInspector]
//         [SerializeField]
//         UIAtlas m_PadAtlas;
        [HideInInspector]
        [SerializeField]
        string m_PadSpriteName;
        [HideInInspector]
        [SerializeField]
        string m_ThumbSpriteName;

        #endregion

        #region Data
        private bool m_EnableTouch;
        public bool isEnable
        {
            get
            {
                return m_EnableTouch;
            }
            set
            {
                m_EnableTouch = value;
            }
            
        }
        private Transform m_MoveJoystick;
        private Transform m_PressArea;
        private Transform m_Pad;
        private Transform m_Thumb;
        //private UISprite m_Arrow;

        private Vector3 m_MoveJoystickOriLocalPosition;
        private Vector3 m_ThumbOriLocalPostion;
        private Quaternion m_ThumbOriLocalRotation;
        private Vector2 m_ThumbScreenPosition = Vector3.zero;

        private float m_PadRadius;
        private float m_ThumbRadius;
        private float m_ThumbMoveRadius;

        private bool m_UseDirectionArrow = false;

        private int disableCount;

        //public UIPanel panel { get; private set; }
        public Vector2 joystickAxis { get; private set; }

        #endregion

        private void Awake()
        {
            if (Application.isPlaying)
            {
//                 m_MoveJoystick = transform.Find("MoveJoystick");
//                 m_MoveJoystickOriLocalPosition = m_MoveJoystick.localPosition;
// 
//                 panel = GetComponent<UIPanel>();
// 
//                 m_PressArea = m_MoveJoystick.Find("PressArea");
//                 UIEventListener.Get(m_PressArea.gameObject).onDrag = OnDrag;
//                 UIEventListener.Get(m_PressArea.gameObject).onDragEnd = OnDragEnd;
//                 UIEventListener.Get(m_PressArea.gameObject).onPress += OnPress;
// 
//                 m_Pad = m_MoveJoystick.Find("Pad");
//                 m_PadRadius = m_Pad.GetComponent<UISprite>().width / 2;
// 
//                 m_Thumb = m_MoveJoystick.Find("Thumb");
//                 m_ThumbRadius = m_Thumb.GetComponent<UISprite>().width / 2;
// 
//                 m_ThumbOriLocalRotation = m_Thumb.localRotation;
//                 m_ThumbOriLocalPostion = m_Thumb.localPosition;
// 
//                 m_Arrow = m_Thumb.Find("Arrow").GetComponent<UISprite>();
//                 m_UseDirectionArrow = m_Arrow != null && m_UseDirectionArrow;
//                 if (m_UseDirectionArrow)
//                 {
//                     m_Arrow.alpha = 0f;
//                 }
//                 else
//                 {
//                     if (m_Arrow != null)
//                         m_Arrow.enabled = false;
//                 }
// 
//                 m_ThumbMoveRadius = m_PadRadius - m_ThumbRadius - 5;
// 
//                 UICamera.onScreenResize += OnScreenResize;
            }
        }

        private void OnDestroy()
        {
            //UICamera.onScreenResize -= OnScreenResize;
        }


        private void Update()
        {
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }


        #region Public
        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetEnable(bool enable)
        {
            //enable = enable & gameObject.activeSelf;
            if (enable)
            {
                disableCount--;
                if (disableCount <= 0)
                {
                    disableCount = 0;
                    m_EnableTouch = enable;
                }
            }
            else
            {
                disableCount++;
                m_EnableTouch = enable;
            }
            startTouch = false;

            //joystickAxis = Vector3.zero;
            //m_Thumb.localPosition = Vector2.zero;
        }

        public void ResetThumbPos()
        {
            joystickAxis = Vector3.zero;
            m_Thumb.localPosition = Vector2.zero;
        }

        bool startTouch = false;
        public void SetThumbPos(Vector2 dir)
        {
            if (!startTouch /*&& m_EnableTouch*/)
            {
                dir = dir.normalized * m_ThumbMoveRadius;
                m_Thumb.localPosition = new Vector3(dir.x, dir.y, 0);
            }
        }

        public void SetThumbPos(float x, float y)
        {
            SetThumbPos(new Vector2(x, y));
        }

        #endregion

        #region Life & Callback

        private void StartTouch()
        {
            ////if (!m_EnableTouch)
            ////    return;

            //if (m_ThumbScreenPosition == Vector2.zero)
            //    m_ThumbScreenPosition = UICamera.currentCamera.WorldToScreenPoint(m_Thumb.position);

            //Vector2 touchDir = UICamera.lastEventPosition - m_ThumbScreenPosition;

            //// 看了下源代码，里面有个开方
            //Vector2 pos = Vector2.ClampMagnitude(touchDir, m_ThumbMoveRadius);
            //m_Thumb.localPosition = new Vector3(pos.x, pos.y, 0);

            //// ClampMagnitude 和 normalized 都有一个开方，无所谓用哪个了
            ////if (touchDir.sqrMagnitude > radius * radius)
            ////{
            ////    var s = touchDir.normalized * radius;
            ////    m_Thumb.localPosition = new Vector3(s.x, s.y);
            ////}
            ////else
            ////    m_Thumb.localPosition = new Vector3(touchDir.x, touchDir.y, 0);

            //joystickAxis = touchDir.normalized;

            //startTouch = true;

            //// 旋转 需求里现在不要旋转
            ////float rot_z = Mathf.Atan2(joystickAxis.y, joystickAxis.x) * Mathf.Rad2Deg;
            ////m_Thumb.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

            //if (m_UseDirectionArrow)
            //    m_Arrow.alpha = 200 / 255f;
        }

        private void EndTouch()
        {
            m_Thumb.localPosition = m_ThumbOriLocalPostion;
            m_Thumb.localRotation = m_ThumbOriLocalRotation;
            if (m_UseDirectionArrow)
                //m_Arrow.alpha = 0f;

            joystickAxis = Vector2.zero;
            startTouch = false;
        }

        private void CalcJoystickPosition(bool state)
        {
            if (m_Mode == Mode.Flexible)
            {
                if (state)
                {
                    SetAnchor(false);
                    //m_MoveJoystick.localPosition = m_MoveJoystick.InverseTransformVector(UICamera.lastWorldPosition);
                    //m_ThumbScreenPosition = UICamera.currentCamera.WorldToScreenPoint(m_Thumb.position);
                }
                else
                {
                    m_MoveJoystick.localPosition = m_MoveJoystickOriLocalPosition;
                    //m_ThumbScreenPosition = UICamera.currentCamera.WorldToScreenPoint(m_Thumb.position);
                    SetAnchor(true);
                }
            }
        }

        private void OnPress(GameObject go, bool state)
        {
            // 固定模式下，按下就直接开跑
            if (mode == Mode.Fixed)
            {
                if (state)
                    StartTouch();
                else
                    EndTouch();
            }
            // 灵活模式下，按下摇杆跟随按下的位置，不用跑
            else
            {
                CalcJoystickPosition(state);
            }
        }

        private void OnDrag(GameObject go, Vector2 delta)
        {
            StartTouch();
        }

        private void OnDragEnd(GameObject go)
        {
            EndTouch();
        }

        private void OnScreenResize()
        {
            //m_ThumbScreenPosition = UICamera.currentCamera.WorldToScreenPoint(m_Thumb.position);
        }

        #endregion

        #region Tools
        private void SetAnchor(bool enable)
        {
//             if (enable)
//             {
//                 var widget = m_MoveJoystick.GetComponent<UIWidget>();
//                 widget.SetAnchor(transform);
//             }
//             else
//             {
//                 var widget = m_MoveJoystick.GetComponent<UIWidget>();
//                 if (widget.isAnchored)
//                 {
//                     widget.leftAnchor.target = null;
//                     widget.rightAnchor.target = null;
//                     widget.bottomAnchor.target = null;
//                     widget.topAnchor.target = null;
//                 }
//             }
        }
        #endregion
    }

}
