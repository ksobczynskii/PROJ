namespace PROJ.Goods.Classes.Valuables;

public class Coin : Valuable
{
    public override int Value => 1;
    public override char Visual => '¢';
    public override string Name => "Coin";
    public override string Description => "A worn coin from the plague-stricken port.";
    
    public override void PickUp(Player player) // TODO update wealthBox
    {
        player.coins++;
        if(ObjBoard != null)
            ObjBoard.RemoveFromMap(X,Y);
        player.UpdateWealth();
    }
}