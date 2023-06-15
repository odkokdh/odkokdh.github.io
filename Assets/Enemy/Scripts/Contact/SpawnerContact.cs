using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerContact : MonoBehaviour
{
    //���� ���·� ������ִ� �Ҹ���
    public bool onPlayer = false;

    //�÷��̾��� ��ġ�� �������ִ� ����
    public Transform target;

    private void Start()
    {
        onPlayer = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onPlayer = true;
            target = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onPlayer = false;
        }
    }
}
