using Constant;
using UnityEngine;

namespace TankGame
{
    public class MapFactory : MonoBehaviour
    {
        /// <summary>
        /// 创建地图对象
        /// </summary>
        /// <param name="goName"></param>
        /// <param name="vector3"></param>
        /// <param name="parent"></param>
        public static void CreateMapItem(string goName, Vector3 vector3, Transform parent)
        {
            GameObject prefab = AssetTool.GetSingleton().LoadPrefab(goName);
            GameObject go = Instantiate(prefab, vector3, Quaternion.identity, parent);
            GameContext.GameObjectMap.Add($"{vector3.x}-{vector3.y}", go);
        }


        /// <summary>
        /// 判断该位置是否有物体
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static bool IsEmpty(Vector3 vector3)
        {
            if (GameContext.GameObjectMap.ContainsKey($"{vector3.x}-{vector3.y}"))
                return false;

            return true;
        }
    }
}