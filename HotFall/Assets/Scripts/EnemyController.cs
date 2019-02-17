using UnityEngine;

public class EnemyController : ICharacter, IPooledObject {

    const string ANIMATION_DEATH = "Death";
    const string ANIMATION_DAMAGED = "Damaged";

    protected GameObject player;

    protected Vector2 destination;

    [SerializeField]
    protected float meleeDamage = 10;

    [SerializeField]
    protected float speedIncrease = 4;

    private Vector2 destination;

    float step;


    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    }

    public void OnObjectSpawn()
    {
        base.healthPoints = base.maxHealth;
        base.updateHealthBar();

    }

    protected virtual void FixedUpdate()
    {
        setSpeed();
        updateDestination();
        move();
        //updateSpriteDirection();
    }




    #region Motion
    void setSpeed()
    {
        if (isWithinSpeedUp())
        {
            speedIncrease = base.speedModifier + player.GetComponent<Rigidbody2D>().velocity.magnitude;
            step = speedIncrease * Time.deltaTime;
        }
        else
        {
            step = base.SpeedModifier() * Time.deltaTime;
        }

    }

    void move()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, step);
    }

    bool isWithinSpeedUp()
    {
        return Vector3.Distance(player.transform.position, transform.position) - player.GetComponent<CircleCollider2D>().radius <= player.GetComponent<PlayerController>().getSpeedUpRange();
    }

    void updateDestination()
    {
        destination = player.transform.position;
    }

    /*
    void updateSpriteDirection()
    {
        // not sure if this is right
        transform.localScale = gameObject.transform.position;
    }
    */
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
