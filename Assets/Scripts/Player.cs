using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.ParticleSystem;
namespace HoonsCodes
{
    public class Player : Character
    {
        [Header("AttackInfo")]
        [SerializeField] private int AttackCnt; //���� Ƚ��
        [Header("DodgeInfo")]
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
        public bool isAttack;
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
        [Header("PlayerData")]
        public CharacterData playerData;

        // Delegate ����
        private delegate IEnumerator SkillDelegate();
        private SkillDelegate currentSkill;  // ���� ����� ��ų�� ���� ����

        public Vector3 targetPosition; // �̵��� ��ǥ ��ġ

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            animator = GetComponent<Animator>();

            InitPlayerStatus();
        }

        private void Update()
        {
            if (!isDash && !isAttack && !usingSkillX) // ��ó� ���� ���� �ƴ� ���� �̵�
            {
                Move();
                PlayerRotation();
            }

            LookMouseCursor();

            if (Input.GetMouseButton(0) && isAttack == false)
            {
                RotateToClickPosition();
                isAttack = true;  // ���� ���·� ����
                animator.SetTrigger("Attack");
            }

            if (Input.GetKeyDown(KeyCode.Q) && usingSkillX == false)
            {
                RotateToClickPosition();
                animator.SetTrigger("SkillX");
                usingSkillX = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isDash == false)
            {
                StartCoroutine(Dodge());
            }
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Chest") && other.GetComponent<ChestReward>().getReward == false)
            {
                Debug.Log("���� ���ڿ� ����");
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Chest") && other.GetComponent<ChestReward>().getReward == false
                && other.GetComponent<ChestReward>().isOpen == false && Input.GetKeyDown(KeyCode.F))
            {
                //UI��ȣ�ۿ� ���� ���� ���
                StartCoroutine(other.GetComponent<ChestReward>().ArcanaResult());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            //UI��ȣ�ۿ� ���� ���� ��Ȱ��ȭ
        }
        public override void Move()
        {
            if (targetPosition != Vector3.zero)
            {
                // ��ǥ ��ġ�� �̵�
                float step = moveSpeed * Time.deltaTime; // �̵� �ӵ�
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                animator.SetFloat("Speed", step);
                //Debug.Log("�̵� �ӵ�:" + step);
                // ��ǥ�� �����ϸ� �̵��� ����
                if (transform.position == targetPosition)
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
            if (Input.GetMouseButton(1) && isAttack == false && usingSkillX == false) // ���콺 ��Ŭ�� Ȥ�� Ȧ�� ��
            {
                // ���콺 ��ǥ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // ���콺 ��ġ���� ����ĳ��Ʈ�� ���� Ŭ���� ���� ã��
                // ����ĳ��Ʈ�� groundLayer���� �����ϵ��� ����
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // �浹 ������ ���� ��
                    //Debug.Log("�浹 ����: " + hit.point);  // �浹�� ������ ��ġ ���

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
            targetPosition = Vector3.zero;
            if(ArcanaManager.Instance.canEnhanceMeleeAttack == true && AttackCnt == 3)
            {
                AttackCnt = 0;
                StartCoroutine(ArcanaManager.Instance.EnhanceFireArrow());
            }
            else
            {
                if(ArcanaManager.Instance.canEnhanceMeleeAttack == true)
                    AttackCnt++;
                StartCoroutine(FireArrow());
            }
        }

        public void XSkill()
        {
            targetPosition = Vector3.zero;
            ParticleSystem xParticle = Instantiate(xSkillParticle, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            ParticlePlay(xParticle);
            // ��ƼŬ�� ���� �� Destroy �ϱ� ���� StopAction ����
            var main = xParticle.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            usingSkillX = false;
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
            usingSkillX = false ;
            isAttack = false ;
            targetPosition = Vector3.zero;
            // ȸ�� ����
            isDash = true;
            animator.SetTrigger("Dodge");

            // ���콺 �������� ȸ�� ���� ���
            Vector3 mouseDirection = GetMouseDirection();
            RotateToClickPosition();    //���콺 ���� �ٶ󺸱�
            // ȸ�� �̵� ��ǥ ��ġ ���
            Vector3 dodgeTarget = transform.position + mouseDirection * dashDistance;  // ���콺 �������� ���� �Ÿ� �̵�

            // ȸ�� �ִϸ��̼��� ���� ������ ��ٸ��ų� ��Ÿ���� ����
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(transform.position, dodgeTarget); // �̵� �Ÿ�
            float endTime = startTime + dashDuration;

            // �̵� ����
            while (Time.time < endTime)
            {
                float distanceCovered = (Time.time - startTime) * dashSpeed;  // �̵� �Ÿ�
                float fractionOfJourney = distanceCovered / journeyLength; // �̵� ����

                // �ε巴�� �̵�
                transform.position = Vector3.Lerp(transform.position, dodgeTarget, fractionOfJourney);
                yield return null;
            }

            // �̵��� ������ ��� ���� ��Ȱ��ȭ
            isDash = false;

            // ȸ�� �� ��Ÿ�� ó��
            yield return new WaitForSeconds(dashCooltime); // ��Ÿ�� ���
        }

        // ���콺 ���� ��� �Լ�
        private Vector3 GetMouseDirection()
        {
            // ���콺 ��ġ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���콺 ��ġ���� ����ĳ��Ʈ�� ���� Ŭ���� ���� ã��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // ���콺 ��ġ�� �������� ���� ���
                Vector3 direction = (hit.point - transform.position).normalized;
                return direction;  // ���콺 ���� ��ȯ
            }

            return Vector3.zero;  // ���� �� �⺻ ����
        }


        public void ParticlePlay(ParticleSystem usedParticle)
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
            name = playerData.name;
            maxHp = playerData.maxHp;
            dmgValue = playerData.damage;
            moveSpeed = playerData.moveSpeed;
            atkSpeed = playerData.attackSpeed;  //�ִϸ��̼� ����ӵ�
            //�ִϸ��̼ǿ� ���缭 �����ϹǷ� ���� �����̰� �ʿ� ����.

            curHp = maxHp;
            isDead = false;

            isAttack = false;
            usingSkillX = false;
            isDash = false;
        }
    }
}
