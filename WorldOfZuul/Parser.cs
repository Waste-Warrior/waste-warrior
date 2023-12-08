namespace WorldOfZuul
{
    public class Parser
    {
        public Command? GetCommand(string inputLine)
        {
            string[] words = inputLine.ToLower().Split(); //The parser will convert every command to lowercase and put it into an array

            if (words.Length == 0)
            {
                return null;
            }

            if (words.Length > 1)
            {
                //Maybe the remaining words from the input should be collected in a separate array instead of a string, this could be useful if a command uses multiple arguments.
                string? remainingCommand = null; 
                for (int i = 1; i < words.Length; i++)
                {
                    remainingCommand += words[i];
                    if (i != words.Length - 1)
                    {
                        remainingCommand += " ";
                    }
                }
                return new Command(words[0], remainingCommand);
            }
            
            return new Command(words[0]);
        }
    }

}
