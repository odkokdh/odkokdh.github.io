using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerContact : MonoBehaviour
{
    //추적 상태로 만들어주는 불리언
    public bool onPlayer = false;

    //플레이어의 위치를 저장해주는 변수
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
