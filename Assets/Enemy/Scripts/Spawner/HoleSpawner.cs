using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleSpawner : MonoBehaviour
{
    //������ ���� �������� �޴� ������
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    //�����ϰ� ���͸� ������ ���� ������ ����Ʈ
    List<GameObject> monsters = new List<GameObject>();
    int randomMonster;

    //������ �ѹ��� �̷������ ������ִ� �Ҹ���
    private bool spawning;

    //�� �� �������� ������ų���� �ִ�ġ�� ���ϴ� ����
    public int monsterCount;

    //�ٽ� ���͸� ������Ű�µ� �ɸ��� �ð��� ���ϴ� ����
    public float respawnTime;

    // Start is called before the first frame update
    void Start()
    {
        //�� �������� ������Ʈ�� �ִ��� Ȯ���Ͽ� ���� ��� ����Ʈ�� �߰�
        if (enemy1 != null)
        {
            monsters.Add(enemy1);
        }

        if (enemy2 != null)
        {
            monsters.Add(enemy2);
        }

        if (enemy3 != null)
        {
            monsters.Add(enemy3);
        }

        if (enemy4 != null)
        {
            monsters.Add(enemy4);
        }
    }

    private void Update()
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

        //�ڽ� ������Ʈ ���� 4�� �̸��� ���
        while (transform.childCount < monsterCount)
        {
            //��ٷ� �������� �ʵ��� �����̸� �ɾ��ش�
            yield return new WaitForSeconds(respawnTime);

            //����Ʈ�� ��ġ ���� �����ϴ� ���� ���� �������� �����ϰ�
            randomMonster = Random.Range(0, monsters.Count);

            //�ش� ��ġ�� �ִ� ���� �������� �����s
            //Quaternion.identity�� ȸ���� ���� �����ǰ� ���ش�
            //���͸� �����ϰ� �ش� �������� ���� ������Ʈ�� ������ �����ϰ� ������Ʈ�� �ڽ� ������Ʈ�� �����.
            GameObject childs = Instantiate(monsters[randomMonster], transform.position, Quaternion.identity);
            childs.transform.SetParent(transform);
        }
    }
}
