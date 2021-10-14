using bingo_api.Entities;
using System.Collections.Generic;

namespace bingo_api.Request
{
    public class GameSessionRequest
    {
        public List<string> PlayersIds { get; set; }
        public GameSession GameSession { get; set; }
    }
}