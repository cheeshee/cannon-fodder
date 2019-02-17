using UnityEngine;

public class EnemyController : ICharacter, IPooledObject {

    protected const string ANIMATION_DEATH = "Death";
    protected const string ANIMATION_DAMAGED = "Damaged";

    protected GameObject player;
    [SerializeField]
    protected float meleeDamage = 1;
    [SerializeField]
    protected float speedMultiplier = 1;
    [SerializeField]
    protected float maximumSpeedMultiplier = 3f;

    protected Vector2 destination;
    

    float step;





    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    }

    public virtual void OnObjectSpawn()
    {
       
        healthPoints = maxHealth;
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
    protected void setSpeed()
    {
        if (isWithinSpeedUp())
        {
            float speedIncrease = base.speedModifier * (speedMultiplier * player.GetComponent<Rigidbody2D>().velocity.magnitude);
            step = Mathf.Min(speedIncrease * Time.deltaTime, maximumSpeedMultiplier * base.MoveSpeed() * Time.deltaTime);
        }
        else
        {
            step = MoveSpeed() * Time.deltaTime;
        }
    }

    protected virtual void move()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, step);
    }

    protected bool isWithinSpeedUp()
    {
        return Vector3.Distance(player.transform.position, transform.position) - player.GetComponent<CircleCollider2D>().radius <= player.GetComponent<PlayerController>().getSpeedUpRange();
    }

    protected virtual void updateDestination()
    {
        this.destination = player.transform.position;
    }

    
    protected void updateSpriteDirection()
    {

        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        Vector2 position = new Vector2(x, y);

        Vector2 distance = position - destination;
        float angle = Utilities.getAngleDegBetween(distance.y, distance.x) - 90;

        gameObject.transform.eulerAngles = new Vector3(0f, 0f, angle);
        base.updateHealthBar();

    }
    
    #endregion

    #region LifeDeath

    protected override void onDeath()
    {
        gameObject.SetActive(false);
        onCharacterDeath(this);

        
        
        // runAnimation(ANIMATION_DEATH);
        //Invoke("completeDeathAnimation", 1.5f);
        
    }

    void runAnimation(string name)
    {
        //GetComponent<Animator>().SetTrigger(name);
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

    #endregion
}