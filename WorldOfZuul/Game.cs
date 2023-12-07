﻿namespace WorldOfZuul
{
    public class Game
    {
        private Room? currentRoom;
        private Room? previousRoom;
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

        private bool ClearConsole(bool CanClear = true, bool returnValue = true) // This is used to clear console, and the bool is required so that the console doesn't clear when it doesn't need to.
        {
            if (CanClear)
            {
                Console.Clear();
            }
            return returnValue;
        }
        private void printMessage(Room currentRoom)
        {
            // This solves the possibility of a null reference exception
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
                Console.WriteLine($"To advance to the next day, you still have \x1b[93m{trashSpawnedOnDay[(Days)currentDay] - trashSortedToday}\x1b[39m trash to get through."); //maybe make it so it says the rooms you should visit not the trash left? idk
            }
            if (currentRoom.dayDescription.ContainsKey((Days)currentDay))
            {
                Console.WriteLine($"\n{currentRoom.dayDescription[(Days)currentDay]}");
            }
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
                new ("a crushed light bulb", Trash.TrashType.Miljøkasse, 0),
                new ("a wine bottle", Trash.TrashType.Glas, 0),
                new ("a plastic bag", Trash.TrashType.Plast, 0),
                new ("an orange juice carton", Trash.TrashType.Madkartoner, 0),
                new ("a paper cup", Trash.TrashType.Restaffald, 0),
                new ("a torn apart T-shirt", Trash.TrashType.Tekstilaffald, 0),
                new ("a paper bag", Trash.TrashType.PapirOgPap, 0),
                new ("a towel", Trash.TrashType.Tekstilaffald, 0),
                new ("a broken pan", Trash.TrashType.Metal, 0),
                new ("a bent fork", Trash.TrashType.Metal, 0),
                new ("a graffiti can", Trash.TrashType.Miljøkasse, 0),
                new ("a bucket of paint", Trash.TrashType.Miljøkasse, 0),
                new ("a carrot", Trash.TrashType.Madaffald, 0),
                new ("a sandwich", Trash.TrashType.Madaffald, 0),
                new ("a bone", Trash.TrashType.Madaffald, 0),
                new ("some gift wrapping", Trash.TrashType.Restaffald, 0),
                new ("a sliced carpet", Trash.TrashType.Tekstilaffald, 0),
                new ("a cloth", Trash.TrashType.Tekstilaffald, 0),
                new ("a glass container", Trash.TrashType.Glas, 0),
                new ("pile of glass shards", Trash.TrashType.Glas, 0),
                new ("a crushed newspaper", Trash.TrashType.PapirOgPap, 0),
                new ("a plastic bottle", Trash.TrashType.Plast, 0),
                new ("a plastic wrapper", Trash.TrashType.Plast, 0)
            };
            
            //The trash arrays have to be set outside of the room declaration
            Trash[] outsideTrash =
            {
                new ("an empty beer can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty cola can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty beer can", Trash.TrashType.Metal, Days.Tuesday),
                new ("a wine bottle", Trash.TrashType.Metal, Days.Tuesday),
                new ("a plastic bag", Trash.TrashType.Plast, Days.Tuesday),
                new ("an empty beer can", Trash.TrashType.Metal, Days.Wednesday),
                new ("an empty Cola can", Trash.TrashType.Metal, Days.Wednesday),
                new ("pile of glass shards", Trash.TrashType.Glas, Days.Wednesday),
                new ("a graffiti can", Trash.TrashType.Miljøkasse, Days.Thursday),
                new ("a bucket of paint", Trash.TrashType.Miljøkasse, Days.Thursday)
            };
            
            Trash[] lobbyTrash =
            {
                new ("an empty cola can", Trash.TrashType.Metal, Days.Monday),
                new ("a crushed newspaper", Trash.TrashType.PapirOgPap, Days.Tuesday),
                new ("a glass container", Trash.TrashType.Glas, Days.Tuesday),
                new ("an old magazine", Trash.TrashType.PapirOgPap, Days.Wednesday),
                new ("a plastic wrapper", Trash.TrashType.Plast, Days.Wednesday),
                new ("a crushed light bulb", Trash.TrashType.Miljøkasse, Days.Thursday),
                new ("a cloth", Trash.TrashType.Tekstilaffald, Days.Thursday)
            };

            Trash[] u101Trash =
            {
                new ("a paper cup", Trash.TrashType.Restaffald, Days.Tuesday),
                new ("a plastic wrapper", Trash.TrashType.Plast, Days.Tuesday),
                new ("a paper cup", Trash.TrashType.Restaffald, Days.Wednesday),
                new ("a plastic bottle", Trash.TrashType.Plast, Days.Wednesday),
                new ("a tissue", Trash.TrashType.Restaffald, Days.Thursday),
                new ("a paper cup", Trash.TrashType.Restaffald, Days.Thursday)
            };

            Trash[] concerthallTrash =
            {
                new ("an empty cola can", Trash.TrashType.Metal, Days.Monday),
                new ("an empty cola can", Trash.TrashType.Metal, Days.Monday),
                new ("a paper bag", Trash.TrashType.PapirOgPap, Days.Tuesday),
                new ("a plastic bottle", Trash.TrashType.Plast, Days.Tuesday),
                new ("a pizza box", Trash.TrashType.Restaffald, Days.Wednesday),
                new ("a chips bag", Trash.TrashType.Restaffald, Days.Wednesday),
                new ("a battery", Trash.TrashType.Miljøkasse, Days.Thursday),
                new ("some gift wrapping", Trash.TrashType.Restaffald, Days.Thursday)
            };
            
            Room outside = new("outside", "You are standing outside the main entrance of the university. The only way to clean the university is to clean it outside and inside, right? Type 'forward' if you want to enter the university", outsideTrash);
            Room lobby = new("in the lobby", "You find yourself inside a large lobby with reception and an elevator. Several corridors going everywhere. What path will you choose. It's quite dark and quiet.", lobbyTrash);
            Room u101 = new("in U101", "You've entered the big lecture hall. It's a cozy place, where every student here has at least one lecture. There's a couple of people staying here, using the projector to watch movies. Perhaps you can stay wit them too", u101Trash);
            Room concertHall = new("at theConcert Hall", "You're in the Alsion Concert Hall. Seats fill the room as you think that everyone could come here and the hall wont even be full. It is chilly, but bearable", concerthallTrash);
            Room cafeteria = new("at the Cafeteria", "You have entered the school cafeteria. A place known to be full during lunch, but almost empty during other times. It is also known to have much more trash here, so I would look for it and clean it, if I was you :D.");
            Room u108 = new("in u108", "You've entered the lecture hall u108. Professors concerned about the environment hold lectures here, where they share a few tips everyday about sorting waste correctly.");
            Room u106 = new("in U106", "You've entered the second biggest lecture hall on this floor. It is always busy and full of people here. You find it empty, so why don't you snatch the opportunity and take a look around.");
            Room u201 = new("in U201", "Ah... Room U201. A room used for only one thing. Learning Danish. This is the place where one of the two Danish teaching structures is based - A2B. Look around and Held og Lykke!");
            Room u203 = new("in U203", "At last, A2B's rivals in room U203 - UCplus. A small room used so students can learn danish in the second danish teaching structure in campus. It is small, but practical. Oh, I almost forgot... The teacher brings pies. PIES.");
            
            outside.setDayDescriptions(
                "Greetings, Warrior! As it is your first day as a trash warrior, you should learn to sort the first categories of trash today.\nHead on to the Lobby (by typing 'Forward') for the introduction on how to sort trash! Also remember to go u108 and the concert hall everyday to learn why your trash sorting efforts are important. \nIf at any point you forget how to play, just type in '\x1b[93mhelp\x1b[39m' and you will be reminded of the controls.",
                "Congratulations on completing the first day of your training and welcome back! Today you will learn to sort the next categories of trash. Head on to the Lobby (by typing 'Forward') for the introduction!",
                "Welcome back, we are half way through the introduction, hopefully you will enjoy the remaining time as well.",
                "Today is the last day, stick through this to become a perfect recycler.",
                "By now you should already know to pick up trash on your way to the Lobby, no? After that, head on to the Lobby (by typing 'Forward')"
            );
            lobby.setDayDescriptions(
                "Welcome, student! In Denmark, we sort trash by the materials it is made of! Simple, right?\nThe first sorting category is \x1b[93mMetal\x1b[39m. Try to (type) \"collect an empty cola can\" and afterwards sort it into the \x1b[93mMetal\x1b[39m trash can (by typing \"sort an empty cola can metal\")! When you have collected all the trash in the university the next day will begin. !\nMetal trash example: an empty beer can",
                "Welcome back! As it is your second day, you should be ready to be thrown in the wild!\nNow, lets make sorting a little bit harder. As of Today there are 3 more trash categories: \x1b[93mGlas\x1b[39m (glass), \x1b[93mPapirOgPap\x1b[39m (paper and carton) and \x1b[93mPlast\x1b[39m (plastic).\nExamples:\nGlas: a wine bottle\nPapirOgPap: an old magazine\nPlast: a plastic bag",
                "Oh, you think it is easy? \nToday’s new  trash categories are: \x1b[93mMadaffald\x1b[39m (Food waste), \x1b[93mRestaffald\x1b[39m (residual waste) and \x1b[93mMadkartoner\x1b[39m (Food-Cartons).\nMake sure to bag your \x1b[93mRestaffald\x1b[39m and \x1b[93mMadaffald\x1b[39m before throwing it out at home. Don’t worry though, you don’t have to do that in-game.\nExamples:\nMadaffald: a rotten apple\nRestaffald: a pizza box\nMadkartonner: an orange juice carton",
                "Congratulations on reaching the last training day!\nThe last categories are \x1b[93mMiljøkasse\x1b[39m and \x1b[93mTekstilaffald\x1b[39m.\nBe careful when sorting trash that belongs to the Miljøkasse – as its hazardous!\nAll trash that is made of fabric belongs to the Tekstilaffald \nExample of Miljøkasse trash: a battery \nTekstilaffald: a towel"
            );
            u108.setDayDescriptions(
                "Even if you've sorted your waste correctly, it cannot be recycled if it is dirty. For example, wet and dirty paper cannot be recycled and is thus designated as residual waste. Your waste does not have to be spotless before you throw it away, but it must be free from food residue or liquids so it is ready to be recycled. A quick rinse, shake or wipe-down of the waste is fine.\nMetal is resorted and recycled, depending on the type of metal. Old cans, for example, can be recycled into new computer parts. Plastic and metal bins are generally emptied once every 4 weeks.",
                "Glas - glass and glass bottles not covered by the Danish deposit system can be brought to the recycling station or disposed of in the containers for glass recycling.\nPapir og pap - paper and cardboard also includes printed paper and cardboard. Cardboard must be no bigger than the size of a shoebox, and must be able to fall freely out of the bin when emptied. Bigger pieces of cardboard must be delivered to the recycling station. Paper and cardboard must be clean and dry. Wet and dirty paper and cardboard must be treated as residual waste and cannot be recycled.\nPlast - plastic is resorted into different types of plastic and subsequently recycled into anything from a new backpack to a green milk crate.",
                "Madaffald - Bio or food waste is the organic part of the waste. Food waste should be disposed of in bags, preferably tied with a knot to reduce odors in the bin and problems with flies. Food waste is sent for recycling at a Biogas plant and turned into energy. The food waste bins are generally emptied every 14 days.\nRestaffald - residual waste is the waste that is left after you have sorted everything else for recycling apart from hazardous waste. Residual waste is converted into heating and electricity at an incineration plant.\nFood scraps and yard waste can also turn into healthy soil in a compost bin. It's like nature's recycling program.\nMadkartoner – food and drink cartons, most of the food containers used are part of this category, while many of them are made of carton most of them have a plastic layer, which is why they are sorted separately. Most of the cartons can also be sorted as plastic.",
                "Miljøkasse - hazardous waste is all waste that could be hazardous to health or harmful for the environment. To the extent possible, all hazardous waste must be disposed of in its original packaging or properly packaged in some way. Hazardous waste that is collected and treated correctly is not harmful to the environment nor people. It is sent for special treatment and recycled to the greatest extent possible."
            );
            concertHall.setDayDescriptions(
                //The word gyres might need an explanation
                "The brightly lit projector screen is displaying a slide titled \"Environmental Impacts of Improperly Handled Trash\":\n\nTrash not collected and recycled can travel throughout rivers and oceans accumulating in beaches and creating trash islands in the ocean, when trapped within gyres. Hopefully this incentivized you to spend a few seconds to sort your trash and save the beauty of our planet.\nDid you know that Denmark's impressive recycling efforts save considerable energy? For example, recycling in Denmark has been associated with a notable reduction in energy consumption, with each aluminum can recycled saving up to 95% of the energy required for its original production.",
                "The brightly lit projector screen is displaying a slide titled \"Environmental Impacts of Plastic Waste\":\n\nOut of all trash, plastic trash has the greatest potential to harm the environment, wildlife and humans. It can be found floating at the surface, suspended in the water column, or on the bottom of almost all water bodies. It is transported by rivers to the ocean, where it moves with the currents, and is often eaten by birds and fish, concentrating toxic chemicals in their tissues, and filling their stomachs, causing them to starve. Plastic aquatic debris is much more than a mere aesthetic problem.",
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

            foreach (Trash item in outsideTrash.Concat(lobbyTrash).Concat(u101Trash).Concat(concerthallTrash)) // add .Concat(nextTrashName) to add more trash items for score comparison.
            {
                trashSpawnedOnDay[item.Day] += 1;
            }
            //Now each day has a score of how many trash items are on the floor. Eg. trashSpawnedOnDay[Days.Monday]

        }
        public void Play()
        {
            Parser parser = new();
            
            bool canClear = false; // This is used to determine if the console should be cleared or not.
            PrintWelcome();

            bool continuePlaying = true;
            while (continuePlaying)
            {
                canClear = ClearConsole(canClear);
                Console.WriteLine($"You are now \x1b[93m{currentRoom?.ShortDescription}\x1b[39m");
                if (currentRoom != null)
                {
                    printMessage(currentRoom);
                }

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
                        canClear = ClearConsole(canClear, false);
                        PrintHelp();
                        break;

                    case "collect":
                        List<Trash> TrashList; // This is the list of trash in the room
                        bool exit = false; // This is one of the exit conditions for the first while loop, used if player wants to go back.
                        while (!exit && (TrashList = currentRoom?.GetItems(currentDay) ?? new List<Trash>()).Count > 0) // first checks if the player wants to exit, second condition is that there are trash items in room.
                        {
                            canClear = ClearConsole(canClear); // if canClear, then clears, if not, then canClear turns true.
                            Console.WriteLine("\nYou can collect and sort the following trash:\n");
                            int i = 0; foreach (Trash trash in TrashList)
                            {
                                i += 1; Console.WriteLine($"{i}    *\x1b[93m{trash.Name}\x1b[39m"); // prints out the trash items in the room with a number (that corresponds to index + 1) in front of them.
                            }
                            Console.WriteLine("\nType in the number of the trash you want to collect and sort or type '\x1b[93m0\x1b[39m' to go back.");
                            Console.Write("> ");
                            string? input2 = Console.ReadLine();
                            if (!int.TryParse(input2, out int trashIndex) || trashIndex > TrashList.Count || trashIndex < 0 || string.IsNullOrEmpty(input2)) // checks if the input is a number and if it is within the range of the trash items in the room.
                            {
                                Console.Clear();
                                Console.WriteLine("Please enter a \x1b[93mvalid\x1b[39m number.");
                                canClear = false;
                            }
                            else
                            {
                                if (trashIndex == 0)
                                {
                                    exit = true;
                                }
                                else
                                {
                                    trashIndex -= 1; //because in programming counting starts with 0, while in life 1.
                                    Trash trash = TrashList[trashIndex];
                                    canClear = ClearConsole(canClear);
                                    Console.WriteLine($"\nYou have selected: \x1b[93m{trash.Name}\x1b[39m");
                                    bool chosen = false; // to be able to quit the while loop without quitting the other while loop
                                    while (!chosen)
                                    {
                                        Console.WriteLine("\nSelect one of the following trash cans to sort the trash into:\n");
                                        int trashTypeIndex = 0;
                                        foreach (Trash.TrashType trashType in Enum.GetValues(typeof(Trash.TrashType)))
                                        {
                                            trashTypeIndex += 1;
                                            Console.WriteLine($"{trashTypeIndex}    *\x1b[93m{trashType}\x1b[39m");
                                        }
                                        Console.WriteLine("\nType in the number of the trash category you want to sort it in or type '\x1b[93m0\x1b[39m' to go back.");
                                        Console.Write("> ");
                                        string? input3 = Console.ReadLine();
                                        if (string.IsNullOrEmpty(input3) || !int.TryParse(input3, out int trashTypeIndexChosen) || trashTypeIndexChosen > Enum.GetNames(typeof(Trash.TrashType)).Length || trashTypeIndexChosen < 0) // checks if the input is a number and if it is within the range of the categories.
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Please enter a valid number.");
                                            canClear = false;
                                        }
                                        else
                                        {
                                            if (trashTypeIndexChosen == 0) //chose to leave the while loop
                                            {
                                                exit = true;
                                                break;
                                            }
                                            else
                                            {
                                                if (trashTypeIndexChosen-1 != (int)trash.Type) //-1 because of the programming counting from 0 and the player counting from 1, because 0 is leave.
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("You have selected the \x1b[93mwrong\x1b[39m category!");
                                                    canClear = false;
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine($"\nYou have sorted \x1b[93m{trash.Name}\x1b[39m!");
                                                    canClear = false;
                                                    currentRoom?.RemoveTrash(trash.Name, (int)trash.Day);
                                                    trashSortedToday += 1;
                                                    chosen = true;
                                                    if (trashSpawnedOnDay[(Days)currentDay] == trashSortedToday) // checks if maybe today has ended?
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
                                                        exit = true;
                                                    }
                                                    if (TrashList.Count == 1)
                                                    {
                                                        Console.WriteLine("You have collected all the trash in the room!");
                                                        exit = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
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
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Welcome to the World of Zuul: \x1b[93mWaste Warriors\x1b[39m!");
            Console.WriteLine("World of Zuul: Waste Warriors is a new, incredibly trashy adventure game.");
            PrintHelp();
            Console.WriteLine();
        }
        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Navigate by typing '\x1b[93mforward\x1b[39m', '\x1b[93mbackwards\x1b[39m', '\x1b[93mright\x1b[39m', or '\x1b[93mleft\x1b[39m' and if you find an elevator, try going '\x1b[93mup\x1b[39m' or '\x1b[93mdown\x1b[39m'.");
            Console.WriteLine("Type '\x1b[93mback\x1b[39m' to go to the previous room.");
            Console.WriteLine("Type '\x1b[93mcollect\x1b[39m' to collect trash within the room");
            Console.WriteLine("Type '\x1b[93mhelp\x1b[39m' to print this message again.");
            Console.WriteLine("Type '\x1b[93mquit\x1b[39m' to exit the game.");
        }
    }
}
