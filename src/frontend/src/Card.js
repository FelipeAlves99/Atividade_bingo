export const Card = ({ bingoCard, numerosSorteados }) => {
  return bingoCard?.nativeNumbers.map(({ number }) => {
    return (
      <div key={number} style={{ border: '1px solid blue', backgroundColor: numerosSorteados.includes(number) ? 'red' : '' }}>
        {number}
      </div>
    );
  });
};
