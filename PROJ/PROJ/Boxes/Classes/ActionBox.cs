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
    
    public void AfterMoveAsessment(BoardObject? boardObject) //TODO funkcja bedzie rozwinieta o mur
    {
        DisplayAction(boardObject);
    }

    public void DisplayAction(BoardObject? boardObject)
    {
        if (boardObject == null)
        {
            ClearInside();
            return;
        }
        // Console.WriteLine();
        // boardObject.Description
        // TODO Logika gdy za dlugi description
        string nameStr = "You're standing on: ";
        nameStr += boardObject.Name;
        
        Console.SetCursorPosition((GameConstants.ActionBoxLeft + GameConstants.ActionBoxRight - nameStr.Length)/2, GameConstants.ActionBoxWritingPointName);
        Console.Write(nameStr);
        
        Console.SetCursorPosition((GameConstants.ActionBoxLeft + GameConstants.ActionBoxRight - boardObject.Description.Length)/2, GameConstants.ActionBoxWritingPointDesc);
        Console.Write(boardObject.Description);
        if (boardObject.Pickupable)
        {
            string pickupStr = "Press 'E' to pick it up";
            Console.SetCursorPosition((GameConstants.ActionBoxLeft + GameConstants.ActionBoxRight - pickupStr.Length)/2, GameConstants.ActionBoxWritingPointPickup);
            Console.Write(pickupStr);
        }
        
        
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