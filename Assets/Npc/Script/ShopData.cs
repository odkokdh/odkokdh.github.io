using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    public List<Item>stocks = new List<Item>();
    public bool[] soldOuts;

    void Start()
    {
        stocks.Add(ItemDataBase.instance.itemDB[1]);
        stocks.Add(ItemDataBase.instance.itemDB[5]);
        stocks.Add(ItemDataBase.instance.itemDB[4]);
        stocks.Add(ItemDataBase.instance.itemDB[6]);
        stocks.Add(ItemDataBase.instance.itemDB[7]);
        stocks.Add(ItemDataBase.instance.itemDB[8]);
        soldOuts = new bool[stocks.Count];
        for(int i = 0; i < soldOuts.Length; i++)
        {
            soldOuts[i] = false;
        }
    }
}
