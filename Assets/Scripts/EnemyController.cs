using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const int platformLayer = 12;
    private const int maxHealth = 5;
    private int health;
    private Rigidbody2D rb2d;
    private const float knockbackXStartVelocity = 2f;
    private const float knockbackYStartVelocity = 2f;
    private float currentXVelocity;
    private float smoothVelocity;
    private const float smoothTime = 0.3f;
    private bool knockback = false;
    public bool grounded = false;

    public int MaxHealth => maxHealth;
    public int Health => health;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (knockback)
        {
            currentXVelocity = Mathf.SmoothDamp(currentXVelocity, 0f, ref smoothVelocity, smoothTime);
            if (Math.Abs(currentXVelocity) < 0.1)
            {
                knockback = false;
                currentXVelocity = 0;
            }
            rb2d.velocity = new Vector2(currentXVelocity, rb2d.velocity.y);
        }
        else if (grounded)
        {
            currentXVelocity = -1f;
            rb2d.velocity = new Vector2(currentXVelocity, rb2d.velocity.y);
            
        }

        grounded = false;
    }

    public void OnDamage(int damage, float fireballDirection)
    {
        health -= damage;

        if (health <= 0) Destroy(this.gameObject);

        currentXVelocity = knockbackXStartVelocity;
        if (fireballDirection < 0) currentXVelocity *= -1;
        rb2d.velocity = new Vector2(currentXVelocity, knockbackYStartVelocity);
        knockback = true;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        for (int i = 0; i < other.contactCount; i++)
        {
            if (Vector2.Dot(other.GetContact(i).normal, Vector2.up) > 0.5 && other.gameObject.layer == platformLayer)
            {
                grounded = true;
            }
        }
    }
}
