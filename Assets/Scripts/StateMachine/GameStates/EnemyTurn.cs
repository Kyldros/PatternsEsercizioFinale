using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : State
{
    float elapsed;

    public override void OnEnter()
    {
        elapsed = 0;
        PubSub.Instance.SendMessages(eMessageType.TurnStart, this);
        Debug.Log(this.ToString());
    }

    public override void OnExit()
    {
        PubSub.Instance.SendMessages(eMessageType.TurnEnded, this);
    }

    public override void OnUpdate()
    {
        if (GameManager.Instance.GetAllEnemyesHaveMoved()) 
        {
            if (elapsed > GameManager.Instance.waitTime)
            {
                GameManager.Instance.stateMachine.SetState(eGameStates.PlayerTurn);
            }
            else
            {
                elapsed += Time.deltaTime;
            }
            
        }
    }
}
