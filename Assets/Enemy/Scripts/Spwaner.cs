using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������ ���� �ڵ�

public class Spwaner : MonoBehaviour
{
    //������ ���� �������� �޴� ������
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    //�����ϰ� ���͸� ������ ���� ������ ����Ʈ
    List<GameObject> monsters = new List<GameObject>();
    int randomMonster;

    //���� ���� �ȿ� ������ ��ġ���� �����ǵ��� ������ִ� ��ǥ ������
    private float xPos;
    private float yPos;
    private Vector3 randomVector3;

    //�������� ��ġ�� �����ϴ� ����
    private Vector3 spawnerPos;

    //������ �ѹ��� �̷������ ������ִ� �Ҹ���
    private bool spawning;

    //�� �� �������� ������ų���� �ִ�ġ�� ���ϴ� ����
    public int monsterCount;

    //�ٽ� ���͸� ������Ű�µ� �ɸ��� �ð��� ���ϴ� ����
    public float respawnTime;

    // Start is called before the first frame update
    void Start()
    {
        //�ڽ��� ��ġ�� ����
        spawnerPos = transform.position;

        //�� �������� ������Ʈ�� �ִ��� Ȯ���Ͽ� ���� ��� ����Ʈ�� �߰�
        if(enemy1 != null)
        {
            monsters.Add(enemy1);
        }

        if (enemy2 != null)
        {
            monsters.Add(enemy2);
        }

        if(enemy3 != null)
        {
            monsters.Add(enemy3);
        }

        if(enemy4 != null)
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
            //����Ʈ�� ��ġ ���� �����ϴ� ���� ���� �������� �����ϰ�
            randomMonster = Random.Range(0, monsters.Count);

            //�ش� ���Ͱ� ���� ��ġ�� �������� �������ش�
            xPos = Random.Range(spawnerPos.x - 5, spawnerPos.x + 6);
            yPos = Random.Range(spawnerPos.y - 5, spawnerPos.y + 6);
            randomVector3 = new Vector3(xPos, yPos, 0);

            //�ش� ��ġ�� �ִ� ���� �������� �����
            //���� �����յ��� ����ִ� monsters ����Ʈ���� ������ ���� ������ ��ġ�� ���� ������ �������� �������� �����ϰ�
            //���� ������ ������ ��ġ�� �����ǰ� �ϸ�
            //Quaternion.identity�� ȸ���� ���� �����ǰ� ���ش�
            //���͸� �����ϰ� �ش� �������� ���� ������Ʈ�� ������ �����ϰ� ������Ʈ�� �ڽ� ������Ʈ�� �����.
            GameObject childs = Instantiate(monsters[randomMonster], randomVector3, Quaternion.identity);
            childs.transform.SetParent(transform);

            //��ٷ� �������� �ʵ��� �����̸� �ɾ��ش�
            yield return new WaitForSeconds(respawnTime);
        }
    }
}
