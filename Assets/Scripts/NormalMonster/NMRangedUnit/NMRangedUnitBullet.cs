using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMRangedUnitBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;
    public float bulletLifeTime;

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player¸¦ °ø°Ý");
            //collision.gameObject.GetComponent<Character>().TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }

}
