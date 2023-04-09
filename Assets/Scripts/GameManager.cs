using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get 
        { 
            if (_instance == null)
            {
                GameObject gameManagerObject = new("# Game Manager");
                _instance = gameManagerObject.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    [SerializeField] public float waitTime { get; private set; } = 0.1f;
    [SerializeField] eGameStates staritingGameState = eGameStates.Start;
    [SerializeField] int invincibilityTurn = 10;
    [SerializeField] public KeyCode startingKey { get; private set; } = KeyCode.E;
    public bool playerIsInvincible { get; private set; }
    public bool gameEnded { get; private set; }
    [SerializeField] int coinScore = 10;
    [SerializeField] int keyScore = 5;
    [SerializeField] int powerUpScore = 5;
    [SerializeField] int deathScore = -50;

    List<Movement> enemyes;
    List<Movement> players;
    List<Collectible> collectibles;
    List<Door> doors;

    private float nextID;
    public int playerRoundCount { get; private set; }
    public int enemyRoundCount { get; private set; }
    private int playerMoved;
    private int enemyMoved;
    public int treasureCollected { get; private set; }
    public int remainingInvicibilityTurn { get; private set; }
    public int totalReset { get; private set; }
    public int keyCollected { get; private set; }
    public int poweUpCollected { get; private set; }


    public StateMachine<eGameStates> stateMachine { get; private set; } = new();

    //INITIAL SETUP
    //===================================================================================================

    private void Awake()
    {
        StateMachineSetup();
        PubSubSetup();
        SetVariables();
    }

    private void SetVariables()
    {
        nextID = -1;
        ResetScore();
        remainingInvicibilityTurn = invincibilityTurn;
        enemyes = new List<Movement>();
        players = new List<Movement>();
        doors = new List<Door>();
        collectibles = new List<Collectible>();
        ResetEnemyMoved();
        ResetPlayerMoved();
    }

    private void ResetScore()
    {
        playerRoundCount = 0;
        enemyRoundCount = 0;
        treasureCollected = 0;
        remainingInvicibilityTurn = 0;
        totalReset = 0;
        keyCollected = 0;
        poweUpCollected = 0;
    }

    internal void RegisterActor(object obj)
    {
        if (obj is Movement)
        {
            Movement movement = (Movement)obj;

            if (movement.team == Team.Player)
                players.Add(movement);
            if (movement.team == Team.Enemy)
                enemyes.Add(movement);
        }
    }

    //PUBSUB
    //===================================================================================================
    private void PubSubSetup()
    {
        PubSub.Instance.RegisterFuncion(eMessageType.TurnEnded, OnTurnEnd);
        PubSub.Instance.RegisterFuncion(eMessageType.TurnStart, OnTurnStart);
        PubSub.Instance.RegisterFuncion(eMessageType.RegisterActor, RegisterActor);
        PubSub.Instance.RegisterFuncion(eMessageType.ActorMove, ActorHasMoved);
        PubSub.Instance.RegisterFuncion(eMessageType.CollectibleCollected, CollectibleReceived);
        PubSub.Instance.RegisterFuncion(eMessageType.RegisterCollectible, RegisterCollectible);
        PubSub.Instance.RegisterFuncion(eMessageType.RegisterDoor, RegisterDoor);
        PubSub.Instance.RegisterFuncion(eMessageType.PlayerDied, PlayerDied);
        PubSub.Instance.RegisterFuncion(eMessageType.StartingGame, StartingSetup);
    }

    //ALTRO
    //===================================================================================================

    private void Update()
    {
        stateMachine.Update();
    }


    internal float GetNewID()
    {
        nextID++;
        return nextID;
    }

    internal int GetTotalPlayer()
    {
        return players.Count;
    }

    internal int GetTotalEnemy()
    {
        return enemyes.Count;
    }

    internal bool GetAllPlayerHaveMoved()
    {
        return playerMoved >= GetTotalPlayer();
    }

    internal bool GetAllEnemyesHaveMoved()
    {
        return enemyMoved >= GetTotalEnemy();
    }

    private void ResetEnemyMoved()
    {
        enemyMoved = 0;
    }

    private void ResetPlayerMoved()
    {
        playerMoved = 0;
    }

    public void PlayerDeathReset()
    {
        ResetAllPosition();
        totalReset++;
        stateMachine.SetState(eGameStates.PlayerTurn);
    }

    private void StartingSetup(object obj)
    {
        ResetAllPosition();
        gameEnded = false;
        ResetScore();
        
    }

    public void ResetAllPosition()
    {
        foreach (Movement m in players)
        {
            m.ResetPosition();
        }
        foreach (Movement m in enemyes)
        {
            m.ResetPosition();
        }
        foreach (Collectible c in collectibles)
        {
            c.gameObject.SetActive(true);
        }
        foreach (Door d in doors)
        {
            d.gameObject.SetActive(true);
            d.Activate();
        }
    }


    //STATE MACHINE
    //===================================================================================================
    private void StateMachineSetup()
    {
        stateMachine.RegisterState(eGameStates.PlayerTurn, new PlayerTurn());
        stateMachine.RegisterState(eGameStates.EnemyTurn, new EnemyTurn());
        stateMachine.RegisterState(eGameStates.Start, new StartGame());
        stateMachine.RegisterState(eGameStates.End, new EndGame());

        stateMachine.SetState(staritingGameState);
    }

    private void OnTurnStart(object obj)
    {
        if (obj is PlayerTurn)
        {
            foreach(Movement movement in players)
            {
                movement.EnableMovement();
                ResetPlayerMoved();
            }
        }

        if (obj is EnemyTurn)
        {
            foreach(Movement movement in enemyes)
            {
                movement.EnableMovement();
                ResetEnemyMoved();
            }
        }

    }

    private void OnTurnEnd(object obj)
    {
        if(obj is PlayerTurn)
        {
            playerRoundCount++;
        }

        if(obj is EnemyTurn)
        {
            enemyRoundCount++;
        }

    }

    private void ActorHasMoved(object obj)
    {
        if (obj is Movement)
        {
            Movement movement = (Movement)obj;
            if (movement.moved)
            {
                if(movement.team == Team.Player)
                {
                    playerMoved++;
                }
                
                if(movement.team == Team.Enemy)
                {
                    enemyMoved++;
                }
            }
        }
    }

    //STATE MACHINE
    //===================================================================================================
    private void CollectibleReceived(object obj)
    {
        if(obj is Collectible)
        {
            Collectible received = (Collectible)obj;

            if(received is Treasure)
            {
                treasureCollected++;
            }

            if(received is Key)
            {
                keyCollected++;
            }

            if(received is PowerUp)
            {
                poweUpCollected++;
                SetPlayerinvincibility(true);
            }

            if (received is EndLabirint)
            {
                gameEnded = true;
            }

        }
    }

    private void RegisterCollectible(object obj)
    {
        if (obj is Collectible)
        {
            collectibles.Add((Collectible)obj);
        }
    }



    private void RegisterDoor(object obj)
    {
        if (obj is Door)
        {
            doors.Add((Door)obj);
        }
    }

    //GAME STATUS
    //===================================================================================================
    private void PlayerDied(object obj)
    {
        if (obj is bool)
        {
            if((bool)obj)
            {
                PlayerDeathReset();
            }
        }
    }


    private void SetPlayerinvincibility(bool value)
    {
        playerIsInvincible = value;
    }

    internal void InvicibilityTurnPassed()
    {
        remainingInvicibilityTurn--;
        if(remainingInvicibilityTurn <= 0)
        {
            playerIsInvincible = false;
            remainingInvicibilityTurn = invincibilityTurn;
        }
    }

    internal string GetTotalScore()
    {
        int totallTreasureScore = coinScore * treasureCollected;
        int totalPowerUpScore = powerUpScore * poweUpCollected;
        int totalKeyScore = keyScore * keyCollected;
        int totalDeathScore = deathScore * totalReset;
        int totalScore = totalDeathScore + totalKeyScore + totalKeyScore + totallTreasureScore;

        return totalScore.ToString();
    }

    internal string GetTreasureScore()
    {
        return (coinScore * treasureCollected).ToString();
    }
}
