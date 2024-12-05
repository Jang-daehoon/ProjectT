using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanaManager : Singleton<ArcanaManager>
{
    [Header("ArcanaData Group")]
    public ArcanaData[] ArcanaData;

    [Header("아르카나 획득 여부 확인")]
    public bool canEnhanceMeleeAttack;  //아르카나를 통해 강화 공격을 획득하였는가?
    public bool canRandomBulletInit;    //아르카나를 통해 랜덤 투사체 발사를 획득하였는가?
    public bool canCatalyst;    //아르카나를 통해 모든 공격이 유도체로 변경되었는가?
    
    public BulletProjectile enhanceMeleeProjectile;   //강화 화살 프리펩
    public ParticleSystem enhanceAttackParticle;    //강화 공격 파티클
    public ParticleSystem enhanceHitParticle;   //강화공격 피격 파티클

    public BulletProjectile RandomProjectile;   //랜덤 투사체 프리펩
    public ParticleSystem randomAttackParticle; //랜덤 공격 파티클
    public ParticleSystem randomHitParticle;    //랜덤 공격 피격 파티클


    private void Awake()
    {
        enhanceAttackParticle = ArcanaData[0].EnhancedAttackParticle;
        enhanceMeleeProjectile = ArcanaData[0].EnhancedBullet;
        enhanceHitParticle = ArcanaData[0].EnhancedHitParticle;
    }
    //기본공격 강화 (폭발)
    public IEnumerator EnhanceFireArrow()
    {
        BulletProjectile enhanceArrow = Instantiate(enhanceMeleeProjectile, GameManager.Instance.player.firePoint.position, GameManager.Instance.player.firePoint.transform.rotation);
        enhanceArrow.Speed = GameManager.Instance.player.projectileSpeed;
        enhanceArrow.Damage = ArcanaData[0].baseDamage;
        enhanceArrow.HitParticle = enhanceHitParticle;
        enhanceArrow.isEnhanced = true;
        // ParticleSystem prefab을 소환
        ParticleSystem enhanceAttackParticles = Instantiate(
            enhanceAttackParticle, // 소환할 ParticleSystem 프리팹
            GameManager.Instance.player.firePoint.position, // 소환 위치
            GameManager.Instance.player.firePoint.transform.rotation // 소환 회전값
        );

        yield return null;
        GameManager.Instance.player.isAttack = false;
    }
    //엑스트라 어택(추가 타격)
    public IEnumerator RandomExtraArrow()
    {
        // ArcanaData[1]의 RandomExtraShotRate 확률에 따라 랜덤 투사체 발사
        if (canRandomBulletInit && Random.value <= ArcanaData[1].RandomExtraShotRate) // Random.value는 0.0 ~ 1.0 사이의 난수
        {
            // 랜덤 투사체 선택
            int randomIndex = Random.Range(0, ArcanaData[1].RandomBulletPrefab.Length);
            GameObject randomBulletPrefab = ArcanaData[1].RandomBulletPrefab[randomIndex];

            // 선택된 프로젝타일 정보 디버깅
            Debug.Log($"선택된 랜덤 투사체: {randomBulletPrefab.name} (Index: {randomIndex})");

            // 랜덤 투사체 생성
            BulletProjectile randomProjectile = Instantiate(
                randomBulletPrefab.GetComponent<BulletProjectile>(),
                GameManager.Instance.player.randomExtraShotPoint.position,
                GameManager.Instance.player.randomExtraShotPoint.transform.rotation
            );

            // 랜덤 투사체 속도와 데미지 설정
            randomProjectile.Speed = ArcanaData[1].randomExtraShotSpeed;
            randomProjectile.Damage = ArcanaData[1].baseDamage;
            randomProjectile.HitParticle = ArcanaData[1].RandomHitParticle[randomIndex];
            randomProjectile.isEnhanced = false;

            // 파티클 효과 생성
            ParticleSystem attackParticle = Instantiate(
                ArcanaData[1].RandomAttackParticle[randomIndex],
                GameManager.Instance.player.randomExtraShotPoint.position,
                GameManager.Instance.player.randomExtraShotPoint.transform.rotation
            );

            Debug.Log("RandomExtraShot 발사 완료!");
            // 발사 후 대기
            yield return null;
        }
        else
        {
            // 랜덤 투사체 발사가 실패한 경우 다른 처리(필요 시 구현)
            Debug.Log("랜덤 발사 실패.");
            yield return null;
        }
    }


}
