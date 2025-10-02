using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts; //2 ou + images de coeurs ou autre à assigner dans l'inspecteur

    public void SetLives(int lives)
    {
        if (hearts == null) return;
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < lives;
    }
}