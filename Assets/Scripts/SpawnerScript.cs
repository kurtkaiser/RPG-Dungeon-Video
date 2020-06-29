using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    private float timer;
    private int spawnIndex = 0;
    private float health = 5;
    public Sprite deathSprite;
    public Sprite gateway;
    private bool isGateway = false;

    public Sprite weaponUpgrade;
    private bool isWeaponUpgrade = false;

    public Sprite[] sprites;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Instantiate(enemyPrefab, spawnPoints[0].transform.position, Quaternion.identity);
        Instantiate(enemyPrefab, spawnPoints[1].transform.position, Quaternion.identity);
        timer = Time.time + 7.0f;
        int rnd = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[rnd];
        gameManager.SetZombieCount(2);
    }

    void Update()
    {
        if(timer<Time.time && gameManager.GetZombieCount()< gameManager.GetZombieLimit())
        {
            if (GetComponent<SpriteRenderer>().sprite != gateway)
            {
                Instantiate(enemyPrefab, spawnPoints[spawnIndex % 2].transform.position, Quaternion.identity);
                timer = Time.time + 7.0f;
                spawnIndex++;
                gameManager.SetZombieCount(1);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (GetComponent<SpriteRenderer>().sprite != gateway)
        {
            health -= amount;
            GetComponent<SpriteRenderer>().color = Color.red;
            if (health < 0)
            {
                GetComponent<SpriteRenderer>().sprite = deathSprite;
                if (isGateway)
                {
                    Invoke("OpenGateway", 0.5f);
                }
                else if (isWeaponUpgrade)
                {
                    Invoke("OpenWeapon", 0.5f);
                } else
                {
                    Invoke("DestroySpawner", 0.6f);
                }

            }
            Invoke("DefaultColor", 0.3f);
        }
    }

    private void OpenGateway()
    {
        GetComponent<SpriteRenderer>().sprite = gateway;
    }

    private void DestroySpawner()
    {
        Destroy(gameObject);
    }

    private void DefaultColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public void SetGateway(bool maybe)
    {
        isGateway = maybe;
    }

    public void GetGatewayWeapon()
    {
        if(GetComponent<SpriteRenderer>().sprite == gateway)
        {
            gameManager.LoadLevel();
        } else if (GetComponent<SpriteRenderer>().sprite == weaponUpgrade)
        {
            GameObject.Find("Weapon").GetComponent<WeaponScript>().UpgradeWeapon();
            Destroy(gameObject);
        }
    }

    public void SetWeapon(bool maybe)
    {
        isWeaponUpgrade = true;
    }

    private void OpenWeapon()
    {
        GetComponent<SpriteRenderer>().sprite = weaponUpgrade;
    }
}
