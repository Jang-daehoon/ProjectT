using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArrowHandler : MonoBehaviour
{
    public float damage; // 데미지 값 (외부에서 설정 가능)

    private void Awake()
    {
        damage = GameManager.Instance.player.dmgValue;
    }
    private void OnParticleCollision(GameObject other)
    {
        // 적과 충돌했는지 확인
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<ITakeDamage>().TakeDamage(damage);
            Debug.Log($"Enemy {other.name} took {damage} damage from DummyArrowProjectile.");
        }
    }
}
