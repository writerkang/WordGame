using WordGame.Domain.SeedWork;

namespace WordGame.Domain.Models.GameBoards
{
    public class Direction : Enumeration
    {
        public static Direction Row = new Direction(1, nameof(Row));
        public static Direction Column = new Direction(1, nameof(Column));
        
        public Direction(int id, string name) : base(id, name)
        {
        }
    }
}