using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTest : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 startPos; //�ڽ��� ���� ��ġ�� �����ϱ� ���� ����
    float xPos; //���� �������� �������� ������ x��ǥ�� �����ϱ� ���� ����
    float yPos; //���� �������� �������� ������ y��ǥ�� �����ϱ� ���� ����
    Vector3 randomPos; //�������� ������ ��ǥ ���� �����ϴ� ����

    public float patrolRanged; //�ڽ��� ��ȸ ������ ���ϴ� ����

    private float speed = 10f; //�̵��ӵ�

    private bool moving; //�ڽ��� ���� �̵������� üũ�ϴ� �Ҹ���
    private bool randoming = false; //��ǥ�� ������ ������ false�� ���ȸ� �̷�������� ���� �Ҹ���


    //�̵��� ������ �����¿�� ���� �ִ��� Ȯ���� �ϰ� ������ ��� ����
    RaycastHit2D rayHitLeft;
    RaycastHit2D rayHitRigth;
    RaycastHit2D rayHitDown;
    RaycastHit2D rayHitUp;

    bool contactWall; //���� ������ �˷��ִ� �Ҹ���
    bool loopFor; //for������ �ڵ尡 �ѹ� ������ٴ� ���� �˷��ִ� �Ҹ���

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        moving = false; //�������� �ʾұ⿡ false�� �����Ѵ�

        startPos = transform.position; //�ڽ��� ���� ��ġ�� ����
    }

    // Update is called once per frame
    void Update()
    {
        //���� ��ǥ�� ���� �ʾ��� ��� ����
        if(randoming == false)
        {
            //x��ǥ�� y��ǥ ���� ������ ���� ���� �������� ���ѵ� Vector3 �������� �����Ѵ�
            xPos = Random.Range(startPos.x - patrolRanged, startPos.x + patrolRanged + 1);
            yPos = Random.Range(startPos.y - patrolRanged, startPos.y + patrolRanged + 1);
            randomPos = new Vector3(xPos, yPos, 0);

            //���� ��ǥ�� �ѹ� ���������Ƿ� true�� �ٲ� ǥ��
            randoming = true;
        }

        //���� �̵��� �Ϸ����� �ʾ��� ��� ����
        if (moving == false)
        {
            //���� ���� ����� ���
            if (contactWall == true)
            {
                //�Ʒ� �ڵ带 �������� �ʴ´�.
                return;
            }
            
            //���� ��ġ�� ������ ��ǥ ���� �Ÿ��� ���� �̵��Ѵ�.
            transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

            //���� ���� ��ǥ�� ��ǥ ��ǥ�� ��ġ�� ���
            if(randomPos == transform.position)
            {
                //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                Invoke("Switching", Random.Range(3, 6));
                //�̵��� �Ϸ� �Ǿ����� ǥ���Ѵ�.
                moving = true;
            }
        }
    }

    public void Switching()
    {
        //�ٽ� ������ �� �ֵ��� false�� �ٲ��ش�
        moving = false;
        randoming = false;

        contactWall = false;
        loopFor = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���� ����� ���
        if (collision.gameObject.tag == "Wall")
        {
            //���� �̵��� �����.
            moving = true;
            //���� ������� ǥ�����ش�.
            contactWall = true;

            //����ĳ��Ʈ�� �������� 2�� ������ �߻��ϸ� �������� Wall�̶� Layer���� ���� ���� �߰��ϸ� �ش� ���� �����Ѵ�.
            rayHitLeft = Physics2D.Raycast(rigid.position, Vector2.left, 1, LayerMask.GetMask("Wall"));
            rayHitRigth = Physics2D.Raycast(rigid.position, Vector2.right, 1, LayerMask.GetMask("Wall"));
            rayHitDown = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Wall"));
            rayHitUp = Physics2D.Raycast(rigid.position, Vector2.up, 1, LayerMask.GetMask("Wall"));

            //�迭�� �ش� �������� ��´�.
            RaycastHit2D[] raycastHit2Ds = { rayHitLeft, rayHitRigth, rayHitDown, rayHitUp };
            //������ �Բ� �Լ��� ���� �����ش�.
            BackStep(raycastHit2Ds);
        }
    }


    //�� ��ġ�� ���� �ڷ� �������� ������ִ� �Լ�
    private void BackStep(RaycastHit2D[] raycastHit2Ds)
    {
        //�ִ� �迭�� ���̸�ŭ, �� �� 4�� �����Ѵ�.
        for (int i = 0; i < raycastHit2Ds.Length; i++)
        {
            //��ǥ ���ġ�� �̷������ �ʾ��� ��� �ٽ� �����Ѵ�.
            if (loopFor == false)
            {
                //�迭 �� ���� ���� ���ʴ�� �˻��Ѵ�. ���� �˻� �ߴµ� ���� ������ ���
                if (raycastHit2Ds[i].collider != null)
                {
                    //ù��° �迭, �� ���ʿ� ���� �ִ�
                    if (i == 0)
                    {
                        //���� ��� �ִ� ���°� ���� �ʵ��� ���������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //�ι�° �迭, �� �����ʿ� ���� �ִ�.
                    else if (i == 1)
                    {
                        if (loopFor == true) return;

                        //���� ��� �ִ� ���°� ���� �ʵ��� �������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x - 0.3f, transform.position.y);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //����° �迭, �� �Ʒ��� ���� �ִ�.
                    else if (i == 2)
                    {
                        if (loopFor == true) return;

                        //���� ��� �ִ� ���°� ���� �ʵ��� �������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x, transform.position.y + 0.3f);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }

                    //�׹�° �迭, �� ���� ���� �ִ�.
                    else if (i == 3)
                    {
                        if (loopFor == true) return;

                        //���� ��� �ִ� ���°� ���� �ʵ��� �Ʒ������� �ణ ����ش�.
                        transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);

                        //for������ �ڵ尡 �ѹ� ��������Ƿ� �̸� ǥ�����ش�
                        loopFor = true;

                        //�ٽ� �̵��� �̷���� �� �ְ� �Ҹ����� �ٲ��ִ� �Լ��� �θ���.
                        Invoke("Switching", Random.Range(3, 6));
                    }
                }
            }
        }
    }
}
