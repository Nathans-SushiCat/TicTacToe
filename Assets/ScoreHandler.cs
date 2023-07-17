using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] float[] playerTimes;
    [SerializeField] float score, time;
    [SerializeField] TMP_Text[] PlayerTimeText;
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] TMP_Text PlayerText;
    [SerializeField] TMP_Text TimeText;
    bool scoreUploaded = false;
    int selPlayer = 10;
    TickTackToeGame game;

    private void Start()
    {
        game = gameObject.GetComponent<TickTackToeGame>();
        playerTimes = new float[PlayerTimeText.Length];

    }

    private void Update()
    {
        
        if (!game.gameRunning && !scoreUploaded)
        {
            Debug.Log("SETTTT");
            GameLootLockerManager LL = gameObject.GetComponent<GameLootLockerManager>();

            StartCoroutine(LL.SubmitScore(1)); //LL.SubmitScore((int)score));

            scoreUploaded = true;
        }

        if (game.maxplayer != 0 && selPlayer != 10)
            countTime();

        score = (int)(time / 10f);
    }
    public void setPlayer(int p)
    {
        selPlayer = p;
    }
    public void countTime()
    {
        playerTimes[selPlayer == game.maxplayer ? 0 : selPlayer] += Time.deltaTime;
        time += Time.deltaTime;
    }

    public void WriteScore(string player)
    {
        PlayerText.text = player;
        ScoreText.text = "Score: " + Math.Round(score, 2).ToString();
        TimeText.text = "Time: " + Math.Round(time,2).ToString() + " s";
        for(int i = 0; i < PlayerTimeText.Length; i++)
        {
            PlayerTimeText[i].text = "Time Player " + (i+1) + ": "  + Math.Round(playerTimes[i],2).ToString() + " s"; 
        }
    }


}
