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
        [SerializeField] private float dashSpeed;  // ��� �ӵ�
        [SerializeField] private float dashPower;   // ��� �Ŀ�
        [SerializeField] private float dashDuration = 0.5f; // ��� ���� �ð�
        [SerializeField] private float dashCooltime = 1f;   // ��� ��Ÿ��
        [SerializeField] private float dashDistance = 5f;  // ��� �Ÿ�
        [Header("----------------------------")]
        [Header("SkillInfo")]
        [SerializeField] private bool usingSkillX;
        [SerializeField] private bool isAttack;
        [SerializeField] private bool isHit;
        [Header("----------------------------")]
        [Header("ProjectileInfo")]
        public float projectileSpeed;
        public Transform firePoint; // �Ѿ� �߻� ��ġ
        public BulletProjectile bulletProjectile;
        public ParticleSystem fireParticle;
        public ParticleSystem xSkillParticle;

        [Header("CheckGround")]
        public LayerMask groundLayer;

        // Delegate ����
        private delegate IEnumerator SkillDelegate();
        private SkillDelegate currentSkill;  // ���� ����� ��ų�� ���� ����

        private Vector3 targetPosition; // �̵��� ��ǥ ��ġ

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            animator = GetComponent<Animator>();

            InitPlayerStatus();
        }

        private void Update()
        {
            if (!isDash && !isAttack) // ��ó� ���� ���� �ƴ� ���� �̵�
            {
                Move();
                PlayerRotation();
            }

            LookMouseCursor();

            if (Input.GetMouseButtonDown(0) && isAttack == false)
            {
                RotateToClickPosition();
                isAttack = true;  // ���� ���·� ����
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
                // ��ǥ ��ġ�� �̵�
                float step = moveSpeed * Time.deltaTime; // �̵� �ӵ�
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                animator.SetFloat("Speed", step);
                Debug.Log("�̵� �ӵ�:" + step);
                // ��ǥ�� �����ϸ� �̵��� ����
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
                Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up); // �ش� ������ �ٶ�
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            }
        }

        public void LookMouseCursor()
        {
            if (Input.GetMouseButtonDown(1) && isAttack == false) // ���콺 ��Ŭ�� ��
            {
                // ���콺 ��ǥ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // ���콺 ��ġ���� ����ĳ��Ʈ�� ���� Ŭ���� ���� ã��
                // ����ĳ��Ʈ�� groundLayer���� �����ϵ��� ����
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // �浹 ������ ���� ��
                    Debug.Log("�浹 ����: " + hit.point);  // �浹�� ������ ��ġ ���

                    // ���� ĳ��Ʈ�� ������(transform.position)���� ����(hit.point)���� ���� ǥ��
                    Debug.DrawRay(transform.position, hit.point - transform.position, Color.red, 2f);

                    // �̵��� ��ǥ ��ġ ����
                    targetPosition = hit.point;

                    // ���콺�� Ŭ���� ��ġ�� ȸ�� (�÷��̾ �ش� ��ġ�� �ٶ󺸵���)
                    Vector3 lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    transform.LookAt(lookAtPosition); // ���콺 Ŭ���� ��ġ�� ȸ��
                }
                else
                {
                    // ���̰� groundLayer�� �浹���� �ʾ��� ��
                    Debug.Log("���̰� groundLayer�� �浹���� ����");
                }
            }
        }
        private void RotateToClickPosition()
        {
            // ���콺 ��ǥ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���콺 ��ġ���� ����ĳ��Ʈ�� ���� Ŭ���� ���� ã��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // Ŭ���� �������� ȸ��
                Vector3 lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookAtPosition); // ���콺 Ŭ���� ��ġ�� ȸ��
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
            // ��ƼŬ�� ���� �� Destroy �ϱ� ���� StopAction ����
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
            // ȸ�� ����
            isDash = true;
            animator.SetTrigger("Dodge");

            // ȸ�� �ִϸ��̼��� ���� ������ ��ٸ��ų� ��Ÿ���� ����
            yield return new WaitForSeconds(dashDuration); // dashDuration�� ȸ�� ���ӽð�
            isDash = false; // ��� ���� ��Ȱ��ȭ

            // ȸ�� �� ��Ÿ�� ó��
            yield return new WaitForSeconds(dashCooltime); // ��Ÿ�� ���
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
