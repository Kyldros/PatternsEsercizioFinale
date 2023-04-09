using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : State
{
    public override void OnEnter()
    {
        Debug.Log(this.ToString());
        PubSub.Instance.SendMessages(eMessageType.EndingGame, true);
    }

    public override void OnExit()
    {
        PubSub.Instance.SendMessages(eMessageType.StartingGame, true);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(GameManager.Instance.startingKey))
        {
            GameManager.Instance.stateMachine.SetState(eGameStates.PlayerTurn);
        }
    }
}
