import { useState, useEffect } from 'react';
import { Game } from './Game';
import { initGame } from './service/GameSessions';

function App() {
  const [gameId, setGameId] = useState('');

  useEffect(() => {
    (async () => {
      const game = await initGame();
      setGameId(game.data.id);
    })();
  }, []);

  return <div style={{ height: '100vh', width: '100vw' }}>{<Game gameId={gameId} />}</div>;
}

export default App;
