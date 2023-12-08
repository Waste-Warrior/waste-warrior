namespace WorldOfZuul;
public class Trash
{
    public enum TrashType 
    {
        //We may want to make all types english named
        Metal,
        Glas,
        PapirOgPap,
        Madaffald,
        Plast,
        Madkartoner,
        Restaffald,
        Miljøkasse, //We may want to rename this to Farligtaffald
        Tekstilaffald,
    }
    
    public readonly string Name;
    public readonly TrashType Type; //This will later be used to add the trash sorting feature
    public readonly Game.Days Day; // Add a Day property to the Trash class

    public Trash(string name, TrashType type, Game.Days day)
    {
        this.Name = name;
        this.Type = type;
        this.Day = day;
    }
}