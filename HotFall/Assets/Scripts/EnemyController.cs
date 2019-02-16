using UnityEngine;

[RequireComponent(typeof(PolyNavAgent))]
public class EnemyController : ICharacter, IPooledObject {

    const string ANIMATION_DEATH = "Death";
    const string ANIMATION_DAMAGED = "Damaged";

    protected PolyNavAgent agent;
    protected GameObject player;

    [SerializeField]
    protected float meleeDamage = 10;

    [SerializeField]
    protected float speedUpRange = 4;

    [SerializeField]
    protected Vector2 destination = new Vector2(0, 0);

    bool isWithinSpeedUp = false;
    
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        agent = GetComponent<PolyNavAgent>();
        setSpeed();
    }

    public void OnObjectSpawn()
    {
        setSpeed();
        agent.enabled = true;
        agent.SetDestination(destination);
    }

    protected virtual void FixedUpdate()
    {
        speedUpIfNeeded();
        updateSpriteDirection();
    }

    #region Motion
    void setSpeed()
    {
        agent.maxSpeed = base.moveSpeed * base.speedModifier;
    }

    public override void modifySpeed(float mod, float time)
    {
        base.modifySpeed(mod, time);
        setSpeed();
    }

    protected override void resetSpeedCoolDown()
    {
        base.resetSpeedCoolDown();
        setSpeed();
    }


    protected virtual void speedUpIfNeeded()
    {
        if (player == null)
        {
            agent.enabled = false;
        }
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= this.speedUpRange)
            {
                speedUp();
            } else
            {
                setSpeed();
            }
        }
    }

    protected virtual void speedUp()
    {
        //time variable is for cool down, which is nothing rn
        float fastSpeed = base.moveSpeed * player.gameObject.GetComponent<ICharacter>().SpeedModifier();
        modifySpeed(fastSpeed, 1);
    }

    /* //only for following player
    void updateSpriteDirection()
    {
        if (agent.movingDirection.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = Vector3.one;
        }
    }
    */
    #endregion

    #region LifeDeath
    public override void decrementHealth(float damage)
    {
        base.decrementHealth(damage);
        if (!isHealthZero())
        {
            runAnimation(ANIMATION_DAMAGED);
        }
    }

    protected override void onDeath()
    {
        runAnimation(ANIMATION_DEATH);
        agent.enabled = false;
        Invoke("completeDeathAnimation", 1.5f);
    }

    void runAnimation(string name)
    {
        GetComponent<Animator>().SetTrigger(name);
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
            player.GetComponent<ICharacter>().damagedByAttacker(meleeDamage);
        }
    }

    #endregion

    #region Gizmo
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, speedUpRange);
    }
    #endregion
}
