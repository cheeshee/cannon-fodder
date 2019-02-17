using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : EnemyController
{


    public override void OnObjectSpawn()
    {

        base.OnObjectSpawn();
        destination = new Vector2(-transform.position.x, -transform.position.y);

    }

    void OnBecameInvisible()
    {       
       gameObject.SetActive(false);
       onDeath();
    }

    protected override void updateDestination()
    {
        return;
    }
}
