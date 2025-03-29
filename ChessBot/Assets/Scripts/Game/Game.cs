using System.Collections.Generic;

public class Game
{
    private Board board;
    private Player player;
    private Bot bot;
    private GameState gameState;

    public Game(int botColor)
    {
        board = new Board();
        player = new Player(Piece.OppositeColor(botColor));
        bot = new Bot(botColor);
        gameState = new GameState();
    }

    public void Start()
    {

        // While no checkmate...

        Move move = bot.FindBestMove(board, gameState);
        ExecuteMove(move);

        // Now player makes his move

    }

    private void ExecuteMove(Move move)
    {
        board.RecordMove(move);
        gameState.RecordMove(move);
    }


}
