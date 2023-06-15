using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEffect : MonoBehaviour
{
    Animator anim;
    BoxCollider2D boxcollider;

    public float yPos; //수치만큼 오브젝트가 상승한다
    public float speed; //수치만큼 해당 속도로 움직인다

    bool contact = false; //플레이어와 닿음을 나타내는 불리언

    Vector3 movePos;

    private void Start()
    {
        anim = GetComponent<Animator>();

        boxcollider = GetComponent<BoxCollider2D>();

        //닿았을 경우 목표 위치를 지정해준다
        movePos = new Vector3(transform.position.x, transform.position.y + yPos, transform.position.z);
    }

    private void Update()
    {
        //만약 플레이어와 닿았을 경우
        if (contact == true)
        {
            //정해진 위치로 정해진 속도로 움직인다
            transform.position = Vector3.MoveTowards(transform.position, movePos, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어와 닿았을 경우
        if (collision.tag == "Player")
        {
            //충돌 판정이 일어나지 않도록 박스 콜라이더를 끈다
            boxcollider.enabled = false;

            //트리거를 발동 시킨다
            contact = true;

            //애니메이션을 재생한다
            anim.SetTrigger("up");

            
            if(tag == "MediKit")
            {
                Player.instance.mediKitAmount += 1;
            }

            StartCoroutine(ItemDestroy());
        }
    }

    IEnumerator ItemDestroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
