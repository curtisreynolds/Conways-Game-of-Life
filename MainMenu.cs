using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public static int cellSize;

    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("setSize", 50);
    }

    // Starts the game on button press
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    //Changes grid size based on slider
    public void OnValueChanged(float newValue)
    {
        int tempSize = Mathf.RoundToInt(newValue);
        //checks to see if number is even & makes it even if it's not
        if (tempSize % 2 == 0)
        {

            cellSize = tempSize;
            txt.text = ":" + cellSize.ToString();
            PlayerPrefs.SetInt("setSize", cellSize);
        }
        else
        {
            cellSize = (tempSize + 1);
            txt.text = ":" + cellSize.ToString();
            PlayerPrefs.SetInt("setSize", cellSize);
        }
    }
}

