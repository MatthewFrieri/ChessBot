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

    public GameObject PieceObjectAt(int square)
    {
        return pieceObjects[square];
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
                pieceObjectScript.Game = game;
            }

            pieceObjects[i] = pieceObjectClone;

        }
    }



    public void RecordMove(Move move)
    {

        int friendlyColor = Piece.Color(game.Board.PieceAt(move.StartSquare));

        if (PieceObjectAt(move.TargetSquare) != null)
        {
            Object.Destroy(PieceObjectAt(move.TargetSquare));
        }

        PieceObjectAt(move.StartSquare).transform.position = Helpers.SquareToLocation(move.TargetSquare);
        pieceObjects[move.TargetSquare] = PieceObjectAt(move.StartSquare);
        pieceObjects[move.StartSquare] = null;

        void HandlePromotion(int newPiece)
        {
            GameObject pieceObject = game.PieceToGameObject[newPiece | friendlyColor];
            GameObject pieceObjectClone = Object.Instantiate(pieceObject, Helpers.SquareToLocation(move.TargetSquare), Quaternion.identity);
            Object.Destroy(PieceObjectAt(move.TargetSquare));
            pieceObjects[move.TargetSquare] = pieceObjectClone;

            if (friendlyColor == game.Player.color)
            {
                PieceObject pieceObjectScript = pieceObjectClone.AddComponent<PieceObject>();
                pieceObjectScript.Game = game;
            }

        }

        switch (move.MoveFlag)
        {
            case Move.Flag.None:
                break;

            case Move.Flag.EnPassantCapture:
                int enPassantCaptureSquare = move.StartSquare < move.TargetSquare ? move.TargetSquare - 8 : move.TargetSquare + 8;
                if (PieceObjectAt(enPassantCaptureSquare) != null)
                {
                    Object.Destroy(PieceObjectAt(enPassantCaptureSquare));
                }
                break;

            case Move.Flag.Castling:
                switch (move.TargetSquare)
                {
                    case 6:  // White king side castle
                        PieceObjectAt(7).transform.position = Helpers.SquareToLocation(5);
                        pieceObjects[5] = PieceObjectAt(7);
                        pieceObjects[7] = null;
                        break;
                    case 2:  // White queen side castle
                        PieceObjectAt(0).transform.position = Helpers.SquareToLocation(3);
                        pieceObjects[3] = PieceObjectAt(0);
                        pieceObjects[0] = null;
                        break;
                    case 62:  // Black king side castle
                        PieceObjectAt(63).transform.position = Helpers.SquareToLocation(61);
                        pieceObjects[61] = PieceObjectAt(63);
                        pieceObjects[63] = null;
                        break;
                    case 58:  // Black queen side castle
                        PieceObjectAt(56).transform.position = Helpers.SquareToLocation(59);
                        pieceObjects[59] = PieceObjectAt(56);
                        pieceObjects[56] = null;
                        break;
                }
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