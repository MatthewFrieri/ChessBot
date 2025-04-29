using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

    void Update()
    {

        if (Game.IsGameOver) { return; }

        if (GameState.Pgn.Count % 2 == 0)
        {
            GameState.DecrementWhiteTime(Time.deltaTime);
        }
        else
        {
            GameState.DecrementBlackTime(Time.deltaTime);
        }
    }
}

