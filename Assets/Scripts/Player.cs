using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("DashInfo")]
    [SerializeField] private Vector3 dir;
    [SerializeField] private float rotSpeed;
    [SerializeField] private bool isDash;
    [SerializeField] private float dashSpeed;  // 대시 속도
    [SerializeField] private float dashPower;   // 대시 파워
    [SerializeField] private float dashDuration = 0.5f; // 대시 지속 시간; // 대시 지속 시간
    [SerializeField] private float dashCooltime = 1f;   // 대시 쿨타임
    [SerializeField] private float dashDistance = 5f;  // 대시 거리
    [Header("SkillInfo")]
    [SerializeField] private bool usingSkillX;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isHit;

    [Header("ProjectileInfo")]
    public float projectileSpeed;
    public Transform firePoint; // 총알 발사 위치
    public BulletProjectile bulletProjectile;
    // Delegate 선언

    private delegate IEnumerator SkillDelegate();
    private SkillDelegate currentSkill;  // 현재 사용할 스킬을 담을 변수

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
