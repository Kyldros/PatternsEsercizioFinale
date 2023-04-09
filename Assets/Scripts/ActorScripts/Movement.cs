using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Transform movePoint;
    public Team team { get; private set; }
    Tilemap grid => GridManager.Instance.GetTilemap();
    public float ID { get; private set; }

    public bool moved { get; private set; }

    private Vector3 startingPosition;

    private bool registered = false;

    public bool Move(Vector2 direction)
    {
        bool canMove = CanMove(direction);
        if (canMove)
        {
            movePoint.position += (Vector3)direction;
            SetMoved(true);
        }
        SendMoveMessage();
        return canMove;
    }

    private void SendMoveMessage()
    {
        PubSub.Instance.SendMessages(eMessageType.ActorMove,this);
    }

    public bool CanMove(Vector2 direction)
    { 
        return !moved && GridManager.Instance.TileIsWalkable(movePoint.position + (Vector3)direction);
    }

    private void Awake()
    {
        ResetMovePoint();
        SetStartingTeam();
        PickID();
        SetMoved(true);
        SetPubSub();
        startingPosition = transform.position;
    }

    private void SetPubSub()
    {
        PubSub.Instance.RegisterFuncion(eMessageType.StartingGame, RegisterActor);
    }

    private void SetMoved(bool v)
    {
        moved = v;
    }

    public void RegisterActor(object obj)
    {
        if (!registered)
        {
            PubSub.Instance.SendMessages(eMessageType.RegisterActor, this);
            registered = true;
        }  
    }

    private void PickID()
    {
        ID = GameManager.Instance.GetNewID();
        string name= gameObject.name;
        if (team == Team.Player)
            name = "Player";
        if (team == Team.Enemy)
            name = "Enemy";

        gameObject.name = name + " #" + ID;
    }

    public void SetStartingTeam()
    {
        if (GetComponent<PlayerInput>() != null)
            SetTeam(Team.Player);
        else
            SetTeam(Team.Enemy);
    }

    public void SetTeam(Team newTeam)
    {
        team = newTeam;
    }

    private void ResetMovePoint()
    {
        movePoint.parent = null;
        movePoint.position = transform.position;
    }

    private void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    internal Vector3 GetMovePointPosition()
    {
        return movePoint.position;
    }

    public void EnableMovement()
    {
        SetMoved(false);
    }

    internal void ResetPosition()
    {
        transform.position = startingPosition;
        movePoint.position = transform.position;
    }
}
