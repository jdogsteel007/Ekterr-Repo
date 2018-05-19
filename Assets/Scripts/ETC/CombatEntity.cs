﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Implements health and bullet handling
/// </summary>
public class CombatEntity : MonoBehaviour
{
    public float StartWaitSeconds = 1;
    public bool IsActiveInGame = false, UseRaycastVision = true;

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

    public bool RaycastPlayer()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, Globals.Inst.Player.transform.position);
        Debug.DrawLine(transform.position, Globals.Inst.Player.transform.position, Color.red);
        foreach (RaycastHit2D rh in hits)
            if (rh.collider.gameObject != gameObject && rh.collider.gameObject != Globals.Inst.Player.gameObject && rh.collider.gameObject != Globals.Inst.Player.PlayerShield) return false;
        Debug.DrawLine(transform.position, Globals.Inst.Player.transform.position, Color.green);
        return true;
    }

    public IEnumerator StartWaitDelayCoroutine() { yield return new WaitForSeconds(StartWaitSeconds); IsActiveInGame = true; }

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
