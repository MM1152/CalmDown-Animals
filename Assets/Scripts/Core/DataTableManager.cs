using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables
        = new Dictionary<string, DataTable>();

    public static StringTable stringTable => Get<StringTable>(DataTableIds.StringTableIds);
    public static RoundTable roundTable => Get<RoundTable>(DataTableIds.RoundTableIds);
    public static AnimalInfoTable animalInfoTable => Get<AnimalInfoTable>(DataTableIds.AnimalInfoTable);
    public static AnimalSizeTable animalSizeTable => Get<AnimalSizeTable>(DataTableIds.AnimalSizeTable);
    public static AnimalSpeedTable animalSpeedTable => Get<AnimalSpeedTable>(DataTableIds.AnimalSpeedTable);
    public static AnimalCRRank animalCRRankTable => Get<AnimalCRRank>(DataTableIds.AnimalCRRankTable);
    public static CrewTable crewTable => Get<CrewTable>(DataTableIds.CrewTable);

    static DataTableManager()
    {
        Init();
    }

    private static void Init()
    {
        StringTable stringTable = new StringTable();
        stringTable.Load(DataTableIds.StringTableIds);

        RoundTable roundTable = new RoundTable();
        roundTable.Load(DataTableIds.RoundTableIds);
        
        AnimalInfoTable animalInfoTable = new AnimalInfoTable();
        animalInfoTable.Load(DataTableIds.AnimalInfoTable);

        AnimalSizeTable animalSizeTable = new AnimalSizeTable();
        animalSizeTable.Load(DataTableIds.AnimalSizeTable);

        AnimalSpeedTable animalSpeedTable = new AnimalSpeedTable();
        animalSpeedTable.Load(DataTableIds.AnimalSpeedTable);

        AnimalCRRank animalCRRankTable= new AnimalCRRank();
        animalCRRankTable.Load(DataTableIds.AnimalCRRankTable);

        CrewTable crewTable = new CrewTable();
        crewTable.Load(DataTableIds.CrewTable);

        tables.Add(DataTableIds.StringTableIds , stringTable);
        tables.Add(DataTableIds.RoundTableIds, roundTable);
        tables.Add(DataTableIds.AnimalInfoTable, animalInfoTable);
        tables.Add(DataTableIds.AnimalSpeedTable, animalSpeedTable);
        tables.Add(DataTableIds.AnimalSizeTable, animalSizeTable);
        tables.Add(DataTableIds.AnimalCRRankTable, animalCRRankTable);
        tables.Add(DataTableIds.CrewTable, crewTable);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if(!tables.ContainsKey(id))
        {
#if DEV_MODE
            Debug.Log("Fail to Load");
#endif
            return null;
        }

        return tables[id] as T;
    }
}
