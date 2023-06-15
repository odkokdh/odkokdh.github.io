using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Weapon/SG")]

public class ItemSgEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        Inventory.instance.items.Add(Player.instance.rangedWeapon[0]);
        Player.instance.SGSwitching();
        return true;
    }
}
