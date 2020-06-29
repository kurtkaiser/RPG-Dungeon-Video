using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponScript : MonoBehaviour
{
    private bool swing = false;
    int degree = 0;
    private float weaponY = -0.4f;
    private float weaponX = 0.3f;
    public Sprite[] upgrades;
    private int spriteIndex = 0;
    public float weaponPower = 1.0f;

    Vector3 pos;
    public GameObject player;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (swing)
        {
            degree -= 7;
            if(degree < -65)
            {
                degree = 0;
                swing = false;
                GetComponent<SpriteRenderer>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
            transform.eulerAngles = Vector3.forward * degree;
        }
    }

    void Attack()
    {
        if(player.GetComponent<PlayerScript>().turnedLeft)
        {
            transform.localScale = new Vector3(-1.8f, 1.8f, 1);
            weaponX = -0.3f;
        }
        else
        {
            transform.localScale = new Vector3(1.8f, 1.8f, 1);
            weaponX = 0.3f;
        }
        pos = player.transform.position;
        pos.x += weaponX;
        pos.y += weaponY;
        transform.position = pos;
        swing = true;
    }

    public void UpgradeWeapon()
    {
        if(spriteIndex < upgrades.Length - 1)
        {
            spriteIndex++;
        }
        GetComponent<SpriteRenderer>().sprite = upgrades[spriteIndex];
        weaponPower += 0.4f;
    }

}
