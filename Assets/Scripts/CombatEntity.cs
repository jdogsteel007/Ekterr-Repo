using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Implements health and bullet handling
/// </summary>
public class CombatEntity : MonoBehaviour
{
    public UnityEvent OnKill = new UnityEvent();

    private int _health = 0;
    public int MaxHealth = 20;
    public int Health
    {
        get { return _health; }
        set {
            if (value < 0)
                _health = 0;
            else if (value > MaxHealth)
                _health = MaxHealth;
            else _health = value;
            }
    }

    private void Start()
    {
        _health = MaxHealth;
    }

    public bool IsFriendly = true; //whether the entity is a friend or an enemy

    public virtual void HandleBullet(Bullet bullet)
    {
        if (bullet.FriendlyBullet != IsFriendly)
            Health -= bullet.Damage;
        if (_health == 0)
        {
            OnKill.Invoke();
            Destroy(gameObject); //RIP!
        }
    }
}
