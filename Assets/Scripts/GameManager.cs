using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TankGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private bool isInited = false;
        public bool isEditor = false;
        //进度条
        private int mInitProgressValue = 50;
        private int mCurInitProgress = 50;
        GameObject mInitProgressObj = null;
        void Awake()
        {

            if (isEditor)
                return;

            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);

                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        void Start()
        {
            UnityEngine.Debug.Log("GameManager start");
            if (isEditor)
                return;

            StartCoroutine(Init());
        }
        private IEnumerator Init()
        {
            if (!isEditor)
            {
                InputManager.GetSingleton().Init();

                yield return UpdateInitProgress();
                isInited = true;
            }
            yield return null;
        }

        void Update()
        {
            if (isEditor)
                return;

            if (!isInited)
                return;
            float fDeltaTime = Time.deltaTime;

            InputManager.GetSingleton().Update(fDeltaTime);
        }
        void OnDestroy()
        {
            if (isEditor)
            {
                return;
            }

            InputManager.GetSingleton().UnInit();
        }
        public static GameManager GetSingleton()
        {
            return instance;
        }
        public bool UpdateInitProgress(bool end = false)
        {
            if (end)
                mCurInitProgress = 99;

            ++mCurInitProgress;

            if (mCurInitProgress > 100)
            {
                mCurInitProgress = 100;
            }

            return true;
        }
    }
}
