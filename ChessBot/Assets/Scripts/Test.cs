using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Chess;

public class Test : MonoBehaviour
{
    [MenuItem("MY TOOLS/Run Test")]
    public static void Run()
    {
        Debug.Log("started!");
        Board.LoadFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        LogBoard();
    }

    public static void LogBoard()
    {
        foreach (int piece in Board.GetSquares())
        {
            Debug.Log(piece);
        }

    }

}
