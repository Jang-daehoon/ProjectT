using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanaManager : Singleton<ArcanaManager>
{
    [Header("ArcanaData Group")]
    public ArcanaData[] ArcanaData;
    public List<ArcanaData> removedArcana = new List<ArcanaData>(); // ���ܵ� �Ƹ�ī�� ������ ����Ʈ

    [Header("Damage Enhancement")]
    public int curEnhanceLevel = 0; // ���� ���� �ܰ�
    public float enhanceBaseDamage;          // �⺻ ������
    public float enhanceAtkDamage;        // ������ ������

    public int randomAtkLevel = 0;
    public float randomBaseDamage;
    public float randomAtkDamage;   //������ ������

    [Header("�Ƹ�ī�� ȹ�� ���� Ȯ��")]
    public bool canEnhanceMeleeAttack;  //�Ƹ�ī���� ���� ��ȭ ������ ȹ���Ͽ��°�?
    public bool canRandomBulletInit;    //�Ƹ�ī���� ���� ���� ����ü �߻縦 ȹ���Ͽ��°�?
    public bool canCatalyst;    //�Ƹ�ī���� ���� ��� ������ ����ü�� ����Ǿ��°�?
    
    public BulletProjectile enhanceMeleeProjectile;   //��ȭ ȭ�� ������
    public ParticleSystem enhanceAttackParticle;    //��ȭ ���� ��ƼŬ
    public ParticleSystem enhanceHitParticle;   //��ȭ���� �ǰ� ��ƼŬ

    public BulletProjectile RandomProjectile;   //���� ����ü ������
    public ParticleSystem randomAttackParticle; //���� ���� ��ƼŬ
    public ParticleSystem randomHitParticle;    //���� ���� �ǰ� ��ƼŬ


    private void Awake()
    {
        // ��ȭ ���� �ʱ� ��ƼŬ ����
        enhanceAttackParticle = ArcanaData[0].EnhancedAttackParticle;
        enhanceMeleeProjectile = ArcanaData[0].EnhancedBullet;
        enhanceHitParticle = ArcanaData[0].EnhancedHitParticle;

        // �ʱ� ������ ����
        enhanceBaseDamage = GameManager.Instance.player.dmgValue;
        randomBaseDamage = GameManager.Instance.player.dmgValue / 2;

        //������ ������ ó�� ������ Ȱ��ȭ�� baseDamage�� ���� ������ ���� ī�� ���� �� curEnhanceLevel�� ���� baseDamage�� �����Ǹ� ���Ŀ� level�� �����Ѵ�.
        // ù �ܰ� ������ ������ ����
        enhanceAtkDamage = enhanceBaseDamage;
        randomAtkDamage = randomBaseDamage;
    }

    //�⺻���� ��ȭ (����)
    public IEnumerator EnhanceFireArrow()
    {
        BulletProjectile enhanceArrow = Instantiate(enhanceMeleeProjectile, GameManager.Instance.player.firePoint.position, GameManager.Instance.player.firePoint.transform.rotation);
        enhanceArrow.isTargeting = GameManager.Instance.player.isAtkTarGeting;
        enhanceArrow.Speed = GameManager.Instance.player.projectileSpeed;
        enhanceArrow.Damage = enhanceAtkDamage;
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
    //����Ʈ�� ����(�߰� Ÿ��)
    public IEnumerator RandomExtraArrow()
    {
        // ArcanaData[1]�� RandomExtraShotRate Ȯ���� ���� ���� ����ü �߻�
        if (canRandomBulletInit && Random.value <= ArcanaData[1].RandomExtraShotRate) // Random.value�� 0.0 ~ 1.0 ������ ����
        {
            // ���� ����ü ����
            int randomIndex = Random.Range(0, ArcanaData[1].RandomBulletPrefab.Length);
            GameObject randomBulletPrefab = ArcanaData[1].RandomBulletPrefab[randomIndex];

            // ���õ� ������Ÿ�� ���� �����
            Debug.Log($"���õ� ���� ����ü: {randomBulletPrefab.name} (Index: {randomIndex})");

            // ���� ����ü ����
            BulletProjectile randomProjectile = Instantiate(
                randomBulletPrefab.GetComponent<BulletProjectile>(),
                GameManager.Instance.player.randomExtraShotPoint.position,
                GameManager.Instance.player.randomExtraShotPoint.transform.rotation
            );
            randomProjectile.isTargeting = GameManager.Instance.player.isAtkTarGeting;

            // ���� ����ü �ӵ��� ������ ����
            randomProjectile.Speed = ArcanaData[1].randomExtraShotSpeed;
            randomProjectile.Damage = randomAtkDamage;
            randomProjectile.HitParticle = ArcanaData[1].RandomHitParticle[randomIndex];
            randomProjectile.isEnhanced = false;

            // ��ƼŬ ȿ�� ����
            ParticleSystem attackParticle = Instantiate(
                ArcanaData[1].RandomAttackParticle[randomIndex],
                GameManager.Instance.player.randomExtraShotPoint.position,
                GameManager.Instance.player.randomExtraShotPoint.transform.rotation
            );

            Debug.Log("RandomExtraShot �߻� �Ϸ�!");
            // �߻� �� ���
            yield return null;
        }
        else
        {
            // ���� ����ü �߻簡 ������ ��� �ٸ� ó��(�ʿ� �� ����)
            Debug.Log("���� �߻� ����.");
            yield return null;
        }
    }
    //�⺻���� ����ü�� ��ȯ
    public void ChanageCatalyst()
    {
        // �Ƹ�ī�� �������� ����ü Ȱ��ȭ ����
        canCatalyst = ArcanaData[2].isCatalyst;

        // �÷��̾��� �ڽ� ������Ʈ�� ��ƼŬ ����
        ParticleSystem CatalystParticle = Instantiate(
            ArcanaData[2].getCatalystParticle,
            new Vector3(
                GameManager.Instance.player.transform.position.x,
                GameManager.Instance.player.transform.position.y + 1,
                GameManager.Instance.player.transform.position.z
            ),
            GameManager.Instance.player.transform.rotation
        );
        CatalystParticle.transform.SetParent(GameManager.Instance.player.transform);

        // ArcanaData[2]�� removedArcana ����Ʈ�� �̵�
        removedArcana.Add(ArcanaData[2]);

        // ArcanaData �迭���� �� ��° ������ ����
        List<ArcanaData> arcanaList = new List<ArcanaData>(ArcanaData);
        arcanaList.RemoveAt(2);
        ArcanaData = arcanaList.ToArray(); // �迭�� �ٽ� ��ȯ
    }



}
