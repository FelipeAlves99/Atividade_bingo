import { useState, useEffect } from 'react';
import { Game } from './components/Game';
import { initGame } from './service/GameSessions';

function App() {
  const [gameId, setGameId] = useState('');

  useEffect(() => {
    (async () => {
      const game = await initGame();
      setGameId(game.data.id);
    })();
  }, []);

  return (
    <div className='content' style={{ height: '105vh', width: '100vw', boxSizing: 'content-box' }}>
      <Game gameId={gameId} />
    </div>
  );
}

export default App;
