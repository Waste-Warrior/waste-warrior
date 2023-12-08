namespace WorldOfZuul
{
    public class Parser
    {
        public Command? GetCommand(string inputLine)
        {
            string[] words = inputLine.ToLower().Split(" ", 1); //The parser will convert every command to lowercase and put it into an array

            if (words.Length == 0)
            {
                return null;
            }
            
            return new Command(words[0]);
        }
    }

}
