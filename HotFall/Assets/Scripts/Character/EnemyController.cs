﻿using UnityEngine;

public class EnemyController : ICharacter, IPooledObject {

    const string ANIMATION_DEATH = "Death";
    const string ANIMATION_DAMAGED = "Damaged";

    protected GameObject player;
    [SerializeField]
    protected float meleeDamage = 10;
    [SerializeField]
    protected float speedMultiplier = 1;
    [SerializeField]
    protected float maximumSpeedMultiplier = 3f;

    protected Vector2 destination;

    float step;

    protected void OnTriggerEnter2D(Collider2D col)
    {
        int damage = 1;

        GameObject hitTarget = col.gameObject;
        if (hitTarget.tag == Tags.PLAYER)
        {
            Debug.Log("Ouch that hurt");
            gameObject.SetActive(false);
            hitTarget.GetComponent<PlayerController>().decrementHealth(damage);

        }
    }


    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    }

    public void OnObjectSpawn()
    {
        base.healthPoints = base.maxHealth;
        base.updateHealthBar();
        updateSpriteDirection();

    }

    protected virtual void FixedUpdate()
    {
        setSpeed();
        updateDestination();
        move();
        updateSpriteDirection();
    }




    #region Motion
    void setSpeed()
    {
        if (isWithinSpeedUp())
        {
            float speedIncrease = base.speedModifier * (speedMultiplier * player.GetComponent<Rigidbody2D>().velocity.magnitude);
            step = Mathf.Min(speedIncrease * Time.deltaTime, maximumSpeedMultiplier * base.SpeedModifier() * Time.deltaTime);
        }
        else
        {
            step = base.SpeedModifier() * Time.deltaTime;
        }
    }

    private void move()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, step);
    }

    private bool isWithinSpeedUp()
    {
        return Vector3.Distance(player.transform.position, transform.position) - player.GetComponent<CircleCollider2D>().radius <= player.GetComponent<PlayerController>().getSpeedUpRange();
    }

    public virtual void updateDestination()
    {
        destination = player.transform.position;
    }

    
    void updateSpriteDirection()
    {
        
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        Vector2 position = new Vector2(x,y);

        Vector2 distance = position - destination;   
             
        float angle = Utilities.getAngleDegBetween(distance.y, distance.x) - 90;
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, angle);

    }
    
    #endregion

    #region LifeDeath

    protected override void onDeath()
    {
        onCharacterDeath(this);

        gameObject.SetActive(false);
        
        // runAnimation(ANIMATION_DEATH);
        //Invoke("completeDeathAnimation", 1.5f);
        
    }

    void runAnimation(string name)
    {
        //GetComponent<Animator>().SetTrigger(name);
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