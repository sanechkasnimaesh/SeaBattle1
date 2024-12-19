using Microsoft;
using Microsoft.Data.Sqlite;
using SeaBattle.Model;
using System;
using System.Collections.Generic;

namespace SeaBattle
{
    //public static class SQLiteHelper
    //{
    //    private static string connectionString = "Data Source=SeaBattle.db;Version=3;";

    //    public static void InitializeDatabase()
    //    {
    //        try
    //        {
    //            using (var connection = new SqliteConnection(connectionString))
    //            {
    //                connection.Open();
    //                var command = connection.CreateCommand();
    //                command.CommandText = "CREATE TABLE IF NOT EXISTS GameStats (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserScore INTEGER, EnemyScore INTEGER, StartTime TEXT)";
    //                command.ExecuteNonQuery();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error initializing database: {ex.Message}");
    //        }
    //    }

    //    public static void AddGameStats(int userScore, int enemyScore, DateTime startTime)
    //    {
    //        try
    //        {
    //            using (var connection = new SqliteConnection(connectionString))
    //            {
    //                connection.Open();
    //                var command = connection.CreateCommand();
    //                command.CommandText = "INSERT INTO GameStats (UserScore, EnemyScore, StartTime) VALUES (@UserScore, @EnemyScore, @StartTime)";
    //                command.Parameters.AddWithValue("@UserScore", userScore);
    //                command.Parameters.AddWithValue("@EnemyScore", enemyScore);
    //                command.Parameters.AddWithValue("@StartTime", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
    //                command.ExecuteNonQuery();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error adding game stats: {ex.Message}");
    //        }
    //    }

    //    public static List<GameStatsEntry> GetGameStats()
    //    {
    //        var gameStats = new List<GameStatsEntry>();
    //        try
    //        {
    //            using (var connection = new SqliteConnection(connectionString))
    //            {
    //                connection.Open();
    //                var command = connection.CreateCommand();
    //                command.CommandText = "SELECT UserScore, EnemyScore, StartTime FROM GameStats ORDER BY StartTime DESC";
    //                using (var reader = command.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        gameStats.Add(new GameStatsEntry
    //                        {
    //                            UserScore = reader.GetInt32(0),
    //                            EnemyScore = reader.GetInt32(1),
    //                            StartTime = DateTime.Parse(reader.GetString(2))
    //                        });
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Ошибка при получении игровой статистики: {ex.Message}");
    //        }
    //        return gameStats;
    //    }
    //}
}