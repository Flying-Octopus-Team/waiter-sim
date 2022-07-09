using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighScore
{
    public string userName;
    public int score;

    public HighScore()
    {

    }

    public HighScore(string userName, int score)
    {
        this.userName = userName;
        this.score = score;
    }
}
