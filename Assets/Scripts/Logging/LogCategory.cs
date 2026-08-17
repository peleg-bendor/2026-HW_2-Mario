// The groupings log lines are filtered by, one level above the class the Console already shows.
// Taken from where the calls actually are rather than invented, so every call site has an obvious
// home and no category ends up holding a single line.
public enum LogCategory
{
    Player,
    Enemy,
    Weapon,
    Projectile,
    Pickup,
    Tile,
    Game
}
