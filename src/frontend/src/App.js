import { useState, useEffect } from 'react';
import { Game } from './Game';

const API_URL = 'http://localhost:5000/api';
function App() {
  const [startGame, setStartGame] = useState(false);
  const [gameId, setGameId] = useState();
  const [error, setError] = useState(false);

  useEffect(() => {
    fetch(`${API_URL}/GameSessions`, {
      method: 'POST',
    })
      .then((resp) => {
        return resp.json();
      })
      .then((data) => {
        setStartGame(true);
        setGameId(data.id);
      })
      .catch((error) => {
        setError(true);
      });
  }, [startGame]);

  return (
    <div>
      {error && <div>Erro</div>}
      {gameId && <Game gameId={gameId} />}
    </div>
  );
}

export default App;
