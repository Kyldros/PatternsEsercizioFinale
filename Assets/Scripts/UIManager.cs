using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI invincibilityTurn;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI endTotalScore;
    [SerializeField] private TextMeshProUGUI totalTurn;
    [SerializeField] private TextMeshProUGUI treasureCollected;
    [SerializeField] private TextMeshProUGUI keyCollected;
    [SerializeField] private TextMeshProUGUI powerUpCollected;
    [SerializeField] private TextMeshProUGUI totalDeath;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject playingScreen;


    public static UIManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        ValueReset();
        SetStartingScreen();
        PubSubRegister();
    }

    private void PubSubRegister()
    {
        PubSub.Instance.RegisterFuncion(eMessageType.StartingGame, StartingGameSetup);
        PubSub.Instance.RegisterFuncion(eMessageType.EndingGame, EndingGameSetup);
        PubSub.Instance.RegisterFuncion(eMessageType.CollectibleCollected, UpdatePlayingText);
        PubSub.Instance.RegisterFuncion(eMessageType.TurnStart, UpdateOnTurnStart);
    }

    private void UpdateOnTurnStart(object obj)
    {
        totalScore.text = GameManager.Instance.GetTreasureScore();
        if (GameManager.Instance.playerIsInvincible)
        {
            invincibilityTurn.gameObject.SetActive(true);
            invincibilityTurn.text = GameManager.Instance.remainingInvicibilityTurn.ToString();
        }
        else
        {
            invincibilityTurn.gameObject.SetActive(false);
        }
    }

    private void UpdatePlayingText(object obj)
    {
        totalScore.text = GameManager.Instance.GetTreasureScore();
    }

    private void EndingGameSetup(object obj)
    {
        SetEndingScreen();
        SetEndingValues();
    }

    private void SetEndingValues()
    {
        totalDeath.text = GameManager.Instance.totalReset.ToString();
        endTotalScore.text = GameManager.Instance.GetTotalScore(); 
        totalTurn.text = GameManager.Instance.playerRoundCount.ToString();
        treasureCollected.text = GameManager.Instance.treasureCollected.ToString();
        powerUpCollected.text = GameManager.Instance.poweUpCollected.ToString();
        keyCollected.text = GameManager.Instance.keyCollected.ToString();
    }

    private void StartingGameSetup(object obj)
    {
        ValueReset();
        SetPlayingScreen();
    }



    private void ValueReset()
    {
        startText.text = "Press ''" + GameManager.Instance.startingKey.ToString() + "''  to start the game";
        restartText.text = "Press ''" + GameManager.Instance.startingKey.ToString() + "''  to restart the game";
        invincibilityTurn.text = "0";
        totalDeath.text = "0";
        totalScore.text = "0";
        totalTurn.text = "0";
        treasureCollected.text = "0";
        powerUpCollected.text = "0";
        keyCollected.text = "0";
        endTotalScore.text = "0";
        invincibilityTurn.gameObject.SetActive(false);
    }

    private void SetStartingScreen()
    {
        endScreen.SetActive(false);
        playingScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    private void SetPlayingScreen()
    {
        endScreen.SetActive(false);
        startScreen.SetActive(false);
        playingScreen.SetActive(true);
    }

    private void SetEndingScreen()
    {
        endScreen.SetActive(true);
        startScreen.SetActive(false);
        playingScreen.SetActive(false);
    }

    private void Update()
    {
       

    }

}
