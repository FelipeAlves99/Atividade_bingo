using System.Collections.Generic;

namespace bingo_api.Entities
{
    public class GameSession : Entity
    {
        public GameSession()
        {
            Players = new List<Player>();
            Round = 1;
        }

        public List<Player> Players { get; private set; }
        public int Round { get; set; }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
    }
}