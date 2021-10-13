using System.Collections.Generic;
using System.Linq;

namespace bingo_api.Entities
{
    public class BingoCard : Entity
    {
        public BingoCard()
        {
            NativeNumbers = new List<NativeNumber>();
            MarkedNumbers = new List<MarkedNumber>();
        }

        public List<NativeNumber> NativeNumbers { get; private set; }
        public List<MarkedNumber> MarkedNumbers { get; private set; }

        public bool MarkNumber(int number)
        {
            if (NativeNumbers.Select(nn => nn.Number).Contains(number))
            {
                MarkedNumbers.Add(new MarkedNumber(number, Id));
                return true;
            }

            return false;
        }
    }
}