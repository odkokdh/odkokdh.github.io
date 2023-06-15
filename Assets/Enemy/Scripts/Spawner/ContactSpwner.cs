using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactSpwner : MonoBehaviour
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

    //������ ���������� �˷��ִ� ����
    bool nowSpawning;

    //�� �� �������� ������ų���� �ִ�ġ�� ���ϴ� ����
    public int monsterCount;

    //�ٽ� ���͸� ������Ű�µ� �ɸ��� �ð��� ���ϴ� ����
    public float respawnTime;

    //�ڽ� ������Ʈ�� ������ �ϰ� ������ �Ǵ��ϴ� �Ҹ���
    public bool chase;

    public Transform target;

    //��ũ��Ʈ ���� �ڵ���� ��Ƶδ� ����
    SpawnerContact parent;

    // Start is called before the first frame update
    void Start()
    {
        //�θ��� ��ũ��Ʈ�� �޾ƿ´�
        parent=GetComponentInParent<SpawnerContact>();

        //�ڽ��� ��ġ�� ����
        spawnerPos = transform.position;

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

        chase = false;
    }

    private void Update()
    {
        //�����ʰ� ������Ҷ�
        // 1. �÷��̾ �ν� �������� ����� ��
        // 2. ������ ���� ��ŭ ������ ������ ��.
        // �ν� �������� ����� ������ŭ ������ �� �Ǿ ������ ����.

        if(nowSpawning == true)
        {
            //������ ���� ��ŭ ������ ������
            if (transform.childCount == monsterCount)
            {
                //�ڷ�ƾ�� �����
                StopCoroutine(SpawnMonster());
                //���� ������ �ٽ� ������ �Ͼ �� �ְ� false�� �ٲ��ش�
                nowSpawning = false;
                return;
            }
        }

        //�÷��̾ �ν� ������ �����
        if (parent.onPlayer == false)
        {
            StopCoroutine(SpawnMonster());
            //�ڽ� ������Ʈ�� �߰��� �Ұ����ϰ� �������� �����
            chase = false;
            return;
        }

        

        //�÷��̾ �ν� ���� ���� ���� ���
        if (parent.onPlayer == true)
        {
            //�ڽ� ������Ʈ�� �߰��� �����ϰ� ������ �����
            chase = true;

            target = parent.target;
            
            //������ �̷������ �ʾ��� ���
            if (nowSpawning == false)
            {
                //�ڽ� ������Ʈ ���� ���� �������� ���� ���
                if (transform.childCount < monsterCount)
                {
                    //�ڷ�ƾ�� �����Ѵ�.
                    StartCoroutine(SpawnMonster());
                }
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        //���� ������ �������̹Ƿ� true�� ǥ���Ѵ�
        nowSpawning = true;

        //�ڽ� ������Ʈ ���� 4�� �̸��� ���
        while (transform.childCount < monsterCount)
        {
            //������ ���� �÷��̾ �´���� ���� �̷������ �����
            if (parent.onPlayer == true)
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

            //�ƴ� ��� ���� �ݺ��� �����.
            else
            {
                //�ٽ� ������ �̷������ �����
                nowSpawning = false;
                break;
            }
        }
    }
}
