using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public int lives = 2;
    public TMP_Text livesText;
    

    void Start()
    {
        
    }
    
    void Update()
    {
        livesText.text = "Vies: " + lives;
    }
}
