using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : ProjectileController
{
    protected override void Awake()
    {
        _lifetime = 3.0f;
        base.Awake();
        speed = 7;
        damage = 1;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == platformLayer && Vector2.Dot(other.GetContact(0).normal, Vector2.up) > 0.5) rb2d.velocity = new Vector2(rb2d.velocity.x, 6);
        else if (other.gameObject.layer == enemyLayer) 
        {
            UnitController enemyController = other.gameObject.GetComponent<UnitController>();
            // These checks are used instead of simply using the projectile direction because the projectile will bounce off the enemy, reversing its direction before the check
            if (Vector2.Dot(other.GetContact(0).normal, Vector2.left) > 0.5)
            {
                enemyController.OnDamage(damage, false);
            }
            else if (Vector2.Dot(other.GetContact(0).normal, Vector2.right) > 0.5)
            {
                enemyController.OnDamage(damage, true);
            }
            // If projectile hits top or bottom of Kami, knockback with direction that projectile is travelling
            else enemyController.OnDamage(damage, rb2d.velocity.x < 0);
            
            Destroy(this.gameObject);
        }
    }
}
