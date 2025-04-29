using System.Threading.Tasks;
using UnityEngine;

public class BotManager : MonoBehaviour
{

    public async void StartBotTurn()
    {

        Bot.IsThinking = true;

        Move bestMove = await Task.Run(async () =>
        {
            Move move = Bot.Think();
            if (Bot.CanUseBook)
            {
                await Task.Delay(500);
            }
            return move;
        });

        Game.ExecuteMove(bestMove);

        Bot.IsThinking = false;

    }
}
