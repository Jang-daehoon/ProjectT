using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("DashInfo")]
    [SerializeField] private Vector3 dir;
    [SerializeField] private float rotSpeed;
    [SerializeField] private bool isDash;
    [SerializeField] private float dashSpeed;  // ��� �ӵ�
    [SerializeField] private float dashPower;   // ��� �Ŀ�
    [SerializeField] private float dashDuration = 0.5f; // ��� ���� �ð�; // ��� ���� �ð�
    [SerializeField] private float dashCooltime = 1f;   // ��� ��Ÿ��
    [SerializeField] private float dashDistance = 5f;  // ��� �Ÿ�
    [Header("SkillInfo")]
    [SerializeField] private bool usingSkillX;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isHit;

    [Header("ProjectileInfo")]
    public float projectileSpeed;
    public Transform firePoint; // �Ѿ� �߻� ��ġ
    public BulletProjectile bulletProjectile;
    // Delegate ����

    private delegate IEnumerator SkillDelegate();
    private SkillDelegate currentSkill;  // ���� ����� ��ų�� ���� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();

        isAttack = false;
        usingSkillX = false;
        isDash = false;
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator TakeDamage()
    {
        yield return new WaitForSeconds(1f);
    }
    public override void Dead()
    {
        throw new System.NotImplementedException();
    }
}
