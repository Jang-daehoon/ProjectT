using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Arcana Data", menuName = "Scriptable Object/ArcanaData")]
public class ArcanaData : ScriptableObject
{
    public enum ArcanaInfo { AttackEnhancement, SkillEnhancement, GetSpecialAttack };

    public ArcanaInfo arcanaInfo;
    [Header("ArcanaInfoData")]
    public int ArcanaId;    //아르카나 ID
    public Sprite ArcanaImage;   //아르카나 카드 이미지
    public string ArcanaName;   //아르카나 카드 이름
    [TextArea(3,3)]public string ArcanaDesc;   //아르카나 카드 설명

    [Header("LevelData")]
    public int baseCount;     //기본 투사체 개수
    public float baseInitBulletRate;    //기본 랜덤투사체 소환 확률;     

    public float[] enhanceDamage;   //증가한 데미지
    public int[] enhanceCount;  //증가한 투사체 개수            
    public float[] enhanceInitBulletRate;   //증가한 랜덤투사체 확률
    public float[] enhanceDamageMultiplier;    //증가한 데미지 배율
    public Vector3[] enhanceSkillScale;   //증가한 스킬 사이즈 증가
    public float[] cooldownReduction; //쿨타임 감소량

    [Header("EnhanceData")]
    [Header("공격 횟수마다 강화공격")]
    public int EnhanceAttackCnt;    //강화공격을 사용하기 위한 카운트
    public BulletProjectile EnhancedBullet;   //강화된 발사체 프리펩
    public ParticleSystem EnhancedAttackParticle;   //강화 발사 파티클
    public ParticleSystem EnhancedHitParticle;  //강화 발사체 피격 파티클

    [Header("공격시 확률적으로 투사체 추가 발사")]
    public float RandomExtraShotRate;    //랜덤투사체 발사 확률
    public float randomExtraShotSpeed; //랜덤 투사체 발사 속도
    public GameObject[] RandomBulletPrefab;   //랜덤 투사체 오브젝트
    public ParticleSystem[] RandomAttackParticle; //랜덤 투사체에 어울리는 파티클
    public ParticleSystem[] RandomHitParticle;    //랜덤 투사체 피격에 어울리는 파티클

    [Header("모든 공격 유도체로 변경")]
    public bool isCatalyst;
    public ParticleSystem getCatalystParticle;  //유도체 공격 활성화 시 몸 주변에 아우라가 생성(이상한 가호 효과)

    //스킬 강화 데이터 추가

}