using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    internal void Deactivate()
    {
        GridManager.Instance.ChangeTile(transform.position, GridManager.Instance.terrainTile);
        gameObject.SetActive(false);
    }

    internal void Setup(Color keyColor)
    {
        GetComponentInChildren<SpriteRenderer>().color = keyColor;
        PubSub.Instance.SendMessages(eMessageType.RegisterDoor, this);
        Activate();
    }

    internal void Activate()
    {
        GridManager.Instance.ChangeTile(transform.position, GridManager.Instance.doorTile);
    }

}
