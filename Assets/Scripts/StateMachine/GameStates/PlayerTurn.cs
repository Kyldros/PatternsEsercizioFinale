using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerTurn : State
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
        if(GameManager.Instance.GetAllPlayerHaveMoved())
        {
            if(elapsed > GameManager.Instance.waitTime) 
            {
                if (GameManager.Instance.gameEnded)
                {
                    GameManager.Instance.stateMachine.SetState(eGameStates.End);
                }
                else
                {
                    if (GameManager.Instance.playerIsInvincible)
                    {
                        GameManager.Instance.InvicibilityTurnPassed();
                        GameManager.Instance.stateMachine.SetState(eGameStates.PlayerTurn);
                    }
                    else
                    {
                        GameManager.Instance.stateMachine.SetState(eGameStates.EnemyTurn);
                    }
                }
            }
            else
            {
                elapsed += Time.deltaTime;
            }
            
        }
    }

}
