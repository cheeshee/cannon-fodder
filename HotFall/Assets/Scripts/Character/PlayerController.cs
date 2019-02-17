﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Health")]
    [SerializeField]
    public float healthPoints;
    [SerializeField]
    public GameObject healthBar;

    public float maxHealth;


    [SerializeField]
    float forceMultipler;

    [SerializeField]
    protected float speedUpRange = 5;

    private Rigidbody2D playerRigidbody;
    private Animator animate;

    bool delayFlag = false;

    // Use this for initialization
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = Utilities.directionBetweenMouseAndCharacter(gameObject);
        SpriteRotation(direction);
        activateBullet(direction);
    }

    #region Health

    public virtual void decrementHealth(float damage)
    {
        healthPoints = Mathf.Clamp(healthPoints - damage, 0, maxHealth);
        Debug.Log(healthPoints);
        updateHealthBar();
        if (isHealthZero())
        {
            onDeath();
        }

    }

    public void updateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(1, healthPoints / maxHealth * 1, 1);
    }

    protected bool isHealthZero()
    {
        return healthPoints <= 0;
    }

    protected virtual void onDeath()
    {
        gameObject.SetActive(false);
    }

    #endregion



    #region CharacterMovement

    //Behaviour while colliding with another solid object e.g enemy meele


    private void Move(Vector3 direction)
    {
        //playerRigidbody.velocity = (Vector3.Normalize(direction) * playerSpell.MoveSpeed() * playerSpell.SpeedModifier());
    }


    private void SpriteRotation(Vector3 direction)
    {
        float angle = Utilities.getAngleDegBetweenMouseAnd(gameObject) - 90;
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, angle);
        /*

        if (direction != Vector3.zero)
        {
            if (Mathf.Abs(x) >= Mathf.Abs(y)) //Look horizontally as priority
            {
                animate.SetBool(IDLE, (x > 0));
            }
            else
            {
                if (y > 0)
                {
                    //playerSpriteRenderer.sprite = upSprite;
                }
                else
                {
                    //playerSpriteRenderer.sprite = downSprite;
                }
            }
        }*/

    }

    #endregion

    private Quaternion getPlayerRotation()
    {
        float angle = Utilities.getAngleDegBetweenMouseAnd(gameObject) - 90 + 11;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }


    void activateBullet(Vector3 direction)
    {
        if (InputManager.isFiring() && delayFlag)// && !cooldownHolder.isCoolingDown(0)
        {
            playerRigidbody.AddForce(-forceMultipler * Vector3.Normalize(direction), ForceMode2D.Impulse);
            // cooldownHolder.InitiateCooldown(0);
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool(Pool.BULLET, transform.position, getPlayerRotation());
            bullet.GetComponent<Bullet>().OnObjectSpawn();
        }
        delayFlag = !delayFlag;
    }

    public float getSpeedUpRange()
    {
        return speedUpRange;
    }

    /*
    protected void onDeath()
    {
        if (onCharacterDeath != null)
        {
            gameObject.SetActive(false);
            onCharacterDeath();
        }
        onCharacterDeath = null;
    }
    */
    
}