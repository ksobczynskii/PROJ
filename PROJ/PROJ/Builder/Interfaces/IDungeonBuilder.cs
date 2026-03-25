namespace PROJ.Builder;
public interface IDungeonBuilder
{
    void CreateEmptyDungeon();
    void CreateFilledDungeon();
    void AddCorridors();
    void AddRooms(int count);
    void AddCentralHall(int width, int height);
    void AddItems(int count);
    void AddWeapons(int count);
    Tile[,] GetDungeon();

}