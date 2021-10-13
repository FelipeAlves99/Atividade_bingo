using System;

namespace bingo_api.Entities
{
    public class NativeNumber : Entity
    {
        public NativeNumber(int number, Guid bingoCardId)
        {
            Number = number;
            BingoCardId = bingoCardId;
        }

        public int Number { get; private set; }
        public Guid BingoCardId { get; private set; }
        public BingoCard BingoCard { get; private set; }
    }
}