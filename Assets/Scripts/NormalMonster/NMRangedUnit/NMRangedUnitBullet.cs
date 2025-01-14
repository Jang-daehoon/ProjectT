using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMRangedUnitBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;
    public float bulletLifeTime;
    public ParticleSystem hitParticle;

    private void Start()
    {
        StartCoroutine(BulletDestroy());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(bulletLifeTime);
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player�� ����");
            GameManager.Instance.player.TakeDamage(bulletDamage);
            Destroy(this.gameObject);
            Instantiate(hitParticle, GameManager.Instance.player.transform.position,transform.rotation);
        }
    }
}
