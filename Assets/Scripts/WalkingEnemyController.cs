using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyController : GroundUnitController
{
    private void Start()
    {
        currentXVelocity = -1f;
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
