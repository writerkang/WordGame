using System.Collections.Generic;
using WordGame.Domain.SeedWork;

namespace WordGame.Domain.Models.GameBoards
{
    public class Word : ValueObject
    {
        public string Name { get; }
        public Word(string name)
        {
            Name = name;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}