using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBattle;
using System.Linq;
using System.Collections.Generic;

namespace UTest
{
    [TestClass]
    public class GameTest
    {
        List<Tuple<int, int>> _CellsCoordinates_CorrectlyFilled = new List<Tuple<int, int>>()
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

        [TestMethod]
        public void Game_Test()
        {
            //arrange

            //act
            Game _Game = new Game();

            //assert
            Assert.IsNotNull(_Game);
            Assert.IsNotNull(_Game.UserBattleField);
            Assert.IsNotNull(_Game.EnemyBattleField);
            Assert.IsNotNull(_Game.UserBattleField.Cells);
            Assert.IsNotNull(_Game.EnemyBattleField.Cells);
            Assert.AreEqual(_Game.UserBattleField.Cells.Cast<Cell>().ToList().Count(), 100);
            Assert.AreEqual(_Game.EnemyBattleField.Cells.Cast<Cell>().ToList().Count(), 100);
        }        

        [TestMethod]
        public void ShootToUserBFAutomatically_NewBF_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.CurrentActor = Actor.Computer;
            UserBattleField _UBF = _Game.UserBattleField;
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            _EBF.Boats = new List<Boat>();

            foreach (var coordinates in _CellsCoordinates_CorrectlyFilled)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }

            _UBF.ParseArrangement();
            string err;
            _UBF.CheckArrangement(out err);

            //act
            _Game.ShootToUserBFAutomatically();

            //assert
            List<Cell> _shootedCells =
                _UBF.Cells.Cast<Cell>().ToList().
                Where(c => (c.IsShooted == true)).ToList();


            Assert.AreEqual(_shootedCells.Count, 1);

            if (_shootedCells.Count == 1)
            {
                Assert.AreEqual(_shootedCells[0].Style, CellStyle.Shooted);
            }
            else if (_shootedCells.Count > 1)
	        {
                Assert.AreEqual(_shootedCells.Where(c=>c.Style == CellStyle.Shooted).Count(), 1);     
	        }
            else
	        {
                Assert.Fail();
	        }
        }

        [TestMethod]
        public void ShootToUserBFAutomatically_WoundedBF_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.CurrentActor = Actor.Computer;
            UserBattleField _UBF = _Game.UserBattleField;
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            _EBF.Boats = new List<Boat>();

            foreach (var coordinates in _CellsCoordinates_CorrectlyFilled)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }

            _UBF.ParseArrangement();
            string err;
            _UBF.CheckArrangement(out err);

            Random r = new Random();
            List<Boat> _boats = _UBF.Boats.Where(b => b.Cells.Count > 1).ToList();
            int index1 = r.Next(1,_boats.Count) - 1;
            Boat _boat = _boats[index1];

            int index2 = r.Next(1,_boat.Cells.Count) - 1;
            Cell _cellToShoot = _boat.Cells[index2];
            _boat.Shoot(_cellToShoot); //now we have a wounded boat

            //act
            _Game.ShootToUserBFAutomatically();

            //assert
            //Computer had to try to finish him
            List<Cell> _potentiallyshootedCells =
                _UBF.Cells.Cast<Cell>().ToList().
                Where(c =>
                        (c.x == _cellToShoot.x && 
                        (c.y == (_cellToShoot.y -1) || c.y == (_cellToShoot.y +1))) ||

                        (c.y == _cellToShoot.y && 
                        (c.x == (_cellToShoot.x -1) || c.x == (_cellToShoot.x +1)))
                     ).ToList();
            if (_potentiallyshootedCells.Where(c=> (c.IsShooted == true)).Count()!=1)
            {
                Assert.Fail();
            }
            if (!_potentiallyshootedCells.Any(c =>
                (c.Style == CellStyle.Shooted ||
                c.Style == CellStyle.WoundedCell ||
                c.Style == CellStyle.DeadCell)))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void IsOver_ComputersShot_False_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.CurrentActor = Actor.Computer;
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_CorrectlyFilled)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }

            _UBF.ParseArrangement();
            string err;
            _UBF.CheckArrangement(out err);

            //act
            bool result = _Game.IsOver();

            //assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void IsOver_ComputersShot_True_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.CurrentActor = Actor.Computer;
            UserBattleField _UBF = _Game.UserBattleField;

            foreach (var coordinates in _CellsCoordinates_CorrectlyFilled)
            {
                _UBF.Cells[coordinates.Item1, coordinates.Item2].DrawCell();
            }

            _UBF.ParseArrangement();
            string err;
            _UBF.CheckArrangement(out err);

            foreach (var boat in _UBF.Boats)
            {
                foreach (var cell in boat.Cells)
                {
                    cell.Style = CellStyle.DeadCell;
                }

                boat.State = BoatState.Dead;
            }

            //act
            bool result = _Game.IsOver();

            //assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(_Game.Result, GameResult.Defeat);
            Assert.AreEqual(_Game.Stage, GameStage.Finished);
        }

        [TestMethod]
        public void IsOver_UsersShot_False_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.CurrentActor = Actor.User;
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            _EBF.ArrangeAutomatically();

            //act
            bool result = _Game.IsOver();

            //assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void IsOver_UsersShot_True_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.CurrentActor = Actor.User;
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            _EBF.ArrangeAutomatically();

            foreach (var boat in _EBF.Boats)
            {
                foreach (var cell in boat.Cells)
                {
                    cell.Style = CellStyle.DeadCell;
                }

                boat.State = BoatState.Dead;
            }

            //act
            bool result = _Game.IsOver();

            //assert
            Assert.AreEqual(result, true);
            Assert.AreEqual(_Game.Result, GameResult.Victory);
            Assert.AreEqual(_Game.Stage, GameStage.Finished);
        }

        [TestMethod]
        public void AssessShoot_Battlecruiser_Computer_Test()
        {
            //arrange
            Game _Game = new Game();
            var _UBF = _Game.UserBattleField;
            var _EBF = _Game.EnemyBattleField;

            Boat _boat = new Boat(
                new List<Cell>(){
                    _UBF.Cells[5, 0],
                    _UBF.Cells[5, 1],
                    _UBF.Cells[5, 2],
                    _UBF.Cells[5, 3]
                });

            int _oldUserScore = _Game.UserScores;
            int _oldEnemyScore = _Game.EnemyScores;
               
            //act
            _Game.AssessShoot(Actor.Computer, _boat.Cells[1]);

            int _newUserScore = _Game.UserScores;
            int _newEnemyScore = _Game.EnemyScores;

            //assert
            Assert.AreEqual(_newEnemyScore - _oldEnemyScore, 15);
            Assert.AreEqual(_newUserScore - _oldUserScore, 0);
        }

        [TestMethod]
        public void AssessShoot_Cruiser_User_Test()
        {
            //arrange
            Game _Game = new Game();
            var _UBF = _Game.UserBattleField;
            var _EBF = _Game.EnemyBattleField;

            Boat _boat = new Boat(
                new List<Cell>(){
                    _EBF.Cells[5, 0],
                    _EBF.Cells[5, 1],
                    _EBF.Cells[5, 2]
                });

            int _oldUserScore = _Game.UserScores;
            int _oldEnemyScore = _Game.EnemyScores;

            //act
            _Game.AssessShoot(Actor.User, _boat.Cells[1]);

            int _newUserScore = _Game.UserScores;
            int _newEnemyScore = _Game.EnemyScores;

            //assert
            Assert.AreEqual(_newEnemyScore - _oldEnemyScore, 0);
            Assert.AreEqual(_newUserScore - _oldUserScore, 30);
        }

        [TestMethod]
        public void AssessShoot_Destroyer_User_Test()
        {
            //arrange
            Game _Game = new Game();
            var _UBF = _Game.UserBattleField;
            var _EBF = _Game.EnemyBattleField;

            Boat _boat = new Boat(
                new List<Cell>(){
                    _EBF.Cells[5, 0],
                    _EBF.Cells[5, 1]
                });

            int _oldUserScore = _Game.UserScores;
            int _oldEnemyScore = _Game.EnemyScores;

            //act
            _Game.AssessShoot(Actor.User, _boat.Cells[1]);

            int _newUserScore = _Game.UserScores;
            int _newEnemyScore = _Game.EnemyScores;

            //assert
            Assert.AreEqual(_newEnemyScore - _oldEnemyScore, 0);
            Assert.AreEqual(_newUserScore - _oldUserScore, 60);
        }

        [TestMethod]
        public void AssessShoot_Torpedoboat_Computer_Test()
        {
            //arrange
            Game _Game = new Game();
            var _UBF = _Game.UserBattleField;
            var _EBF = _Game.EnemyBattleField;

            Boat _boat = new Boat(
                new List<Cell>(){
                    _UBF.Cells[5, 0]
                });

            int _oldUserScore = _Game.UserScores;
            int _oldEnemyScore = _Game.EnemyScores;

            //act
            _Game.AssessShoot(Actor.Computer, _boat.Cells[0]);

            int _newUserScore = _Game.UserScores;
            int _newEnemyScore = _Game.EnemyScores;

            //assert
            Assert.AreEqual(_newEnemyScore - _oldEnemyScore, 150);
            Assert.AreEqual(_newUserScore - _oldUserScore, 0);
        }

        [TestMethod]
        public void IsTargetStageAvailable_Test()
        {
            //arrange
            Game _Game = new Game();

            List<Tuple<GameStage, GameStage, bool>> _SwitchingVariants =
                new List<Tuple<GameStage, GameStage, bool>>()
                {
                    Tuple.Create(GameStage.BoatsArrange, GameStage.BoatsArrange, false),
                    Tuple.Create(GameStage.BoatsArrange, GameStage.Playing, true),
                    Tuple.Create(GameStage.BoatsArrange, GameStage.Finished, false),
                    Tuple.Create(GameStage.Playing, GameStage.BoatsArrange, false),
                    Tuple.Create(GameStage.Playing, GameStage.Playing, false),
                    Tuple.Create(GameStage.Playing, GameStage.Finished, true),
                    Tuple.Create(GameStage.Finished, GameStage.BoatsArrange, false),
                    Tuple.Create(GameStage.Finished, GameStage.Playing, false),
                    Tuple.Create(GameStage.Finished, GameStage.Finished, false)
                };

            foreach (var variant in _SwitchingVariants)
            {
                //arrange
                _Game.Stage = variant.Item1;

                //act
                bool result = _Game.IsTargetStageAvailable(variant.Item2);

                //assert
                Assert.AreEqual(result, variant.Item3);
            }
        }
    }
}
