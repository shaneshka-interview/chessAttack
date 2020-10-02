using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Durnev;
using Interface;
using Xunit;
using Xunit.Abstractions;

namespace AttackSolverTest
{
    public class DurnevTest
    {
        private readonly ITestOutputHelper output;

        public DurnevTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void NotRightTest()
        {
            var inst = new DurnevAttackCounter();
            // Rook - ladja
            var res = inst.CountUnderAttack(ChessmanType.Rook, new Size(3, 2), new Point(1, 1),
                new[] {new Point(2, 2), new Point(1, 3)}); //new Point(1, 3) - не на достке, по этому ответ будет 3
            Assert.Equal(2, res);

            // Bishop - slon
            res = inst.CountUnderAttack(ChessmanType.Bishop, new Size(4, 5), new Point(2, 2),
                new[] {new Point(3, 3), new Point(1, 3),}); // new Point(3, 3) не из примера, ответ будет 2
            Assert.Equal(3, res);
        }

        [Fact]
        public void Simple()
        {
            var inst = new DurnevAttackCounter();
            output.WriteLine("Testing " + inst.GetType().FullName);
            // Rook - ladja
            var res = inst.CountUnderAttack(ChessmanType.Rook, new Size(3, 2), new Point(1, 1),
                new[] {new Point(2, 2), new Point(3, 1)});
            Assert.Equal(2, res);

            // Bishop - slon
            res = inst.CountUnderAttack(ChessmanType.Bishop, new Size(4, 5), new Point(2, 2),
                new[] {new Point(3, 3), new Point(1, 3)});
            Assert.Equal(2, res);
        }

        [Fact]
        public void AccessTypes()
        {
            var accessTypes = new[] {ChessmanType.Bishop, ChessmanType.Knight, ChessmanType.Rook};
            foreach (var type in Enum.GetValues(typeof(ChessmanType)))
            {
                Assert.Contains((ChessmanType) type, accessTypes);
            }
        }

        [Fact]
        public void Throw_IfStartPointWithoutBoard()
        {
            var inst = new DurnevAttackCounter();
            var size = new Size(1, 1);
            var point = new Point(0, 0);
            foreach (var type in Enum.GetValues(typeof(ChessmanType)))
            {
                Assert.Throws<ArgumentException>(() => inst.CountUnderAttack((ChessmanType) type, size, point, null));
            }
        }

        [Fact]
        public void Throw_IfBadSizeBoard()
        {
            var inst = new DurnevAttackCounter();
            var size = new Size(0, 0);
            var point = new Point(1, 1);
            foreach (var type in Enum.GetValues(typeof(ChessmanType)))
            {
                Assert.Throws<ArgumentException>(() => inst.CountUnderAttack((ChessmanType) type, size, point, null));
            }
        }

        [Theory]
        [MemberData("AttackWithObtaclesCase")]
        [MemberData("AttackWithoutObtaclesCase")]
        [MemberData("AttackWithFullObtaclesCase")]
        [MemberData("AttackExtraBoardCase")]
        public void Should_Attack(ChessmanType cmType, Size boardSize, Point startCoords, Point[] obstacles,
            int result)
        {
            var inst = new DurnevAttackCounter();
            var res = inst.CountUnderAttack(cmType, boardSize, startCoords, obstacles);
            Assert.Equal(result, res);
        }

        public static IEnumerable<object[]> AttackWithoutObtaclesCase()
        {
            //5*5
            var size = new Size(5, 5);
            var point = new Point(1, 1);
            var pointCentr = new Point(3, 3);
            yield return new object[] {ChessmanType.Rook, size, point, null, 8};
            yield return new object[] {ChessmanType.Rook, size, pointCentr, null, 8};
            yield return new object[] {ChessmanType.Bishop, size, point, null, 4};
            yield return new object[] {ChessmanType.Bishop, size, pointCentr, null, 8};
            yield return new object[] {ChessmanType.Knight, size, point, null, 2};
            yield return new object[] {ChessmanType.Knight, size, pointCentr, null, 8};
            //3*5
            size = new Size(3, 5);
            point = new Point(1, 1);
            pointCentr = new Point(2, 3);
            yield return new object[] {ChessmanType.Rook, size, point, null, 6};
            yield return new object[] {ChessmanType.Rook, size, pointCentr, null, 6};
            yield return new object[] {ChessmanType.Bishop, size, point, null, 2};
            yield return new object[] {ChessmanType.Bishop, size, pointCentr, null, 4};
            yield return new object[] {ChessmanType.Knight, size, point, null, 2};
            yield return new object[] {ChessmanType.Knight, size, pointCentr, null, 4};
            //5*3
            size = new Size(5, 3);
            point = new Point(1, 1);
            pointCentr = new Point(3, 2);
            yield return new object[] {ChessmanType.Rook, size, point, null, 6};
            yield return new object[] {ChessmanType.Rook, size, pointCentr, null, 6};
            yield return new object[] {ChessmanType.Bishop, size, point, null, 2};
            yield return new object[] {ChessmanType.Bishop, size, pointCentr, null, 4};
            yield return new object[] {ChessmanType.Knight, size, point, null, 2};
            yield return new object[] {ChessmanType.Knight, size, pointCentr, null, 4};
        }

        public static IEnumerable<object[]> AttackWithObtaclesCase()
        {
            var size = new Size(5, 5);
            var point = new Point(1, 1);
            var obtacles = Enumerable.Range(1, 5).Select(x => new Point(x, 3)).ToArray();
            yield return new object[] {ChessmanType.Rook, size, point, obtacles, 5};
            yield return new object[] {ChessmanType.Bishop, size, point, obtacles, 1};
            yield return new object[] {ChessmanType.Knight, size, point, obtacles, 1};

            obtacles = Enumerable.Range(1, 5).Select(x => new Point(x, 2)).ToArray();
            yield return new object[] {ChessmanType.Rook, size, point, obtacles, 4};
            yield return new object[] {ChessmanType.Bishop, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, point, obtacles, 1};

            obtacles = new[] {new Point(1, 2), new Point(2, 2), new Point(2, 1)};
            yield return new object[] {ChessmanType.Rook, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Bishop, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, point, obtacles, 2};
        }

        public static IEnumerable<object[]> AttackWithFullObtaclesCase()
        {
            //5*5
            var size = new Size(5, 5);
            var point = new Point(1, 1);
            var pointCentr = new Point(3, 3);
            var obtacles = Enumerable.Range(1, size.Width)
                .Select(x => Enumerable.Range(1, size.Height).Select(y => new Point(x, y))).SelectMany(x => x)
                .ToArray();
            yield return new object[] {ChessmanType.Rook, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Rook, size, pointCentr, obtacles, 0};
            yield return new object[] {ChessmanType.Bishop, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Bishop, size, pointCentr, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, pointCentr, obtacles, 0};
            //3*5
            size = new Size(3, 5);
            point = new Point(1, 1);
            pointCentr = new Point(3, 2);
            yield return new object[] {ChessmanType.Rook, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Rook, size, pointCentr, obtacles, 0};
            yield return new object[] {ChessmanType.Bishop, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Bishop, size, pointCentr, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, pointCentr, obtacles, 0};
            //5*3
            size = new Size(5, 3);
            point = new Point(1, 1);
            pointCentr = new Point(2, 3);
            yield return new object[] {ChessmanType.Rook, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Rook, size, pointCentr, obtacles, 0};
            yield return new object[] {ChessmanType.Bishop, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Bishop, size, pointCentr, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, point, obtacles, 0};
            yield return new object[] {ChessmanType.Knight, size, pointCentr, obtacles, 0};
        }

        public static IEnumerable<object[]> AttackExtraBoardCase()
        {
            var size = new Size(1, 1);
            var point = new Point(1, 1);
            foreach (var type in Enum.GetValues(typeof(ChessmanType)))
            {
                yield return new object[] {type, size, point, null, 0};
            }
        }
    }
}