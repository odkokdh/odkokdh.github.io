using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Weapon/SMG")]

public class ItemSmgEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        Inventory.instance.items.Add(Player.instance.rangedWeapon[0]);
        Player.instance.SMGSwitching();
        return true;
    }
}
