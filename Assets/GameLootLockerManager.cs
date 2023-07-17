using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameLootLockerManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoginRoutine());
    } 
    IEnumerator LoginRoutine() {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Succes");
                done = true;
            }
            else
            {
                Debug.Log("Failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator SubmitScore(int scoreToUpload)
    {
        Debug.Log("SUBMIT SCORE");

        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        string ID = "schmekle";
        
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, ID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Succes");
                done = true;
            }
            else
            {
                Debug.Log("Failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}