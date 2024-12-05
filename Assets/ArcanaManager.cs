using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanaManager : Singleton<ArcanaManager>
{
    [Header("ArcanaData Group")]
    public ArcanaData[] ArcanaData;

    [Header("�Ƹ�ī�� ȹ�� ���� Ȯ��")]
    public bool canEnhanceMeleeAttack;  //�Ƹ�ī���� ���� ��ȭ ������ ȹ���Ͽ��°�?
    public bool canRandomBulletInit;    //�Ƹ�ī���� ���� ���� ����ü �߻縦 ȹ���Ͽ��°�?
    public bool canCatalyst;    //�Ƹ�ī���� ���� ��� ������ ����ü�� ����Ǿ��°�?
    
    public BulletProjectile enhanceMeleeProjectile;   //��ȭ ȭ�� ������
    public ParticleSystem enhanceAttackParticle;    //��ȭ ���� ��ƼŬ
    public ParticleSystem enhanceHitParticle;   //��ȭ���� �ǰ� ��ƼŬ


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
        // ParticleSystem prefab�� ��ȯ
        ParticleSystem enhanceAttackParticles = Instantiate(
            enhanceAttackParticle, // ��ȯ�� ParticleSystem ������
            GameManager.Instance.player.firePoint.position, // ��ȯ ��ġ
            GameManager.Instance.player.firePoint.transform.rotation // ��ȯ ȸ����
        );

        yield return null;
        GameManager.Instance.player.isAttack = false;
    }

}
