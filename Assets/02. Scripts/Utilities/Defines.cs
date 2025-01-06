
using System.Linq;

public static class Defines
{

    public enum UnitGrade
    {
        common,
        rare,
        Unique,
        Legendary
    }



    public const int CHARACTER_MAXQUANTITY = 99;
    public const int CHARACTER_INVENTORY_SIZE = 32;
    public const int MAX_ENERGY = 100; // 최대 에너지 값 
    public const float ENERGY_RECOVERY_TIME = 180f; // 에너지 충전 시간 3분

    public static readonly float RANGEADJUST = 0.1f;

    public static readonly int BehindSortingOrder = 199;
    public static readonly int EnemySortingOrder = 200;
    public static readonly int PlayerSortingOrder = 201;

    public static readonly string DefaultProejectileTag = "DefaultProjectile";

    public static readonly string[] MeleeUnitID = { "1001", "1004", "1006" , "1051", "1054", "1056", 
    "1101", "1102", "1107", "1108", "1111", "1112"};
    public static readonly string[] ArrowUnitID = { "1002", "1052", "1103", "1104" };
    public static readonly string[] MagicUnitID = { "1003", "1005", "1053", "1055", "1105", "1106", 
        "1109", "1110" };
}

public static class UnitChecker
{
    public static string GetUnitType(int unitID)
    {
        if (Defines.MeleeUnitID.Contains(unitID.ToString()))
            return "Melee";
        if (Defines.ArrowUnitID.Contains(unitID.ToString()))
            return "Arrow";
        if (Defines.MagicUnitID.Contains(unitID.ToString()))
            return "Magic";

        return "Unknown";
    }
}

