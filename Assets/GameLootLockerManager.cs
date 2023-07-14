using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameLootLockerManager : MonoBehaviour
{
    public void FunnyMeme()
    {
        Debug.Log("BAGUETTE");
    }

    public void SubmitScore(int scoreToUpload)
    {
        Debug.Log("SUBMIT SCORE");

        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, "schmekle", (LootLockerSubmitScoreResponse response) =>
        {   
            if (response.success)
            {
                Debug.Log("Successfully uploaded");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
    }
}