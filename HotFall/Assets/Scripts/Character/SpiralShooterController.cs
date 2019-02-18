using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShooterController : EnemyController
{

    [SerializeField]
    float approachSpeed = 50;
    
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        transform.LookAt(base.player.transform);
    }

    protected override void FixedUpdate()
    {
        //Debug.Log(destination);
        updateDestination();
        move();
        updateSpriteDirection();
    }

    #region Motion

    void spiral(float step)
    {

        transform.Translate(transform.right * Time.deltaTime * 0.1f * step);
        transform.RotateAround((player.transform.position + transform.position)/2, new Vector3(0,0,1), step * 10 * Time.deltaTime);

    }

    protected override void move()
    {
        if (isWithinSpeedUp())
        {
            spiral(approachSpeed  * base.player.GetComponent<PlayerController>().getVelocity());
        }
        else
        {
            spiral(approachSpeed);
        }
    }
    
    #endregion

}
