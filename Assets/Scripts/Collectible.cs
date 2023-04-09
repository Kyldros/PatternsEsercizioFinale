using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public virtual void OnCollect()
    {
        PubSub.Instance.SendMessages(eMessageType.CollectibleCollected, this);
        gameObject.SetActive(false);
    }

    public virtual void Start()
    {
        PubSub.Instance.SendMessages(eMessageType.RegisterCollectible, this);
    }

}
