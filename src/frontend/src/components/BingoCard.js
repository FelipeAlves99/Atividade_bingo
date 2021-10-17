import { useState } from 'react';
import { Card } from './Card';
import { createBingoCard, fetchBingoCard } from '../service/BingoCards';
import { drawnNumber } from '../service/GameSessions';

export const BingoCard = ({ playerId, name, gameId }) => {
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
    const numeros = await drawnNumber(gameId);
    setNumerosSorteados([numeros.data, ...numerosSorteados]);
  };

  return (
    <div style={{ display: 'flex' }}>
      <div>
        {show && <button onClick={generateBingoCard}>Gerar cartela</button>}
        {bingoCard?.nativeNumbers?.length > 0 ? (
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr 1fr 1fr', border: '5px solid #0266aa', borderRadius: '15px', width: 'fit-content', padding: 10, fontSize: '60px', backgroundColor: 'rgba(255, 255, 255 , 1)' }}>
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
      {!show && (
        <div style={{ marginLeft: 50, border: '5px solid #0266aa', borderRadius: '15px', padding: 10, backgroundColor: 'rgba(255, 255, 255 , 1)' }}>
          <span style={{ fontSize: '30px' }}>Jogador: {name}</span>
          <p style={{ fontSize: '20px' }}> Números sorteados:</p>
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr 1fr 1fr 1fr' }}>
            {numerosSorteados.map((number) => {
              return <span>{number}</span>;
            })}
          </div>
        </div>
      )}
    </div>
  );
};
