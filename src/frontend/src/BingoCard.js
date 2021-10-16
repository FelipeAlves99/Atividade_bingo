import { useState } from 'react';
import { Card } from './Card';
import { createBingoCard, fetchBingoCard } from './service/BingoCards';
import { drawnNumber } from './service/GameSessions';

export const BingoCard = ({ playerId, name }) => {
  const [bingoCard, setBingoCard] = useState('');
  const [numerosSorteados, setNumerosSorteados] = useState([]);
  const [show, setShow] = useState(true);

  const generateBingoCard = async (e) => {
    const bingoCardId = await createBingoCard(playerId);
    const bingoCard = await fetchBingoCard(bingoCardId.data.id);
    setBingoCard(bingoCard.data);
    setShow(false);
  };

  const sortearNumero = async (e) => {
    const numeros = await drawnNumber();
    setNumerosSorteados([numeros.data, ...numerosSorteados]);
  };

  return (
    <div>
      {name}
      {show && <button onClick={generateBingoCard}>Gerar cartela</button>}
      {bingoCard?.nativeNumbers?.length > 0 ? (
        <div style={{ display: 'grid', gridTemplateColumns: '50px 50px 50px 50px 50px', border: '1px solid red', width: 'fit-content' }}>
          <span>B</span>
          <span>I</span>
          <span>N</span>
          <span>G</span>
          <span>O</span>
          <Card bingoCard={bingoCard} numerosSorteados={numerosSorteados} />
        </div>
      ) : (
        ''
      )}
      {!show && <button onClick={sortearNumero}>Sortear Numero</button>}
    </div>
  );
};
