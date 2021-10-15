export const Card = ({ bingoCard }) => {
  if (bingoCard.length) return '';

  return bingoCard?.nativeNumbers.map((number) => {
    return <div style={{ border: '1px solid blue' }}>{number?.number}</div>;
  });
};
