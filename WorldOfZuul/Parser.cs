namespace WorldOfZuul
{
    public class Parser
    {
        private readonly CommandWords commandWords = new();

        public Command? GetCommand(string inputLine)
        {
            
            string[] words = inputLine.ToLower().Split(); //The parser will convert every command to lowercase and put it into an array
            

            if (words.Length == 0 || !commandWords.IsValidCommand(words[0]))
            {
                return null;
            }

            return new Command(words[0]);
        }
    }

}
