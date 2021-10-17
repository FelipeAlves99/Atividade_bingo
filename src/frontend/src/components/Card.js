export const Card = ({ bingoCard, numerosSorteados }) => {
  return bingoCard?.nativeNumbers.map(({ number }) => {
    return (
      <div key={number} style={{ padding: 15, border: '1px solid white', backgroundColor: numerosSorteados.includes(number) ? 'red' : '' }}>
        {number}
      </div>
    );
  });
};
