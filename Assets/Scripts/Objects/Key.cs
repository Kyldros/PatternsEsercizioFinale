using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Key : Collectible
{
    [SerializeField] Door connectedDoor;
    [SerializeField] Color keyColor = Color.white;
    SpriteRenderer sprite;

    public override void OnCollect()
    {
        connectedDoor.Deactivate();
        base.OnCollect();
    }

    public override void Start()
    {
        base.Start();
        if (connectedDoor == null)
        {
            Debug.LogError("Nessuna porta connessa alla chiave " + keyColor.ToString() +"!");
        }
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = keyColor;
        connectedDoor.Setup(keyColor);
    }

}
