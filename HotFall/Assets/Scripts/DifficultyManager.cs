using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField]
    GameObject Spawner;

    private SpawnManager SpawnScript;
    private SpawnManager.Enemies basic;
    private SpawnManager.Enemies zero;
    private SpawnManager.Enemies spiral;
    // Start is called before the first frame update
    void Start()
    {
        SpawnScript = Spawner.GetComponent<SpawnManager>();
        SpawnManager.Enemies basic = SpawnScript.monstersToSpawn[0];
        SpawnManager.Enemies zero = SpawnScript.monstersToSpawn[1];
        SpawnManager.Enemies spiral = SpawnScript.monstersToSpawn[2];
        basic.percentage = 0;
        zero.percentage = 1;
        spiral.percentage = 0;
        SpawnScript.setMaxEnemies(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= 5 && Time.time <= 6)
        {
            SpawnManager.Enemies basic = SpawnScript.monstersToSpawn[0];
            SpawnManager.Enemies zero = SpawnScript.monstersToSpawn[1];
            SpawnManager.Enemies spiral = SpawnScript.monstersToSpawn[2];
            basic.percentage = 0.2f;
            zero.percentage = 1;
            spiral.percentage = 0;
            SpawnScript.setMaxEnemies(20);
        }
    }
}
