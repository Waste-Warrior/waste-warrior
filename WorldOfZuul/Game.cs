using System.Collections;

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
                new ("an empty beer can", Trash.TrashType.Metal, Days.Tuesday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Tuesday),
                new ("an empty beer can", Trash.TrashType.Metal, Days.Wednesday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Wednesday)
            };
            Trash[] lobbyTrash = {
                new ("an empty beer can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty beer can", Trash.TrashType.Metal, Days.Tuesday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Tuesday),
            };
            
            Room outside = new("Outside", "You are standing outside the main entrance of the university. The only way to clean the university is to clean it outside and inside, right? Type 'forward' if you want to enter the university", outsideTrash);
            Room lobby = new("Lobby", "You find yourself inside a large lobby with reception and an elevator. Several corridors going everywhere. What path will you choose. It's quite dark and quiet.", lobbyTrash);
            Room u101 = new("U101", "You've entered the big lecture hall. It's a cozy place, where every student here has at least one lecture. There's a couple of people staying here, using the projector to watch movies. Perhaps you can stay wit them too");
            Room concertHall = new("Concert Hall", "You're in the Alsion Concert Hall. Seats fill the room as you think that everyone could come here and the hall wont even be full. It is chilly, but bearable");
            Room cafeteria = new("Cafeteria", "You have entered the school cafeteria. A place known to be full during lunch, but almost empty during other times. It is also known to have much more trash here, so I would look for it and clean it, if I was you :D.");
            Room u108 = new("u108", "You've entered the lecture hall u108. Professors concerned about the environment hold lectures here, where they share a few tips everyday about sorting waste correctly.");
            Room u106 = new("U106", "You've entered the second biggest lecture hall on this floor. It is always busy and full of people here. You find it empty, so why don't you snatch the opportunity and take a look around.");
            Room u201 = new("U201", "Ah... Room U201. A room used for only one thing. Learning Danish. This is the place where one of the two Danish teaching structures is based - A2B. Look around and Held og Lykke!");
            Room u203 = new("U203", "At last, A2B's rivals in room U203 - UCplus. A small room used so students can learn danish in the second danish teaching structure in campus. It is small, but practical. Oh, I almost forgot... The teacher brings pies. PIES.");
            
            outside.setDayDescriptions(
                "Greetings, Warrior! As it is your first day as a trash warrior, you should learn to sort the first categories of trash today. Head on to the Lobby (by typing 'Forward') for the introduction!",
                "Congratulations on completing the first day of your training and welcome back! Today you will learn to sort the next categories of trash. Head on to the Lobby (by typing 'Forward') for the introduction!",
                "By now you should already know to pick up trash on your way to the Lobby, no? After that, head on to the Lobby (by typing 'Forward')"
            );
            lobby.setDayDescriptions(
                "Welcome, student! In Denmark, we sort trash by the materials it is made of! Simple, right?\nThe first sorting category is \x1b[93mMetal\x1b[39m. Try to sort trash into the \x1b[93mMetal\x1b[39m trash can! Collect and sort the trash in every room to move on to the next day!\nMetal trash example: an empty beer can",
                "Welcome back! As it is your second day, you should be ready to be thrown in the wild!\nNow, lets make sorting a little bit harder. As of Today there are 3 more trash categories: Glas, PapirOgPap and Plast.\nExamples:\nGlas: a wine bottle\nPapirOgPap: an old magazine\nPlast: a plastic bag",
                "Oh, you think it is easy? \nToday’s new  trash categories are: Madaffald, Restaffald and Madkartoner.\nMake sure to bag your Restaffald and Madaffald before throwing it out at home. Don’t worry though, you don’t have to do that in-game.\nExamples:\nMadaffald: a rotten apple\nRestaffald: a pizza box\nMadkartonner: an orange juice carton",
                "Congratulations on reaching the last training day!\nThe last category is Miljøkasse.\nBe careful with this type of waste – as its hazardous!\nExample of Miljøkasse trash: a battery"
            );
            u108.setDayDescriptions(
                "Even if you've sorted your waste correctly, it cannot be recycled if it is dirty. For example, wet and dirty paper cannot be recycled and is thus designated as residual waste. Your waste does not have to be spotless before you throw it away, but it must be free from food residue or liquids so it is ready to be recycled. A quick rinse, shake or wipe-down of the waste is fine.\nMetal is resorted and recycled, depending on the type of metal. Old cans, for example, can be recycled into new computer parts. Plastic and metal bins are generally emptied once every 4 weeks.",
                "Glas - glass and glass bottles not covered by the Danish deposit system can be brought to the recycling station or disposed of in the containers for glass recycling.\nPapir og pap - paper and cardboard also includes printed paper and cardboard. Cardboard must be no bigger than the size of a shoebox, and must be able to fall freely out of the bin when emptied. Bigger pieces of cardboard must be delivered to the recycling station. Paper and cardboard must be clean and dry. Wet and dirty paper and cardboard must be treated as residual waste and cannot be recycled.\nPlast - plastic is resorted into different types of plastic and subsequently recycled into anything from a new backpack to a green milk crate.",
                "Madaffald - Bio or food waste is the organic part of the waste. Food waste should be disposed of in bags, preferably tied with a knot to reduce odors in the bin and problems with flies. Food waste is sent for recycling at a Biogas plant and turned into energy. The food waste bins are generally emptied every 14 days.\nRestaffald - residual waste is the waste that is left after you have sorted everything else for recycling or hazardous waste. Residual waste is converted into heating and electricity at an incineration plant.\nFood scraps and yard waste can also turn into healthy soil in a compost bin. It's like nature's recycling program.\nMadkartoner – food and drink cartons.",
                "Miljøkasse - hazardous waste is all waste that could be hazardous to health or harmful for the environment. To the extent possible, all hazardous waste must be disposed of in its original packaging or properly packaged in some way. Hazardous waste that is collected and treated correctly is not harmful to the environment nor people. It is sent for special treatment and recycled to the greatest extent possible."
            );
            concertHall.setDayDescriptions(
                "The brightly lit projector screen is displaying a slide titled \"Environmental Impacts of Improperly Handled Trash\":\n\nTrash not collected and recycled can travel throughout rivers and oceans accumulating in beaches and event creating trash islands, when trapped within gyres. Do you really think saving a few seconds when not recycling is worth polluting our environment and ruining the beauty of our planet?\nDid you know that Denmark's impressive recycling efforts save considerable energy? For example, recycling in Denmark has been associated with a notable reduction in energy consumption, with each aluminum can recycled saving up to 95% of the energy required for its original production.",
                "The brightly lit projector screen is displaying a slide titled \"Environmental Impacts of PLastic Waste\":\n\nOut of all trash, plastic trash has the greatest potential to harm the environment, wildlife and humans. It can be found floating at the surface, suspended in the water column, or on the bottom of almost all water bodies. It is transported by rivers to the ocean, where it moves with the currents, and is often eaten by birds and fish, concentrating toxic chemicals in their tissues, and filling their stomachs, causing them to starve. Plastic aquatic debris is much more than a mere aesthetic problem.",
                "The brightly lit projector screen is displaying a slide titled \"Environmental Impacts of Residual Waste\":\n\nResidual waste usually ends up as landfill which generate and release Biogas. Furthermore, landfills emanate bad smells, contaminate soil and water.\nIn Denmark most of the Residual waste is incinerated and used to produce district heating and electricity.\nIt is really important to leave as little residual waste as possible and that can be done by thoroughly sorting your trash.\nDenmark's good recycling practices help safeguard precious natural resources. The country's commitment to recycling is synonymous with a reduced need for raw material extraction, benefiting ecosystems like forests and minimizing the depletion of minerals and fossil fuels.",
                "The brightly lit projector screen is displaying a slide titled \"Environmental Impacts of Hazardous Waste\":\n\nHazardous waste that is not handled appropriately and recycled possesses a serious harm to our health and the environment – it can cause fires, contaminate ground and surface waters."
            );

            outside.SetExit("forward", lobby); // Forward, Right, Backward, Left, Up, Down

            lobby.SetExits(cafeteria, u101, outside, concertHall, u201, null);

            u101.SetExits(u108, null, lobby, null, null, null);

            concertHall.SetExit("backwards", lobby);

            cafeteria.SetExit("backwards", lobby);

            u108.SetExit("backwards", u101);

            u106.SetExit("backwards", u101);

            u201.SetExits(u203, null, null, null, null, lobby);

            u203.SetExit("backwards", u201);

            currentRoom = outside;

            foreach (Trash item in outsideTrash.Concat(lobbyTrash)) // add .Concat(nextTrashName) to add more trash items for score comparison.
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

                    case "forward":
                    case "backwards":
                    case "right":
                    case "left":
                    case "up":
                    case "down":
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
                                    trashSortedToday = 0; // reset trash sorted today counter
                                    currentDay += 1; // increment day
                                    Console.WriteLine("\x1b[93mYou have sorted all the trash for today!\x1b[39m You are now going home to return back tomorrow.");
                                    Console.WriteLine("");
                                    while (currentRoom?.Exits.ContainsKey("backwards") == true) // returns you to the outside room after day ends.
                                    {
                                        previousRoom = currentRoom;
                                        currentRoom = currentRoom?.Exits["backwards"];
                                    }
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
            Console.WriteLine("Navigate by typing '\x1b[93mforward\x1b[39m', '\x1b[93mbackwards\x1b[39m', '\x1b[93mright\x1b[39m', or '\x1b[93mleft\x1b[39m' and if you find an elevator, try going '\x1b[93mup\x1b[39m' or '\x1b[93mdown\x1b[39m'");
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
