using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArrowHandler : MonoBehaviour
{
    public float damage; // ������ �� (�ܺο��� ���� ����)

    private void Awake()
    {
        damage = GameManager.Instance.player.dmgValue;
    }
    private void OnParticleCollision(GameObject other)
    {
        // ���� �浹�ߴ��� Ȯ��
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<ITakeDamage>().TakeDamage(damage);
            Debug.Log($"Enemy {other.name} took {damage} damage from DummyArrowProjectile.");
        }
    }
}
