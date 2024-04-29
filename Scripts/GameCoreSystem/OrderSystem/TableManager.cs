using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public static TableManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    public List<Table> availableTables = new List<Table>();

    public Table FindUnoccupiedTable(out bool success, int groupNumber) // finc available table
    {
        Table suitableTable = null; // selected table

        foreach(Table t in availableTables)
        {
            TableStatus sts = t.tableStatus;

            if(groupNumber <= t.tableCap &&  sts == TableStatus.Unoccupied) 
                suitableTable = t; // if this table Unoccupied, select it as candidate;
                
            if(groupNumber == t.tableCap && sts == TableStatus.Unoccupied) // if this table has exact number of chair as group numbers
            {
                suitableTable = t; // priority is the exact number chair, not the distance
                break; // leave from loop
            }
        }

        if(suitableTable == null)
            success = false;
        else
            success = true;

        return suitableTable;
    }
}
