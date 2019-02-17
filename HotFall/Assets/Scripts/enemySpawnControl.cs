using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnControl : MonoBehaviour
{
    Transform[] spawnPoints;

    [SerializeField]
    Enemy[] enemyToSpawn;

    [System.Serializable]
    public class Enemy
    {
        public ENEMY tag;
        [Range(0, 1)]
        [SerializeField]
        public float percentage;
    }

    [Range(0, 20)]
    [SerializeField]
    int maxActiveEnemySpawned = 1;
    int currentActiveEnemies;

    private void Awake()
    {
        Debug.Log("CHECKPOINT REACHED");
        spawnPoints = gameObject.GetComponentsInChildren<Transform>();
    }

    bool isSpawnNeeded()
    {
        return currentActiveEnemies < maxActiveEnemySpawned;
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
        enemy.onCharacterDeath += enemyOnDeath;
        enemy.OnObjectSpawn();
    }

    //CHANGE THE REGION FOR SPAWN POSITIONS
    Vector3 getSpawnPosition()
    {
        //Note - Need 1 since index 0 is parent SpawnManager.
        int index = Random.Range(1, spawnPoints.Length);
        return spawnPoints[index].position;
    }

    string generateRandomEnemy()
    {
        float rate = Random.Range(0.0f, 1.0f);
        ENEMY e = generateEnemyBasedOnPercentage(rate);
        return enemyToSpawn.Length == 0 ? convertEnumToString(0) : convertEnumToString(e);
    }
    
    ENEMY generateEnemyBasedOnPercentage(float rate)
    {
        ENEMY chosenEnemy = enemyToSpawn[0].tag;
        float accumulatedChance = accumulatedEnemyChances();
        float percentage = 0;
        foreach (Enemy e in enemyToSpawn)
        {
            percentage += e.percentage / accumulatedChance;
            if (percentage <= rate)
            {
                chosenEnemy = e.tag;
            }
        }

        return chosenEnemy;
    }

    float accumulatedEnemyChances()
    {
        float accumulatedChance = 0;
        foreach (Enemy e in enemyToSpawn)
        {
            accumulatedChance += e.percentage;
        }

        return Mathf.Clamp(accumulatedChance, 0.0001f, 1);
    }

    string convertEnumToString(ENEMY e)
    {
        switch (e)
        {
            case ENEMY.ENEMY:
                return Pool.ENEMY;
            default:
                return Pool.ENEMY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnEnemyIfNeeded();
    }

    #region Delegate
    void enemyOnDeath()
    {
        currentActiveEnemies--;
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

public enum ENEMY
    {
        ENEMY,
    }
