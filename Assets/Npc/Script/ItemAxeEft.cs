using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Weapon/Axe")]

public class ItemAxeEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        Inventory.instance.items.Add(Player.instance.meleeWeapon[0]);
        Player.instance.AxeSwitching();
        return true;
    }
}
