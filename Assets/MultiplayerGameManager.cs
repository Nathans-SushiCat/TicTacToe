using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerGameManager : MonoBehaviour
{
    [SerializeField] GameObject[] gm;
    GameObject[,] Map;
    [SerializeField] GameObject[] grid;
    [SerializeField] GameObject[] playerCanvasObjects;
    [SerializeField] GameObject scoreCanvas;
    public int maxplayer = 0;
    int selectedPlayer = 0;
    float[] Scales = { 0.8f, 0.6f, 0.5f, 0, 0.22f };
    float[] distances = { 3, 2.5f, 2, 0, 1 };
    float[] startsAt = { -3f, -3.75f, -4, 0, -4 };
    public bool gameRunning = true;
    public bool startGame = false;
    float currentCameraRotationTime = -1; // small delay before rotation camera

    //Grid 4*4 1,25  2,5

    private void Update()
    {
        //WIN SCREEN
        if (!gameRunning)
        {
            currentCameraRotationTime += Time.deltaTime;

            // Calculate the interpolation value (between 0 and 1)
            float t = Mathf.Clamp01(currentCameraRotationTime / 0.5f);
            Camera.main.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 30, 10), t);

            for (int i = 0; i < playerCanvasObjects.GetLength(0); i++)
            {
                playerCanvasObjects[i].SetActive(false);
            }

            // Check if the transition is complete
            if (t >= 1f)
            {
                // Transition is complete
                scoreCanvas.SetActive(true);
            }
        }

        //Back To Menu Selection
        if (Input.GetKey(KeyCode.Escape))
            Reload();


        if (!startGame)
        {
            return;
        }


        //Deactivate Score Canvas
        scoreCanvas.SetActive(false);

        for (int i = 0; i < playerCanvasObjects.Length; i++)
        {
            playerCanvasObjects[i].SetActive(false);
        }
        for (int i = 0; i < maxplayer; i++)
            playerCanvasObjects[i].SetActive(true);

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i].SetActive(false);
        }

        grid[maxplayer - 2].SetActive(true);
        Map = new GameObject[maxplayer + 1, maxplayer + 1];

        for (int i = 0; i < maxplayer + 1; i++)
        {
            for (int j = 0; j < maxplayer + 1; j++)
            {
                Map[i, j] = Instantiate(gm[0], new Vector3(i * distances[maxplayer - 2] + startsAt[maxplayer - 2], j * distances[maxplayer - 2] + startsAt[maxplayer - 2], 0), Quaternion.identity);
                Map[i, j].transform.localScale = new Vector3(Scales[maxplayer - 2], Scales[maxplayer - 2], Scales[maxplayer - 2]);
                Map[i, j].GetComponent<EmptySpace>().setPos(i, j);
            }
        }
        selectedPlayer = Random.Range(0, maxplayer);
        playerCanvasObjects[selectedPlayer].GetComponent<CanvasRenderer>().SetColor(Color.red);
        setScorePlayer();
        startGame = false;
    }
    public void setPos(int x, int y)
    {
        if (!gameRunning)
            return;

        selectedPlayer = selectedPlayer < maxplayer ? (selectedPlayer += 1) : 1;

        for (int i = 0; i < maxplayer; i++)
        {
            playerCanvasObjects[i].GetComponent<CanvasRenderer>().SetColor(Color.white);
        }

        playerCanvasObjects[selectedPlayer == maxplayer ? 0 : selectedPlayer].GetComponent<CanvasRenderer>().SetColor(Color.red);

        Destroy(Map[x, y]);
        Map[x, y] = Instantiate(gm[selectedPlayer],
            new Vector3(
                x * distances[maxplayer - 2] + startsAt[maxplayer - 2],
                y * distances[maxplayer - 2] + startsAt[maxplayer - 2],
                0),
            Quaternion.identity
        );

        Map[x, y].transform.localScale = new Vector3(Scales[maxplayer - 2] - 0.1f, Scales[maxplayer - 2] - 0.1f, Scales[maxplayer - 2] - 0.1f);
        checkWin();
        setScorePlayer();
        checkTie();
    }

    public void setScorePlayer()
    {
        ScoreHandler h = gameObject.GetComponent<ScoreHandler>();
        h.setPlayer(selectedPlayer);
        h.WriteScore("Player " + selectedPlayer + " Wins");
    }
    public void setScoreTie()
    {
        ScoreHandler h = gameObject.GetComponent<ScoreHandler>();
        h.WriteScore("Tie : No one Wins");
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainPlayingScene");
    }

    public void Multiplayer()
    {
        SceneManager.LoadScene("Multiplayer Scene");
    }
    //Called By Buttons
    public void setPlayer(int value)
    {
        grid[value - 2].SetActive(true);
        maxplayer = value;
        startGame = true;
        GameObject.Find("ChooseMode").SetActive(false);
    }

    GameObject GetItemAt(int rows, int cols)
    {
        return Map[rows, cols];
    }
    private void checkTie()
    {
        bool containsEmptyObj = false;
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j].name == "Empty(Clone)")
                    containsEmptyObj = true;
            }
        }
        if (!containsEmptyObj)
        {
            setScoreTie();
            gameRunning = containsEmptyObj;
        }
    }
    private void checkWin()
    {
        int rowsColsLength = Map.GetLength(0);

        for (int rows = 0; rows < rowsColsLength; rows++)
        {
            for (int cols = 0; cols < rowsColsLength; cols++)
            {
                GameObject currentItem = GetItemAt(rows, cols);
                if (currentItem.name == "Empty(Clone)")
                    continue;

                foreach (var item in CheckSurroundingPoints(rows, cols))
                {
                    if (rowsColsLength > item.x &&
                        rowsColsLength > item.y &&
                        item.x >= 0 && item.y >= 0 &&
                        currentItem.name == GetItemAt(item.x, item.y).name
                       )
                    {
                        highlightObj(currentItem);
                        highlightObj(Map[item.x, item.y]);
                        highlightObj(Map[(item.x + rows) / 2, (item.y + cols) / 2]);
                        gameRunning = false;
                    }
                }
            }
        }

    }

    private void highlightObj(GameObject gm)
    {
        gm.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private IEnumerable<(int x, int y)> CheckSurroundingPoints(int x, int y)
    {
        int rowCount = this.Map.GetLength(0);
        int colCount = this.Map.GetLength(1);

        // Check the surrounding points
        string centerValue = Map[x, y].name;

        // Check DownLeft
        if (x > 0 && y < colCount - 1 && Map[x - 1, y + 1].name == centerValue)
            yield return (x - 2, y + 2);

        // Check Left
        if (x > 0 && Map[x - 1, y].name == centerValue)
            yield return (x - 2, y);

        // Check UpLeft
        if (x > 0 && y > 0 && Map[x - 1, y - 1].name == centerValue)
            yield return (x - 2, y - 2);

        // Check Up
        if (y > 0 && Map[x, y - 1].name == centerValue)
            yield return (x, y - 2);

        // Check UpRight
        if (x < rowCount - 1 && y > 0 && Map[x + 1, y - 1].name == centerValue)
            yield return (x + 2, y - 2);

        // Check Right
        if (x < rowCount - 1 && Map[x + 1, y].name == centerValue)
            yield return (x + 2, y);

        // Check DownRight
        if (x < rowCount - 1 && y < colCount - 1 && Map[x + 1, y + 1].name == centerValue)
            yield return (x + 2, y + 2);

        // Check Down
        if (y < colCount - 1 && Map[x, y + 1].name == centerValue)
            yield return (x, y + 2);
    }

}
