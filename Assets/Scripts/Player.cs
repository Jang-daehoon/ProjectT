using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.ParticleSystem;
namespace HoonsCodes
{
    public class Player : Character, ITakeDamage
    {
        [Header("플레이어 이동 가능 여부")]
        public bool canMove;

        [Header("Exp")]
        public float exp = 0;
        public int Level = 1;
        private float[] maxExp = { 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };
        [Header("AttackInfo")]
        [SerializeField] private int AttackCnt; //공격 횟수
        [Header("DodgeInfo")]
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
        [Tooltip("화살 추적 여부")]
        public bool isAtkTarGeting = false;
        [SerializeField] private bool usingSkillX;
        public bool isAttack;
        [SerializeField] private bool isHit;
        [Header("----------------------------")]
        [Header("ProjectileInfo")]
        public float projectileSpeed;
        public Transform firePoint; // 총알 발사 위치
        public BulletProjectile bulletProjectile;
        public ParticleSystem fireParticle;
        public ParticleSystem xSkillParticle;

        public Transform randomExtraShotPoint;  //랜덤투사체 발사 위치


        [Header("CheckGround")]
        public LayerMask groundLayer;
        [Header("PlayerData")]
        public CharacterData playerData;

        // Delegate 선언
        private delegate IEnumerator SkillDelegate();
        private SkillDelegate currentSkill;  // 현재 사용할 스킬을 담을 변수

        public Vector3 targetPosition; // 이동할 목표 위치

        private Coroutine HitCoroutine;

        private void Awake()
        {
            canMove = true;
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            animator = GetComponent<Animator>();

            InitPlayerStatus();
        }

        private void Update()
        {
            if (isDash)
                return;

            // 대시나 공격 중이 아닐 때만 이동, 대사중에 이동 불가.
            if (!isDash && !isAttack && !usingSkillX && !UiManager.Instance.isDialogUiActive
                && !(UiManager.Instance.isMapUIActive || UiManager.Instance.isArcanaUIActive
                || UiManager.Instance.isUnknownUiActive || UiManager.Instance.isUnknownUiActive2 || UiManager.Instance.isUnknownUiActive3) && canMove)
            {
                Move();
                PlayerRotation();
                LookMouseCursor();
            }

            if (Input.GetMouseButton(0) && isAttack == false && !UiManager.Instance.isDialogUiActive
                && !(UiManager.Instance.isMapUIActive || UiManager.Instance.isArcanaUIActive
                || UiManager.Instance.isUnknownUiActive || UiManager.Instance.isUnknownUiActive2 || UiManager.Instance.isUnknownUiActive3) && canMove)
            {
                RotateToClickPosition();
                isAttack = true;  // 공격 상태로 변경
                animator.SetTrigger("Attack");
            }

            if (Input.GetKeyDown(KeyCode.Q) && !UiManager.Instance.isDialogUiActive
                && !(UiManager.Instance.isMapUIActive || UiManager.Instance.isArcanaUIActive
                || UiManager.Instance.isUnknownUiActive || UiManager.Instance.isUnknownUiActive2 || UiManager.Instance.isUnknownUiActive3) && canMove)
            {
                RotateToClickPosition();
                SkillManager.Instance.Multi_Shot_Arrow();
                UiManager.Instance.UseQSkill();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SkillManager.Instance.ArrowRain();
                UiManager.Instance.UseWSkill();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                RotateToClickPosition();
                SkillManager.Instance.ImpaleSkill();
                UiManager.Instance.UseESkill();
            }

            if (Input.GetKeyDown(KeyCode.Space) && isDash == false && !UiManager.Instance.isDialogUiActive
                && !(UiManager.Instance.isMapUIActive || UiManager.Instance.isArcanaUIActive ||
                UiManager.Instance.isUnknownUiActive || UiManager.Instance.isUnknownUiActive2 || UiManager.Instance.isUnknownUiActive3) && canMove)
            {
                StartCoroutine(Dodge());
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Chest") && other.GetComponent<ChestReward>().getReward == false
                && other.GetComponent<ChestReward>().isOpen == false && other.gameObject.layer == 8)
            {
                Debug.Log("보상 상자와 접촉");
                UiManager.Instance.interactiveText.text = "F를 눌러 상자를 열 수 있어.";
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
            else if (other.CompareTag("Potal"))
            {
                Debug.Log("Potal과 접촉");
                UiManager.Instance.interactiveText.text = "F를 눌러 포탈을 이용할 수 있어.";
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
            else if (other.CompareTag("NPC") && other.GetComponent<UnknownNPC>().isTalkDone == false)
            {
                Debug.Log("???와 접촉");
                UiManager.Instance.interactiveText.text = "F를 눌러 ???와 상호작용할 수 있어.";
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
            else if (other.CompareTag("RelicBox") && other.GetComponent<RelicBox>().getReward == false
             && other.GetComponent<RelicBox>().isOpen == false && other.gameObject.layer == 8)
            {
                Debug.Log("유물 상자와 접촉");
                UiManager.Instance.interactiveText.text = "F를 눌러 상자를 열 수 있어.";
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Chest") && other.GetComponent<ChestReward>().getReward == false
                && other.GetComponent<ChestReward>().isOpen == false && Input.GetKeyDown(KeyCode.F))
            {
                //UI상호작용 가능 문구 출력
                StartCoroutine(other.GetComponent<ChestReward>().ArcanaResult());
            }
            else if (other.CompareTag("Potal") && Input.GetKeyDown(KeyCode.F))
            {
                UiManager.Instance.ToggleUIElement(UiManager.Instance.MapUIObj, ref UiManager.Instance.isMapUIActive);
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
            else if (other.CompareTag("NPC") && other.GetComponent<UnknownNPC>().isTalkDone == false && Input.GetKeyDown(KeyCode.F))
            {
                switch (other.GetComponent<UnknownNPC>().npcId)
                {
                    case 0:
                        Debug.Log("해골과 접촉했습니다.");
                        other.GetComponent<UnknownNPC>().isTalkDone = true;
                        UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi, ref UiManager.Instance.isUnknownUiActive);
                        UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
                        break;
                    case 1:
                        Debug.Log("더미와 접촉했습니다.");
                        other.GetComponent<UnknownNPC>().isTalkDone = true;
                        UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi2, ref UiManager.Instance.isUnknownUiActive);
                        UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
                        break;
                    case 2:
                        Debug.Log("만드라고라와 접촉했습니다.");
                        other.GetComponent<UnknownNPC>().isTalkDone = true;
                        UiManager.Instance.ToggleUIElement(UiManager.Instance.mainUnknownUi3, ref UiManager.Instance.isUnknownUiActive);
                        UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
                        break;
                }
            }
            else if (other.CompareTag("RelicBox") && other.GetComponent<RelicBox>().getReward == false
                    && other.GetComponent<RelicBox>().isOpen == false && Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(other.GetComponent<RelicBox>().RelicResult());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            //UI상호작용 가능 문구 비활성화
            if (other.CompareTag("Chest") && other.GetComponent<ChestReward>().getReward == false
                && other.GetComponent<ChestReward>().isOpen == false)
            {
                Debug.Log("보상 상자 접촉 해제");
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
            else if (other.CompareTag("Potal"))
            {
                Debug.Log("Potal 접촉 해제");
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
            else if (other.CompareTag("NPC") && other.GetComponent<UnknownNPC>().isTalkDone == false)
            {
                Debug.Log("???와 접촉해제");
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
            }
            else if (other.CompareTag("RelicBox") && other.GetComponent<RelicBox>().getReward == false)
            {
                Debug.Log("유물 상자 접촉 해제");
                UiManager.Instance.ToggleUIElement(UiManager.Instance.interactiveObjUi, ref UiManager.Instance.isInteractiveUiActive);
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
                //Debug.Log("이동 속도:" + step);
                // 목표에 도달하면 이동을 멈춤
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
                Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up); // 해당 방향을 바라봄
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            }
        }

        public void LookMouseCursor()
        {
            if (Input.GetMouseButton(1) && isAttack == false && usingSkillX == false) // 마우스 우클릭 혹은 홀딩 시
            {
                // 마우스 좌표를 화면에서 월드 좌표로 변환
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 마우스 위치에서 레이캐스트를 쏴서 클릭한 지점 찾기
                // 레이캐스트가 groundLayer에만 반응하도록 설정
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // 충돌 지점이 있을 때
                    //Debug.Log("충돌 지점: " + hit.point);  // 충돌한 지점의 위치 출력

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
                    //Debug.Log("레이가 groundLayer에 충돌하지 않음");
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
            targetPosition = Vector3.zero;
            if (ArcanaManager.Instance.canEnhanceMeleeAttack == true && AttackCnt == 3)
            {
                AttackCnt = 0;
                StartCoroutine(ArcanaManager.Instance.EnhanceFireArrow());
                if (ArcanaManager.Instance.canRandomBulletInit == true)
                {
                    StartCoroutine(ArcanaManager.Instance.RandomExtraArrow());
                }
            }
            else
            {
                if (ArcanaManager.Instance.canEnhanceMeleeAttack == true)
                    AttackCnt++;
                StartCoroutine(FireArrow());
                if (ArcanaManager.Instance.canRandomBulletInit == true)
                {
                    StartCoroutine(ArcanaManager.Instance.RandomExtraArrow());
                }
            }
        }

        public void XSkill()
        {
            targetPosition = Vector3.zero;
            ParticleSystem xParticle = Instantiate(xSkillParticle, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            ParticlePlay(xParticle);
            // 파티클이 끝날 때 Destroy 하기 위해 StopAction 설정
            var main = xParticle.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            usingSkillX = false;
        }

        private IEnumerator FireArrow()
        {
            BulletProjectile Arrow = Instantiate(bulletProjectile, firePoint.position, transform.rotation);
            Arrow.isTargeting = ArcanaManager.Instance.canCatalyst;
            Arrow.Speed = projectileSpeed;
            Arrow.Damage = dmgValue;
            ParticlePlay(fireParticle);
            yield return null;
            isAttack = false;
        }

        private IEnumerator Dodge()
        {
            usingSkillX = false;
            isAttack = false;
            targetPosition = Vector3.zero;
            // 회피 시작
            isDash = true;
            animator.SetTrigger("Dodge");

            // 마우스 방향으로 회피 방향 계산
            Vector3 mouseDirection = GetMouseDirection();
            RotateToClickPosition();    //마우스 방향 바라보기
            // 회피 이동 목표 위치 계산
            Vector3 dodgeTarget = transform.position + mouseDirection * dashDistance;  // 마우스 방향으로 고정 거리 이동

            // 회피 애니메이션이 끝날 때까지 기다리거나 쿨타임을 설정
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(transform.position, dodgeTarget); // 이동 거리
            float endTime = startTime + dashDuration;

            // 이동 시작
            while (Time.time < endTime)
            {
                float distanceCovered = (Time.time - startTime) * dashSpeed;  // 이동 거리
                float fractionOfJourney = distanceCovered / journeyLength; // 이동 비율

                // 부드럽게 이동
                transform.position = Vector3.Lerp(transform.position, dodgeTarget, fractionOfJourney);
                yield return null;
            }

            // 이동이 끝나면 대시 상태 비활성화
            isDash = false;

            // 회피 후 쿨타임 처리
            yield return new WaitForSeconds(dashCooltime); // 쿨타임 대기
        }

        // 마우스 방향 계산 함수
        private Vector3 GetMouseDirection()
        {
            // 마우스 위치를 화면에서 월드 좌표로 변환
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 마우스 위치에서 레이캐스트를 쏴서 클릭한 지점 찾기
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // 마우스 위치를 기준으로 방향 계산
                Vector3 direction = (hit.point - transform.position).normalized;
                return direction;  // 마우스 방향 반환
            }

            return Vector3.zero;  // 실패 시 기본 방향
        }


        public void ParticlePlay(ParticleSystem usedParticle)
        {
            usedParticle.Play();
        }

        //피격 -> 한번 피격시 무적시간 1초
        public void TakeDamage(float damage)
        {
            if (HitCoroutine == null)
            {
                curHp -= damage;
                if (curHp <= 0)
                {
                    Dead();
                }
                Debug.Log($"{damage} 만큼의 피해를 받음, 무적시간 시작");
                HitCoroutine = StartCoroutine(PlayerHitCoolTime());
            }
            else
            {
                Debug.Log("무적시간");
            }
        }

        private IEnumerator PlayerHitCoolTime()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("무적시간 종료");
            HitCoroutine = null;
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
            atkSpeed = playerData.attackSpeed;  //애니메이션 실행속도
            //애니메이션에 맞춰서 공격하므로 공격 딜레이가 필요 없다.

            curHp = maxHp;
            isDead = false;

            isAttack = false;
            usingSkillX = false;
            isDash = false;
        }

        public void ExpPlus(float expplus)
        {
            exp += expplus;
            if (exp >= maxExp[Level - 1])
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            exp -= maxExp[Level - 1];
            Level++;
        }

        public void HpPlus(float Hpplus)
        {
            maxHp += Hpplus;
            curHp += Hpplus;
        }

        public void GetRelic(RelicData relicdata)
        {
            //이름으로 필드 값 찾기
            var statsname = typeof(Player).GetField(relicdata.statName);
            print($"{statsname}");
            //그필드의 값 float로 변환해서 가져오기
            float statspoint = (float)statsname.GetValue(this);
            print($"{statspoint}");
            //올라갈 스텟값
            float inpoint;
            //레벨계산식이 들어가는 유물인가?
            if (relicdata.isLevelPlus == true)
            {
                inpoint = this.Level * relicdata.relicStatsPoint;
            }
            else
            {
                inpoint = relicdata.relicStatsPoint;
            }
            print($"{inpoint}");
            //합치기
            //원래스텟  += 추가스텟
            statspoint += inpoint;
            //넣기
            statsname.SetValue(this, statspoint);
        }
        //스킬
        public void Multi_Shot_Arrow()
        {
            if (canMove && !isAttack && !isDash)  // 공격 중, 대시 중, 이동 중이 아닐 때만 스킬 발동
            {
                // 예시: 다중 화살 발사 구현
                Debug.Log("Multi Shot Arrow 발사!");

                // 다중 화살 발사 로직을 여기에 추가합니다.
                // 예를 들어, 여러 개의 화살을 발사하는 코드 등
                for (int i = 0; i < 3; i++)
                {
                    FireArrow();  // FireArrow 메서드를 호출해 다중 발사
                }
            }
        }
    }
}
