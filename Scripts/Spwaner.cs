using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������ ���� �ڵ�

public class Spwaner : MonoBehaviour
{
    //������ ���� �������� �޴� ������
    public GameObject melee;
    public GameObject ranged;

    //�����ϰ� ���͸� ������ ���� ������ ����Ʈ
    List<GameObject> monsters = new List<GameObject>();
    int randomMonster;

    //���� ���� �ȿ� ������ ��ġ���� �����ǵ��� ������ִ� ��ǥ ������
    public float xPos;
    public float yPos;
    private Vector3 randomVector3;

    //�������� ��ġ�� �����ϴ� ����
    private Vector3 spawnerPos;

    //������ �ѹ��� �̷������ ������ִ� �Ҹ���
    private bool spawning;

    // Start is called before the first frame update
    void Start()
    {
        //�ڽ��� ��ġ�� ����
        spawnerPos = transform.position;

        //����Ʈ �ȿ� ���� �����յ��� �߰�
        monsters.Add(melee);
        monsters.Add(ranged);

        //�ڷ�ƾ�� �����Ͽ� ���� ������ ���۵ȴ�
        StartCoroutine(SpawnMonster());
    }

    private void Update()
    {
        //������ �����ϱ� �� �����̸�
        if (spawning == false)
        {
            //������ �����ϱ� ���� �ڷ�ƾ�� ������ ���� ������ ���۵ȴ�
            StartCoroutine(SpawnMonster());
        }
    }

    IEnumerator SpawnMonster()
    {
        //�ڽ� ������Ʈ ���� 4�� �̸��� ���
        while(transform.childCount < 4)
        {
            //���� ���¸� Ʈ��� �ٲ� �ش�
            spawning = true;

            if (spawning == true)
            {
                //����Ʈ�� ��ġ ���� �����ϴ� ���� ���� �������� �����ϰ�
                randomMonster = Random.Range(0, 2);

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
            }
            //��ٷ� �������� �ʵ��� �����̸� �ɾ��ش�
            yield return new WaitForSeconds(5f);
        }
        //������ �����⿡ false�� �����Ѵ�
        spawning = false;
    }
}
