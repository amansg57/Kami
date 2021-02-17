using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    protected const int platformLayer = 12;
    protected const int enemyLayer = 13;
    protected int speed;
    protected int damage;
    protected float _lifetime;
    protected Rigidbody2D rb2d;

    public float Lifetime => _lifetime;

    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, Lifetime);
    }

    public void SetDirection(int direction)
    {
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
    }
}
