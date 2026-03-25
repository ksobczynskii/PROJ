using PROJ.Boxes;
using PROJ.GameConstansts;

namespace PROJ;

public class ActionBox : Box
{
    private Board _board;
    private Player _player;

    

    public override void DisplayFrame()
    {
        // ┌ ┐ └ ┘ ─ │
        Console.SetCursorPosition(GameConstants.ActionBoxLeft, GameConstants.ActionBoxTop);
        Console.Write('┌');
        Console.SetCursorPosition(GameConstants.ActionBoxRight, GameConstants.ActionBoxTop);
        Console.Write('┐');
        Console.SetCursorPosition(GameConstants.ActionBoxLeft, GameConstants.ActionBoxBottom);
        Console.Write('└');
        Console.SetCursorPosition(GameConstants.ActionBoxRight, GameConstants.ActionBoxBottom);
        Console.Write('┘');

        for (int i = 1; i < GameConstants.ActionBoxBottom - GameConstants.ActionBoxTop; i++)
        {
            Console.SetCursorPosition(GameConstants.ActionBoxLeft, GameConstants.ActionBoxTop + i);
            Console.Write('│');
            Console.SetCursorPosition(GameConstants.ActionBoxRight, GameConstants.ActionBoxTop + i);
            Console.Write('│');
        }
        for (int i = 1; i < GameConstants.ActionBoxRight - GameConstants.ActionBoxLeft ; i++)
        {
            Console.SetCursorPosition(GameConstants.ActionBoxLeft + i, GameConstants.ActionBoxTop);
            Console.Write('─');
            Console.SetCursorPosition(GameConstants.ActionBoxLeft + i, GameConstants.ActionBoxBottom);
            Console.Write('─');
        }

        string tmp = " Action ";
        Console.SetCursorPosition((GameConstants.ActionBoxRight + GameConstants.ActionBoxLeft) / 2 - tmp.Length/2, GameConstants.ActionBoxTop);
        Console.Write(tmp);
    }
    
    public void AfterMoveAsessment(List<BoardObject>? objects, int count, int seek = 0) //TODO funkcja bedzie rozwinieta o mur
    {
        if (count == 0 || objects == null)
        {
            ClearInside();
            return;
        }
            
        DisplayAction(objects, count-1,seek);
    }
    public void DisplaySpecificObject(List<BoardObject> objects, bool Last, bool First, int i)
    {
        ClearInside(); // TODO needed???
        string nameStr = "You're standing on: ";
        nameStr += objects[i].Name;
        
        Console.SetCursorPosition((GameConstants.ActionBoxLeft + GameConstants.ActionBoxRight - nameStr.Length)/2, GameConstants.ActionBoxWritingPointName);
        Console.Write(nameStr);
        
        Console.SetCursorPosition((GameConstants.ActionBoxLeft + GameConstants.ActionBoxRight - objects[i].Description.Length)/2, GameConstants.ActionBoxWritingPointDesc);
        Console.Write(objects[i].Description);
        if (objects[i].Pickupable)
        {
            string pickupStr = "Press 'E' to pick it up";
            Console.SetCursorPosition((GameConstants.ActionBoxLeft + GameConstants.ActionBoxRight - pickupStr.Length)/2, GameConstants.ActionBoxWritingPointPickup);
            Console.Write(pickupStr);
        }

        if (!First)
        {
            Console.SetCursorPosition(GameConstants.ActionBoxLeft + 6, GameConstants.ActionBoxWritingPointName);
            Console.Write("<---");
        }

        if (!Last)
        {
            Console.SetCursorPosition(GameConstants.ActionBoxRight - 10, GameConstants.ActionBoxWritingPointName);
            Console.Write("--->");
        }
    }
    
    public void DisplayAction(List<BoardObject> objects, int maxIdx, int seek)
    {
        
        // Console.WriteLine();
        // boardObject.Description
        // TODO Logika gdy za dlugi description
        DisplaySpecificObject(objects,seek == maxIdx ,seek == 0 ,seek);
    }
    
    

    private void ClearInside()
    {
        int innerWidth = GameConstants.ActionBoxRight - GameConstants.ActionBoxLeft - 1;

        for (int y = GameConstants.ActionBoxTop + 1; y < GameConstants.ActionBoxBottom; y++)
        {
            Console.SetCursorPosition(GameConstants.ActionBoxLeft + 1, y);
            Console.Write(new string(' ', innerWidth));
        }
    }
    
}