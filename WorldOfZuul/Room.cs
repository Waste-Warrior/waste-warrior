namespace WorldOfZuul
{
    public class Room
    {
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set;}
        public Dictionary<string, Room> Exits { get; private set; } = new();
        public Trash[]? ScatteredTrash;

        public Room(string shortDesc, string longDesc, Trash[]? trash = null)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
            ScatteredTrash = trash ?? Array.Empty<Trash>();
        }

        public void SetExits(Room? north, Room? east, Room? south, Room? west)
        {
            SetExit("north", north);
            SetExit("east", east);
            SetExit("south", south);
            SetExit("west", west);
        }

        public void SetExit(string direction, Room? neighbor)
        {
            if (neighbor != null)
                Exits[direction] = neighbor;
        }
        
        //This method removes one element from the scatteredTrash array and returns it
        public Trash? CollectTrash(int index = 0)
        {
            Trash? pickedTrash = null;
            if (ScatteredTrash is not { Length: > 0 })
            {
                return pickedTrash;
            }
            
            Trash[] newTrashArray = new Trash[ScatteredTrash.Length - 1];
            
            for (int i = 0; i < ScatteredTrash.Length; i++)
            {
                if (i != index)
                {
                    newTrashArray[i] = ScatteredTrash[i];
                }
                else
                {
                    pickedTrash = ScatteredTrash[i];
                }
            }

            ScatteredTrash = newTrashArray;
            return pickedTrash;
        }
    }
}
