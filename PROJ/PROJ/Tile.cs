namespace PROJ;

public class Tile
{
    public List<BoardObject>? Content;
    public int Objects;
    public bool BlocksMovement;
    public bool IsEmpty;

    public Tile()
    {
        Content = null;
        Objects = 0;
        BlocksMovement = false;
        IsEmpty = true;
    }
    public void Reset()
    {
        Content = null;
        BlocksMovement = false;
        Content = new List<BoardObject>();
        Objects = 0;
        IsEmpty = true;
    }

    public void AddObj(BoardObject obj) // TODO logika - wall jako jedyny obj
    {

        if (obj.Blocker && Objects > 0)
            return;
        if (Content == null)
            Content = new List<BoardObject>();
        
        Content.Add(obj);
        Objects++;
        BlocksMovement = obj.Blocker;
        IsEmpty = false;
    }
    public void RemoveLast()
    {
        if (Content == null || Objects == 0)
            return;
        Content.RemoveAt(Objects-1);
        Objects--;
    }

    public void Remove(int i)
    {
        if (Content == null || Objects <= i)
            return;
        Content.RemoveAt(i);
        Objects--;
        if (Objects == 0)
            IsEmpty = true;
    }
    
}
    