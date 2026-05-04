using PROJ.Builder.Classes;
using PROJ.Enemies;
using PROJ.Tools.Classes;
using PROJ.Tools.Classes.Items;
using PROJ.Tools.Classes.Weapons;

namespace PROJ.Themes;

public class HospitalThemeFactory : IDungeonThemeFactory
{
    public Func<Enemy>[] CreateEnemyPool()
    {
        return new Func<Enemy>[]
        {
            () => new SmallEnemy(name: "Nurse", vis:'✚'),
            () => new MediumEnemy(name: "Doctor", vis:'⚕'),
            () => new BigEnemy(name: "Priest", vis: '☬')
        };
    }

    public Func<Item>[] CreateItemPool(Player p)
    {
        return new Func<Item>[]
        {
            () => new MediumItem(p, name: "Stethoscope", vis:'∿' ),
            () => new CoolItem(p, name:"Surgical Gloves", vis:'≋' ),
            () => new SmallItem(p,"Triage Tag", vis:'⌑' )
        };
    }

    public Func<Weapon>[] CreateWeaponPool(Player p)
    {
        return new Func<Weapon>[]
        {
            () => new SmallWeapon(p, name:"Drug", vis:'※'),
            () => new TwoHandedWeapon(p, name:"Hospital Trolley", vis: '▣'),
            () => new MediumWeapon(p, name: "Syringe", vis: '¡')
        };
    }

    public void Build(DungeonBuilder db, Player p)
    {
        db.CreateEmptyDungeon();
        db.AddItems(5, CreateItemPool(p)); // TODO kolorki zmien jak motyw
        db.AddWeapons(10, CreateWeaponPool(p));
        db.AddArtifact(new SmallItem(p,"Unfound Triage Tag", vis:'*' ));
        db.AddEnemies(30, CreateEnemyPool());
        db.AddCentralHall(10,20);
        db.AddRooms(4);
        db.AddCorridors(10);
    }

    public string Name => "Sea Theme";
    public string IntroMessage => "Welcome to the depths of the sea!";
}