namespace PROJ.Goods.Classes.Valuables;

public class Gold : Valuable
{
    public override int Value => 2;
    public override char Visual => '✶';
    public override string Name => "Gold";
    public override string Description => "A precious piece of gold hidden from the dying";

    public override void PickUp(Player player) // TODO update wealthBox
    {
        player.gold++;
        if(ObjBoard != null)
            ObjBoard.RemoveFromMap(X,Y);
        player.UpdateWealth();
    }
}