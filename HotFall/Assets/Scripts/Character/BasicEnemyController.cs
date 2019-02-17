using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : EnemyController
{

    bool enteringFlag = true;

    public override void OnObjectSpawn()
    {

        base.OnObjectSpawn();
        destination = new Vector2(-transform.position.x, -transform.position.y);
        bool enteringFlag = true;

    }

    protected override void updateDestination()
    {
        if ((transform.position.x == destination.x) && (transform.position.y == destination.y))
        {
            destination = -destination;
        }
        return;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitTarget = col.gameObject;
        if (hitTarget.tag == Tags.PLAYER)
        {
            Debug.Log(this.name);
            onDeath();
            hitTarget.GetComponent<PlayerController>().decrementHealth(meleeDamage);

        }
    }

}
