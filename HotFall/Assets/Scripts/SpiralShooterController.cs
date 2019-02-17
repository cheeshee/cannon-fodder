using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShooterController : ICharacter
{
    const string ANIMATION_DEATH = "Death";
    const string ANIMATION_DAMAGED = "Damaged";

    protected GameObject player;

    [SerializeField]
    protected float meleeDamage = 10;

    [SerializeField]
    protected float speedIncrease = 4;

    [SerializeField]
    protected Vector2 destination = new Vector2(0, 0);

    [SerializeField]
    float angle, radius = 10;

    [SerializeField]
    float angleSpeed = 2;

    [SerializeField]
    float radialSpeed = 10;

    bool isWithinSpeedUp = false;
    Vector2 playerPos;
    //float step;


    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        playerPos = player.transform.position;
        setSpeed();
    }

    public void OnObjectSpawn()
    {
        setSpeed();
    }

    protected virtual void FixedUpdate()
    {
        speedUpIfNeeded();
        //updateSpriteDirection();
    }

    #region Motion

    void spiral(float step)
    {
        //angle, radius = 10;
        //angleSpeed = 2;
        //radialSpeed = 0.5f;
        
        angle += Time.deltaTime * angleSpeed + angleSpeed * step;
        //radius = Vector2.Distance(player.transform.position, transform.position);
        radius = Mathf.Max(0, radius - Time.deltaTime);

        

        float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);

        transform.position = new Vector2(x, y);
        playerPos = player.transform.position;
    }


    void setSpeed()
    {
        //step = base.SpeedModifier() * Time.deltaTime;
        //transform.position = Vector2.MoveTowards(transform.position, destination, step);
        spiral(0);
    }

    /*public override void modifySpeed(float mod, float time)
    {
        base.modifySpeed(mod, time);
        setSpeed();
    }*/

    protected override void resetSpeedCoolDown()
    {
        base.resetSpeedCoolDown();
        setSpeed();
    }


    protected virtual void speedUpIfNeeded()
    {
        if (Vector2.Distance(player.transform.position, transform.position) - player.GetComponent<CircleCollider2D>().radius <= player.GetComponent<PlayerController>().getSpeedUpRange())
        {
            speedUp();
        }
        else
        {
            setSpeed();
        }
    }

    protected virtual void speedUp()
    {
        //time variable is for cool down, which is nothing rn
        //float fastSpeed = base.moveSpeed * player.GetComponent<ICharacter>().SpeedModifier();    //player.gameObject.GetComponent<ICharacter>().SpeedModifier();
        //base.modifySpeed(fastSpeed, 0);
        //agent.maxSpeed = agent.maxSpeed * base.speedModifier;
        //speedIncrease = base.speedModifier + player.GetComponent<Rigidbody2D>().velocity.magnitude;
        //step = speedIncrease * Time.deltaTime;
        //transform.position = Vector2.MoveTowards(transform.position, destination, step);
        spiral(0.01f * player.GetComponent<Rigidbody2D>().velocity.magnitude);
    }

    void updateSpriteDirection()
    {
        // not sure if this is right
        transform.localScale = gameObject.transform.position;
    }
    #endregion


    #region LifeDeath
    public override void decrementHealth(float damage)
    {
        base.decrementHealth(damage);
        if (!isHealthZero())
        {
            //runAnimation(ANIMATION_DAMAGED);
        }
    }

    protected override void onDeath()
    {
        gameObject.SetActive(false);
        // runAnimation(ANIMATION_DEATH);
        // agent.enabled = false;
        //Invoke("completeDeathAnimation", 1.5f);
    }

    void runAnimation(string name)
    {
        //GetComponent<Animator>().SetTrigger(name);
    }

    public void completeDeathAnimation()
    {
        gameObject.SetActive(false);
        if (onCharacterDeath != null)
        {
            onCharacterDeath();
        }
        onCharacterDeath = null;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        GameObject player = other.gameObject;

        if (player.tag == Tags.PLAYER)
        {
            //player.GetComponent<ICharacter>().damagedByAttacker(meleeDamage);
        }
    }

    #endregion
}
