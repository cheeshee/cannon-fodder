using UnityEngine;
using System.Collections;

public abstract class IProjectile : MonoBehaviour
{

    [SerializeField] float velocity;
    //[SerializeField] protected float spellMovementTimeToLive;
    [SerializeField] protected float damage;

    protected float angle;
    //protected float currentTimeToLive; //currentDuration
    protected bool isMoving = true;

    protected string[] listOfObstacleTags = {Tags.ENEMY, Tags.OBSTACLE};

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isMoving)
        {
            transform.Translate(VectorFromAngle(angle + 1.57079632679f) * velocity);
            /*
            currentTimeToLive += Time.fixedDeltaTime;
            if (currentTimeToLive > spellMovementTimeToLive)
            {
                onMovementTimeToLiveStopped();
            }
            */
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        checkIfHitObject(col.tag);
    }

    void checkIfHitObject(string tag)
    {
        for (int i = 0; i < listOfObstacleTags.Length; i++)
        {
            if (tag == listOfObstacleTags[i])
            {
                //onSpellHitObject();
            }
        }
    }

    protected abstract void onHitObject();

    //protected abstract void onMovementTimeToLiveStopped();


    #region Tools
    private Vector2 VectorFromAngle(float theta)
    {
        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
    }
    #endregion

}
