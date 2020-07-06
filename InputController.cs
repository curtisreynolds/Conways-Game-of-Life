using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public simulationController simulationcontroller;

    public GameObject pauseText;

    private bool pause = false;

    // Update is called once per frame
    // looks for player input every frame
    void Update()
    {
        //calls function on left click
        if (Input.GetMouseButton(0))
        {
            simulationcontroller.MouseClicked();
        }

        //calls function on space bar press 
        if (Input.GetKeyDown("space"))
        {
            //checks to see if the game is paused or not and reacts accordingly
            if (pause)
            {
                simulationcontroller.stopTick = false;
                pause = false;
                pauseText.GetComponent<CanvasGroup>().alpha = 0f;
            }
            else if (!pause)
            {
                simulationcontroller.stopTick = true;
                pause = true;
                pauseText.GetComponent<CanvasGroup>().alpha = .6f;         
            }
        }

        //checks for returns key press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            simulationcontroller.ClearScreen();
        }

        //check for backspace key press
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            simulationcontroller.Reload();
        }
    }
}
