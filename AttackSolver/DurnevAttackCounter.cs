using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interface;

namespace Durnev
{
    public class DurnevAttackCounter : IAttackCounter
    {
        private readonly Dictionary<ChessmanType, Func<Size, Point, Point[], int>> _dic =
            new Dictionary<ChessmanType, Func<Size, Point, Point[], int>>
            {
                {
                    ChessmanType.Rook,
                    CountUnderAttackRook
                },
                {
                    ChessmanType.Bishop,
                    CountUnderAttackBishop
                },
                {
                    ChessmanType.Knight,
                    CountUnderAttackKnight
                }
            };

        public int CountUnderAttack(ChessmanType cmType, Size boardSize, Point startCoords, Point[] obstacles)
        {
            if (boardSize.Height < 0 || boardSize.Width < 0 || startCoords.X < 1 || startCoords.X > boardSize.Width ||
                startCoords.Y < 1 || startCoords.Y > boardSize.Height)
                throw new ArgumentException();

            if (!_dic.ContainsKey(cmType) || _dic[cmType] == null)
                throw new NotImplementedException();

            return _dic[cmType].Invoke(boardSize, startCoords, obstacles);
        }

        private static int CountUnderAttackBishop(Size boardSize, Point startCoords, Point[] obstacles)
        {
            var res = 0;
            var hash = GetHash(obstacles);
            var point = startCoords;
            while (point.X > 1 && point.Y > 1)
            {
                point.X--;
                point.Y--;

                if (hash.Contains(point))
                    break;
                res++;
            }

            point = startCoords;
            while (point.X > 1 && point.Y < boardSize.Height)
            {
                point.X--;
                point.Y++;

                if (hash.Contains(point))
                    break;
                res++;
            }

            point = startCoords;
            while (point.X < boardSize.Width && point.Y < boardSize.Height)
            {
                point.X++;
                point.Y++;

                if (hash.Contains(point))
                    break;
                res++;
            }

            point = startCoords;
            while (point.X < boardSize.Width && point.Y > 1)
            {
                point.X++;
                point.Y--;

                if (hash.Contains(point))
                    break;
                res++;
            }

            return res;
        }

        private static int CountUnderAttackRook(Size boardSize, Point startCoords, Point[] obstacles)
        {
            var res = 0;

            var hash = GetHash(obstacles);
            var point = startCoords;
            while (point.X > 1)
            {
                point.X--;

                if (hash.Contains(point))
                    break;
                res++;
            }

            point = startCoords;
            while (point.X < boardSize.Width)
            {
                point.X++;

                if (hash.Contains(point))
                    break;

                res++;
            }

            point = startCoords;
            while (point.Y > 1)
            {
                point.Y--;

                if (hash.Contains(point))
                    break;
                res++;
            }

            point = startCoords;
            while (point.Y < boardSize.Height)
            {
                point.Y++;

                if (hash.Contains(point))
                    break;
                res++;
            }

            return res;
        }

        private static int CountUnderAttackKnight(Size boardSize, Point startCoords, Point[] obstacles)
        {
            var res = 0;
            var hash = GetHash(obstacles);
            foreach (var v in new[] {(2, 1), (-2, 1), (2, -1), (-2, -1), (1, 2), (-1, -2), (1, -2), (-1, 2)})
            {
                var point = startCoords;
                point.X += v.Item1;
                point.Y += v.Item2;

                if (1 <= point.X && point.X <= boardSize.Width && 1 <= point.Y && point.Y <= boardSize.Height &&
                    !hash.Contains(point))
                {
                    res++;
                }
            }

            return res;
        }

        private static HashSet<Point> GetHash(Point[] obstacles)
        {
            return new HashSet<Point>(obstacles?.Any() == true ? obstacles : Array.Empty<Point>());
        }
    }
}