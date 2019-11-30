using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text player1Score, player2Score;
    public TMP_Text player1TimeLeft, player2TimeLeft;
    public GameObject winningScreen;
    public TMP_Text winnerName;

    void OnEnable()
    {
        PlayerController.OnSendPlayerDataToUIManager += SetPlayersData;
        GameController.OnShowWinnerScreen += EnableWinningScreen;
    }
    // Start is called before the first frame update
    void Start()
    {
        winningScreen.SetActive(false);
    }
    public void SetPlayersData(int playerId,int score, float timeLeft)
    {
        if (playerId == 1)
        {
            player1Score.text = score.ToString();
            player1TimeLeft.text = timeLeft.ToString("F0") + "s";
        }
        if (playerId == 2)
        {
            player2Score.text = score.ToString();
            player2TimeLeft.text = timeLeft.ToString("F0") + "s";
        }

    }

    public void EnableWinningScreen(string playerName)
    {
        winningScreen.SetActive(true);
        winnerName.text = playerName;
    }
    void OnDisable()
    {
        PlayerController.OnSendPlayerDataToUIManager -= SetPlayersData;
        GameController.OnShowWinnerScreen -= EnableWinningScreen;
    }
}

