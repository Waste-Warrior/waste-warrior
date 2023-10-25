namespace WorldOfZuul
{
    public class Inventory
    {
        public List<Trash> items = new List<Trash>();
        public void AddItem(Trash item)
        {
            items.Add(item);
        }
        public bool sortItem(Trash item, Trash.TrashType type)
        {
            if (items.Contains(item))
            {
                if (item.Type == type)
                {
                    items.Remove(item);
                    return true;
                }
            }
            return false;    
        }

    }
}