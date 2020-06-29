using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private float range;
    private Transform target;
    private float minDistance = 5.0f;
    private bool targetCollision = false;
    private float speed = 2.0f;
    private float thrust = 1.5f;
    public float health = 5;
    private int hitStrength = 10;

    public Sprite deathSprite;
    public Sprite[] sprites;

    private GameManager gameManager;

    private bool isDead = false;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int rnd = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[rnd];
        target = GameObject.Find("Player").transform;
        health += (0.1f * gameManager.GetLevel());
    }

    void Update()
    {
        range = Vector2.Distance(transform.position, target.position);
        if(range < minDistance && !isDead)
        {
            if (!targetCollision)
            {
                // Get the position of the player
                transform.LookAt(target.position);

                // Correct the rotation
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !targetCollision)
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collision.collider.bounds.center;

            targetCollision = true;

            bool right = contactPoint.x > center.x;
            bool left = contactPoint.x < center.x;
            bool top = contactPoint.y > center.y;
            bool bottom = contactPoint.y < center.y;

            if (right) GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
            if (left) GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust, ForceMode2D.Impulse);
            if (top) GetComponent<Rigidbody2D>().AddForce(transform.up * thrust, ForceMode2D.Impulse);
            if (bottom) GetComponent<Rigidbody2D>().AddForce(-transform.up * thrust, ForceMode2D.Impulse);
            Invoke("FalseCollision", 0.5f);
        }
    }

    void FalseCollision()
    {
        targetCollision = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health < 0)
        {
            isDead = true;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<SpriteRenderer>().sprite = deathSprite;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            GetComponent<Collider2D>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            target.GetComponent<PlayerScript>().GainExperience(100);
            Invoke("EnemyDeath", 1.5f);
        } else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Invoke("HideBlood", 0.25f);
        }
    }

    void HideBlood()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void EnemyDeath()
    {
        gameManager.SetZombieCount(-1);
        Destroy(gameObject);
    }

    public int GetHitStrength()
    {
        return hitStrength;
    }
}
