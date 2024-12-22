using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootSO : ScriptableObject
{
    [Serializable]
    public class Items
    {
        public GameObject Item;
        public float dropRate;
    }

    public List<Items> itemsList;


}