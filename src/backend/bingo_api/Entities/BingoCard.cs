using System;
using System.Collections.Generic;
using System.Linq;

namespace bingo_api.Entities
{
    public class BingoCard : Entity
    {
        private readonly Random _random = new Random();

        public BingoCard()
        {
            NativeNumbers = new List<NativeNumber>();
            MarkedNumbers = new List<MarkedNumber>();
        }

        public BingoCard(Guid playerId)
        {
            PlayerId = playerId;
            NativeNumbers = new List<NativeNumber>();
            MarkedNumbers = new List<MarkedNumber>();
        }

        public Player Player { get; private set; }
        public Guid PlayerId { get; private set; }
        public List<NativeNumber> NativeNumbers { get; private set; }
        public List<MarkedNumber> MarkedNumbers { get; private set; }

        public void FillNativeNumbers()
        {
            for (int i = 0; i < 9; i++)
            {
                var number = _random.Next(1, 99);

                while (NativeNumbers.Select(nn => nn.Number).Contains(number))
                {
                    number = _random.Next(1, 99);
                }

                NativeNumbers.Add(new NativeNumber(number, Id));
            }
        }

        public bool HasNativeNumberToMark(int number)
        {
            if (NativeNumbers.Select(nn => nn.Number).Contains(number))
                return true;

            return false;
        }
    }
}