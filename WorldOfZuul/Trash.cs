namespace WorldOfZuul;

public class Trash
{
    public enum TrashType
    {
        Undefined, //Undefined exists to be able to identify trash that was initialized incorrectly or is not supposed to exist.
        Metal
    }
    
    public readonly string Name;
    public readonly TrashType Type; //This will later be used to add the trash sorting feature
    public readonly Game.Days Day; // Add a Day property to the Trash class

    public Trash(string name = "", TrashType type = TrashType.Undefined, Game.Days day = Game.Days.Monday)
    {
        this.Name = name;
        this.Type = type;
        this.Day = day;
    }
}