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
    public int ArcanaId;    //�Ƹ�ī�� ID
    public Sprite ArcanaImage;   //�Ƹ�ī�� ī�� �̹���
    public string ArcanaName;   //�Ƹ�ī�� ī�� �̸�
    [TextArea(3,3)]public string ArcanaDesc;   //�Ƹ�ī�� ī�� ����

    [Header("LevelData")]
    public float baseDamage;    //�⺻ ������
    public int baseCount;     //�⺻ ����ü ����
    public float baseInitBulletRate;    //�⺻ ��������ü ��ȯ Ȯ��;     

    public float[] enhanceDamage;   //������ ������
    public int[] enhanceCount;  //������ ����ü ����            
    public float[] enhanceInitBulletRate;   //������ ��������ü Ȯ��

    [Header("EnhanceData")]
    [Header("���� Ƚ������ ��ȭ����")]
    public int EnhanceAttackCnt;    //��ȭ������ ����ϱ� ���� ī��Ʈ
    public BulletProjectile EnhancedBullet;   //��ȭ�� �߻�ü ������
    public ParticleSystem EnhancedAttackParticle;   //��ȭ �߻� ��ƼŬ
    public ParticleSystem EnhancedHitParticle;  //��ȭ �߻�ü �ǰ� ��ƼŬ

    [Header("���ݽ� Ȯ�������� ����ü �߰� �߻�")]
    public float RandomExtraShotRate;    //��������ü �߻� Ȯ��
    public float randomExtraShotSpeed; //���� ����ü �߻� �ӵ�
    public GameObject[] RandomBulletPrefab;   //���� ����ü ������Ʈ
    public ParticleSystem[] RandomAttackParticle; //���� ����ü�� ��︮�� ��ƼŬ
    public ParticleSystem[] RandomHitParticle;    //���� ����ü �ǰݿ� ��︮�� ��ƼŬ

    [Header("��� ���� ����ü�� ����")]
    public bool isCatalyst;

    //��ų ��ȭ ������ �߰�

}