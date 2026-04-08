namespace PROJ.Builder;
public interface IDungeonBuilder
{
    void CreateEmptyDungeon();
    void CreateFilledDungeon();
    void AddCorridors(int count);
    void AddRooms(int count);
    void AddCentralHall(int width, int height);
    void AddItems(int count);
    void AddWeapons(int count);
    void AddEnemies(int count);
    Tile[,] GetDungeon();

}