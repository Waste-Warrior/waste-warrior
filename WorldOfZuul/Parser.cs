using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class Parser
    {
        private readonly CommandWords commandWords = new();

        public Command? GetCommand(string inputLine)
        {
            string[] words = inputLine.Split();

            if (words.Length == 0 || !commandWords.IsValidCommand(words[0]))
            {
                return null;
            }

            if (words.Length > 1)
            {
                string? remainingCommand = null;
                for (int i = 1; i < words.Length; i++)
                {
                    remainingCommand += words[i];
                    if (i != words.Length - 1)
                    {
                        remainingCommand += " ";
                    }
                    Debug.WriteLine(remainingCommand);
                }
                return new Command(words[0], remainingCommand);
            }

            return new Command(words[0]);
        }
    }

}
