using System;
using Constant;
using UnityEngine;

namespace Entity
{
    /**
 * 1. 攻击
 * 2. 移动
 * 3. 死亡
 * 4. 攻击
 */
    public class Player1 : MonoBehaviour
    {
        //移动速度
        private float moveSpeed = 3;
        //坦克旋转角度
        private float tankRotationZ = 0f;
        /*枪口方向*/
        private Vector3 bulletEulerAngles;
        private float bulletCD = 0.1f;
        /*攻击冷却时间*/
        private float attackTime;

        /*护盾时间*/
        private float protectTimeVal = 3;

        private AudioSource tankAudio;
        private GameObject bulltePrefab = null;
        private GameObject exploadEffPrefab = null;
        private GameObject bornEffPrefab = null;

        private void Awake()
        {
            tankAudio = GetComponent<AudioSource>();
            bulltePrefab = TankGame.AssetTool.GetSingleton().LoadPrefab(GameConst.PlayerBulletPrefab);
            exploadEffPrefab = TankGame.AssetTool.GetSingleton().LoadPrefab(GameConst.ExplodePrefab);
            bornEffPrefab = TankGame.AssetTool.GetSingleton().LoadPrefab(GameConst.BornPrefab1);
        }

        private void Update()
        {
            if (GameContext.IsGameOver)
            {
                return;
            }

            Attack();
            CheckShield();
        }

        private void FixedUpdate()
        {
            if (GameContext.IsGameOver)
            {
                return;
            }

            Move();
        }


        private void CheckShield()
        {
            if (!(protectTimeVal > 0)) return;
            protectTimeVal -= Time.deltaTime;
            if (protectTimeVal <= 0)
            {
                transform.Find("Shield").GetComponent<Renderer>().enabled = false;
            }
        }


        private void Attack()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (attackTime < bulletCD)
                {
                    attackTime += Time.deltaTime;
                }
                else
                {
                    Instantiate(bulltePrefab, transform.position, Quaternion.Euler(bulletEulerAngles));
                    attackTime -= bulletCD;
                }
            }
        }


        /// <summary>
        /// 死
        /// 1. 销毁
        /// 2. 爆炸效果，0
        /// 3. 在出生地重新实例化
        /// 4. 播放重生效果
        /// </summary>
        private void Die()
        {
            // 无敌状态不会死亡
            if (protectTimeVal > 0)
            {
                return;
            }

            Destroy(gameObject);

            // 爆炸
            Instantiate(exploadEffPrefab, transform.position, transform.rotation);
            GameContext.Player1Hp -= 1;
            Debug.Log("hp is " + GameContext.Player1Hp);

            if (GameContext.Player1Hp == 0 && GameContext.Player2Hp == 0)
            {
                GameContext.IsGameOver = true;
                return;
            }

            Relive();
        }


        /**
     * 重生
     * 只能在玩家的死亡方法调用
     */
        private void Relive()
        {
            if (GameContext.Player1Hp <= 0) return;
            // 重生
            Instantiate(bornEffPrefab, GameConst.Player1BornVector3, Quaternion.identity);
        }

        private void Move()
        {
            Vector2 position = transform.position;

            float distanceCnt = moveSpeed * Time.fixedDeltaTime;
            //移动
            if (Input.GetKey(KeyCode.RightArrow))
            {
                position.x += distanceCnt;
                tankRotationZ = -90;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                position.x -= distanceCnt;
                tankRotationZ = 90;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                position.y += distanceCnt;
                tankRotationZ = 0;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                position.y -= distanceCnt;
                tankRotationZ = 180;
            }

            //Vector2 velocityY = gameObject.transform.rotation * new Vector2(0, moveSpeed);
            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    tankRotationZ += rotaSpeed;
            //}
            //else if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    tankRotationZ -= rotaSpeed;
            //}

            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    position += velocityY * Time.fixedDeltaTime;
            //}
            //else if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    position -= velocityY * Time.fixedDeltaTime;
            //}

            //else if (Input.GetKey(KeyCode.Q))
            //{
            //    tankRotationZ += rotaSpeed;
            //}
            //else if (Input.GetKey(KeyCode.E))
            //{
            //    tankRotationZ -= rotaSpeed;
            //}

            transform.eulerAngles = new Vector3(0, 0, tankRotationZ);
            bulletEulerAngles = new Vector3(0, 0, tankRotationZ);

            transform.position = position;

            /* 声音部分
            var audioClip = Resources.Load<AudioClip>(GameConst.DrivingAudio);
            tankAudio.Stop();
            tankAudio.clip = audioClip;
            if (!tankAudio.isPlaying)
            {
                AudioSource.PlayClipAtPoint(audioClip, transform.position);
            }
            */
        }

    }
}