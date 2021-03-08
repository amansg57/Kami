using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all ground-based units
public class GroundUnitController : UnitController
{
    protected const int platformLayer = 12;

    [SerializeField]
    protected float knockbackXStartVelocity, knockbackYStartVelocity, knockbackStopVelocity, smoothTime, velocityCap;
    protected float currentXVelocity;
    protected float knockbackVelocity;
    protected float smoothVelocity;

    protected bool grounded = false;
    protected bool knockback = false;

    private Vector2 colliderCheckStartPos = new Vector2(-50, 0);

    private void FixedUpdate()
    {
        if (knockback)
        {
            knockbackVelocity = Mathf.SmoothDamp(knockbackVelocity, 0f, ref smoothVelocity, smoothTime);
            if (Math.Abs(knockbackVelocity) < knockbackStopVelocity)
            {
                OnKnockbackEnd();
            }
            rb2d.velocity = new Vector2(knockbackVelocity, rb2d.velocity.y);
        }
        else if (grounded)
        {
            GroundMovement();
        }
        else
        {
            AirControl();
        }
        // Reset grounded check each frame
        grounded = false;
    }

    protected virtual void OnKnockbackEnd()
    {
        knockback = false;
        knockbackVelocity = 0;
    }

    protected virtual void GroundMovement()
    {
        rb2d.velocity = new Vector2(currentXVelocity, rb2d.velocity.y);
    }

    protected virtual void AirControl() {}

    public override void OnDamage(int damage, bool left)
    {
        base.OnDamage(damage, left);
        knockbackVelocity = knockbackXStartVelocity;
        if (left) knockbackVelocity *= -1;
        rb2d.velocity = new Vector2(knockbackVelocity, knockbackYStartVelocity);
        knockback = true;
    }

    // Returns true if the goal position is not inside the composite collider for the tilemap
    protected bool ColliderTest(Vector2 goal)
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

    private void OnCollisionStay2D(Collision2D other)
    {
        for (int i = 0; i < other.contactCount; i++)
        {
            // Checks for a platform contact below the unit
            if (Vector2.Dot(other.GetContact(i).normal, Vector2.up) > 0.5 && other.gameObject.layer == platformLayer)
            {
                grounded = true;
            }
        }
    }
}
