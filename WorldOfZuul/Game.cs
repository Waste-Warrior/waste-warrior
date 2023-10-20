namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;

        public Game()
        {
            CreateRooms();
        }

        private void CreateRooms()
        {
            //The trash arrays have to be set outside of the room declaration
            Trash[] outsideTrash = {new ("an empty beer can", Trash.TrashType.Metal), new ("an empty Cola can", Trash.TrashType.Metal)};
            
            Room outside = new("Outside", "You are standing outside the main entrance of the university. To the east is a large building, to the south is a computing lab, and to the west is the campus pub.", outsideTrash);
            Room theatre = new("Theatre", "You find yourself inside a large lecture theatre. Rows of seats ascend up to the back, and there's a podium at the front. It's quite dark and quiet.");
            Room pub = new("Pub", "You've entered the campus pub. It's a cozy place, with a few students chatting over drinks. There's a bar near you and some pool tables at the far end.");
            Room lab = new("Lab", "You're in a computing lab. Desks with computers line the walls, and there's an office to the east. The hum of machines fills the room.");
            Room office = new("Office", "You've entered what seems to be an administration office. There's a large desk with a computer on it, and some bookshelves lining one wall.");

            outside.SetExits(null, theatre, lab, pub); // North, East, South, West

            theatre.SetExit("west", outside);

            pub.SetExit("east", outside);

            lab.SetExits(outside, office, null, null);

            office.SetExit("west", lab);

            currentRoom = outside;
        }

        public void Play()
        {
            Parser parser = new();

            PrintWelcome();

            bool continuePlaying = true;
            while (continuePlaying)
            {
                Console.WriteLine(currentRoom?.ShortDescription);
                Console.Write("> ");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter a command.");
                    continue;
                }

                Command? command = parser.GetCommand(input);

                if (command == null)
                {
                    Console.WriteLine("I don't know that command.");
                    continue;
                }

                switch(command.Name)
                {
                    case "look":
                        // This solves the possibility of a null reference exception
                        if (currentRoom != null)
                        {
                            Console.WriteLine(currentRoom.LongDescription);

                            //This currently prints all Trash in the Room to the console.
                            if (currentRoom.ScatteredTrash != null && currentRoom.ScatteredTrash.Length != 0)
                            {
                                string allTrash = "";
                                foreach (Trash trash in currentRoom.ScatteredTrash)
                                {
                                    allTrash += trash.Name + ", ";
                                    //Console.WriteLine(trash.Name);
                                }
                                int lastCommaIndex = allTrash.LastIndexOf(", ");
                                if (lastCommaIndex >= 0)
                                {
                                    allTrash = allTrash.Remove(lastCommaIndex, 2);
                                }
                                Console.WriteLine($"Laying on the floor you can clearly see {allTrash}.");
                            }
                        }
                        break;

                    case "back":
                        if (previousRoom == null)
                            Console.WriteLine("You can't go back from here!");
                        else
                            currentRoom = previousRoom;
                        break;

                    case "north":
                    case "south":
                    case "east":
                    case "west":
                        Move(command.Name);
                        break;

                    case "quit":
                        continuePlaying = false;
                        break;

                    case "help":
                        PrintHelp();
                        break;
                    
                    //In the future this will allow you to collect trash for later disposal. Currently this removes Trash from a Room.
                    case "collect":
                        if (currentRoom != null && command.RemainingInput != null && currentRoom.IsTrashInRoom(command.RemainingInput))
                        {
                            Trash? collectedTrash = currentRoom?.RemoveTrash(command.RemainingInput); //This trash object can later be added to an inventory

                            if (collectedTrash != null) //This check may not be needed
                            {
                                //The string uses an escape sequence to color the text, if we want to color code the text output we should probably create an easy to use system
                                Console.WriteLine($"You collected \x1b[93m{collectedTrash.Name}\x1b[39m"); 
                            }
                        }
                        else
                        {
                            Console.WriteLine("This object is not in the room");
                        }
                        break;

                    default:
                        Console.WriteLine("I don't know that command.");
                        break;
                }
                Console.WriteLine(); //Adding an empty Line at the end of each action output
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        private void Move(string direction)
        {
            if (currentRoom?.Exits.ContainsKey(direction) == true)
            {
                previousRoom = currentRoom;
                currentRoom = currentRoom?.Exits[direction];
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }


        private static void PrintWelcome()
        {
            Console.WriteLine("Welcome to the World of Zuul: Waste Warriors!");
            Console.WriteLine("World of Zuul: Waste Warriors is a new, incredibly trashy adventure game.");
            PrintHelp();
            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around the university.");
            Console.WriteLine();
            Console.WriteLine("Navigate by typing 'north', 'south', 'east', or 'west'.");
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'back' to go to the previous room.");
            Console.WriteLine("Type 'collect' to collect trash within the room");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }
    }
}
