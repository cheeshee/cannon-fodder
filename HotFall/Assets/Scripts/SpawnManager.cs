﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    Transform[] spawnPoints;

    [SerializeField]
    Enemies[] monstersToSpawn;

    [System.Serializable]
    public class Enemies
    {
        public ENEMIES tag;
        [Range(0, 1)]
        [SerializeField]
        public float percentage;
    }


    [SerializeField]
    int maxActiveEnemySpawned = 1;
    [SerializeField]
    int currentActiveEnemies;


    private int totalEnemiesSpawned = 0;


    private void Awake()
    {
        spawnPoints = gameObject.GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        spawnEnemyIfNeeded();
    }


    bool isSpawnNeeded()
    {
        
        return  currentActiveEnemies < maxActiveEnemySpawned;
    }


    void spawnEnemyIfNeeded()
    {
        if (isSpawnNeeded())
        {
            
            GameObject enemy = ObjectPooler.Instance.SpawnFromPool(generateRandomEnemy(), getSpawnPosition(), Quaternion.identity);
            setupEnemy(enemy.GetComponent<EnemyController>());
            currentActiveEnemies++;
        }

    }

    void setupEnemy(EnemyController enemy)
    {
        totalEnemiesSpawned++;
        enemy.OnObjectSpawn();
  
            enemy.onCharacterDeath += enemyOnDeath;
       
       
        Debug.Log(enemy.onCharacterDeath);

        
     
    }

    Vector3 getSpawnPosition()
    {
        //Note - Need 1 since index 0 is parent SpawnManager.
        int index = Random.Range(1, spawnPoints.Length);
        return spawnPoints[index].position;
    }

    string generateRandomEnemy()
    {
        float rate = Random.Range(0.0f, 1.0f);
        ENEMIES m = generateMonsterBasedOnPercentage(rate);
        return monstersToSpawn.Length == 0 ? convertEnumToString(0) : convertEnumToString(m);
    }

    ENEMIES generateMonsterBasedOnPercentage(float rate)
    {
        ENEMIES chosenMonster = monstersToSpawn[0].tag;
        float accumulatedChance = accumulatedMonsterChances();
        float percentage = 0;
        foreach (Enemies m in monstersToSpawn)
        {
            percentage += m.percentage / accumulatedChance;
            if (percentage <= rate)
            {
                chosenMonster = m.tag;
            }
        }

        return chosenMonster;
    }

    //0.3 0.5 0.2 
    //0.7

    //0.4 0.5 0.1
    //0.1


    float accumulatedMonsterChances()
    {
        float accumulatedChance = 0;
        foreach (Enemies m in monstersToSpawn)
        {
            accumulatedChance += m.percentage;
        }

        return Mathf.Clamp(accumulatedChance, 0.0001f, 1);
    }



    string convertEnumToString(ENEMIES m)
    {
        switch(m)
        {
            case ENEMIES.BASIC_ENEMY:
                return Pool.BASIC_ENEMY;
            default:
                return Pool.BASIC_ENEMY;
        }
    }

    #region Delegate
    void enemyOnDeath(ICharacter character)
    {
        Debug.Log("DIE" + currentActiveEnemies);
        currentActiveEnemies--;
        character.onCharacterDeath -= enemyOnDeath;
            
    }
    #endregion

    #region Debug
    void OnDrawGizmosSelected()
    {
        // Draw SPawn Points
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
    #endregion

}

public enum ENEMIES
{
    BASIC_ENEMY,
}
