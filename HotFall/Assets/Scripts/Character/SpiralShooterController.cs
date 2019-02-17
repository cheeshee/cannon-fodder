using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShooterController : EnemyController
{


    [SerializeField]
    float angleSpeed = 2;

    [SerializeField]
    float radialSpeed = 10;

    float initialAngle;
    float initialRadius;

    float angle, radius;

    public override void OnObjectSpawn()
    {

        base.OnObjectSpawn();
        if (transform.position.x == 0)
        {
            angle = 90;
        }
        else
        {
            initialAngle = Mathf.Atan(transform.position.y / transform.position.x);
        }

        Vector3 dirVector = transform.position - player.transform.position;
        Debug.Log(dirVector);
        radius = 20;
    }

    protected override void FixedUpdate()
    {
        //Debug.Log(destination);
        updateDestination();
        move();
        //updateSpriteDirection();
    }

    #region Motion

    void spiral(float step)
    {
        angle += (Time.deltaTime + step) * angleSpeed;

        radius = Mathf.Max(0, radius - radialSpeed * Time.deltaTime);
        //sDebug.Log(radius);

        float x = radius * Mathf.Cos(angle + initialAngle);
        float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle + initialAngle);

        transform.position = new Vector2(destination.x - x,destination.y - y);
    }



    protected override void move()
    {
        if (isWithinSpeedUp())
        {
            spiral(0.01f * player.GetComponent<Rigidbody2D>().velocity.magnitude);
        }
        else
        {
            spiral(0);
        }
    }


    void updateSpriteDirection()
    {
        // not sure if this is right
        transform.localScale = gameObject.transform.position;
    }
    #endregion

}
