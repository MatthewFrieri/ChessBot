using System.Threading.Tasks;
using UnityEngine;

public class BotManager : MonoBehaviour
{

    public async void StartBotTurn()
    {

        Bot.IsThinking = true;

        Move bestMove = await Task.Run(() =>
        {
            return Bot.Think();
        });

        Game.ExecuteMove(bestMove);  // This must run on Unity's main thread

        Bot.IsThinking = false;

    }
}
