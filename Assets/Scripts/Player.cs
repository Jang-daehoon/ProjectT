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
        [SerializeField] private float dashDuration = 0.5f; // 대시 지속 시간
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

        [Header("CheckGround")]
        public LayerMask groundLayer;

        // Delegate 선언
        private delegate IEnumerator SkillDelegate();
        private SkillDelegate currentSkill;  // 현재 사용할 스킬을 담을 변수

        private Vector3 targetPosition; // 이동할 목표 위치

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            animator = GetComponent<Animator>();

            InitPlayerStatus();
        }

        private void Update()
        {
            if (!isDash && !isAttack) // 대시나 공격 중이 아닐 때만 이동
            {
                Move();
                PlayerRotation();
            }

            LookMouseCursor();

            if (Input.GetMouseButtonDown(0) && isAttack == false)
            {
                RotateToClickPosition();
                isAttack = true;  // 공격 상태로 변경
                animator.SetTrigger("Attack");
            }

            if (Input.GetKeyDown(KeyCode.Q) && usingSkillX == false)
            {
                RotateToClickPosition();
                animator.SetTrigger("SkillX");
            }

            if (Input.GetKeyDown(KeyCode.Space) && isDash == false)
            {
                StartCoroutine(Dodge());
            }
        }

        public override void Move()
        {
            if (targetPosition != Vector3.zero)
            {
                // 목표 위치로 이동
                float step = moveSpeed * Time.deltaTime; // 이동 속도
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                animator.SetFloat("Speed", step);
                Debug.Log("이동 속도:" + step);
                // 목표에 도달하면 이동을 멈춤
                if (transform.position == targetPosition || isAttack == true)
                {
                    targetPosition = Vector3.zero;
                    animator.SetFloat("Speed", 0);
                }
            }

        }

        public void PlayerRotation()
        {
            if (dir != Vector3.zero && isDash == false)
            {
                Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up); // 해당 방향을 바라봄
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            }
        }

        public void LookMouseCursor()
        {
            if (Input.GetMouseButtonDown(1) && isAttack == false) // 마우스 우클릭 시
            {
                // 마우스 좌표를 화면에서 월드 좌표로 변환
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 마우스 위치에서 레이캐스트를 쏴서 클릭한 지점 찾기
                // 레이캐스트가 groundLayer에만 반응하도록 설정
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // 충돌 지점이 있을 때
                    Debug.Log("충돌 지점: " + hit.point);  // 충돌한 지점의 위치 출력

                    // 레이 캐스트의 시작점(transform.position)에서 끝점(hit.point)으로 레이 표시
                    Debug.DrawRay(transform.position, hit.point - transform.position, Color.red, 2f);

                    // 이동할 목표 위치 설정
                    targetPosition = hit.point;

                    // 마우스를 클릭한 위치로 회전 (플레이어가 해당 위치를 바라보도록)
                    Vector3 lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    transform.LookAt(lookAtPosition); // 마우스 클릭한 위치로 회전
                }
                else
                {
                    // 레이가 groundLayer에 충돌하지 않았을 때
                    Debug.Log("레이가 groundLayer에 충돌하지 않음");
                }
            }
        }
        private void RotateToClickPosition()
        {
            // 마우스 좌표를 화면에서 월드 좌표로 변환
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 마우스 위치에서 레이캐스트를 쏴서 클릭한 지점 찾기
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // 클릭한 지점으로 회전
                Vector3 lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookAtPosition); // 마우스 클릭한 위치로 회전
            }
        }

        public void Attack()
        {
            StartCoroutine(FireArrow());
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
            yield return null;
            isAttack = false;
        }

        private IEnumerator Dodge()
        {
            // 회피 시작
            isDash = true;
            animator.SetTrigger("Dodge");

            // 회피 애니메이션이 끝날 때까지 기다리거나 쿨타임을 설정
            yield return new WaitForSeconds(dashDuration); // dashDuration은 회피 지속시간
            isDash = false; // 대시 상태 비활성화

            // 회피 후 쿨타임 처리
            yield return new WaitForSeconds(dashCooltime); // 쿨타임 대기
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
