using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    [SerializeField] private int health = 100;
    private int _healthMax;
    public event EventHandler OnDead;
    public event EventHandler OnDamged;

    private void Awake()
    {
        _healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }
        
        OnDamged?.Invoke(this, EventArgs.Empty);

        if (health == 0)
        {
            Die();
        }
    }
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float) health / _healthMax;
    }

}
