using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogicScript : MonoBehaviour
{
    public XOSpawnerScript spawnerXO; // get the link to the XOSpawnerScript
    public WinLineScript spawnerLine;   // get the lin to the WinLineScript
    private Collider2D[] colliders;  // list of game fields (1-9)
    public Transform fieldParent;  // Reference to the parent object (e.g., "Field")
    public GameSceneManagerScript gameSceneManager;
    public Text xScoreText;
    public Text oScoreText;

    public int turn = 1;    // player turn. start from X
    private int amountOfTurns = 0;  // amount of played turns
    private static int xScore = 0;
    private static int oScore = 0;

    private int firstWinCollider = 0; // the first win collider
    private int middleWinCollider = 0; // the second win collider
    private int lastWinCollider = 0;  // the last win collider
    private string winValue = "";       // define which figure is win X or O

    private bool isWin = false;    // used for check that win condition is checked or no
    private bool isDraw = false;    // used for check that draw
    private bool isDiagnolWinLine = false;    // used to check that Win line is diagnol or not
    private bool isHorizontalWinLine = false;    // used to check that Win line is horizontal or not

    private Dictionary<int, int> clickedColliders = new Dictionary<int, int>(); // Track already clicked colliders
    private List<Collider2D> collider2Ds = new List<Collider2D>();  // list which contains all colliders. used to disable other colliders when the win condition is get

    // List of win conditions
    List<int[]> winConditions = new List<int[]>
        {
            new int[] { 1, 2, 3 }, new int[] { 3, 2, 1 },
            new int[] { 1, 4, 7 }, new int[] { 7, 4, 1 },
            new int[] { 1, 5, 9 }, new int[] { 9, 5, 1 },
            new int[] { 2, 5, 8 }, new int[] { 8, 5, 2 },
            new int[] { 3, 5, 7 }, new int[] { 7, 5, 3 },
            new int[] { 3, 6, 9 }, new int[] { 9, 6, 3 },
            new int[] { 4, 5, 6 }, new int[] { 6, 5, 4 },
            new int[] { 7, 8, 9 }, new int[] { 9, 8, 7 }
        };

    List<int[]> diagonalWinLineList = new List<int[]>
    {
        new int[] {0, 8}, new int[] {8, 0},
        new int[] {2, 6}, new int[] {6, 2}
    };

    List<int[]> horizontalWinLineList = new List<int[]>
    {
        new int[] {0, 2}, new int[] {2, 0},
        new int[] {3, 5}, new int[] {5, 3},
        new int[] {6, 8}, new int[] {8, 6}
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // XOSpawnerScript object
        spawnerXO = GameObject.FindGameObjectWithTag("SpawnerXO").GetComponent<XOSpawnerScript>();
        spawnerLine = GameObject.FindGameObjectWithTag("SpawnerLine").GetComponent<WinLineScript>();

        // Find all Collider2D components in the "Field" parent
        if (fieldParent != null)
        {
            colliders = fieldParent.GetComponentsInChildren<Collider2D>();
        }
        else
        {
            Debug.LogError("Field parent is not assigned!");
        }
        collider2Ds.AddRange(colliders);

        xScoreText.text = $"{xScore}";
        oScoreText.text = $"{oScore}";
    }

    // Update is called once per frame
    void Update()
    {
        // if LMB is pressed
        if (Input.GetMouseButtonDown(0) == true)
        {
            // Convert mouse position to world point
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Iterate through the colliders
            foreach (Collider2D collider in colliders)
            {
                int colliderNameInt = Convert.ToInt32(collider.gameObject.name);

                // Check if the mouse position overlaps the collider and it's not already clicked
                if (collider.OverlapPoint(mousePosition) && !clickedColliders.ContainsKey(colliderNameInt))
                {
                    clickedColliders.Add(colliderNameInt, turn); // Add to the set of clicked colliders

                    if (turn == 1)  // if X turn
                    {
                        // spawn X on corespondent field
                        spawnerXO.SpawnX(collider.transform.position.x, collider.transform.position.y, collider.transform.position.z);

                        turn = 0;   //change turn to O
                    }
                    else if (turn == 0) // if O turn
                    {
                        // spawn O on corespondent field
                        spawnerXO.SpawnO(collider.transform.position.x, collider.transform.position.y, collider.transform.position.z);

                        turn = 1;   // change turn to X
                    }

                    collider.enabled = false;   // Optionally disable the collider
                    collider2Ds.Remove(collider);
                    amountOfTurns++;    // increase the turns amount
                    break; // Break the loop since we've handled this click
                }
            }

            // check from turm 5 the win condition
            if (amountOfTurns >= 5 && amountOfTurns < 9)
            {
                if (CheckWinCondition(clickedColliders, winConditions) && !isWin)
                {
                    for (int i = 0; i < collider2Ds.Count; i++)
                    {
                        collider2Ds[i].enabled = false;
                    }

                    InscreaseScore(winValue);
                    UpdateScoreText();
                    SpawnWinLine();
                    gameSceneManager.GameOver($"{winValue} WIN");

                    isWin = true;
                }
            }
            else if (amountOfTurns == 9 && !isDraw && !isWin)
            {
                gameSceneManager.GameOver("DRAW");
                isDraw = true;
            }
        }

    }

    bool CheckWinCondition(Dictionary<int, int> fields, List<int[]> winConditions)
    {
        // Iterate through all win conditions
        foreach (int[] condition in winConditions)
        {
            // Ensure all keys in the condition exist in the dictionary
            if (fields.ContainsKey(condition[0]) && fields.ContainsKey(condition[1]) && fields.ContainsKey(condition[2]))
            {
                // Get the values for the current win condition
                int value1 = fields[condition[0]];
                int value2 = fields[condition[1]];
                int value3 = fields[condition[2]];

                // Check if all values are the same (either all 0 or all 1)
                if ((value1 == value2 && value2 == value3) && (value1 == 0 || value1 == 1))
                {
                    firstWinCollider = condition[0] - 1;
                    middleWinCollider = condition[1] - 1;
                    lastWinCollider = condition[2] - 1;

                    DefineWinValue(value1);

                    return true; // A win condition is met
                }
            }
        }

        return false; // No win condition is met
    }

    void SpawnWinLine()
    {
        bool startLeft = true;
        foreach (int[] condition in diagonalWinLineList)
        {
            if (firstWinCollider == condition[0] && lastWinCollider == condition[1])
            {
                isDiagnolWinLine = true;
                startLeft = (lastWinCollider == 2 || lastWinCollider == 6) ? false : true;
            }
        }

        foreach (int[] condition in horizontalWinLineList)
        {
            if (firstWinCollider == condition[0] && lastWinCollider == condition[1])
            {
                isHorizontalWinLine = true;
            }
        }

        if (!isHorizontalWinLine && !isDiagnolWinLine)
        {
            spawnerLine.SpawnStraightLine(colliders[middleWinCollider].transform.position.x, colliders[middleWinCollider].transform.position.y);
        }
        else if (isDiagnolWinLine)
        {
            spawnerLine.SpawnDiagonalLine(colliders[middleWinCollider].transform.position.x, colliders[middleWinCollider].transform.position.y, startLeft);
        }
        else if (isHorizontalWinLine)
        {
            spawnerLine.SpawnHorizontalLine(colliders[middleWinCollider].transform.position.x, colliders[middleWinCollider].transform.position.y);
        }
    }

    void DefineWinValue(int value)
    {
        if (value == 0)
        {
            winValue = "O";
        }
        else if (value == 1)
        {
            winValue = "X";
        }
    }

    void InscreaseScore(string whoWin)
    {
        if (whoWin == "X")
        {
            xScore++;
        }
        else if (whoWin == "O")
        {
            oScore++;
            
        }
    }

    private void UpdateScoreText()
    {
        if (xScoreText != null && oScoreText != null)
        {
            xScoreText.text = $"{xScore}";
            oScoreText.text = $"{oScore}";
        }
    }
}
