using System.Collections.Generic;
using UnityEngine;

public class ObjectBoard
{
    private Game game;
    private GameObject[] pieceObjects;

    public ObjectBoard(Game game)
    {
        this.game = game;
        pieceObjects = new GameObject[64];
        InstantiatePieceObjects();
    }

    private void InstantiatePieceObjects()
    {
        for (int i = 0; i < 64; i += 1)
        {
            int piece = game.Board.PieceAt(i);

            if (piece == Piece.None) continue;

            GameObject pieceObject = game.PieceToGameObject[piece];

            GameObject pieceObjectClone = Object.Instantiate(pieceObject, Helpers.SquareToLocation(i), Quaternion.identity);

            if (Piece.Color(piece) == game.Player.color)
            {
                PieceObject pieceObjectScript = pieceObjectClone.AddComponent<PieceObject>();
                pieceObjectScript.Player = game.Player;
            }

            pieceObjects[i] = pieceObjectClone;

        }
    }



    public void RecordMove(Move move)
    {
        Debug.Log(move);


        int friendlyColor = Piece.Color(game.Board.PieceAt(move.StartSquare));

        if (pieceObjects[move.TargetSquare] != null)
        {
            Object.Destroy(pieceObjects[move.TargetSquare]);
        }

        pieceObjects[move.StartSquare].transform.position = Helpers.SquareToLocation(move.TargetSquare);
        pieceObjects[move.TargetSquare] = pieceObjects[move.StartSquare];
        pieceObjects[move.StartSquare] = null;

        void HandlePromotion(int newPiece)
        {
            GameObject pieceObject = game.PieceToGameObject[newPiece | friendlyColor];
            GameObject pieceObjectClone = Object.Instantiate(pieceObject, Helpers.SquareToLocation(move.TargetSquare), Quaternion.identity);
            PieceObject pieceObjectScript = pieceObjectClone.AddComponent<PieceObject>();
            pieceObjectScript.Player = game.Player;
            Object.Destroy(pieceObjects[move.TargetSquare]);
            pieceObjects[move.TargetSquare] = pieceObjectClone;
        }


        switch (move.MoveFlag)
        {
            case Move.Flag.None:
                break;

            case Move.Flag.EnPassantCapture:
                int enPassantCaptureSquare = move.StartSquare < move.TargetSquare ? move.TargetSquare - 8 : move.TargetSquare + 8;
                if (pieceObjects[enPassantCaptureSquare] != null)
                {
                    Object.Destroy(pieceObjects[enPassantCaptureSquare]);
                }
                break;

            case Move.Flag.Castling:
                break;

            case Move.Flag.PromoteToQueen:
                HandlePromotion(Piece.Queen);
                break;

            case Move.Flag.PromoteToKnight:
                HandlePromotion(Piece.Knight);
                break;

            case Move.Flag.PromoteToRook:
                HandlePromotion(Piece.Rook);
                break;

            case Move.Flag.PromoteToBishop:
                HandlePromotion(Piece.Bishop);
                break;

            case Move.Flag.PawnTwoForward:
                break;
        }
    }


}