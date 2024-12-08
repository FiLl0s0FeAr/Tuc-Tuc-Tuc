using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{    
    public XOSpawnerScript spawnerXO; // get the link to the XOSpawnerScript
    public WinLineScript spawnerLine;   // get the lin to the WinLineScript
    public Collider2D[] colliders;  // list of game fields (1-9)

    public int turn = 1;    // player turn. start from X
    private int amountOfTurns = 0;  // amount of played turns
    private float firstWinCollider = 0; // the first win collider
    private float lastWinCollider = 0;  // the last win collider
    private bool win = false;    // used for check that win condition is checked or no

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // XOSpawnerScript object
        spawnerXO = GameObject.FindGameObjectWithTag("SpawnerXO").GetComponent<XOSpawnerScript>();
        spawnerLine = GameObject.FindGameObjectWithTag("SpawnerLine").GetComponent<WinLineScript>();
        collider2Ds.AddRange(colliders);
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
            if (amountOfTurns >= 5)
            {
                if (CheckWinCondition(clickedColliders, winConditions) && !win)
                {
                    for(int i = 0; i < collider2Ds.Count; i++) 
                    {
                        collider2Ds[i].enabled = false;
                    }
                    //Debug.Log(firstWinCollider);
                    //Debug.Log(lastWinCollider);
                    spawnerLine.spawnLine(colliders[0].transform.position.x, colliders[8].transform.position.x);    // test
                    // TODO: add Win or GaveOver screen
                    win = true;
                }
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
                    firstWinCollider = condition[0];
                    lastWinCollider = condition[2];

                    return true; // A win condition is met
                }
            }
        }

        return false; // No win condition is met
    }

    float DefineWinLineCoordinates()
    {
        return 0;
    }
}
