using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class AssetTool : Singleton<AssetTool>
    {
        static Dictionary<string, GameObject> mAllPrefabs = new Dictionary<string, GameObject>();
        public bool UnloadAllAsset = false;

        GameObject _LoadPrefab(string name)
        {
            if (mAllPrefabs.ContainsKey(name))
            {
                return mAllPrefabs[name];
            }
            else
            {
                GameObject obj = Resources.Load(name, typeof(GameObject)) as GameObject;
                if (obj == null)
                    Debug.LogError($"没有该资源 ,Name :{name}");
                mAllPrefabs.Add(name, obj);
                return obj as GameObject;
            }
        }
        GameObject _LoadParticlePrefab(string name, string baseName)
        {

            if (mAllPrefabs.ContainsKey(name))
            {
                return mAllPrefabs[name];
            }
            else
            {
                GameObject obj = Resources.Load(name, typeof(GameObject)) as GameObject;
                if (obj == null)
                    return _LoadPrefab(baseName);
                else
                {
                    mAllPrefabs.Add(name, obj);
                    return obj as GameObject;
                }
            }
        }
        public GameObject LoadPrefab(string name)
        {
            return _LoadPrefab(name);
        }
        public bool PrefabLoaded(string name)
        {
            return mAllPrefabs.ContainsKey(name);
        }
        public void UnLoadAllPrefab()
        {
            UnloadAllAsset = true;
            mAllPrefabs.Clear();
            UnloadAllAsset = false;
        }

    }
}
