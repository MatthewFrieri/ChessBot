using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Opening
{

    private const string path = "Assets/Games.txt";
    private static List<string> matchingLines;

    private static System.Random random = new System.Random(5334);


    static Opening()
    {
        matchingLines = File.ReadAllLines(path).ToList();
    }

    public static string NextMoveAlgebraic()
    {
        List<string> nextMovesOptions = FindNextMoveOptions();

        if (nextMovesOptions.Count == 0) { return null; }

        return nextMovesOptions[random.Next(nextMovesOptions.Count)];
    }

    private static List<string> FindNextMoveOptions()
    {
        List<string> pgn = GameState.Pgn;
        List<string> nextMoveOptions = new List<string>();
        List<string> newMatchingLines = new List<string>();

        foreach (string line in matchingLines)
        {
            string[] moves = line.Split(" ");
            bool match = true;

            for (int i = 0; i < pgn.Count; i++)
            {
                if (pgn[i] != moves[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                newMatchingLines.Add(line);
                nextMoveOptions.Add(moves[pgn.Count]);
            }
        }

        matchingLines = newMatchingLines;

        return nextMoveOptions;
    }

}