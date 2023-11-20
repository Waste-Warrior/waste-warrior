namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;
        Inventory inventory = new Inventory(); // create an instance of the Inventory class
        private int trashSortedToday = 0;
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
            /*List of Trash to choose from, you should remember to set the day.
             This should be removed from the actual game*/
            Trash[] preset =
            {
                new ("an empty beer can", Trash.TrashType.Metal, 0),
                new ("an empty cola can", Trash.TrashType.Metal, 0),
                new ("a rotten apple", Trash.TrashType.Madaffald, 0),
                new ("a battery", Trash.TrashType.Miljøkasse, 0),
                new ("a pizza box", Trash.TrashType.Restaffald, 0),
                new ("a chips bag", Trash.TrashType.Restaffald, 0),
                new ("an old magazine", Trash.TrashType.PapirOgPap, 0),
                new ("an eggshell", Trash.TrashType.Madaffald, 0),
                new ("a tissue", Trash.TrashType.Restaffald, 0),
                new ("a broken light bulb", Trash.TrashType.Miljøkasse, 0),
                new ("a wine bottle", Trash.TrashType.Glas, 0),
                new ("a plastic bag", Trash.TrashType.Plast, 0),
                new ("an orange juice carton", Trash.TrashType.Madkartoner, 0),
                new ("a paper cup", Trash.TrashType.Restaffald, 0),
                new ("a torn apart T-shirt", Trash.TrashType.Tekstilaffald, 0)  
            };
            
            //The trash arrays have to be set outside of the room declaration
            Trash[] outsideTrash = {
                new ("an empty beer can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Monday),
                new ("Miljokasse", Trash.TrashType.Miljøkasse, Days.Monday),
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
            
            outside.setDayDescriptions(
                "Greetings, Warrior! As it is your first day as a trash warrior, you should learn to sort the first categories of trash today. Head on to the theater (by typing 'east') for the lecture!",
                "Hi, it is Tuesday!",
                "Hi, it is Wednesday!",
                "Hi, it is Thursday!",
                "Hi, it is Friday!",
                "Hi, it is Saturday!",
                "Hi, it is Sunday!"
            );
            theatre.setDayDescriptions(
                "Welcome, student! In Denmark, we sort trash by the materials it is made of! Simple, right?\nThe first sorting category is \x1b[93mMetal\x1b[39m. Try to sort trash into the \x1b[93mMetal\x1b[39m trash can! Collect and sort the trash in every room to move on to the next day!\nMetal trash example: an empty beer can",
                "Hi, it is Theater Tuesday!\nNow, lets make sorting a little bit harder. As of Today there are 3 more trash categories: Glas, PapirOgPap and Plast.\nExamples:\nGlas: a wine bottle\nPapirOgPap: an old magazine\nPlast: a plastic bag",
                "Hi, it is Theater Wednesday!\nToday’s new  trash categories are: Madaffald, Restaffald and Madkartoner.\nMake sure to bag your Restaffald and Madaffald before throwing it out at home. Don’t worry though, you don’t have to do that in-game.\nExamples:\nMadaffald: a rotten apple\nRestaffald: a pizza box\nMadkartonner: an orange juice carton",
                "Hi, it is Theater Thursday!\nThe last category is Miljøkasse.\nBe careful with this type of waste – as its hazardous!\nExample of Miljøkasse: a battery",
                "Hi, it is Friday!",
                "Hi, it is Saturday!",
                "Hi, it is Sunday!"
            );

            outside.SetExits(null, theatre, lab, pub); // North, East, South, West

            theatre.SetExit("west", outside);

            pub.SetExit("east", outside);

            lab.SetExits(outside, office, null, null);

            office.SetExit("west", lab);

            currentRoom = outside;

            foreach (Trash item in outsideTrash.Concat(theatreTrash)) // add .Concat(nextTrashName) to add more trash items for score comparison.
            {
                trashSpawnedOnDay[item.Day] += 1;
            }
            //Now each day has a score of how many trash items are on the floor. Eg. trashSpawnedOnDay[Days.Monday]

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
                                    if (trash.Day == (Days)currentDay)
                                    {
                                        allTrash += $"\x1b[93m{trash.Name}\x1b[39m" + ", ";
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
                            if (currentRoom.dayDescription.ContainsKey((Days)currentDay))
                            {
                                Console.WriteLine($"\n{currentRoom.dayDescription[(Days)currentDay]}");
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

                            if (collectedTrash != null) //This check may not be needed
                            {
                                inventory.AddItem(collectedTrash); // call the AddItem method of Inventory

                                Console.WriteLine($"You collected \x1b[93m{collectedTrash.Name}\x1b[39m");
                            }
                        }
                        else
                        {
                            Console.WriteLine("This object is not in the room");
                        }
                        break;
                    
                    case "inventory":
                        if (inventory.items.Count == 0)
                        {
                            Console.WriteLine("You have no items in your inventory. Go collect some trash!");
                        }
                        else
                        {
                            Console.WriteLine("You have the following items in your inventory:");
                            foreach (Trash itemInventory in inventory.items)
                            {
                                Console.WriteLine($"    * \x1b[93m{itemInventory.Name}\x1b[39m");
                            }
                        }
                        break;

                    case "sort":
                        if (currentRoom == null || command.RemainingInput == null)
                        {Console.WriteLine("\x1b[93mInvalid command usage.\x1b[39m (Syntax: sort <item name> <sorting category>)"); break;}

                        string[] commandRemainder = command.RemainingInput.Split(" "); //Splitting the command input into an array of strings

                        if (inventory.items.Count == 0)
                        {Console.WriteLine("Your \x1b[93minventory is empty\x1b[39m. Please collect trash from the floors first.");break;}
                        
                        else if (commandRemainder.Length < 2) // checking if command has an input of at least 2 strings
                        {
                            Console.WriteLine("\x1b[93mPlease enter the item name and sorting category\x1b[39m. (Syntax: sort <item name> <sorting category>)");
                            break;
                        }

                        string itemName = string.Join(" ", commandRemainder.Take(commandRemainder.Length - 1)).ToLower(); //takes everything except the last word to make the name of item
                        string sortingCategory = char.ToUpper(commandRemainder.Last()[0]) + commandRemainder.Last().Substring(1).ToLower(); //takes last word to use as category
                        if (sortingCategory == "Miljokasse")
                        {
                            sortingCategory = "Miljøkasse";
                        }

                        if (Enum.TryParse(sortingCategory, out Trash.TrashType trashType)) //sees if cateogry exists
                        {
                             Trash? item = inventory.items.FirstOrDefault(i => i.Name.ToLower() == itemName); //finds item in inventory
                            if (item == null)
                            {
                                Console.WriteLine("Item \x1b[93mnot found in inventory\x1b[39m, try again. (Syntax: sort <item name> <sorting category>)");
                                break;
                            }
                            if (inventory.sortItem(item, trashType))
                            {
                                Console.WriteLine($"Sorted \x1b[93m{item.Name}\x1b[39m as \x1b[93m{trashType}\x1b[39m");
                                trashSortedToday += 1;
                                if (trashSpawnedOnDay[(Days)currentDay] == trashSortedToday)
                                {   
                                    trashSortedToday = 0;
                                    currentDay += 1;
                                    Console.WriteLine("\x1b[93mYou have sorted all the trash for today!\x1b[39m You are now going home to return back tomorrow.");
                                    Console.WriteLine("");
                                    Console.WriteLine("You have returned to the university. \x1b[93mContinue on your quest to sort trash!\x1b[39m");
                                }
                            }
                            else // I dont think this will ever happen, but let's leave it for now
                            {
                                Console.WriteLine("Couldn't remove from inventory.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid sorting category: \x1b[93m{sortingCategory}\x1b[39m");
                        }
                        break;

                    default:
                        Console.WriteLine("I don't know that command.");
                        break;
                }
                Console.WriteLine(); //Adding an empty Line at the end of each action output
            }

            Console.WriteLine("Thank you for playing World of Zuul: Waste Warriors!");
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
            Console.WriteLine();
            Console.WriteLine("Welcome to the World of Zuul: \x1b[93mWaste Warriors\x1b[39m!");
            Console.WriteLine("World of Zuul: Waste Warriors is a new, incredibly trashy adventure game.");
            PrintHelp();
            Console.WriteLine();
        }
        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around the university.");
            Console.WriteLine();
            Console.WriteLine("Navigate by typing '\x1b[93mnorth\x1b[39m', '\x1b[93msouth\x1b[39m', '\x1b[93meast\x1b[39m', or '\x1b[93mwest\x1b[39m'.");
            Console.WriteLine("Type '\x1b[93mlook\x1b[39m' for more details about the room.");
            Console.WriteLine("Type '\x1b[93mback\x1b[39m' to go to the previous room.");
            Console.WriteLine("Type '\x1b[93mcollect <trash name>\x1b[39m' to collect trash within the room");
            Console.WriteLine("Type '\x1b[93minventory\x1b[39m' to look at the trash you are hoarding");
            Console.WriteLine("Type '\x1b[93msort <trash name> <sorting category>\x1b[39m' to sort the trash into trash cans");
            Console.WriteLine("Type '\x1b[93mhelp\x1b[39m' to print this message again.");
            Console.WriteLine("Type '\x1b[93mquit\x1b[39m' to exit the game.");
        }
    }
}
