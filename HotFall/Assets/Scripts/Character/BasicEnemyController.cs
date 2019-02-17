using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : EnemyController
{
    public override void updateDestination()
    {
        base.destination = new Vector2(0f, 0f);
    }
}
