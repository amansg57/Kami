using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KamiController : MonoBehaviour
{
    private const int groundLayer = 8;
    private const int platformLayer = 12;
    private const int enemyLayer = 13;

    private const int velocityMult = 5;
    private const int airControlMult = 2;
    private const int jumpVelocity = 13;
    private const int projectileLimit = 5;
    private const float knockbackXStartVelocity = 5f;
    private const float knockbackYStartVelocity = 5f;
    private const float smoothTime = 0.3f;

    private const float fireballCooldown = 0.3f;
    private const float hadoukenCooldown = 0.3f;
    private const float laserCooldown = 0.3f;

    private Vector2 colliderCheckStartPos = new Vector2(-50, 0);

    // Time until next projectile can be fired
    private float nextFireball;
    private float nextHadouken;
    private float nextLaser;
    
    [SerializeField]
    private GameObject fireballPrefab;
    [SerializeField]
    private GameObject hadoukenPrefab;
    [SerializeField]
    private GameObject laserPrefab;

    private int health;
    private Rigidbody2D rb2d;
    private bool knockback = false;
    public bool grounded = false;
    private float currentXVelocity;
    private float knockbackVelocity;
    private float smoothVelocity;
    [SerializeField]
    private Transform projectileSpawnLocation;
    private Projectile currentProjectile;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        nextFireball = Time.time;
        currentProjectile = Projectile.Fireball;
    }

    private void FixedUpdate()
    {
        // When being knocked back, player cannot control movement
        if (knockback)
        {
            knockbackVelocity = Mathf.SmoothDamp(knockbackVelocity, 0f, ref smoothVelocity, smoothTime);
            if (Math.Abs(knockbackVelocity) < 0.3)
            {
                knockback = false;
                knockbackVelocity = 0;
                Physics2D.IgnoreLayerCollision(9, 13, false);
            }
            rb2d.velocity = new Vector2(knockbackVelocity, rb2d.velocity.y);
        }
        else if (grounded)
        {
            rb2d.velocity = new Vector2(currentXVelocity, rb2d.velocity.y);
        }
        // If in the air, apply movement as acceleration with a speed cap
        else if ((rb2d.velocity.x < velocityMult && currentXVelocity > 0) || (rb2d.velocity.x > -velocityMult && currentXVelocity < 0))
        {
            rb2d.AddForce(new Vector2(currentXVelocity * airControlMult, 0));
        }
        // Reset grounded check each frame
        grounded = false;
    }

    public void OnMove(InputValue value)
    {
        float direction = value.Get<float>();
        currentXVelocity = direction * velocityMult;

        if (direction > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void OnJump()
    {
        if (grounded) rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
    }

    public void OnFire()
    {
        switch (currentProjectile)
        {
            case Projectile.Fireball:
                ProjectileSpawn(fireballPrefab, ref nextFireball, fireballCooldown);
                break;
            case Projectile.Hadouken:
                break;
            case Projectile.Laser:
                break;
        }
    }

    // left refers to x direction that damage is coming from
    private void OnDamage(bool left)
    {
        knockbackVelocity = knockbackXStartVelocity;
        if (left) knockbackVelocity *= -1;
        rb2d.velocity = new Vector2(knockbackVelocity, knockbackYStartVelocity);
        knockback = true;
        Physics2D.IgnoreLayerCollision(9, 13, true);
    }

    private void ProjectileSpawn(GameObject projectilePrefab, ref float nextProjectile, float projectileCooldown)
    {
        if (Time.time > nextProjectile && ColliderTest(projectileSpawnLocation.position))
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnLocation.position, Quaternion.identity);
            nextProjectile = Time.time + projectileCooldown;
            ProjectileController projectileScript = projectile.GetComponent<ProjectileController>();
            if (transform.localScale.x > 0) projectileScript.SetDirection(1);
            else projectileScript.SetDirection(-1);
        }
    }

    // Returns true if the goal position is not inside the composite collider for the tilemap
    private bool ColliderTest(Vector2 goal)
    {
        RaycastHit2D[] results;
        Vector2 currentPoint = colliderCheckStartPos;
        Vector2 direction = goal - colliderCheckStartPos;
        direction.Normalize();
        int hits = 0;

        while (currentPoint != goal)
        {
            results = Physics2D.LinecastAll(currentPoint, goal, 1<<platformLayer);

            if (results.Length == 0)
            {
                currentPoint = goal;
            }
            else
            {
                currentPoint = results[0].point + (direction / 100f);
                hits++;
            }
        }

        return hits % 2 == 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            OnDamage(Vector2.Dot(other.GetContact(0).normal, Vector2.left) > 0.5);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        for (int i = 0; i < other.contactCount; i++)
        {
            // Checks for a platform contact below the player
            if (Vector2.Dot(other.GetContact(i).normal, Vector2.up) > 0.5 && other.gameObject.layer == platformLayer)
            {
                grounded = true;
            }
        }
    }

}
