using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] int damage = 1;
    internal int GetDamage()
    {
        return damage;
    }

}
