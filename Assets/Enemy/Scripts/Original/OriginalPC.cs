using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalPC : MonoBehaviour
{
    //���� ���·� ������ִ� �Ҹ��� �ڽ� ��ũ��Ʈ�� ����ϱ⿡ protected
    protected bool chase = false;

    //�÷��̾��� ��ġ�� �������ִ� ���� �ڽ� ��ũ��Ʈ�� ����ϱ⿡ protected
    protected Transform target;

    //���� ���� �ȿ� �÷��̾ ������
    private void OnTriggerStay2D(Collider2D collision)
    {
        //���� ����� �÷��̾��
        if (collision.tag == "Player")
        {
            //���� ���¸� Ȱ��ȭ �ϰ�
            chase = true;

            //�÷��̾��� �� ��ġ�� ��� �����Ѵ�
            target = collision.gameObject.transform;
        }
    }

    //�÷��̾ ���� ���� ������ ������
    private void OnTriggerExit2D(Collider2D collision)
    {
        //���� ����� �÷��̾��
        if (collision.tag == "Player")
        {
            //���� ���¸� ��Ȱ��ȭ �ϰ�
            chase = false;

            //Ÿ���� null�� �ٲ� �� �̻� �÷��̾ �i�� �ʴ´�
            target = null;
        }
    }
}
