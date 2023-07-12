using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmptySpace : MonoBehaviour
{
    TickTackToeGame manager;
    int x, y;
    public void setPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    void Start()
    {
        manager= GameObject.Find("GameManager").GetComponent<TickTackToeGame>();
    }
    private void OnMouseDown()
    {
        manager.setPos(x,y);
    }
}
