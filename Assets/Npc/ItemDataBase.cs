using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;
    public int money = 0;
	public Text moneyUI;

	private void Awake()
    {
        instance = this; 
    }

    public List<Item> itemDB = new List<Item>();
    public GameObject fieldItemPrefab;
    public GameObject dropItmePrefab;
    public Vector3[] pos;

    private void Start()
    {
        Player.instance.rangedWeapon.Add(itemDB[0]);
        Player.instance.meleeWeapon.Add(itemDB[9]);

        GameObject key = Instantiate(fieldItemPrefab, pos[0], Quaternion.identity);
        key.GetComponent<FieldItems>().SetItem(itemDB[2]);
    }

    public void DropBullet(Vector3 pos2)
    {
        GameObject dropItem1 = Instantiate(fieldItemPrefab, pos2, Quaternion.identity);
        dropItem1.GetComponent<FieldItems>().SetItem(itemDB[3]);
    }

    private void Update()
    {
		moneyUI.text = "¼ÒÁö±Ý : " + money.ToString();
	}
}
