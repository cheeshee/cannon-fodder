using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IProjectile, IPooledObject
{
    //Animation
    //const string ANIMATION_EXPLOSION = "Explode";
    //const float EXPLOSION_ANIMATION_TIME = 0.4f;

    /*
protected override void onMovementTimeToLiveStopped()
{
    gameObject.SetActive(false);
}
*/
    Animator anim;

    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    public void OnObjectSpawn()
    {
        anim = GetComponent<Animator>();
        Vector3 dir = transform.rotation.eulerAngles;
        angle = Utilities.getAngleDegBetween(dir.y, dir.x) + 90;
        //base.currentTimeToLive = 0;
        isMoving = true;
        anim.SetTrigger("isMoving");
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitTarget = col.gameObject;
        if (hitTarget.tag == Tags.ENEMY)
        {
            gameObject.SetActive(false);
            hitTarget.GetComponent<ICharacter>().decrementHealth(damage);
            
        }


        base.OnTriggerEnter2D(col);
    }


    protected override void onHitObject()
    {
        isMoving = false;
        //GetComponent<Animator>().SetTrigger(ANIMATION_EXPLOSION);
        //Invoke("onFinishExplosionAnimation", EXPLOSION_ANIMATION_TIME);
    }

    void onFinishExplosionAnimation()
    {
        gameObject.SetActive(false);
    }

}
