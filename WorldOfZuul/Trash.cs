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

    public Trash(string name = "", TrashType type = TrashType.Undefined)
    {
        this.Name = name;
        this.Type = type;
    }
}