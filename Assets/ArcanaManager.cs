using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanaManager : Singleton<ArcanaManager>
{
    [Header("ArcanaData Group")]
    public ArcanaData[] ArcanaData;
    public List<ArcanaData> removedArcana = new List<ArcanaData>(); // 제외된 아르카나 데이터 리스트

    [Header("Damage Enhancement")]
    public int curEnhanceLevel = 0; // 현재 증가 단계
    public float enhanceBaseDamage;          // 기본 데미지
    public float enhanceAtkDamage;        // 데미지 증가값

    public int randomAtkLevel = 0;
    public float randomBaseDamage;
    public float randomAtkDamage;   //데미지 증가값

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
        // 강화 공격 초기 파티클 설정
        enhanceAttackParticle = ArcanaData[0].EnhancedAttackParticle;
        enhanceMeleeProjectile = ArcanaData[0].EnhancedBullet;
        enhanceHitParticle = ArcanaData[0].EnhancedHitParticle;

        // 초기 데미지 설정
        enhanceBaseDamage = GameManager.Instance.player.dmgValue;
        randomBaseDamage = GameManager.Instance.player.dmgValue / 2;

        //데미지 증가값 처음 선택해 활성화시 baseDamage의 값을 가지고 같은 카드 선택 시 curEnhanceLevel의 값이 baseDamage에 증가되며 이후엔 level이 증가한다.
        // 첫 단계 데미지 증가값 설정
        enhanceAtkDamage = enhanceBaseDamage;
        randomAtkDamage = randomBaseDamage;
    }

    //기본공격 강화 (폭발)
    public IEnumerator EnhanceFireArrow()
    {
        BulletProjectile enhanceArrow = Instantiate(enhanceMeleeProjectile, GameManager.Instance.player.firePoint.position, GameManager.Instance.player.firePoint.transform.rotation);
        enhanceArrow.isTargeting = GameManager.Instance.player.isAtkTarGeting;
        enhanceArrow.Speed = GameManager.Instance.player.projectileSpeed;
        enhanceArrow.Damage = enhanceAtkDamage;
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
            randomProjectile.isTargeting = GameManager.Instance.player.isAtkTarGeting;

            // 랜덤 투사체 속도와 데미지 설정
            randomProjectile.Speed = ArcanaData[1].randomExtraShotSpeed;
            randomProjectile.Damage = randomAtkDamage;
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
    //기본공격 유도체로 변환
    public void ChanageCatalyst()
    {
        // 아르카나 데이터의 유도체 활성화 설정
        canCatalyst = ArcanaData[2].isCatalyst;

        // 플레이어의 자식 오브젝트로 파티클 생성
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

        // ArcanaData[2]를 removedArcana 리스트로 이동
        removedArcana.Add(ArcanaData[2]);

        // ArcanaData 배열에서 세 번째 데이터 제거
        List<ArcanaData> arcanaList = new List<ArcanaData>(ArcanaData);
        arcanaList.RemoveAt(2);
        ArcanaData = arcanaList.ToArray(); // 배열로 다시 변환
    }



}
