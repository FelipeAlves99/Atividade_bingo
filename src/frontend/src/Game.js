import { useState } from 'react';
import { BingoCard } from './BingoCard';

export const Game = ({ gameId }) => {
  const API_URL = 'http://localhost:5000/api';

  const [playerId, setPlayerId] = useState('');
  const [playerName, setPlayerName] = useState('');

  const registerPlayer = (e) => {
    e.preventDefault();
    fetch(`${API_URL}/Players`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        gameSessionId: gameId,
        name: playerName,
      }),
    })
      .then((resp) => {
        return resp.json();
      })
      .then((data) => {
        setPlayerId(data.id);
        console.log('Player', data);
      })
      .catch((error) => {});
  };

  return (
    <div>
      <input type='text' placeholder='digite seu nome' onChange={(e) => setPlayerName(e.target.value)} />
      <button onClick={registerPlayer}>Send</button>
      gameID {gameId}
      <BingoCard playerId={playerId} />
    </div>
  );
};
