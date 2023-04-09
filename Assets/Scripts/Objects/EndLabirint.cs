using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLabirint : Collectible
{
    public override void OnCollect()
    {
        PubSub.Instance.SendMessages(eMessageType.CollectibleCollected, this);
    }
}
