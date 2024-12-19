using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Model
{
    [Serializable]
    public class GameStatsEntry
    {
        public int Id { get; set; }
        public int UserScore { get; set; }
        public int EnemyScore { get; set; }
        public string StartTime { get; set; }
    }
}
