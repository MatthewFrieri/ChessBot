using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{

    public TextMeshProUGUI playerColor;
    public TextMeshProUGUI botColor;
    public TextMeshProUGUI depth;
    public TextMeshProUGUI tableSize;
    public TextMeshProUGUI bestMove;
    public TextMeshProUGUI evaluation;

    public TextMeshProUGUI winner;

    public TextMeshProUGUI whiteClock;
    public TextMeshProUGUI blackClock;




    void Update()
    {

        playerColor.text = "Player: " + (Player.Color == Piece.White ? "White" : "Black");
        botColor.text = "Bot: " + (Bot.Color == Piece.White ? "White" : "Black");
        tableSize.text = "TT Size: " + TranspositionTable.Size();
        depth.text = "Depth: " + Search.Depth;
        bestMove.text = "Best Move: " + Search.BestMoveAlgebraic;
        evaluation.text = "Evaluation: " + (float)(Bot.Color == Piece.White ? 1 : -1) * Search.BestEval / 100;

        if (Game.IsGameOver)
        {
            if (Game.IsCheckmate)
            {
                winner.text = $"CHECKMATE {(GameState.ColorToMove == Piece.White ? "Black" : "White")} Wins!";
            }
            else
            {
                winner.text = "DRAW";
            }
        }
        else
        {
            winner.text = "";
        }

        whiteClock.text = Helpers.FormatTime(GameState.WhiteTime);
        blackClock.text = Helpers.FormatTime(GameState.BlackTime);
    }
}
