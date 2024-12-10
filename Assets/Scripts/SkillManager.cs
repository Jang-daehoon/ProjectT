using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [Header("Q Skill Multi_Shot_Arrow")]
    public Sprite multiShotSkillImg;

    public int arrowCount;  // 발사할 화살의 갯수
    public float fireSpeed;  // 발사 속도
    public float qCoolTime;   // 쿨타임 (Q스킬)
    public float qDamageMultiplier;  // 데미지 증가값
    public float spreadAngle = 60f;  // 발사되는 화살의 각도 범위
    public BulletProjectile multiShotBullet;
    public ParticleSystem multiShotParticle;
    public ParticleSystem multishotHitParticle;

    [Header("W Skill ArrowRain")]
    public float wCoolTime; //쿨타임 (W스킬)
    public float wDamageMultiplier; //데미지 증가값
    public ParticleSystem arrowRainBullet;
    public ParticleSystem arrowRainParticle;
    

    [Header("E Skill")]
    public float eCoolTime;
    public float eDamageMultiplier;
    public ParticleSystem impaleSkillBullet;
    public ParticleSystem startImpaleParticle;
    public Collider skillRange;

    private float qCooldownTimer = 0f;  // 쿨타임 타이머 (Q 스킬)
    private float wCooldownTimer = 0f;  // 쿨타임 타이머 (W 스킬)
    private float eCooldownTimer = 0f;  // 쿨타임 타이머 (E 스킬)


    private void Update()
    {
        // 쿨타임 타이머 업데이트
        if (qCooldownTimer > 0f)
        {
            qCooldownTimer -= Time.deltaTime;  // Q 쿨타임 감소
        }

        if (wCooldownTimer > 0f)
        {
            wCooldownTimer -= Time.deltaTime;  // W 스킬 쿨타임 감소
        }

        if(eCooldownTimer > 0f)
        {
            eCooldownTimer -= Time.deltaTime;   //E 스킬 쿨타임 감소
        }
    }

    // Q 스킬: Multi_Shot_Arrow
    public void Multi_Shot_Arrow()
    {
        if (qCooldownTimer <= 0f)  // 쿨타임이 끝났을 때만 발동
        {
            // ParticleSystem 소환
            ParticleSystem enhanceAttackParticles = Instantiate(
                multiShotParticle,  // 소환할 ParticleSystem 프리팹
                GameManager.Instance.player.firePoint.position,  // 소환 위치
                GameManager.Instance.player.firePoint.transform.rotation  // 소환 회전값
            );

            // bulletCount 만큼 화살을 발사
            for (int i = 0; i < arrowCount; i++)
            {
                // 원뿔 모양으로 발사되는 화살의 방향을 계산
                float angleOffset = GetAngleOffset(i, arrowCount);

                // 발사될 화살의 방향 계산
                Vector3 direction = GetConeDirection(angleOffset);

                // Bullet 소환
                BulletProjectile MultiBullet = Instantiate(
                    multiShotBullet,
                    GameManager.Instance.player.firePoint.position,
                    GameManager.Instance.player.firePoint.transform.rotation
                );

                MultiBullet.Speed = fireSpeed;
                MultiBullet.Damage = GameManager.Instance.player.dmgValue * qDamageMultiplier;  // 데미지 증가값 적용
                MultiBullet.HitParticle = multishotHitParticle;

                // 화살 방향 설정
                MultiBullet.transform.forward = direction;

            }

            // 쿨타임 시작
            qCooldownTimer = qCoolTime;

        }
        else
        {
            Debug.Log("Q Skill is on cooldown.");
        }
    }

    // 화살의 개수에 맞게 각도 계산
    private float GetAngleOffset(int arrowIndex, int totalArrows)
    {
        // 총 화살 범위의 반을 기준으로 각도를 나누기
        float halfSpread = spreadAngle / 2f;

        // 각 화살이 차지할 각도 간격
        float angleStep = spreadAngle / (totalArrows - 1); // 화살 간의 간격 계산

        // 각 화살의 오프셋 계산
        return -halfSpread + (arrowIndex * angleStep); // 화살의 index에 맞는 각도 오프셋
    }

    // 원뿔 모양으로 퍼지는 방향을 계산하는 함수
    private Vector3 GetConeDirection(float angleOffset)
    {
        // 기본 방향 (플레이어의 전방 방향)
        Vector3 forwardDirection = GameManager.Instance.player.transform.forward;

        // 오프셋 각도로 회전시키기 위한 회전 값 생성
        Quaternion rotation = Quaternion.Euler(0, angleOffset, 0);  // Y축을 기준으로 회전

        // 회전된 방향 반환
        return rotation * forwardDirection;  // 원뿔 모양으로 발사될 방향
    }

    // W 스킬: ArrowRain 
    public void ArrowRain()
    {
        if (wCooldownTimer <= 0f) // 쿨타임 체크
        {
            ParticleSystem startArrowRain = Instantiate(arrowRainParticle, GameManager.Instance.player.transform.position, GameManager.Instance.player.transform.rotation);
            // 마우스 클릭 위치를 Raycast로 탐지
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 spawnPosition = hit.point; // 화살비 생성 위치 설정

                // ArrowRain 파티클 생성
                ParticleSystem arrowRainEffect = Instantiate(
                    arrowRainBullet,
                    spawnPosition,
                    Quaternion.identity // 회전값은 기본 설정
                );
                arrowRainEffect.transform.localScale = ArcanaManager.Instance.skillSize;
                // 특정 지속 시간 후 파티클 제거 (필요한 경우)
                Destroy(arrowRainEffect.gameObject, 2f); // 2초 후 파티클 삭제

                // 쿨타임 시작
                wCooldownTimer = wCoolTime;
            }
            else
            {
                Debug.Log("Invalid target for ArrowRain. Please click on the ground.");
            }
        }
        else
        {
            Debug.Log("W Skill is on cooldown.");
        }
    }

    public void ImpaleSkill()
    {
        if (eCooldownTimer <= 0f) // 쿨타임 체크
        {
            // 시전 파티클 생성
            ParticleSystem startImpaleEffect = Instantiate(
                startImpaleParticle,
                GameManager.Instance.player.transform.position,
                Quaternion.identity
            );

            // Impale 스킬 발사체 생성
            ParticleSystem impaleBullet = Instantiate(
                impaleSkillBullet,
                GameManager.Instance.player.transform.position,
                GameManager.Instance.player.transform.rotation
            );

            // 발사체의 Collider 가져오기
            Collider bulletCollider = impaleBullet.GetComponent<Collider>();
            if (bulletCollider != null)
            {
                Vector3 boxCenter = bulletCollider.bounds.center;
                Vector3 boxSize = bulletCollider.bounds.size;
                Quaternion boxRotation = impaleBullet.transform.rotation;

                // 범위 내 적 찾기
                Collider[] hitEnemies = Physics.OverlapBox(
                    boxCenter,
                    boxSize / 2,
                    boxRotation,
                    LayerMask.GetMask("Enemy")
                );

                foreach (Collider enemy in hitEnemies)
                {
                    // 적에 데미지를 입히고 피격 파티클을 생성
                    if (enemy.GetComponent<ITakeDamage>() != null)
                    {
                        enemy.GetComponent<ITakeDamage>().TakeDamage(
                            GameManager.Instance.player.dmgValue * eDamageMultiplier
                        );

                        // 피격 파티클 생성 (필요한 경우)
                    }
                }
            }
            else
            {
                Debug.LogError("ImpaleSkillBullet does not have a Collider component.");
            }

            // 쿨타임 시작
            eCooldownTimer = eCoolTime;
        }
        else
        {
            Debug.Log("E Skill is on cooldown.");
        }
    }

}
