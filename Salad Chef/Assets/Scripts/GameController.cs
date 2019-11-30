using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    public static event Action<string> OnShowWinnerScreen;
    public static event Action OnGameOver;

    private int gameOverBothPlayerIndex = 0;
    private Dictionary<int, int> playerScoreDictionary = new Dictionary<int, int>();

    void OnEnable()
    {
        PlayerController.OnTimeOver += GameOver;
    }

    //Checking GameOver
    void GameOver(PlayerController playerController)
    {
        gameOverBothPlayerIndex++;
        playerScoreDictionary.Add(playerController.PlayerId, playerController.playerScore);
        if (gameOverBothPlayerIndex != 2)
        {
            return;
        }
        OnGameOver();
        CheckingWinner();

    }

    //Checking Winner and Call Event For UI
    void CheckingWinner()
    {
        int player1Score = playerScoreDictionary[1];
        int player2Score = playerScoreDictionary[2];
        if(player1Score > player2Score)
        {
            Debug.Log("Player 1 Win");
            OnShowWinnerScreen("Player 1 Win");
        }
        else if(player1Score < player2Score)
        {
            OnShowWinnerScreen("Player 2 Win");
        }
        else
        {
            OnShowWinnerScreen("Draw");
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void OnDisable()
    {
        PlayerController.OnTimeOver -= GameOver;
    }

}
