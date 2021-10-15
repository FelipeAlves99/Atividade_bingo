using System;

namespace bingo_api.Request
{
    public class PlayerRequest
    {
        public string Name { get; set; }
        public Guid GameSessionId { get; set; }
    }
}