using System;

namespace bingo_api.Entities
{
    public class Player : Entity
    {
        public Player(string name, Guid bingoCardId)
        {
            Name = name;
            BingoCardId = bingoCardId;
        }

        public string Name { get; private set; }
        public BingoCard BingoCard { get; private set; }
        public Guid BingoCardId { get; private set; }
    }
}