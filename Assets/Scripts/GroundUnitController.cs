using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all ground-based units
public class GroundUnitController : UnitController
{
    protected const int platformLayer = 12;

    protected float knockbackXStartVelocity;
    protected float knockbackYStartVelocity;
    protected float currentXVelocity;
    protected float knockbackVelocity;
    protected float smoothVelocity;
    protected float smoothTime;
    protected float knockbackStopVelocity;
    protected float velocityCap;

    protected bool grounded = false;
    protected bool knockback = false;

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
        else if ((rb2d.velocity.x < velocityCap && currentXVelocity > 0) || (rb2d.velocity.x > -velocityCap && currentXVelocity < 0))
        {
            AirMovement();
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

    protected virtual void AirMovement() {}

    public override void OnDamage(int damage, bool left)
    {
        base.OnDamage(damage, left);
        knockbackVelocity = knockbackXStartVelocity;
        if (left) knockbackVelocity *= -1;
        rb2d.velocity = new Vector2(knockbackVelocity, knockbackYStartVelocity);
        knockback = true;
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
