using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : EnemyController
{
    protected override void updateDestination()
    {
        base.destination = new Vector2(0f, 0f);
    }
}
