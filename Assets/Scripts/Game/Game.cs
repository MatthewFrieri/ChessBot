using System;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private Dictionary<int, GameObject> pieceToGameObject;

    private Board board;
    private ObjectBoard objectBoard;
    private GameState gameState;
    private Player player;
    private Bot bot;

    public static string StartingFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public Game(int botColor, Dictionary<int, GameObject> pieceToGameObject)
    {
        this.pieceToGameObject = pieceToGameObject;

        board = new Board();
        gameState = new GameState();
        player = new Player(this, Piece.OppositeColor(botColor));
        bot = new Bot(this, botColor);
        objectBoard = new ObjectBoard(this);


        if (botColor == Piece.White)
        {
            bot.MakeMove();
        }
        else
        {
            player.CurrentLegalMoves = LegalMoves.GetLegalMoves(board, gameState);
        }
    }

    public Board Board
    {
        get
        {
            return board;
        }
    }
    public ObjectBoard ObjectBoard
    {
        get
        {
            return objectBoard;
        }
    }
    public GameState GameState
    {
        get
        {
            return gameState;
        }
    }
    public Player Player
    {
        get
        {
            return player;
        }
    }
    public Dictionary<int, GameObject> PieceToGameObject
    {
        get
        {
            return pieceToGameObject;
        }
    }

    public void Start()
    {

        // While no checkmate...

        // Move move = bot.FindBestMove(board, gameState);
        // ExecuteMove(move);

        // Now player makes his move

    }

    public void ExecuteMove(Move move)
    {

        objectBoard.RecordMove(move);
        board.RecordMove(move);
        gameState.RecordMove(move);

        if (gameState.ColorToMove == player.color)
        {
            // Now its player's turn
            player.CurrentLegalMoves = LegalMoves.GetLegalMoves(board, gameState);
        }
        else
        {
            // Now its bot's turn
            bot.MakeMove();
        }
    }


}
