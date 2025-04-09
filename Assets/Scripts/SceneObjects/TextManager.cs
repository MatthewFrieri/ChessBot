using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{

    public TextMeshProUGUI playerColor;
    public TextMeshProUGUI botColor;
    public TextMeshProUGUI depth;
    public TextMeshProUGUI bestMove;
    public TextMeshProUGUI evaluation;

    void Update()
    {

        playerColor.text = "Player: " + (Player.Color == Piece.White ? "White" : "Black");
        botColor.text = "Bot: " + (Bot.Color == Piece.White ? "White" : "Black");

        depth.text = "Depth: " + Bot.Depth;
        bestMove.text = "Best Move: " + Bot.MoveToPlayAlgebraic;
        evaluation.text = "Evaluation: " + (double)(Bot.Color == Piece.White ? 1 : -1) * Bot.MoveToPlayEval / 100;

    }
}
