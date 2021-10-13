using System;

namespace bingo_api.Entities
{
    public class MarkedNumber : Entity
    {
        public MarkedNumber(int number, Guid bingoCardId)
        {
            Number = number;
            BingoCardId = bingoCardId;
        }

        public int Number { get; private set; }
        public Guid BingoCardId { get; private set; }
        public BingoCard BingoCard { get; private set; }
    }
}