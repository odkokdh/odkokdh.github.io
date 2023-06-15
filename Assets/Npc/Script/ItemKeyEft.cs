using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemEft/Consumable/Key")]
public class ItemKeyEft : ItemEffect
{

    public override bool ExecuteRole()
    {
        DoorAnimation.instance.DoorOpen();
        return true;
    }
}
