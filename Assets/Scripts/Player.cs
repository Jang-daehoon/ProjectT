using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace HoonsCodes
{
    public class Player : Character
    {
        [Header("DashInfo")]
        [SerializeField] private Vector3 dir;
        [SerializeField] private float rotSpeed;
        [SerializeField] private bool isDash;
        [SerializeField] private float dashSpeed;  // 대시 속도
        [SerializeField] private float dashPower;   // 대시 파워
        [SerializeField] private float dashDuration = 0.5f; // 대시 지속 시간; // 대시 지속 시간
        [SerializeField] private float dashCooltime = 1f;   // 대시 쿨타임
        [SerializeField] private float dashDistance = 5f;  // 대시 거리
        [Header("----------------------------")]
        [Header("SkillInfo")]
        [SerializeField] private bool usingSkillX;
        [SerializeField] private bool isAttack;
        [SerializeField] private bool isHit;
        [Header("----------------------------")]
        [Header("ProjectileInfo")]
        public float projectileSpeed;
        public Transform firePoint; // 총알 발사 위치
        public BulletProjectile bulletProjectile;
        public ParticleSystem fireParticle;
        public ParticleSystem xSkillParticle;

        // Delegate 선언
        private delegate IEnumerator SkillDelegate();
        private SkillDelegate currentSkill;  // 현재 사용할 스킬을 담을 변수

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            animator = GetComponent<Animator>();

            InitPlayerStatus();
        }

        private void Update()
        {
            Move();
            PlayerRotation();

            if (Input.GetKeyDown(KeyCode.Z) && isAttack == false)
            {
                Attack();
            }
            if(Input.GetKeyDown(KeyCode.X) && usingSkillX == false)
            {
                animator.SetTrigger("SkillX");
            }
            if(Input.GetKeyDown(KeyCode.Space) && isDash == false)
            {
                StartCoroutine(Dodge());    
            }
        }

        public override void Move()
        {
            if(isAttack == false && usingSkillX == false && isDash == false && isDead == false )
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                dir = new Vector3(horizontal, 0, vertical).normalized;  //정규화

                transform.position += dir * moveSpeed * Time.deltaTime;
                animator.SetFloat("Speed", dir.magnitude);
            }
        }
        public void PlayerRotation()
        {
            if (dir != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);// 해당 방향을 바라봄
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            }
        }

        public void Attack()
        {
            if(!isAttack)  // 공격이 진행 중이 아닐 때만 실행
            {
                animator.SetTrigger("Attack");
                isAttack = true;  // 공격 상태로 변경
                StartCoroutine(FireArrow());
            }
        }
        public void XSkill()
        {
            ParticleSystem xParticle = Instantiate(xSkillParticle, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            ParticlePlay(xParticle);
            // 파티클이 끝날 때 Destroy 하기 위해 StopAction 설정
            var main = xParticle.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
        }
        private IEnumerator FireArrow()
        {
            BulletProjectile Arrow = Instantiate(bulletProjectile, firePoint.position, transform.rotation);
            Arrow.Speed = projectileSpeed;
            Arrow.Damage = dmgValue;
            ParticlePlay(fireParticle);
            yield return new WaitForSeconds(atkSpeed);
            isAttack = false;   
        }
        private IEnumerator Dodge()
        {
            isDash = true;
            animator.SetTrigger("Dodge");
            
            yield return new WaitForSeconds(dashCooltime);
            isDash = false;
        }
        private void ParticlePlay(ParticleSystem usedParticle)
        {
            usedParticle.Play();
        }

        public IEnumerator TakeDamage()
        {
            yield return new WaitForSeconds(1f);
        }
        public override void Dead()
        {
            throw new System.NotImplementedException();
        }

        private void InitPlayerStatus()
        {
            curHp = maxHp;
            isDead = false;

            isAttack = false;
            usingSkillX = false;
            isDash = false;
        }
    }
}
