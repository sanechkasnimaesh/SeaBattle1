using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBattle;
using System.Collections.Generic;
using System.Linq;

namespace UTest
{
    [TestClass]
    public class UserBattleFieldTest
    {
        #region
        List<Tuple<int,int>> _CellsCoordinates_CorrectlyFilled = new List<Tuple<int,int>>()
            {
                Tuple.Create(0,1),

                Tuple.Create(0,7),

                Tuple.Create(2,4),

                Tuple.Create(4,6),

                 Tuple.Create(0,7),

                 Tuple.Create(5,2),
                 Tuple.Create(5,3),

                 Tuple.Create(7,0),
                 Tuple.Create(7,1),

                 Tuple.Create(6,7),
                 Tuple.Create(7,7),

                 Tuple.Create(7,4),
                 Tuple.Create(8,4),
                 Tuple.Create(9,4),

                 Tuple.Create(9,7),
                 Tuple.Create(9,8),
                 Tuple.Create(9,9),

                 Tuple.Create(2,9),
                 Tuple.Create(3,9),
                 Tuple.Create(4,9),
                 Tuple.Create(5,9)
            };

        List<Tuple<int,int>>  _CellsCoordinates_BoatsTouch = new List<Tuple<int,int>> ()
            {
                Tuple.Create( 0,  1),

                Tuple.Create(  0,   7),

                Tuple.Create(  2,   4),

                Tuple.Create(  4,   6),

                Tuple.Create(  0,   7),

                Tuple.Create(  6,   1),
                Tuple.Create(  6,   2),

                Tuple.Create(  7,   0),
                Tuple.Create(  7,   1),

                Tuple.Create(  6,   7),
                Tuple.Create(  7,   7),

                Tuple.Create(  7,   4),
                Tuple.Create(  8,   4),
                Tuple.Create(  9,   4),

                Tuple.Create(  9,   7),
                Tuple.Create(  9,   8),
                Tuple.Create(  9,   9),

                Tuple.Create(  2,   9),
                Tuple.Create(  3,   9),
                Tuple.Create(  4,   9),
                Tuple.Create(  5,   9)
            };

        List<Tuple<int,int>> _CellsCoordinates_WrongQuantity = new List<Tuple<int,int>>()
            {
                 Tuple.Create(  0,   1),

                Tuple.Create(  0,   7),

                Tuple.Create(  2,   4),

                Tuple.Create(  4,   6),

                Tuple.Create(  0,   7),

                Tuple.Create(  5,   2),
                Tuple.Create(  5,   3),

                Tuple.Create(  7,   0),
                Tuple.Create(  7,   1),

                Tuple.Create(  7,   4),
                Tuple.Create(  8,   4),
                Tuple.Create(  9,   4),

                Tuple.Create(  9,   7),
                Tuple.Create(  9,   8),
                Tuple.Create(  9,   9),

                Tuple.Create(  2,   9),
                Tuple.Create(  3,   9),
                Tuple.Create(  4,   9),
                Tuple.Create(  5,   9)
            };

        List<Tuple<int,int>> _CellsCoordinates_WrongBoatsSet = new List<Tuple<int,int>>()
            {
                Tuple.Create(  0,   1),

                Tuple.Create(  0,   7),

                Tuple.Create(  2,   4),

                Tuple.Create(  4,   6),

                Tuple.Create(  0,   7),

                Tuple.Create(  5,   2),
                Tuple.Create(  5,   3),
                Tuple.Create(  5,   4),

                Tuple.Create(  7,   0),
                Tuple.Create(  7,   1),

                Tuple.Create(  6,   7),
                Tuple.Create(  7,   7),

                Tuple.Create(  7,   4),
                Tuple.Create(  8,   4),
                Tuple.Create(  9,   4),

                Tuple.Create(  9,   7),
                Tuple.Create(  9,   8),
                Tuple.Create(  9,   9),

                Tuple.Create(  2,   9),
                Tuple.Create(  3,   9),
                Tuple.Create(  4,   9),
                Tuple.Create(  5,   9)
            };

        List<Tuple<int,int>> _CellsCoordinates_TooLongBoat = new List<Tuple<int,int>>()
            {
                Tuple.Create(  0,   1),

                Tuple.Create(  0,   7),

                Tuple.Create(  2,   4),

                Tuple.Create(  4,   6),

                Tuple.Create(  0,   7),

                Tuple.Create(  5,   2),
                Tuple.Create(  5,   3),

                Tuple.Create(  7,   0),
                Tuple.Create(  7,   1),

                Tuple.Create(  6,   7),
                Tuple.Create(  7,   7),

                Tuple.Create(  7,   4),
                Tuple.Create(  8,   4),
                Tuple.Create(  9,   4),

                Tuple.Create(  9,   7),
                Tuple.Create(  9,   8),
                Tuple.Create(  9,   9),

                Tuple.Create(  2,   9),
                Tuple.Create(  3,   9),
                Tuple.Create(  4,   9),
                Tuple.Create(  5,   9),
                Tuple.Create(  6,   9)
            };
        #endregion;

        //constructor is tested in GameTest

        [TestMethod]
        public void ParseArrangement_CorrectlyFilled_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_CorrectlyFilled)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }

            //act
            _UBF.ParseArrangement();            

            //assert
            Assert.IsNotNull(_UBF.Boats);
            Assert.AreEqual(_UBF.Boats.Count, 10);

            int _BattleshipCount
                 = _UBF.Boats.Count(b => b.Type == BoatType.Battleship);
            Assert.AreEqual(_BattleshipCount, 1);

            int _CruiserCount
                 = _UBF.Boats.Count(b => b.Type == BoatType.Cruiser);
            Assert.AreEqual(_CruiserCount, 2);

            int _DestroyerCount
                 = _UBF.Boats.Count(b => b.Type == BoatType.Destroyer);
            Assert.AreEqual(_DestroyerCount, 3);

            int _TorpedoboatCount
                 = _UBF.Boats.Count(b => b.Type == BoatType.Torpedoboat);
            Assert.AreEqual(_TorpedoboatCount, 4);
        }

        [TestMethod]
        public void ParseArrangement_BoatsTouch_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_BoatsTouch)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }

            //act
            _UBF.ParseArrangement();

            //assert
            Assert.IsNotNull(_UBF.Boats);
            Assert.AreNotEqual(_UBF.Boats.Count, 10);
        }

        [TestMethod]
        public void CheckArrangement_CorrectlyFilled_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_CorrectlyFilled)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }
            _UBF.ParseArrangement();
            string _errorMsg;

            //act
            bool _result = _UBF.CheckArrangement(out _errorMsg);

            //assert
            Assert.AreEqual(_result, true);
        }

        [TestMethod]
        public void CheckArrangement_BoatsTouch_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_BoatsTouch)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }
            _UBF.ParseArrangement();
            string _errorMsg;

            //act
            bool _result = _UBF.CheckArrangement(out _errorMsg);

            //assert
            Assert.AreEqual(_result, false);
        }

        [TestMethod]
        public void CheckArrangement_WrongQuantity_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_WrongQuantity)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }
            _UBF.ParseArrangement();
            string _errorMsg;

            //act
            bool _result = _UBF.CheckArrangement(out _errorMsg);

            //assert
            Assert.AreEqual(_result, false);
        }

        [TestMethod]
        public void CheckArrangement_WrongBoatsSet_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_WrongBoatsSet)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }
            _UBF.ParseArrangement();
            string _errorMsg;

            //act
            bool _result = _UBF.CheckArrangement(out _errorMsg);

            //assert
            Assert.AreEqual(_result, false);
        }

        [TestMethod]
        public void CheckArrangement_TooLongBoat_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_TooLongBoat)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }
            _UBF.ParseArrangement();
            string _errorMsg;

            //act
            bool _result = _UBF.CheckArrangement(out _errorMsg);

            //assert
            Assert.AreEqual(_result, false);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckArrangement_NullBoats_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            string _errorMsg;

            //act
            bool _result = _UBF.CheckArrangement(out _errorMsg);

            //assert
            
        }
    }
}
