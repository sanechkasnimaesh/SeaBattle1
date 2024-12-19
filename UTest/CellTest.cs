using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBattle;
using System.Collections.Generic;

namespace UTest
{
    [TestClass]
    public class CellTest
    {
        [TestMethod]
        public void Cell_Enemy_Test()
        {
            //arrange           
            
            //act
            Cell _cell = new Cell(0, 0, new EnemyBattleField(new Game()));
            
            //assert
            Assert.IsNotNull(_cell);
            Assert.IsNotNull(_cell.ParentBattleField);
            Assert.AreEqual(_cell.Style, CellStyle.Empty);
            Assert.AreEqual(_cell.IsInvisible, true);
        }

        [TestMethod]
        public void Cell_User_Test()
        {
            //arrange

            //act
            Cell _cell = new Cell(0, 0, new UserBattleField(new Game()));

            //assert
            Assert.IsNotNull(_cell);
            Assert.IsNotNull(_cell.ParentBattleField);
            Assert.AreEqual(_cell.Style, CellStyle.Empty);
            Assert.AreEqual(_cell.IsInvisible, false);
        }

        [TestMethod]
        public void DrawCell_Arrange_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.Stage = GameStage.BoatsArrange;
            UserBattleField _BF = _Game.UserBattleField;

            Random r = new Random();
            int _cellNumber = r.Next(100);
            int x = (int) Math.Floor((double)_cellNumber / 10);
            int y = _cellNumber % 10;
            Cell _cell = _BF.Cells[x, y];

            //act
            _cell.DrawCell();            

            //assert
            Assert.AreEqual(_cell.Style, CellStyle.HealthyCell);
            Assert.AreEqual(_cell.IsInvisible, false);
        }

        [TestMethod]
        public void DrawCell_Playing_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.Stage = GameStage.Playing;
            UserBattleField _BF = _Game.UserBattleField;

            Random r = new Random();
            int _cellNumber = r.Next(100);
            int x = (int)Math.Floor((double)_cellNumber / 10);
            int y = _cellNumber % 10;
            Cell _cell = _BF.Cells[x, y];

            //act
            _cell.DrawCell();

            //assert
            Assert.AreEqual(_cell.Style, CellStyle.Empty);
            Assert.AreEqual(_cell.IsInvisible, false);
        }

        [TestMethod]
        public void RemoveCell_Arrange_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.Stage = GameStage.BoatsArrange;
            UserBattleField _BF = _Game.UserBattleField;

            Random r = new Random();
            int _cellNumber = r.Next(100);
            int x = (int)Math.Floor((double)_cellNumber / 10);
            int y = _cellNumber % 10;
            Cell _cell = _BF.Cells[x, y];
            _cell.Style = CellStyle.HealthyCell;

            //act
            _cell.RemoveCell();

            //assert
            Assert.AreEqual(_cell.Style, CellStyle.Empty);
            Assert.AreEqual(_cell.IsInvisible, false);
        }

        [TestMethod]
        public void RemoveCell_Playing_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.Stage = GameStage.Playing;
            UserBattleField _BF = _Game.UserBattleField;

            Random r = new Random();
            int _cellNumber = r.Next(100);
            int x = (int)Math.Floor((double)_cellNumber / 10);
            int y = _cellNumber % 10;
            Cell _cell = _BF.Cells[x, y];
            _cell.Style = CellStyle.HealthyCell;

            //act
            _cell.RemoveCell();

            //assert
            Assert.AreEqual(_cell.Style, CellStyle.HealthyCell);
            Assert.AreEqual(_cell.IsInvisible, false);
        }

        [TestMethod]
        public void Shoot_HealthyCell_ToWounded_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.Stage = GameStage.Playing;
            EnemyBattleField _EBF = _Game.EnemyBattleField;
            UserBattleField _UBF = _Game.UserBattleField;
            _Game.CurrentActor = Actor.User;

            _EBF.Boats = new List<Boat>();

            Boat _boat = new Boat(
                new List<Cell>
                {
                    new Cell(0,0, _EBF),
                    new Cell(0,1,_EBF),
                    new Cell(0,2, _EBF)
                } );

            _boat.State = BoatState.Healthy;

            foreach (var cell in _boat.Cells)
            {
                cell.Style = CellStyle.HealthyCell;
            }
            _EBF.Cells[0, 0] = _boat.Cells[0];
            _EBF.Cells[0, 1] = _boat.Cells[1];
            _EBF.Cells[0, 2] = _boat.Cells[2];

            _UBF.Boats = new List<Boat>();

            Random r = new Random();
            int _cellNumber = r.Next(_boat.Cells.Count);
            Cell _cell = _boat.Cells[_cellNumber];

            //act
            _cell.Shoot();

            //assert
            Assert.AreEqual(_cell.Style, CellStyle.WoundedCell);
            Assert.AreEqual(_cell.IsInvisible, false);
        }

        [TestMethod]
        public void Shoot_HealthyCell_ToDead_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.Stage = GameStage.Playing;
            EnemyBattleField _EBF = _Game.EnemyBattleField;
            UserBattleField _UBF = _Game.UserBattleField;
            _Game.CurrentActor = Actor.User;

            _EBF.Boats = new List<Boat>();

            Boat _boat = new Boat(
                new List<Cell>
                {
                    new Cell(5,8, _EBF)
                });

            _boat.State = BoatState.Healthy;            
             _boat.Cells[0].Style = CellStyle.HealthyCell;
            
            _EBF.Cells[5, 8] = _boat.Cells[0];

            _UBF.Boats = new List<Boat>();

            
            Cell _cell = _boat.Cells[0];

            //act
            _cell.Shoot();

            //assert
            Assert.AreEqual(_cell.Style, CellStyle.DeadCell);
            Assert.AreEqual(_cell.IsInvisible, false);
        }
        
        [TestMethod]
        public void Shoot_EmptyCell_ToShooted_Test()
        {
            //arrange
            Game _Game = new Game();
            _Game.Stage = GameStage.Playing;
            EnemyBattleField _EBF = _Game.EnemyBattleField;
            UserBattleField _UBF = _Game.UserBattleField;
            _Game.CurrentActor = Actor.User;
            _EBF.Boats = new List<Boat>();
            _UBF.Boats = new List<Boat>();

            Random r = new Random();
            int _cellNumber = r.Next(100);
            int x = (int)Math.Floor((double)_cellNumber / 10);
            int y = _cellNumber % 10;

            Cell _cell = _EBF.Cells[x, y];
            _cell.Style = CellStyle.Empty;

            //act
            _cell.Shoot();

            //assert
            Assert.AreEqual(_cell.Style, CellStyle.Shooted);
            Assert.AreEqual(_cell.IsInvisible, false);
        }
    }
}
