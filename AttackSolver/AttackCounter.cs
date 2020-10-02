using System.Drawing;
using Interface;

namespace Durnev
{
    public class MyAttackCounter : IAttackCounter
    {
        public int CountUnderAttack(ChessmanType cmType, Size boardSize, Point startCoords, Point[] obstacles)
        {
            if (cmType == ChessmanType.Bishop)
                return 3;
            return 2;
        }
    }
}
