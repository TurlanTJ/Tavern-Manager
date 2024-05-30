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

    public Table FindUnoccupiedTable(out bool success, int groupNumber, Vector3 groupPos) // finc available table
    {
        Table suitableTable = null; // selected table
        float closestTable = float.MaxValue;

        foreach(Table t in availableTables)
        {
            TableStatus sts = t.tableStatus;

            if(sts != TableStatus.Unoccupied || groupNumber > t.tableCap)
                continue; // if this iterated table is NOT Unoccupied or have less chairs, move to the next table in the list

            float distance = Vector3.Distance(groupPos, t.transform.position); // calculate the distance between tbale and customers

            if(distance < closestTable) // check the distance between them is the closest
            {
                closestTable = distance; // if yes, make this table the suitable one
                suitableTable = t;
            }

            if(groupNumber == t.tableCap && distance < closestTable) // check if this table matches the group number
            {
                closestTable = distance; // if yes, make this table suitable
                suitableTable = t;
            }
        }

        if(suitableTable == null)
            success = false;
        else
            success = true;

        return suitableTable;
    }
}
