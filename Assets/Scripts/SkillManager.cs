using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [Header("Q Skill Multi_Shot_Arrow")]
    public Sprite multiShotSkillImg;

    public int arrowCount;  // �߻��� ȭ���� ����
    public float fireSpeed;  // �߻� �ӵ�
    public float qCoolTime;   // ��Ÿ�� (Q��ų)
    public float qDamageMultiplier;  // ������ ������
    public float spreadAngle = 60f;  // �߻�Ǵ� ȭ���� ���� ����
    public BulletProjectile multiShotBullet;
    public ParticleSystem multiShotParticle;
    public ParticleSystem multishotHitParticle;

    [Header("W Skill ArrowRain")]
    public float wCoolTime; //��Ÿ�� (W��ų)
    public float wDamageMultiplier; //������ ������
    public ParticleSystem arrowRainBullet;
    public ParticleSystem arrowRainParticle;

    [Header("E Skill")]
    public float eCoolTime;
    public float eDamageMultiplier;
    public ParticleSystem impaleSkillBullet;
    public ParticleSystem startImpaleParticle;
    public Collider SkillRanage;

    private float qCooldownTimer = 0f;  // ��Ÿ�� Ÿ�̸� (Q ��ų)
    private float wCooldownTimer = 0f;  // ��Ÿ�� Ÿ�̸� (W ��ų)
    private float eCooldownTimer = 0f;  // ��Ÿ�� Ÿ�̸� (E ��ų)


    private void Update()
    {
        // ��Ÿ�� Ÿ�̸� ������Ʈ
        if (qCooldownTimer > 0f)
        {
            qCooldownTimer -= Time.deltaTime;  // Q ��Ÿ�� ����
        }

        if (wCooldownTimer > 0f)
        {
            wCooldownTimer -= Time.deltaTime;  // W ��ų ��Ÿ�� ����
        }

        if(eCooldownTimer > 0f)
        {
            eCooldownTimer -= Time.deltaTime;   //E ��ų ��Ÿ�� ����
        }
    }

    // Q ��ų: Multi_Shot_Arrow
    public void Multi_Shot_Arrow()
    {
        if (qCooldownTimer <= 0f)  // ��Ÿ���� ������ ���� �ߵ�
        {
            // ParticleSystem ��ȯ
            ParticleSystem enhanceAttackParticles = Instantiate(
                multiShotParticle,  // ��ȯ�� ParticleSystem ������
                GameManager.Instance.player.firePoint.position,  // ��ȯ ��ġ
                GameManager.Instance.player.firePoint.transform.rotation  // ��ȯ ȸ����
            );

            // bulletCount ��ŭ ȭ���� �߻�
            for (int i = 0; i < arrowCount; i++)
            {
                // ���� ������� �߻�Ǵ� ȭ���� ������ ���
                float angleOffset = GetAngleOffset(i, arrowCount);

                // �߻�� ȭ���� ���� ���
                Vector3 direction = GetConeDirection(angleOffset);

                // Bullet ��ȯ
                BulletProjectile MultiBullet = Instantiate(
                    multiShotBullet,
                    GameManager.Instance.player.firePoint.position,
                    GameManager.Instance.player.firePoint.transform.rotation
                );

                MultiBullet.Speed = fireSpeed;
                MultiBullet.Damage = GameManager.Instance.player.dmgValue * qDamageMultiplier;  // ������ ������ ����
                MultiBullet.HitParticle = multishotHitParticle;

                // ȭ�� ���� ����
                MultiBullet.transform.forward = direction;

            }

            // ��Ÿ�� ����
            qCooldownTimer = qCoolTime;

        }
        else
        {
            Debug.Log("Q Skill is on cooldown.");
        }
    }

    // ȭ���� ������ �°� ���� ���
    private float GetAngleOffset(int arrowIndex, int totalArrows)
    {
        // �� ȭ�� ������ ���� �������� ������ ������
        float halfSpread = spreadAngle / 2f;

        // �� ȭ���� ������ ���� ����
        float angleStep = spreadAngle / (totalArrows - 1); // ȭ�� ���� ���� ���

        // �� ȭ���� ������ ���
        return -halfSpread + (arrowIndex * angleStep); // ȭ���� index�� �´� ���� ������
    }

    // ���� ������� ������ ������ ����ϴ� �Լ�
    private Vector3 GetConeDirection(float angleOffset)
    {
        // �⺻ ���� (�÷��̾��� ���� ����)
        Vector3 forwardDirection = GameManager.Instance.player.transform.forward;

        // ������ ������ ȸ����Ű�� ���� ȸ�� �� ����
        Quaternion rotation = Quaternion.Euler(0, angleOffset, 0);  // Y���� �������� ȸ��

        // ȸ���� ���� ��ȯ
        return rotation * forwardDirection;  // ���� ������� �߻�� ����
    }

    // W ��ų: ArrowRain 
    public void ArrowRain()
    {
        if (wCooldownTimer <= 0f) // ��Ÿ�� üũ
        {
            ParticleSystem startArrowRain = Instantiate(arrowRainParticle, GameManager.Instance.player.transform.position, GameManager.Instance.player.transform.rotation);
            // ���콺 Ŭ�� ��ġ�� Raycast�� Ž��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 spawnPosition = hit.point; // ȭ��� ���� ��ġ ����

                // ArrowRain ��ƼŬ ����
                ParticleSystem arrowRainEffect = Instantiate(
                    arrowRainBullet,
                    spawnPosition,
                    Quaternion.identity // ȸ������ �⺻ ����
                );

                // Ư�� ���� �ð� �� ��ƼŬ ���� (�ʿ��� ���)
                Destroy(arrowRainEffect.gameObject, 2f); // 2�� �� ��ƼŬ ����

                // ��Ÿ�� ����
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
        if (eCooldownTimer <= 0f) // ��Ÿ�� üũ
        {
            // ���� ��ƼŬ ����
            ParticleSystem startImpaleEffect = Instantiate(
                startImpaleParticle,
                GameManager.Instance.player.transform.position,
                Quaternion.identity
            );
            Destroy(startImpaleEffect.gameObject, 1f); // 1�� �� ���� ��ƼŬ ����

            // �÷��̾ �ٶ󺸴� �������� �߻�ü ����
            Transform playerTransform = GameManager.Instance.player.transform;
            ParticleSystem impaleBullet = Instantiate(
                impaleSkillBullet,
                playerTransform.position, // �÷��̾� ��ġ���� ����
                Quaternion.LookRotation(playerTransform.forward) // �÷��̾ �ٶ󺸴� �������� ȸ�� ����
            );

            // Impale ��ų �ߵ� ��ġ ����
            Vector3 startPosition = playerTransform.position;

            // Impale ��ų�� ȿ�� ���� ����
            Vector3 boxSize = new Vector3(5f, 5f, 5f); // �ڽ� ũ�� (x, y, z)
            Quaternion boxRotation = Quaternion.identity; // ȸ�� �� (�ʿ信 ���� ����)

            // ���� �� �� ã��
            Collider[] hitEnemies = Physics.OverlapBox(
                startPosition,
                boxSize / 2,
                boxRotation,
                LayerMask.GetMask("Enemy")
            );

            foreach (Collider enemy in hitEnemies)
            {
                // ���� �������� ������ �ǰ� ��ƼŬ�� ����
                if (enemy.GetComponent<ITakeDamage>() != null)
                {
                    enemy.GetComponent<ITakeDamage>().TakeDamage(
                        GameManager.Instance.player.dmgValue * eDamageMultiplier
                    );

                    // �ǰ� ��ƼŬ ���� (������ �κ� �߰� ����)
                }
            }

            // ��Ÿ�� ����
            eCooldownTimer = eCoolTime;
        }
        else
        {
            Debug.Log("E Skill is on cooldown.");
        }
    }



}
