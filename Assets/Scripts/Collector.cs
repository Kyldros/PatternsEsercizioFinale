using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collectible collectible = (Collectible)collision.GetComponent(typeof(Collectible));

        if (collectible != null)
        {
            Collect(collectible);
        }
    }

    private void Collect(Collectible collectible)
    {
        collectible.OnCollect();
    }
}
