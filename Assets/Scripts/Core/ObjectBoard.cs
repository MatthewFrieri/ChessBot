using UnityEngine;

static class ObjectBoard
{
    private static GameObject[] pieceObjects;

    public static void Init()
    {
        pieceObjects = new GameObject[64];
        InstantiatePieceObjects();
    }

    private static GameObject PieceObjectAt(int square)
    {
        return pieceObjects[square];
    }

    private static void InstantiatePieceObjects()
    {
        for (int i = 0; i < 64; i++)
        {
            int piece = Board.PieceAt(i);

            if (piece == Piece.None) continue;

            GameObject pieceObject = Game.PieceToGameObject[piece];

            GameObject pieceObjectClone = Object.Instantiate(pieceObject, Helpers.SquareToLocation(i), Quaternion.identity);

            // Only give life to the player's pieces
            if (Piece.Color(piece) == Player.Color)
            {
                pieceObjectClone.AddComponent<PieceObject>();
            }

            pieceObjects[i] = pieceObjectClone;

        }
    }

    // ObjectBoard.RecordMove() must happen before Board.RecordMove() and GameState.RecordMove()
    public static void RecordMove(Move move)
    {

        int friendlyColor = GameState.ColorToMove;

        if (PieceObjectAt(move.TargetSquare) != null)
        {
            Object.Destroy(PieceObjectAt(move.TargetSquare));
        }

        PieceObjectAt(move.StartSquare).transform.position = Helpers.SquareToLocation(move.TargetSquare);
        pieceObjects[move.TargetSquare] = PieceObjectAt(move.StartSquare);
        pieceObjects[move.StartSquare] = null;

        void HandlePromotion(int newPiece)
        {
            GameObject pieceObject = Game.PieceToGameObject[newPiece | friendlyColor];
            GameObject pieceObjectClone = Object.Instantiate(pieceObject, Helpers.SquareToLocation(move.TargetSquare), Quaternion.identity);
            Object.Destroy(PieceObjectAt(move.TargetSquare));
            pieceObjects[move.TargetSquare] = pieceObjectClone;

            // Only give life to the player's pieces
            if (friendlyColor == Player.Color)
            {
                pieceObjectClone.AddComponent<PieceObject>();
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