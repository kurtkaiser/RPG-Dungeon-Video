using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToLevel1Script : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.LoadLevel();
        }
    }
}
