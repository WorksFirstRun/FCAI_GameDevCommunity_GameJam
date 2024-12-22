
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    public static DropSystem Instance { get; private set; }
   
   

    private void Awake()
    {
        Instance = this;
    }

    
    public void DropItem(LootSO lootSo,Vector2 position)
    {
        GameObject item = GetDroppedItem(lootSo);
        if (item != null)
        {
            Instantiate(item, position, Quaternion.identity);
        }
    }
   
    private GameObject GetDroppedItem(LootSO loot) 
    {
      
        float randomValue = Random.Range(0, 100f);

        float cumulativeRate = 0f;

        foreach (LootSO.Items item in loot.itemsList)
        {
            cumulativeRate += item.dropRate;
            if (randomValue <= cumulativeRate)
            {
                return item.Item;
            }
        }

        return null; 
    }

}
