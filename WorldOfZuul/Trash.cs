namespace WorldOfZuul;

public class Trash
{
    public enum TrashType
    {
        Undefined, //Undefined exists to be able to identify trash that was initialized incorrectly or is not supposed to exist.
        Metal
    }
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
    
    public readonly string Name;
    public readonly TrashType Type; //This will later be used to add the trash sorting feature

    public readonly Days Day; // This will be used to determine when the trash can be picked up and shows up in the room

    public Trash(string name = "", TrashType type = TrashType.Undefined, Days day = Days.Monday)
    {
        this.Name = name;
        this.Type = type;
        this.Day = day;
    }
}