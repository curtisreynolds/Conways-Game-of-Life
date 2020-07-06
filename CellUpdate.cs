using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellUpdate : MonoBehaviour
{
    public bool isAlive = false;

    private SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         CellState();
    }

    //swaps color based on cell state
    public void CellState()
    {
        if(isAlive)
        {
            render.color = Color.black;
        }
        if(!isAlive)
        {
            render.color = Color.white;
        }
    }
}

