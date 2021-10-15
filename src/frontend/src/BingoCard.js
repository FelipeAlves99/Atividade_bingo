import { useState, useEffect } from 'react';
import { Card } from './Card';

export const BingoCard = ({ playerId }) => {
  const API_URL = 'http://localhost:5000/api';
  const [error, setError] = useState(false);
  const [bingoCardId, setBingoCardId] = useState('');
  const [bingoCard, setBingoCard] = useState('');
  const [showCard, setShowCard] = useState(false);

  const generateBingoCard = (e) => {
    e.preventDefault();
    fetch(`${API_URL}/BingoCards/${playerId}`, {
      method: 'POST',
    })
      .then((resp) => {
        return resp.json();
      })
      .then((data) => {
        setBingoCardId(data.id);
      })
      .catch((error) => {
        setError(true);
      });
  };

  useEffect(() => {
    fetch(`${API_URL}/BingoCards/${bingoCardId}`, {
      method: 'GET',
    })
      .then((resp) => {
        return resp.json();
      })
      .then((data) => {
        setBingoCard(data);
        setShowCard(true);
      })
      .catch((error) => {
        setError(true);
      });
  }, [bingoCardId]);

  return (
    <div>
      <button onClick={generateBingoCard}>Gerar cartela</button>
      {showCard ? (
        <div style={{ display: 'grid', gridTemplateColumns: '100px 100px', border: '1px solid red', width: 200 }}>
          <Card bingoCard={bingoCard} />
        </div>
      ) : (
        ''
      )}
    </div>
  );
};
