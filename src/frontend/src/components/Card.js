export const Card = ({ bingoCard, numerosSorteados }) => {
  return bingoCard?.nativeNumbers.map(({ number }) => {
    return (
      <div className={numerosSorteados.includes(number) ? 'selected-number' : ''} key={number} style={{ padding: 15, border: '1px solid #d197cf' }}>
        {number}
      </div>
    );
  });
};
