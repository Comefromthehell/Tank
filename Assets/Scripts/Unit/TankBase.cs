using Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TankGame
{
    public enum TankType
    {
        None = 0,
        Attacker = 1, //DPS
        Support = 2,//辅助
    }
    public abstract class TankBase
    {
        protected TankType mType;
        protected float mMoveSpeed = 3f;
        protected float mBulletCD = 0.1f;
        protected int mHP = 2;
        /*无敌时间*/
        protected float mInvincibleTimeVal = 3f;

        protected GameObject mTankGameObject = null;
        protected GameObject mBulltePrefab = null;
        protected GameObject mBornEffPrefab = null;
        protected GameObject mExploadEffPrefab = null;

        protected bool mIsDie = false;
        protected bool mIsInvincible = false;
        public TankBase() { }
        public TankType Type
        {
            get { return mType; }
        }
        public float MoveSpeed
        {
            get { return mMoveSpeed; }
        }
        public float BulletCD
        {
            get { return mBulletCD; }
        }
        public GameObject TankGameObject
        {
            get { return mTankGameObject; }
        }
        public float InvincibleTimeVal
        {
            get { return mInvincibleTimeVal; }
        }
        public int HP
        {
            get { return mHP; }
        }
        protected virtual void Move(float _distance, bool isHorizon) { }
        protected virtual void Roate(float _rotation) { }
        protected virtual void Attack() { }
        protected virtual void Die() { }
        protected virtual void Reborn() { }
        protected virtual void DestroySelf() { }
        public void SetPosition(Vector3 position)
        {
            mTankGameObject.transform.position = position;
        }
    }
}
