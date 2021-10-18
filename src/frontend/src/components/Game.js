import { useState } from 'react';
import { BingoCard } from './BingoCard';
import { player } from '../service/Players';

export const Game = ({ gameId }) => {
  const [playerId, setPlayerId] = useState('');
  const [playerName, setPlayerName] = useState('');
  const [show, setShow] = useState(true);
  const [name, setName] = useState('');

  const registerPlayer = (e) => {
    (async () => {
      const body = {
        gameSessionId: gameId,
        name: playerName,
      };
      const newPlayer = await player(JSON.stringify(body));
      setPlayerId(newPlayer.data.id);
      setName(playerName);
      setShow(false);
    })();
  };

  return (
    <div style={{ display: 'flex', justifyContent: 'center', textAlign: 'center', height: '100%', alignItems: 'center' }}>
      {show && (
        <div>
          <input type='text' placeholder='digite seu nome' onChange={(e) => setPlayerName(e.target.value)} />
          <br />
          <button onClick={registerPlayer}>Jogar!</button>
        </div>
      )}

      {!show && <BingoCard name={name} playerId={playerId} gameId={gameId} />}
    </div>
  );
};
