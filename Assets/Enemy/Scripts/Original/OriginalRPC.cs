using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalRPC : MonoBehaviour
{
    //�÷��̾ � �±� ���� ������Ʈ �ö��̴��� ������ ���Դٴ� ���� ǥ���ؼ� ���ʹ̰� �����ϰ� ������ ��.

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
}
