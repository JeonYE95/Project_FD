
public static class Defines
{

    public enum UnitGrade
    {
        common,
        rare,
        Unique,
        Legendary
    }

    public enum UnitEnforceCost
    {
        common = 50,
        rare = 25,
        Unique = 10,
        Legendary

    }



    public const int CHARACTER_MAXQUANTITY = 99;
    public const int CHARACTER_INVENTORY_SIZE = 32;
    public const int MAX_ENERGY = 100; // 최대 에너지 값 
    public const float ENERGY_RECOVERY_TIME = 180f; // 에너지 충전 시간 3분


    public const int CHARACTER_ENFORCE_MAX_LEVEL = 100; // 유닛 강화 최대 레벨 지정
    public const int CLASS_ENFORCE_MAX_LEVEL = 100; // 클래스 강화 최대 레벨 지정
    public const int CLASS_ENFORCE_COST = 100; // 기본 클래스 강화 비용 +  레벨 * 10 


    public static readonly int BehindSortingOrder = 199;
    public static readonly int EnemySortingOrder = 200;
    public static readonly int PlayerSortingOrder = 201;



}
