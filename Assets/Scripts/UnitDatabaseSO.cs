using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDatabase", menuName = "Scriptable Objects/UnitDatabaseSO")]
public class UnitDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public struct UnitData
    {
        public UnitBasement prefab;
        public string name;
        public Sprite icon;
        public int cost;

    }

    
    public List<UnitData> allUnits;


}
