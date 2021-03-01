using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all 'units' - the player character and enemies
public class UnitController : MonoBehaviour
{
    protected int maxHealth;
    protected int health;

    protected Rigidbody2D rb2d;

    public int MaxHealth => maxHealth;
    public int Health => health;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
    }

    public virtual void OnDamage(int damage, bool left)
    {
        health -= damage;
        if (health <= 0) OnDeath();
    }

    protected virtual void OnDeath() {}
}
