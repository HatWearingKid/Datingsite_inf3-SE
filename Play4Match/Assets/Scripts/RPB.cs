﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPB : MonoBehaviour
{
    public GameObject LoadingBar;
    public Transform TextIndicator;
    public GameObject ProgressBar;
    //public Transform TextLoading;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        // If bar is not full, fill it
        if (currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
            TextIndicator.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
        }
        else
        {   // if bar is full reset it
            LoadingBar.GetComponent<Image>().fillAmount = 0;
            currentAmount = 0;
        }
        LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
    }
}