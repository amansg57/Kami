using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSpikeyEnemy : GroundUnitController
{
    private const int enemyLayer = 13;
    private const int enemyVoidLayer = 14;

    [SerializeField]
    private float jumpXVelocity, jumpYVelocity;
    private bool isChargingJump;

    protected override void GroundMovement()
    {
        if (!isChargingJump)
        {
            StartCoroutine(DelayJump());
            isChargingJump = true;
        }
    }

    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(2.5f);
        rb2d.velocity = new Vector2(jumpXVelocity, jumpYVelocity);
        this.gameObject.layer = enemyVoidLayer;
        isChargingJump = false;
    }

    protected override void AirControl()
    {
        if (rb2d.velocity.y < 0 && ColliderTest(transform.position))
        {
            this.gameObject.layer = enemyLayer;
        }
    }
}
