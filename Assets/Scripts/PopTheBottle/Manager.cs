using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject bottleObject;

    private GameManager gameManager;

    private float bottleOffsetY = 0f;
    private float initializedBottleY = 0f;
    private int bottleState = 0;// -1: down, 0: middle, 1: up
    private int lastBottleState = 0;
    private int bottleSaturation = 0;
    private int bottleSaturationMax = 60; //facile : 30, moyen : 60, difficile : 100
    private float winPercentage = 0f;
    private bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        initializedBottleY = bottleObject.transform.position.y;

        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("[Manager] No GameManager found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;

        // Guard against null gameManager
        if (gameManager == null) return;

        // Check for game end
        if (bottleSaturation >= bottleSaturationMax)
        {
            Debug.Log("Game Over! Bottle is full! You won !!");

            gameManager.NotifyWin();

            gameEnded = true;
            //logique de victoire
            return;
        }

        if (gameManager.RemainingTime <= 0f)
        {
            Debug.Log("Game Over! You loose gros nullos...");

            gameManager.NotifyFail();

            gameEnded = true;
            //logique de victoire
            return;
        }

        //Update bottle state
        if (Input.GetAxis("P1_Vertical") != 0)
        {
            if (Input.GetAxis("P1_Vertical") > 0)
            {
                bottleState = 1;
            }
            else
            {
                bottleState = -1;
            }
        }
        else
        {
            bottleState = 0;
        }

        bottleOffsetY = bottleState;

        // Update bottle position
        Vector3 bottlePosition = bottleObject.transform.position;
        bottlePosition.y = initializedBottleY + bottleOffsetY;
        bottleObject.transform.position = bottlePosition;

        // Update bottle saturation
        //make the difference between last and current state
        //the difference is added to the saturation
        if (bottleState != lastBottleState)
        {
            bottleSaturation += Mathf.Abs(bottleState - lastBottleState);
            lastBottleState = bottleState;
        }

        // Calculate win percentage
        winPercentage = (float)bottleSaturation / bottleSaturationMax * 100f;
        Debug.Log("Win Percentage: " + winPercentage + "%");
    }

    // void FixedUpdate()
    // {
    //     if (Input.GetButton("P1_B1"))
    //     {
    //         bottleOffsetY += 0.1f;
    //     }
    // }
}
