using System.Collections.Generic;
using WordGame.Domain.SeedWork;

namespace WordGame.Domain.Models.GameBoards
{
    public class InputParams : ValueObject
    {
        public int StartRow { get; }
        public int StartColumn { get; }
        public int WordLength { get; }
        public Direction Direction { get; }

        public InputParams(int startRow, int startColumn, int wordLength, Direction direction)
        {
            StartRow = startRow;
            StartColumn = startColumn;
            WordLength = wordLength;
            Direction = direction;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartRow;
            yield return StartColumn;
            yield return WordLength;
            yield return Direction;
        }
    }
}