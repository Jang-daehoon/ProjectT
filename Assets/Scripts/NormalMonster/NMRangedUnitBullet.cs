using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMRangedUnitBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;
    public float bulletLifeTime;

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
            Debug.Log("Player¸¦ °ø°Ý");
            //other.GetComponent<Character>().TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }
}
