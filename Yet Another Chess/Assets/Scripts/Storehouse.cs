using System;
using System.Collections.Generic;

public class Storehouse : Singleton<Storehouse>
{
    readonly Dictionary<string, int> inventory = new Dictionary<string, int>();
    public event Action StockChanged;

    public int IncreaseInventory(string key, int num)
    {
        bool exists = inventory.ContainsKey(key);
        if (!exists)
        {
            inventory.Add(key, 0);
        }
        inventory[key] = inventory[key] + num;
        StockChanged?.Invoke();
        return inventory[key];
    }

    public bool TryDecreaseInventory(string key, int num){
        bool exists = inventory.ContainsKey(key);
        if (!exists)
        {
            return false;
        }
        if (inventory[key] < num)
        {
            return false;
        }
        inventory[key] = inventory[key] - num;
        if (inventory[key] == 0 )
        {
            inventory.Remove(key);
        }
        if (StockChanged != null)
        {
            StockChanged();
        }
        return true;
    }

    public int GetInventory(string key)
    {
        bool exists = inventory.ContainsKey(key);
        if (!exists)
        {
            return 0;  
        }
        return inventory[key];
    }
}
