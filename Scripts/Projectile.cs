using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float projectileSpeed = 4f;

    Vector3 target; //�÷��̾� ��ġ�� ��� ����

    Vector3 startPosition; //����ü�� �߻�� ��ġ�� �����ϴ� ����
    Vector3 nowPositionl; //���ư��� ����ü�� ��ġ�� �����ϴ� ����

    Vector3 shootLocaition; //������ ��ǥ�� �����ϴ� ����

    float distance; //��� ��ġ�� ���� ��ġ�� �Ÿ��� �󸶳� ���������� Ȯ���ϴ� ����

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�Ѿ��� �߻�Ǵ� ��ġ ��ǥ�� �����ص�
        startPosition = transform.position;

        //�÷��̾��� ��ġ�� ���� ��ġ ��ǥ�� ���� ��� �������� ����ü�� ���� ����
        shootLocaition = (target - startPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //�߻��ϴ� ������ ���� ��ǥ�� ����ü�� �ӵ�, deltaTime�� ���Ͽ� ����ü�� ������ ����� �̵��ӵ��� �̵��Ѵ�
        transform.Translate(shootLocaition * projectileSpeed * Time.deltaTime);

        //�ڽ��� ��ġ�� �ǽð����� �����Ѵ�
        nowPositionl = transform.position;

        //ó�� ������ ��ġ�� �ڽ��� ��ġ�� ���� ���Ѵ�
        distance = Vector3.Distance(startPosition, nowPositionl);

        //ó�� ���۰� �ڽ��� ��ġ ���� 17�̻��� ���
        if(distance>=17)
        {
            //������Ʈ�� �ı��ȴ�
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���� �ε��� ���
        if(collision.gameObject.tag=="Wall")
        {
            //�ڽ��� �ı�
            Destroy(gameObject);
        }

        //�÷��̾�� �ε��� ���
        if(collision.gameObject.tag == "Player")
        {

            //�ڽ��� �ı�
            Destroy(gameObject);
        }
    }
}
