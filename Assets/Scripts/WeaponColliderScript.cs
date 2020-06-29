using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColliderScript : MonoBehaviour
{
    public GameObject player;
    private float weaponDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        weaponDamage = transform.parent.gameObject.GetComponent<WeaponScript>().weaponPower;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(weaponDamage);
        }
        if (collision.gameObject.CompareTag("Spawner"))
        {
            collision.gameObject.GetComponent<SpawnerScript>().TakeDamage(weaponDamage);
        }
    }
}
