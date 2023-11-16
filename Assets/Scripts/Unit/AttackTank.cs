using Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TankGame
{
    public class AttackTank : TankBase
    {
        public AttackTank(string prefabName)
        {
            mType = TankType.Attacker;
            GameObject prefabObj = AssetTool.GetSingleton().LoadPrefab(prefabName);
            mBornEffPrefab = AssetTool.GetSingleton().LoadPrefab(GameConst.BornPrefab1);
            mBulltePrefab = AssetTool.GetSingleton().LoadPrefab(GameConst.PlayerBulletPrefab);
            mExploadEffPrefab = AssetTool.GetSingleton().LoadPrefab(GameConst.ExplodePrefab);
            mTankGameObject = GameObject.Instantiate(prefabObj);
        }
        protected override void Move(float _distance, bool isHorizon)
        {
            Vector2 position = mTankGameObject.transform.position;
            if (isHorizon)
                position.x += _distance;
            else
                position.y += _distance;

            mTankGameObject.transform.position = position;
        }
        protected override void Roate(float _rotation)
        {
            mTankGameObject.transform.eulerAngles = new Vector3(0, 0, _rotation);
        }
        protected override void Attack()
        {
            GameObject bullteObj = GameObject.Instantiate(mBulltePrefab);
            bullteObj.transform.position = mTankGameObject.transform.position;
            bullteObj.transform.rotation = Quaternion.Euler(mTankGameObject.transform.eulerAngles);
        }
        protected override void Die()
        {
            // 无敌状态不会死亡
            if (mIsInvincible)
                return;

            DestroySelf();

            Debug.Log("hp is " + mHP);
            if (mHP > 0)
            {
                Reborn();
            }
            else
            {
                mIsDie = true;
            }
        }

        protected override void Reborn()
        {
            // 重生
            GameObject bornEffObj = GameObject.Instantiate(mBornEffPrefab);
            bornEffObj.transform.position = GameConst.Player1BornVector3;
            bornEffObj.transform.rotation = Quaternion.identity;
        }

        protected override void DestroySelf()
        {
            if (mTankGameObject != null)
                GameObject.DestroyImmediate(mTankGameObject);

            mHP -= 1;

            // 爆炸
            GameObject exploadEffObj = GameObject.Instantiate(mExploadEffPrefab);
            exploadEffObj.transform.position = mTankGameObject.transform.position;
            exploadEffObj.transform.rotation = mTankGameObject.transform.rotation;
        }
    }
}
