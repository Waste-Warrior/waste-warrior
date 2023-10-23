namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;
        Inventory inventory = new Inventory(); // create an instance of the Inventory class
        private int trashCollectedToday = 0;
        private int currentDay = 0;
        public enum Days
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        private Dictionary<Days, int> trashSpawnedOnDay = new Dictionary<Days, int>()
        {
            { Days.Monday, 0 },
            { Days.Tuesday, 0 },
            { Days.Wednesday, 0 },
            { Days.Thursday, 0 },
            { Days.Friday, 0 },
            { Days.Saturday, 0 },
            { Days.Sunday, 0 }
        };

        public readonly Days Day; // This will be used to determine when the trash can be picked up and shows up in the room

        public Game()
        {
            CreateRooms();
        }

        private void CreateRooms()
        {
            //The trash arrays have to be set outside of the room declaration
            Trash[] outsideTrash = {
                new ("an empty beer can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty beer can", Trash.TrashType.Metal, Days.Tuesday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Tuesday),
                new ("an empty beer can", Trash.TrashType.Metal, Days.Wednesday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Wednesday)
            };
            Trash[] theatreTrash = {
                new ("an empty beer can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty beer can", Trash.TrashType.Metal, Days.Tuesday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Tuesday),
            };
            
            Room outside = new("Outside", "You are standing outside the main entrance of the university. To the east is a large building, to the south is a computing lab, and to the west is the campus pub.", outsideTrash);
            Room theatre = new("Theatre", "You find yourself inside a large lecture theatre. Rows of seats ascend up to the back, and there's a podium at the front. It's quite dark and quiet.", theatreTrash);
            Room pub = new("Pub", "You've entered the campus pub. It's a cozy place, with a few students chatting over drinks. There's a bar near you and some pool tables at the far end.");
            Room lab = new("Lab", "You're in a computing lab. Desks with computers line the walls, and there's an office to the east. The hum of machines fills the room.");
            Room office = new("Office", "You've entered what seems to be an administration office. There's a large desk with a computer on it, and some bookshelves lining one wall.");
            outside.setDayDescriptions("Hi, it is Monday!", "Hi, it is Tuesday!", "Hi, it is Wednesday!", "Hi, it is Thursday!", "Hi, it is Friday!", "Hi, it is Saturday!", "Hi, it is Sunday!");

            outside.SetExits(null, theatre, lab, pub); // North, East, South, West

            theatre.SetExit("west", outside);

            pub.SetExit("east", outside);

            lab.SetExits(outside, office, null, null);

            office.SetExit("west", lab);

            currentRoom = outside;

            foreach (Trash item in outsideTrash.Concat(theatreTrash))
            {
                trashSpawnedOnDay[item.Day] += 1;
            }
            //Now each day has a score of how many trash items are on the floor. Eg. dayScores[Days.Monday]

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
                            if (currentRoom.dayDescription.ContainsKey((Days)currentDay))
                            {
                                Console.WriteLine(currentRoom.dayDescription[(Days)currentDay]);
                            }
                            
                            //This currently prints all Trash in the Room to the console.
                            if (currentRoom.ScatteredTrash != null && currentRoom.ScatteredTrash.Length != 0)
                            {
                                string allTrash = "";
                                foreach (Trash trash in currentRoom.ScatteredTrash)
                                {
                                    if (trash.Day == (Days)currentDay)
                                    {
                                        allTrash += trash.Name + ", ";
                                    }
                                }
                                int lastCommaIndex = allTrash.LastIndexOf(", ");
                                if (lastCommaIndex >= 0)
                                {
                                    allTrash = allTrash.Remove(lastCommaIndex, 2);
                                }
                                if (allTrash.Length > 0)
                                {
                                    Console.WriteLine($"Laying on the floor you can clearly see {allTrash}.");
                                }
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
                        if (currentRoom != null && command.RemainingInput != null && currentRoom.IsTrashInRoom(command.RemainingInput, currentDay))
                        {
                            Trash? collectedTrash = currentRoom?.RemoveTrash(command.RemainingInput, currentDay); //This trash object can later be added to an inventory

                            if (collectedTrash != null)
                            {
                                inventory.AddItem(collectedTrash); // call the AddItem method of Inventory

                                Console.WriteLine($"You collected \x1b[93m{collectedTrash.Name}\x1b[39m");
                                trashCollectedToday += 1;
                                if (IsDayCompleted(trashSpawnedOnDay, currentDay, trashCollectedToday))
                                {   
                                    trashCollectedToday = 0;
                                    currentDay += 1;
                                    Console.WriteLine("You have collected all the trash for today!");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("This object is not in the room");
                        }
                        break;
                    
                    case "inventory":
                        Console.WriteLine("You have the following items in your inventory:");
                        foreach (Trash item in inventory.items)
                        {
                            Console.WriteLine(item.Name);
                        }
                        break;

                    case "sort":
                    {
                        if (currentRoom != null && command.RemainingInput != null)
                        {
                            bool skipLastMessage = false;
                            string[] commandRemainder = command.RemainingInput.Split(" ");
                            if (inventory.items.Count == 0)
                            {
                                Console.WriteLine("Your inventory is empty.");
                                skipLastMessage = true;
                            }
                            else if (commandRemainder.Length < 2)
                            {
                               Console.WriteLine("Please enter the item name and sorting category.");
                            }
                            else
                            {
                                if (commandRemainder.Length < 2)
                                {
                                    Console.WriteLine("Please enter the item name and sorting category.");
                                }
                                else
                                {
                                    string itemName = string.Join(" ", commandRemainder.Take(commandRemainder.Length - 1)).ToLower();
                                    string sortingCategory = commandRemainder.Last().ToLower();

                                    char[] letters = sortingCategory.ToCharArray();
                                    letters[0] = char.ToUpper(letters[0]);
                                    sortingCategory = new string(letters);

                                    foreach (Trash item in inventory.items)
                                    {
                                        if (itemName == item.Name.ToLower())
                                        {
                                            if (Enum.TryParse(sortingCategory, out Trash.TrashType trashType))
                                            {
                                                if (inventory.sortItem(item, trashType))
                                                {
                                                    Console.WriteLine($"Sorted {item.Name} as {trashType}");
                                                    skipLastMessage = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Couldn't remove from inventory.");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Invalid sorting category: {sortingCategory}");
                                                skipLastMessage = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (skipLastMessage == false)
                            {
                            Console.WriteLine("Item not found in inventory, try again. (Syntax: sort <item name> <sorting category>)");
                            }
                        }
                        break;
                    }

                    default:
                        Console.WriteLine("I don't know that command.");
                        break;
                }
                Console.WriteLine(); //Adding an empty Line at the end of each action output
            }

            Console.WriteLine("Thank you for playing World of Zuul: Waste Warriors!");
        }
        private bool IsDayCompleted(Dictionary<Days, int> trashSpawned, int day = 0, int collectedTrashToday = 0)
        {
            return trashSpawned[(Days)day] == collectedTrashToday;
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
