using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TankGame
{
    public static class CommonUtilty
    {
        public static void AddChild(GameObject parent, GameObject child)
        {
            child.transform.SetParent(parent.gameObject.transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
        }
    }
}
