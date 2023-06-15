using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalPC : MonoBehaviour
{
    //추적 상태로 만들어주는 불리언 자식 스크립트도 써야하기에 protected
    protected bool chase = false;

    //플레이어의 위치를 저장해주는 변수 자식 스크립트도 써야하기에 protected
    protected Transform target;

    //감지 범위 안에 플레이어가 들어오면
    private void OnTriggerStay2D(Collider2D collision)
    {
        //들어온 대상이 플레이어면
        if (collision.tag == "Player")
        {
            //추적 상태를 활성화 하고
            chase = true;

            //플레이어의 현 위치를 계속 저장한다
            target = collision.gameObject.transform;
        }
    }

    //플레이어가 감지 범위 밖으로 나가면
    private void OnTriggerExit2D(Collider2D collision)
    {
        //나간 대상이 플레이어면
        if (collision.tag == "Player")
        {
            //추적 상태를 비활성화 하고
            chase = false;

            //타겟을 null로 바꿔 더 이상 플레이어를 쫒지 않는다
            target = null;
        }
    }
}
