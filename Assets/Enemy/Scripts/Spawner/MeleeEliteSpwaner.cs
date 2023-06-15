using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEliteSpwaner : MonoBehaviour
{
    //� ���͸� ��ȯ���� �޴� ����
    public GameObject enemy;

    //������ �ѹ��� �̷������ ������ִ� �Ҹ���
    private bool spawning;

    //�� �� �������� ������ų���� �ִ�ġ�� ���ϴ� ����
    public int monsterCount;

    //�ٽ� ���͸� ������Ű�µ� �ɸ��� �ð��� ���ϴ� ����
    public float respawnTime;

    // Update is called once per frame
    void Update()
    {
        //������ ���۵�����
        if (spawning == true)
        {
            //������ ���� ��ŭ ������ ������
            if (transform.childCount == monsterCount)
            {
                //�ڷ�ƾ�� �����
                StopCoroutine(SpawnMonster());
                //���� ������ �ٽ� ������ �Ͼ �� �ְ� false�� �ٲ��ش�
                spawning = false;
                return;
            }
        }

        //������ �������� �ʾ�����
        if (spawning == false)
        {
            //�ڽ� ������Ʈ ���� ���� �������� ���� ���
            if (transform.childCount < monsterCount)
            {
                //�ڷ�ƾ�� �����Ѵ�.
                StartCoroutine(SpawnMonster());
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        //���� ���¸� Ʈ��� �ٲ� �ش�
        spawning = true;
        
        //�ڽ� ������Ʈ ���� ���� ���� �̸��� ���
        while (transform.childCount < monsterCount)
        {
            //Quaternion.identity�� ȸ���� ���� �����ǰ� ���ش�
            //���͸� �����ϰ� �ش� �������� ���� ������Ʈ�� ������ �����ϰ� ������Ʈ�� �ڽ� ������Ʈ�� �����.
            GameObject childs = Instantiate(enemy, transform.position, Quaternion.identity);
            childs.transform.SetParent(transform);

            //��ٷ� �������� �ʵ��� �����̸� �ɾ��ش�
            yield return new WaitForSeconds(respawnTime);
        }
    }
}
