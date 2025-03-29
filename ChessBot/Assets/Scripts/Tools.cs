// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// // using Chess;

// public class Tools : MonoBehaviour
// {

//     [MenuItem("MY TOOLS/Evaluate Position")]
//     public static void RunEvaluation()
//     {
//         int evaluation = Evaluate.EvaluatePosition(Board.Squares);
//         Debug.Log("Evaluation: " + evaluation);
//     }

//     [MenuItem("MY TOOLS/Get Legal Moves")]
//     public static void RunGetLegalMoves()
//     {
//         List<Move> moves = LegalMoves.GetLegalMoves(Board.Squares, Piece.White);
//         Debug.Log(moves.Count + " legal moves found");
//         foreach (Move move in moves)
//         {
//             Debug.Log(move);
//         }
//         Board.tempLegalMoves = moves;
//     }
// }
