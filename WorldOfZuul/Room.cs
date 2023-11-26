using System.Diagnostics;

namespace WorldOfZuul
{
    public class Room
    {
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set;}
        public Dictionary<string, Room> Exits { get; private set; } = new();
        public Dictionary<Game.Days, string> dayDescription { get; private set; } = new();
        public Trash[]? ScatteredTrash;

        public Room(string shortDesc, string longDesc, Trash[]? trash = null)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
            ScatteredTrash = trash ?? Array.Empty<Trash>();
        }

        public void SetExits(Room? forward, Room? right, Room? backwards, Room? left)
        {
            SetExit("forward", forward);
            SetExit("right", right);
            SetExit("backwards", backwards);
            SetExit("left", left);
        }

        public void SetExit(string direction, Room? neighbor)
        {
            if (neighbor != null)
                Exits[direction] = neighbor;
        }
        
        public void setDayDescriptions(string? LongDesc0 = null, string? LongDesc1 = null, string? LongDesc2 = null, string? LongDesc3 = null, string? LongDesc4 = null, string? LongDesc5 = null, string? LongDesc6 = null)
        {
            setDayDescription(Game.Days.Monday, LongDesc0 ?? "");
            setDayDescription(Game.Days.Tuesday, LongDesc1 ?? "");
            setDayDescription(Game.Days.Wednesday, LongDesc2 ?? "");
            setDayDescription(Game.Days.Thursday, LongDesc3 ?? "");
            setDayDescription(Game.Days.Friday, LongDesc4 ?? "");
            setDayDescription(Game.Days.Saturday, LongDesc5 ?? "");
            setDayDescription(Game.Days.Sunday, LongDesc6 ?? "");
        }

        public void setDayDescription(Game.Days day, string LongDesc)
        {
            if (LongDesc != null)
            {
                dayDescription[day] = LongDesc;
            }
            else
            {
                dayDescription[day] = LongDescription;
            }
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
            int newArrayIndex = 0; //This is needed because an offset is created, when the item that to remove is found.
            
            for (int oldArrayIndex = 0; oldArrayIndex < ScatteredTrash.Length; oldArrayIndex++)
            {
                if (oldArrayIndex != index)
                {
                    newTrashArray[newArrayIndex] = ScatteredTrash[oldArrayIndex];
                    newArrayIndex += 1;
                }
                else
                {
                    pickedTrash = ScatteredTrash[oldArrayIndex];
                }
            }

            ScatteredTrash = newTrashArray;
            return pickedTrash;
        }

        public Trash? RemoveTrash(string trashName = "", int? trashDay = 0)
        {
            if (ScatteredTrash is not { Length: > 0 })
            {
                return null;
            }

            //This looks for the index of the trash within the ScatteredTrash array and then calls the RemoveTrash method that removes Trash by array Index
            for (int i = 0; i < ScatteredTrash.Length; i++)
            {
                if (ScatteredTrash[i].Name.ToLower() == trashName.ToLower())
                {
                    if (trashDay != null && ScatteredTrash[i].Day == (Game.Days) trashDay)
                    {
                        Debug.WriteLine(i);
                        return RemoveTrash(i);
                    }
                }
            }

            return null;
        }

        public bool IsTrashInRoom(string? trashName = null, int? trashDay = 0)
        {
            if (ScatteredTrash != null && ScatteredTrash.Length >= 1 && trashName != null)
            {
                foreach (Trash trash in ScatteredTrash)
                {
                    if (trashName.ToLower() == trash.Name.ToLower())
                    {
                        if (trashDay != null && trash.Day == (Game.Days) trashDay)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
