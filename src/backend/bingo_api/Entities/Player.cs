using System;

namespace bingo_api.Entities
{
    public class Player : Entity
    {
        public Player(string name, Guid gameSessionId)
        {
            Name = name;
            GameSessionId = gameSessionId;
        }

        public string Name { get; private set; }
        public BingoCard BingoCard { get; private set; }
        public Guid GameSessionId { get; private set; }
        public GameSession GameSession { get; private set; }
    }
}