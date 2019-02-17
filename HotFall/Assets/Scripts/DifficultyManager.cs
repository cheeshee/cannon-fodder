using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField]
    GameObject Spawner;

    private SpawnManager SpawnScript;

    // Start is called before the first frame update
    void Start()
    {
        SpawnScript = Spawner.GetComponent<SpawnManager>();
        SpawnManager.Enemies yes = SpawnScript.monstersToSpawn[0];
        yes.percentage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
