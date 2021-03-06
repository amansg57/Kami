﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int groundLayer = 8;
    private const int playerLayer = 9;
    private const int projectileLayer = 10;

    public IntVariable coinCounter;
    
    private void Start()
    {
        ResetState();
    }

    private void ResetState()
    {
        coinCounter.Value = 0;
    }
}
