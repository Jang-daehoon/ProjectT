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


    private void Awake()
    {
        enhanceAttackParticle = ArcanaData[0].EnhancedAttackParticle;
        enhanceMeleeProjectile = ArcanaData[0].EnhancedBullet;
        enhanceHitParticle = ArcanaData[0].EnhancedHitParticle;
    }
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

}
