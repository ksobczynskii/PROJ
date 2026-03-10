namespace PROJ;

public class Tile
{
    public BoardObject? Content;
    public bool BlocksMovement;

    public void Reset()
    {
        Content = null;
        BlocksMovement = false;
    }
    
}
    