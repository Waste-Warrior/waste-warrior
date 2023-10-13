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
        
        //This method removes one element from the scatteredTrash array and returns it. This will be unnecessary when this method is only called from it's string counterpart.
        public Trash? RemoveTrash(int index = 0)
        {
            //checking if there is trash in this room. 
            Trash? pickedTrash = null;
            if (ScatteredTrash is not { Length: > 0 })
            {
                return null;
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

        public Trash? RemoveTrash(string trashName = "")
        {
            if (ScatteredTrash is not { Length: > 0 })
            {
                return null;
            }

            //This looks for the index of the trash within the ScatteredTrash array and then calls the RemoveTrash method that removes Trash by array Index
            for (int i = 0; i < ScatteredTrash.Length; i++)
            {
                if (ScatteredTrash[i].Name == trashName)
                {
                    return RemoveTrash(i);
                }
            }

            return null;
        }

        public bool IsTrashInRoom(string? trashName = null)
        {
            if (ScatteredTrash != null && ScatteredTrash.Length >= 1 && trashName != null)
            {
                foreach (Trash trash in ScatteredTrash)
                {
                    if (trashName == trash.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
