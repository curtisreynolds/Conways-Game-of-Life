
using System;
using System.Collections;


using UnityEngine;

using UnityEngine.SceneManagement;

public class simulationController : MonoBehaviour
{
    public GameObject[,] cell;
    public GameObject emptyCell;
    public GameObject livingCell;

    public bool stopTick;
    
    public Camera mainCamera;

    private Boolean[,] nextCellStates;
    private bool clearing = false;

    private const float tick = 0.1f;
    
    private int size;

    // Start is called before the first frame update
    void Start()
    {
        size = PlayerPrefs.GetInt("setSize");
        GenerateGrid();
        ResizeCamera();

        StartCoroutine(TickRate(tick));
    }

    /// <summary>
    /// Generates the grid from the size of the 2d array
    /// Functon loops through the size of the array instantiating empty cells for each spot
    /// then sets those cells to the grid location
    /// </summary>
    public void GenerateGrid()
    {
        //generates the grid
        cell = new GameObject[size, size];
        nextCellStates = new Boolean[size, size];
        GameObject emptyGrid = new GameObject();
        emptyGrid.name = "Empty Grid";

        for (int x = 0; x < cell.GetLength(0); x++)
        {
            for (int y = 0; y < cell.GetLength(1); y++)
            {
                GameObject tmp = Instantiate(emptyCell, new Vector3(y - size / 2, x - size / 2, 0), Quaternion.identity);
                cell[x, y] = tmp;
                tmp.name = x + "," + y;
                tmp.transform.SetParent(emptyGrid.transform);
            }
        }
    }
    
    //just resizes the camera to be the right size for the grid
    public void ResizeCamera()
    {
        mainCamera.orthographicSize = size/2;
    }

    /// <summary>
    /// the function that actually cycles the grid and applies the rules of life to each cell
    /// </summary>
    public void CellControl()
    {
        for (int x = 0; x < cell.GetLength(0); x++)
        {
            for(int y = 0; y < cell.GetLength(1); y++)
            {
                bool living = cell[x, y].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive;
                int count = GetLivingNeighbors(x, y);
                bool result = false;
               // Apply the rules of life
                if (living && count < 2)
                {
                    result = false;
                }
                if (living && (count == 2 || count == 3))
                {
                    result = true;
                }
                if (living && count > 3)
                {
                    result = false;
                }
                if (!living && count == 3)
                {
                    result = true;
                }
                nextCellStates[x, y] = result;
            }
        }
    }


    /// <summary>
    /// checks the cells around each grid segment for living & dead cells
    /// </summary>
    public int GetLivingNeighbors(int x, int y)
    {
        int count = 0;

        // Check cell on the right.
        if (x != cell.GetLength(0) - 1)
        {
            if (cell[x + 1, y].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            }
        }

        // Check cell on the bottom right.
        if (x != cell.GetLength(0) - 1 && y != cell.GetLength(1) - 1)
        {
            if (cell[x + 1, y + 1].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            }
        }

        // Check cell on the bottom.
        if (y != cell.GetLength(1) - 1)
        {
            if (cell[x, y + 1].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            }  
        }
            
        // Check cell on the bottom left.
        if (x != 0 && y != cell.GetLength(1) - 1)
        {
            if (cell[x - 1, y + 1].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            }
        }
            
        // Check cell on the left.
        if (x != 0)
        {
            if (cell[x - 1, y].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            } 
        }
            
        // Check cell on the top left.
        if (x != 0 && y != 0)
        {
            if (cell[x - 1, y - 1].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            }   
        }
            
        // Check cell on the top.
        if (y != 0)
        {
            if (cell[x, y - 1].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            }    
        }
            
        // Check cell on the top right.
        if (x != cell.GetLength(0) - 1 && y != 0)
        {
            if (cell[x + 1, y - 1].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive)
            {
                count++;
            }   
        }
        return count;
    }

    /// <summary>
    /// once the next stage is done calculating the cell states this function pushes that data to the live grid
    /// </summary>
    private void IncrementCell()
    {
        for(int x = 0; x <cell.GetLength(0); x++)
        {
            for(int y = 0; y < cell.GetLength(0); y++)
            {
                cell[x, y].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive = nextCellStates[x, y];
            }
        }   
    }

    /// <summary>
    /// clears the field of any living tokens
    /// causes an interrupt in the tick function allowing it to clear the grid
    /// </summary>
    public void ClearScreen()
    {
        clearing = true;
        for (int x = 0; x < cell.GetLength(0); x++)
        {
            for (int y = 0; y < cell.GetLength(0); y++)
            {
                cell[x, y].gameObject.transform.GetChild(0).GetComponent<CellUpdate>().isAlive = false;
            }
        }
        clearing = false;
    }

    //reloads game going back to the menu
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    //when mouse is clicked shoots out a raycast colliding with part of the grid marking it as alive
    public void MouseClicked()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null)
            {  
                hit.transform.gameObject.GetComponent<CellUpdate>().isAlive = true;      
            }
    }

    /// <summary>
    ///changes how fast the simulation in ran
    ///also detaches the sim speed from the framerate allowsing smoother placement while keeping sim viewable
    ///</summary>
    private IEnumerator TickRate(float tick)
    {
        while (true)
        {
            if (!stopTick && !clearing)
            {
                CellControl();
                IncrementCell();
            }
            yield return new WaitForSeconds(tick);
        }
    }


}
