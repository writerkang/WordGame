using System;
using System.Collections.Generic;
using MediatR;
using WordGame.Domain.Models.GameBoards;
using WordGame.Domain.Models.GameBoards.Events;
using Xunit;

namespace WordGame.Test
{
    public class GameBoardTest
    {
        [Fact]
        public void CannotGeneratedWithEmptyWordList()
        {
            var wordList = new List<Word>();

            Assert.Throws<ArgumentException>(() => 
                new GameBoard(
                new Guid(),
                wordList,
                15,
                10));
        }

        [Fact]
        public void EqualsTest()
        {
            var wordList = new List<Word>(){new Word("hello")};
            var id = Guid.NewGuid();
            var board1 = new GameBoard(
                id,
                wordList,
                15,
                10);
            
            var board2 = new GameBoard(
                id,
                wordList,
                10,
                5);
            
            Assert.Equal(board1, board2);
        }
        
        [Fact]
        public void NotEqualsTest()
        {
            var wordList = new List<Word>(){new Word("hello")};
            var board1 = new GameBoard(
                Guid.NewGuid(),
                wordList,
                15,
                10);
            
            var board2 = new GameBoard(
                Guid.NewGuid(),
                wordList,
                15,
                10);
            
            Assert.NotEqual(board1, board2);
        }
    }
}