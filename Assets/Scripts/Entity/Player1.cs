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
        //旋转速度
        private float rotaSpeed = 1;
        //坦克旋转角度
        private float tankRotationZ = 0f;
        /*枪口方向*/
        private Vector3 bulletEulerAngles;

        /*攻击冷却时间*/
        private float bulletCoolTime;

        /*护盾时间*/
        private float protectTimeVal = 3;

        private AudioSource tankAudio;


        private void Awake()
        {
            tankAudio = GetComponent<AudioSource>();
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
            bulletCoolTime += Time.deltaTime;

            if ((!(Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.J))) || !(bulletCoolTime >= 0.5f)) return;
            var go = Resources.Load<GameObject>(GameConst.PlayerBulletPrefab);
            Instantiate(go, transform.position, Quaternion.Euler(bulletEulerAngles));
            bulletCoolTime = 0f;
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
            var go = Resources.Load<GameObject>(GameConst.ExplodePrefab);
            Instantiate(go, transform.position, transform.rotation);
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
        private static void Relive()
        {
            if (GameContext.Player1Hp <= 0) return;
            // 重生
            var go = Resources.Load<GameObject>(GameConst.BornPrefab1);
            Instantiate(go, GameConst.Player1BornVector3, Quaternion.identity);
        }

        private void Move()
        {
            Vector2 position = transform.position;

            Vector2 velocityX = gameObject.transform.rotation * new Vector2(moveSpeed, 0);
            Vector2 velocityY = gameObject.transform.rotation * new Vector2(0, moveSpeed);
            //移动
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                tankRotationZ += rotaSpeed;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                tankRotationZ -= rotaSpeed;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                position += velocityY * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                position -= velocityY * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                tankRotationZ += rotaSpeed;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                tankRotationZ -= rotaSpeed;
            }
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