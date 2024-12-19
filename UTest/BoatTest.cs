using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBattle;
using System.Collections.Generic;

namespace UTest
{
    [TestClass]
    public class BoatTest
    {
        [TestMethod]
        public void Boat_Test()
        {
            //arrange
            Game _Game = new Game();
            UserBattleField _UBF = _Game.UserBattleField;

            _UBF.Boats = new List<Boat>();

            List<Cell> _listCells = 
                new List<Cell>
                {
                    new Cell(0,0, _UBF),
                    new Cell(0,1,_UBF),
                    new Cell(0,2, _UBF)
                };

            //act
            Boat _boat = new Boat(_listCells);

            //assert
            Assert.IsNotNull(_boat);

            foreach (var cell in _boat.Cells)
	        {
                Assert.IsNotNull(cell.ParentBoat);
	        }            
            Assert.AreEqual(_boat.State, BoatState.Healthy);
            Assert.AreEqual(_boat.Type, BoatType.Cruiser);
        }

        [TestMethod]
        public void Shoot_HealthyBoat_ToWounded_Test()
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
                });

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
            _boat.Shoot(_cell);

            //assert          
            Assert.AreEqual(_boat.State, BoatState.Wounded);
            Assert.AreEqual(_cell.Style, CellStyle.WoundedCell);
        }

        [TestMethod]
        public void Shoot_WoundedBoat_ToDead_Test()
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
                    new Cell(0,2, _EBF),
                    new Cell(0,3, _EBF)
                });

            _boat.State = BoatState.Wounded;

            foreach (var cell in _boat.Cells)
            {
                cell.Style = CellStyle.WoundedCell;
            }
            _EBF.Cells[0, 0] = _boat.Cells[0];
            _EBF.Cells[0, 1] = _boat.Cells[1];
            _EBF.Cells[0, 2] = _boat.Cells[2];
            _EBF.Cells[0, 3] = _boat.Cells[3];

            _UBF.Boats = new List<Boat>();

            Random r = new Random();
            int _cellNumber = r.Next(_boat.Cells.Count);
            Cell _cell = _boat.Cells[_cellNumber];
            _cell.Style = CellStyle.HealthyCell;

            //act
            _boat.Shoot(_cell);

            //assert          
            Assert.AreEqual(_boat.State, BoatState.Dead);

            foreach (var cell in _boat.Cells)
            {
                Assert.AreEqual(cell.Style, CellStyle.DeadCell);
            }
        }

    }
}
