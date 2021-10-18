using System;
using System.Collections.Generic;

namespace bingo_api
{
    public static class StaticHelpers
    {
        public static List<Tuple<Guid, int>> oldDrawnNumbers = new List<Tuple<Guid, int>>();
    }
}