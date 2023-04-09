using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private bool isInvincible => GameManager.Instance.playerIsInvincible;
    [SerializeField] int HP = 1;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damager damager = (Damager)collision.GetComponent(typeof(Damager));

        if (damager != null)
        {
            TakeDamage(damager);
        }
    }

    private void TakeDamage(Damager damager)
    {
        if(!isInvincible)
        {
            HP -= damager.GetDamage();
            if(HP <= 0) 
            {
                HP = 0;
                PubSub.Instance.SendMessages(eMessageType.PlayerDied, true);
            }
        }
    }
}
