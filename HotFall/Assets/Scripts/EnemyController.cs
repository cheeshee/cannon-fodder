using UnityEngine;

[RequireComponent(typeof(PolyNavAgent))]
public class EnemyController : ICharacter, IPooledObject {

    const string ANIMATION_DEATH = "Death";
    const string ANIMATION_DAMAGED = "Damaged";

    protected GameObject player;

    [SerializeField]
    protected float meleeDamage = 10;

    [SerializeField]
    protected float speedIncrease = 4;

    [SerializeField]
    protected Vector2 destination = new Vector2(0, 0);

    bool isWithinSpeedUp = false;
    float step;


    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
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
    void setSpeed()
    {
        step = base.SpeedModifier() * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, destination, step);
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
            if (Vector3.Distance(player.transform.position, transform.position) - player.GetComponent<CircleCollider2D>().radius <= player.GetComponent<PlayerController>().getSpeedUpRange())
            {
                speedUp();
            } else
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
        speedIncrease = base.speedModifier + player.GetComponent<Rigidbody2D>().velocity.magnitude;
        step = speedIncrease * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, destination, step);
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
           // runAnimation(ANIMATION_DAMAGED);
        }
    }

    protected override void onDeath()
    {
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
        if (onCharacterDeath != null) {
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

    #region Gizmo
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, player.GetComponent<PlayerController>().getSpeedUpRange());
    }
    #endregion
}
