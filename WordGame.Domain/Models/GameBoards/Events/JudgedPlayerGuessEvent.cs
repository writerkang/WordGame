using MediatR;

namespace WordGame.Domain.Models.GameBoards.Events
{
    public class JudgedPlayerGuessEvent : INotification
    {
        private string PlayerGuess { get; }
        private bool Result { get; }

        public JudgedPlayerGuessEvent(string playerGuess, bool result)
        {
            PlayerGuess = playerGuess;
            Result = result;
        }
    }
}