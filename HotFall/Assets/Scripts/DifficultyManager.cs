using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField]
    GameObject Spawner;

    private SpawnManager SpawnScript;
    private float basic;
    private float zero;
    private float spiral;

    private float enemiesNum = 5;
    // Start is called before the first frame update
    void Start()
    {
        SpawnScript = Spawner.GetComponent<SpawnManager>();
        basic = 0;
        zero = 1;
        spiral = 0;
        enemiesNum = 5;
        SpawnScript.setMaxEnemies(Mathf.RoundToInt(enemiesNum));
    }

    // Update is called once per frame
    void Update()
    {

        //SpawnManager.Enemies basic = SpawnScript.monstersToSpawn[0];
        //SpawnManager.Enemies zero = SpawnScript.monstersToSpawn[1];
        //SpawnManager.Enemies spiral = SpawnScript.monstersToSpawn[2];
        
        basic = Mathf.Min(basic + 0.001f, 1);
        //zero.percentage = 1;
        spiral = Mathf.Min(spiral + 0.00005f, 1);
        enemiesNum = Mathf.Min(enemiesNum + 0.001f, 80);



        SpawnScript.setEnemiesPercent(0, basic);
        SpawnScript.setEnemiesPercent(2, spiral);
        SpawnScript.setMaxEnemies(Mathf.RoundToInt(enemiesNum));
    }
}
