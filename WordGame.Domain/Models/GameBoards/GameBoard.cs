using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ardalis.GuardClauses;
using WordGame.Domain.Models.GameBoards.Events;
using WordGame.Domain.SeedWork;

namespace WordGame.Domain.Models.GameBoards
{
    public class GameBoard : Entity, IAggregateRoot
    {
        private readonly List<Word> _wordList;
        private char[,] _matrix;
        public GameBoard(Guid id, List<Word> wordList, int boardSize, int wordCount)
        {
            Guard.Against.Null(id, nameof(id));
            Guard.Against.NullOrEmpty(wordList, nameof(wordList));
            Guard.Against.NegativeOrZero(boardSize, nameof(boardSize));
            Guard.Against.NegativeOrZero(wordCount, nameof(wordCount));
            
            Id = id;
            _wordList = wordList;
            InitializeMatrix(boardSize);
            Randomize(wordCount);
        }
        
        public void Adjust(InputParams playerGuess)
        {
            var guess = new StringBuilder();
            var result = false;

            if (playerGuess.Direction.Equals(Direction.Row))
            {
                result = JudgeByRow(playerGuess, guess);
                if (result)
                {
                    for (int i = 0; i < guess.Length; i++)
                    {
                        SetAlphabet(playerGuess.StartRow, playerGuess.StartColumn + i, 'X');
                    }
                }
            }
            else if(playerGuess.Direction.Equals(Direction.Column))
            {
                result = JudgeByColumn(playerGuess, guess);
                if (result)
                {
                    for (int i = 0; i < guess.Length; i++)
                    {
                        SetAlphabet(playerGuess.StartRow + i, playerGuess.StartColumn, 'X');
                    }
                }
            }
            
            AddDomainEvent(new JudgedPlayerGuessEvent(guess.ToString(), result));
        }
        
        public string Show()
        {
            var result = new StringBuilder();
            
            for (var i = 0; i < _matrix.GetLength(0); i++)
            {
                for (var j = 0; j < _matrix.GetLength(0); j++)
                {
                    result.Append(_matrix[i, j]);
                }

                result.Append("\n");
            }

            return result.ToString();
        }

        private void InitializeMatrix(int size)
        {
            _matrix = new char[size, size];

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    _matrix[i, j] = 'X';
                }
            }
        }
        
        private void Randomize(int count)
        {
            var random = new Random();
            var randomWordList = _wordList.OrderBy(a => Guid.NewGuid()).Take(count).ToList();
            var number = 0;

            while (randomWordList.Count > 0)
            {
                number = number > 1000 ? 0 : ++number;

                var direction = random.Next(2);
                var randomRow = random.Next(_matrix.GetLength(0)) + 1;
                var randomColumn = random.Next(_matrix.GetLength(0)) + 1;

                if (direction == 0)
                {
                    if (_matrix.GetLength(0) - randomColumn + 1 < randomWordList[0].Name.Length)
                    {
                        continue;
                    }

                    var temp = "";
                    for (var i = 0; i < randomWordList[0].Name.Length; i++)
                    {
                        temp += GetAlphabet(randomRow, randomColumn + i);
                    }

                    if (temp.Any(t => t != 'X'))
                    {
                        continue;
                    }

                    for (var i = 0; i < randomWordList[0].Name.Length; i++)
                    {
                        SetAlphabet(randomRow, randomColumn + i, randomWordList[0].Name[i]);
                    }

                    randomWordList.RemoveAt(0);
                }
                else if (direction == 1)
                {
                    if (_matrix.GetLength(0) - randomRow + 1 < randomWordList[0].Name.Length)
                    {
                        continue;
                    }

                    var temp = "";
                    for (var i = 0; i < randomWordList[0].Name.Length; i++)
                    {
                        temp += this.GetAlphabet(randomRow + i, randomColumn);
                    }

                    if (temp.Any(t => t != 'X'))
                    {
                        continue;
                    }

                    for (var i = 0; i < randomWordList[0].Name.Length; i++)
                    {
                        this.SetAlphabet(randomRow + i, randomColumn, randomWordList[0].Name[i]);
                    }

                    randomWordList.RemoveAt(0);
                }
            }
            for (var i = 1; i <= _matrix.GetLength(0); i++)
            {
                for (var j = 1; j <= _matrix.GetLength(0); j++)
                {
                    if (GetAlphabet(i, j) == 'X')
                    {
                        var randomAlphabet = (char) random.Next(97, 122);
                        SetAlphabet(i, j, randomAlphabet);
                    }
                }
            }
        }
        private bool JudgeByColumn(InputParams playerGuess, StringBuilder guess)
        {
            for (int i = 0; i < playerGuess.WordLength; i++)
            {
                guess.Append(GetAlphabet(playerGuess.StartRow + i, playerGuess.StartColumn));
            }
            
            return Exists(guess.ToString());
        }

        private bool JudgeByRow(InputParams playerGuess, StringBuilder guess)
        {
            for (int i = 0; i < playerGuess.WordLength; i++)
            {
                guess.Append(GetAlphabet(playerGuess.StartRow, playerGuess.StartColumn + i));
            }

            return Exists(guess.ToString());
        }

        private bool Exists(string guess)
        {
            return _wordList.Exists(w => w.Name == guess.ToString());
        }
        
        private void SetAlphabet(int row, int column, char alphabet)
        {
            Guard.Against.OutOfRange(row, nameof(row), 1, _matrix.GetLength(0));
            Guard.Against.OutOfRange(column, nameof(column), 1, _matrix.GetLength(0));

            _matrix[row - 1, column - 1] = alphabet;
        }

        private char GetAlphabet(int row, int column)
        {
            Guard.Against.OutOfRange(row, nameof(row), 1, _matrix.GetLength(0));
            Guard.Against.OutOfRange(column, nameof(column), 1, _matrix.GetLength(0));

            return _matrix[row - 1, column - 1];
        }
    }
}