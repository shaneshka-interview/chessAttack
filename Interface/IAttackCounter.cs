using System.Drawing;

namespace Interface
{
    public interface IAttackCounter
    {
        /// <summary>
        /// Returns number of points under attack.
        /// </summary>
        /// <param name="boardSize"></param>
        /// <param name="startCoords"></param>
        /// <param name="obstacles"></param>
        /// <returns></returns>
        int CountUnderAttack(ChessmanType cmType, Size boardSize, Point startCoords, Point[] obstacles);
    }
}
