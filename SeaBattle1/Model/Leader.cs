using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    /// <summary>
    /// Someone who won and had good result.
    /// </summary>
    public class Leader : IComparable
    {
        public string Name { get; set; }
        public int UserScores { get; set; }
        public string DateOfWin { get; set; }

        public Leader(string p_Name, int p_US, string p_date)
        {
            Name = p_Name;
            UserScores = p_US;
            DateOfWin = p_date;
        }

        public int CompareTo(object obj)
        {
            Leader _otherLeader = obj as Leader;

            if (_otherLeader != null)
            {
                return UserScores - _otherLeader.UserScores;
            }
            else
            {
                return 1;
            }
        }
    }
}
