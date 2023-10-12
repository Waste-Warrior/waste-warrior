using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class Command
    {
        public string Name { get; }
        public string? RemainingInput { get; } // this might be used for future expansions, such as "take apple".

        public Command(string name, string? remainingInput = null)
        {
            Name = name;
            RemainingInput = remainingInput;
        }
    }
}
