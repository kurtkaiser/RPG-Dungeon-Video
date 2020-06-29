using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawners;
    private int level = 0;
    private int currentScene = 0;
    private int zombieCount = 0;
    private int zombieLimit = 10;

    public GameObject player;
    public GameObject weapon;
    public GameObject hudCanvas;

    private Scene scene;

    void Start()
    {
        PrepareSpawners();
    }

    void Awake()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(weapon.gameObject);
        DontDestroyOnLoad(hudCanvas.gameObject);
        DontDestroyOnLoad(gameObject);
        
        scene = SceneManager.GetActiveScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(!string.Equals(scene.path, this.scene.path)){
            level++;
            PrepareSpawners();
        }
    }

    void PrepareSpawners()
    {
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        if(spawners.Length > 0)
        {
            int rnd = Random.Range(0, spawners.Length);
            spawners[rnd].GetComponent<SpawnerScript>().SetGateway(true);
            // Weapon Upgrade testing
            if(Random.Range(0, 5) == 3)
            {
                int randTemp = Random.Range(0, spawners.Length);
                spawners[randTemp].GetComponent<SpawnerScript>().SetWeapon(true);
            }
            foreach (GameObject spawner in spawners)
            {
                spawner.GetComponent<SpawnerScript>().SetHealth(level + Random.Range(3, 6));
            }
        }
    }

    public void SetZombieCount(int amount)
    {
        zombieCount += amount;
    }

    public int GetZombieCount()
    {
        return zombieCount;
    }

    public int GetZombieLimit()
    {
        return zombieLimit;
    }

    public void LoadLevel()
    {
        zombieCount = 0;
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            currentScene = 1;
        } else
        {
            currentScene = -1;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + currentScene);
    }

    public int GetLevel()
    {
        return level;
    }
}
