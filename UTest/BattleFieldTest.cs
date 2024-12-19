using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBattle;
using System.Collections.Generic;
using System.Linq;

namespace UTest
{
    [TestClass]
    public class BattleFieldTest : BattleField
    {
        [TestMethod]
        public void SurroundBoat_NearWall_Shooted_Test()
        {
            //arrange
            Game _Game = new Game();
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            _EBF.Boats = new List<Boat>();

            Boat _boat = new Boat(
                new List<Cell>
                {
                    new Cell(0,0, _EBF),
                    new Cell(0,1,_EBF),
                    new Cell(0,2, _EBF)
                });

            _EBF.Boats.Add(_boat);

            _boat.State = BoatState.Healthy;

            foreach (var cell in _boat.Cells)
            {
                cell.Style = CellStyle.HealthyCell;
            }
            _EBF.Cells[0, 0] = _boat.Cells[0];
            _EBF.Cells[0, 1] = _boat.Cells[1];
            _EBF.Cells[0, 2] = _boat.Cells[2];

            //act
            _EBF.SurroundBoat(_boat, CellStyle.Shooted, false);

            //assert
            int minX = _boat.Cells.Min(c => c.x) - 1;
            int maxX =  _boat.Cells.Max(c => c.x) + 1;
            int minY = _boat.Cells.Min(c => c.y) - 1;
            int maxY = _boat.Cells.Max(c => c.y) + 1;

            //asserts that neigbours marked correctly
            List<Cell> _neigbours =
            _EBF.Cells.Cast<Cell>().ToList().
                        Where(
                        c => c.x >= minX && c.x <= maxX
                        && c.y >= minY && c.y <= maxY
                        && !(((c.x > minX) && (c.x < maxX)) && ((c.y > minY) && (c.y<maxY)))
                        ).ToList();
            foreach (var cell in _neigbours)
            {
                Assert.AreEqual(cell.IsInvisible, false);
                Assert.AreEqual(cell.Style, CellStyle.Shooted);
            }

            //asserts that cells of boat weren't changed
            foreach (var cell in _boat.Cells)
            {
                Assert.AreEqual(cell.Style, CellStyle.HealthyCell);
            }

            //asserts that other calles of BF weren't changed
            List<Cell> _others = _EBF.Cells.Cast<Cell>().ToList().
                Where(
                    c => !(_boat.Cells.Contains(c))
                      && !(_neigbours.Contains(c))
                    ).ToList();

            foreach (var cell in _others)
            {
                Assert.AreEqual(cell.IsInvisible, true);
                Assert.AreEqual(cell.Style, CellStyle.Empty);
            }
        }

        [TestMethod]
        public void SurroundBoat_InMiddle_Arranged_Test()
        {
            //arrange
            Game _Game = new Game();
            EnemyBattleField _EBF = _Game.EnemyBattleField;

            _EBF.Boats = new List<Boat>();

            Boat _boat = new Boat(
                new List<Cell>
                {
                    new Cell(3,5, _EBF)
                });

            _EBF.Boats.Add(_boat);

            _boat.State = BoatState.Healthy;

            _boat.Cells[0].Style = CellStyle.HealthyCell;
           
            _EBF.Cells[3, 5] = _boat.Cells[0];

            //act
            _EBF.SurroundBoat(_boat, CellStyle.Empty, true);

            //assert
            int minX = _boat.Cells.Min(c => c.x) - 1;
            int maxX = _boat.Cells.Max(c => c.x) + 1;
            int minY = _boat.Cells.Min(c => c.y) - 1;
            int maxY = _boat.Cells.Max(c => c.y) + 1;

            //asserts that neigbours marked correctly
            List<Cell> _neigbours =
            _EBF.Cells.Cast<Cell>().ToList().
                        Where(
                        c => c.x >= minX && c.x <= maxX
                        && c.y >= minY && c.y <= maxY
                        && !(((c.x > minX) && (c.x < maxX)) && ((c.y > minY) && (c.y < maxY)))
                        ).ToList();
            foreach (var cell in _neigbours)
            {
                Assert.AreEqual(cell.IsInvisible, true);
                Assert.AreEqual(cell.IsSafeZone, true);
                Assert.AreEqual(cell.Style, CellStyle.Empty);
            }

            //asserts that cells of boat weren't changed
            foreach (var cell in _boat.Cells)
            {
                Assert.AreEqual(cell.Style, CellStyle.HealthyCell);
                Assert.AreEqual(cell.IsInvisible, true);
            }

            //asserts that other calles of BF weren't changed
            List<Cell> _others = _EBF.Cells.Cast<Cell>().ToList().
                Where(
                    c => !(_boat.Cells.Contains(c))
                      && !(_neigbours.Contains(c))
                    ).ToList();

            foreach (var cell in _others)
            {
                Assert.AreEqual(cell.IsInvisible, true);
                Assert.AreEqual(cell.Style, CellStyle.Empty);
            }

            //asserts that priority zone created
            List<Cell> _friendlyCells =
            _EBF.Cells.Cast<Cell>().ToList().
                        Where(
                        c => c.x >= (minX-1) && c.x <= (maxX+1)
                        && c.y >= (minY-1) && c.y <= (maxY+1)
                        && !(((c.x > minX-1) && (c.x < maxX+1)) && ((c.y > minY-1) && (c.y < maxY+1)))
                        ).ToList();
            foreach (var cell in _friendlyCells)
            {
                Assert.AreEqual(cell.IsBoatsFriendlyZone, true);
            }

        }

    } 
}
