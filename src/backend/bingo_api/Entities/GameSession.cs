using System;
using System.Collections.Generic;

namespace bingo_api.Entities
{
    public class GameSession : Entity
    {
        public GameSession()
        {
            Players = new List<Player>();
            Round = 0;
            GameStatus = EGameStatus.NotStarted;
        }

        public Guid WinnerPlayerId { get; private set; }
        public EGameStatus GameStatus { get; private set; }
        public List<Player> Players { get; private set; }
        public int Round { get; set; }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void UpdateRound()
        {
            Round++;
        }

        public void UpdateStatus(EGameStatus gameStatus)
        {
            GameStatus = gameStatus;
        }

        public void SetWinner(Guid playerId)
        {
            WinnerPlayerId = playerId;
        }
    }

    public enum EGameStatus
    {
        NotStarted = 1,
        Started = 2,
        Finished = 3
    }
}