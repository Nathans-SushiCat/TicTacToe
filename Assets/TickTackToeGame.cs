using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TickTackToeGame : MonoBehaviour
{
    [SerializeField] GameObject[] gm;
    GameObject[,] Map;
    [SerializeField] GameObject[] grid;
    [SerializeField] GameObject[] playerCanvasObjects;
    Color normalColor, selectedColor;
    int maxplayer = 0;
    int selectedPlayer = 0;
    float[] Scales = {0.8f,0.6f, 0.5f};
    float[] distances = { 3, 2.5f, 2};
    float[] startsAt = { -3f, -3.75f, -4};

    bool startGame = false;

    //Grid 4*4 1,25  2,5

    private void Update()
    {
        if (!startGame)
        {
            return;
        }



        normalColor = Color.white;
        selectedColor = Color.blue;
        for (int i = 0; i < playerCanvasObjects.Length; i++)
        {
            playerCanvasObjects[i].SetActive(false);
        }
        for(int i = 0; i < maxplayer; i++)
            playerCanvasObjects[i].SetActive(true);

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i].SetActive(false);
        }

        grid[maxplayer - 2].SetActive(true);
        Map = new GameObject[maxplayer+1, maxplayer+1];

        for (int i = 0; i < maxplayer+1; i++)
        {
            for (int j = 0; j < maxplayer+1; j++)
            {
                Map[i, j] = Instantiate(gm[0], new Vector3(i * distances[maxplayer - 2] + startsAt[maxplayer - 2], j * distances[maxplayer - 2] + startsAt[maxplayer - 2], 0), Quaternion.identity);
                Map[i, j].transform.localScale = new Vector3(Scales[maxplayer - 2], Scales[maxplayer - 2], Scales[maxplayer - 2]);
                Map[i, j].GetComponent<EmptySpace>().setPos(i, j);
            }
        }
        startGame = false;
    }
    public void setPos(int x, int y)
    {
        selectedPlayer = selectedPlayer < maxplayer ? (selectedPlayer += 1) : 1;
        
        for (int i = 0; i < maxplayer-1; i++)
        {

            //playerCanvasObjects[i].GetComponent<Renderer>().GetComponentInChildren<TMP_Text>().color = Color.white;
        }

        //playerCanvasObjects[selectedPlayer].GetComponent<Renderer>().GetComponentInChildren<TMP_Text>().color = Color.red;

        Destroy(Map[x,y]);
        Debug.Log(selectedPlayer);
        Map[x, y] = Instantiate(gm[selectedPlayer], 
            new Vector3(
                x * distances[maxplayer - 2] + startsAt[maxplayer - 2],
                y * distances[maxplayer - 2] + startsAt[maxplayer - 2],
                0),
            Quaternion.identity
        );

        Map[x,y].transform.localScale = new Vector3(Scales[maxplayer - 2]-0.1f, Scales[maxplayer - 2] - 0.1f, Scales[maxplayer - 2] - 0.1f);
    }

    public void setPlayer(int value)
    {
        grid[value-2].SetActive(true);
        maxplayer = value;
        startGame = true;
        GameObject.Find("ChooseMode").SetActive(false);
    }
}