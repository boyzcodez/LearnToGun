using System.Collections.Generic;

public static class XpHandler
{
    private static Dictionary<string, Gun> _weapons = new();
    public static void AddXP(string name, int xp)
    {
        if (_weapons.ContainsKey(name)) _weapons[name].AddXP(xp);
    }
    public static void AddGun(string newGun, Gun gun)
    {
        _weapons[newGun] = gun;
    }

}
