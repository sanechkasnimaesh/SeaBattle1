using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBattle;
using System.Linq;
using System.Collections.Generic;

namespace UTest
{
    [TestClass]
    public class EnemyBattleFieldTest
    {
        //constructor is tested in GameTest

        [TestMethod]
        public void ArrangeAutomatically_Test()
        {
            //arrange
            Game _Game = new Game();
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            //act
            _EBF.ArrangeAutomatically();

            //assert
            Assert.IsNotNull(_EBF.Boats);
            Assert.AreEqual(_EBF.Boats.Count, 10);

            int _TooLongCount = _EBF.Boats.Count(a => a.Cells.Count > 4);
            Assert.AreEqual(_TooLongCount, 0);

            int _BattlecruisersCount = _EBF.Boats.Count(a => a.Cells.Count == 4);
            Assert.AreEqual(_BattlecruisersCount, 1);

            int _CruisersCount = _EBF.Boats.Count(a => a.Cells.Count == 3);
            Assert.AreEqual(_CruisersCount, 2);

            int _DestroyersCount = _EBF.Boats.Count(a => a.Cells.Count == 2);
            Assert.AreEqual(_DestroyersCount, 3);

            int _TorpedoboatsCount = _EBF.Boats.Count(a => a.Cells.Count == 1);
            Assert.AreEqual(_TorpedoboatsCount, 4);

            //boats bend or touch
            if (!_EBF.Boats.All(b => (b.Cells.Select(c => c.x).Distinct().Count() == 1)
                                || (b.Cells.Select(c => c.y).Distinct().Count() == 1)
                            )
                )
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RemoveFog_Test()
        {
            //arrange
            Game _Game = new Game();
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            //act
            _EBF.RemoveFog();

            int _InvisibleCellsQuantity =
                _EBF.Cells.Cast<Cell>().ToList().
                Count(c => (c.IsInvisible == true));

            Assert.AreEqual(_InvisibleCellsQuantity, 0);
        }
    }
}
