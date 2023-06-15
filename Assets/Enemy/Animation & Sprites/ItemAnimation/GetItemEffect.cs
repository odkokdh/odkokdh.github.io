using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEffect : MonoBehaviour
{
    Animator anim;
    BoxCollider2D boxcollider;

    public float yPos; //��ġ��ŭ ������Ʈ�� ����Ѵ�
    public float speed; //��ġ��ŭ �ش� �ӵ��� �����δ�

    bool contact = false; //�÷��̾�� ������ ��Ÿ���� �Ҹ���

    Vector3 movePos;

    private void Start()
    {
        anim = GetComponent<Animator>();

        boxcollider = GetComponent<BoxCollider2D>();

        //����� ��� ��ǥ ��ġ�� �������ش�
        movePos = new Vector3(transform.position.x, transform.position.y + yPos, transform.position.z);
    }

    private void Update()
    {
        //���� �÷��̾�� ����� ���
        if (contact == true)
        {
            //������ ��ġ�� ������ �ӵ��� �����δ�
            transform.position = Vector3.MoveTowards(transform.position, movePos, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾�� ����� ���
        if (collision.tag == "Player")
        {
            //�浹 ������ �Ͼ�� �ʵ��� �ڽ� �ݶ��̴��� ����
            boxcollider.enabled = false;

            //Ʈ���Ÿ� �ߵ� ��Ų��
            contact = true;

            //�ִϸ��̼��� ����Ѵ�
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
