using Microsoft.EntityFrameworkCore;
using SeaBattle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SeaBattle.Context
{
    internal class ApplicationContext: DbContext
    {
        public DbSet<GameStatsEntry> GameStats { get; set; } // Название таблицы

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=game_stats.db"); // Имя файла базы данных
        }
    }
}
