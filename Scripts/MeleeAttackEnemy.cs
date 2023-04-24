using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���Ϳ� ���� ������ �ڵ�

public class MeleeAttackEnemy : PlayerContect
{
    Rigidbody2D rigid;
    Animator anim;

    Vector3 startPos; //�ڽ��� ���� ��ġ�� �����ϱ� ���� ����
    float xPos; //���� �������� �������� ������ x��ǥ�� �����ϱ� ���� ����
    float yPos; //���� �������� �������� ������ y��ǥ�� �����ϱ� ���� ����
    Vector3 randomPos; //�������� ������ ��ǥ ���� �����ϴ� ����

    Coroutine returnMove; //�ڷ�ƾ ������ üũ�ϴ� ����

    public float patrolRanged; //�ڽ��� ��ȸ ������ ���ϴ� ����

    Vector3 endPos; //��ǥ ������ �������ִ� ����

    float attackDis; //�÷��̾ �����ϴ� �Ÿ��� �����ϴ� ����
    public Transform attackPos; //���� ��ġ�� �������ִ� ��ġ ����
    public Vector2 attackRange; //����Ƽ���� ���� ������ ������ �� �� �ִ� ����

    private float damage; //������ �޾��� ��� ���ݷ� ���� �����ϴ� ����
    public float hp = 25f; //�ִ�ü��
    private float speed = 20f; //�̵��ӵ�

    private bool moving; //�ڽ��� ���� �̵������� üũ�ϴ� �Ҹ���
    private bool randoming = false; //��ǥ�� ������ ������ false�� ���ȸ� �̷�������� ���� �Ҹ���

    private bool atkMotion; //������ �����ߴ��� Ȯ���ϴ� �Ҹ���

    Canvas canvas; //�ڽ� ������Ʈ�� Canvas�� ã�� ������ ����
    public GameObject hpBarBackground; //���� ü���� �������ִ� ����
    public Image hpBarFilled; //������ �޾��� ��� hp���Ҹ� �����ϱ� ���� ����

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startPos = transform.position; //�ڽ��� ���� ��ġ�� ����

        moving = false; //�������� �ʾұ⿡ false�� �����Ѵ�
        atkMotion = false; //������ ���� �ʾұ⿡ false�� �����Ѵ�

        //�ڽ� ������Ʈ�� Canvas�� ã�� ������ �����Ѵ�
        canvas = GetComponentInChildren<Canvas>();
        //Canvas�� ���� ī�޶� �÷��� ���� mainCamera�� �����Ѵ�
        canvas.worldCamera = Camera.main;

        //�� ó�� ü���� ���ظ� ���� �ʾұ⿡ ���� ���ִ� ���·� �����
        hpBarFilled.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���� �ε����ų� �Ҷ� �и��� �����ϱ� ���� �ڵ�
        rigid.velocity = Vector3.zero;

        //������ �����̱� ������ ü�¹ٴ� �Ⱥ��̰� �����.
        //enemyHpbarSlider.gameObject.SetActive(false);

        if(chase==false)
        {
            //ü�¹ٰ� ���� ������ �����̸�
            if (hpBarBackground.activeSelf == true)
            {
                //������ ���� �����̱⿡ �� ���̰� �����
                hpBarBackground.SetActive(false);
            }

            if (randoming == false)
            {
                //x��ǥ�� y��ǥ ���� ������ ���� ���� �������� ���ѵ� Vector3 �������� �����Ѵ�
                xPos = Random.Range(startPos.x - patrolRanged, startPos.x + patrolRanged + 1);
                yPos = Random.Range(startPos.y - patrolRanged, startPos.y + patrolRanged + 1);
                randomPos = new Vector3(xPos, yPos, 0);

                //��ǥ�� �������� ������ ����Ǿ����Ƿ� true�� �����Ѵ�
                randoming = true;
            }

            //���� �̵��� ���� �������� �ʾ��� ���
            if (moving == false)
            {
                anim.SetBool("walk", true);

                //���� �ڽ��� ��ġ�� ������ ������ ������ ��ǥ ���� ���� ��ǥ�� ������ �ӵ� ������ �̵��ϰ� �����
                endPos = Vector3.MoveTowards(rigid.position, randomPos, speed * Time.deltaTime);

                //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� ������ ���� �����̴� ���̱� ������
                if (endPos.x - transform.position.x < 0)
                {
                    //�̹����� ������ ���ϰ� �Ѵ�
                    transform.localScale = new Vector3(-8f, 8f, 1);
                }

                //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� �������� ���� �����̴� ���̱� ������
                else if (endPos.x - transform.position.x > 0)
                {
                    //�̹����� �������� ���ϰ� �Ѵ�
                    transform.localScale = new Vector3(8f, 8f, 1);
                }

                //�ش� ��ġ�� �̵��Ѵ�
                rigid.MovePosition(endPos);

                //���� �ڽ��� ��ġ�� ��ǥ ��ġ�� ���ٸ�
                if (transform.position == endPos)
                {
                    //�̵��� �Ϸ��Ͽ��⿡ true�� �ٲ��ְ�
                    moving = true;

                    anim.SetBool("walk", false);

                    //������ �����̸� �� �Լ��� �����ϰ� �Ѵ�
                    Invoke("Switching", Random.Range(3, 6));
                }
            }

            //���� ������ ���� ������ �Ÿ��� ����Ѵ�
            float distanceStartNow = Vector3.Distance(transform.position, startPos);


            //�� �߰� ���¿��� ���Ͱ� ������ ��ȸ ������ ����� ���
            if (distanceStartNow >= 15f)
            {
                //�̵��� �����ٰ� ǥ���ϱ� ���� �Ҹ����� true�� �����Ѵ�
                randoming = true;
                moving = true;

                //���� ���� ��ġ�� �ű�� �ڵ尡 ������� �ʾ��� ���
                if (returnMove == null)
                {
                    //������ġ�� �ű�� �ڷ�ƾ�� �����Ѵ�
                    returnMove = StartCoroutine(Return());


                }
            }

            //���� ��ġ�� ���� �� �� �� ���, �� ���� ��ġ�� ���� ��� 
            else if (distanceStartNow < 15)
            {
                //�ڷ�ƾ�� �����
                StopCoroutine(Return());

                //null�� �ٲ� ������ġ�� �̵� �ڵ尡 ������� �ʰ� �ٲ۴�
                returnMove = null;
            }
        }

        //�÷��̾ �߰� ���϶��� ����ȴ�
        else if (chase == true)
        {
            //���� ü�� ������ �ȵǾ��ִ� �����̸�
            if (hpBarBackground.activeSelf == false)
            {
                //ü�¹ٸ� Ȱ��ȭ���ش�
                hpBarBackground.SetActive(true);
            }

            anim.SetBool("walk", true);

            //�÷��̾��� ���� ��ġ�� ��ǥ �������� ��� �����δ�
            endPos = Vector3.MoveTowards(rigid.position, target.position, speed * Time.deltaTime);

            //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� ������ ���� �����̴� ���̱� ������
            if (endPos.x - transform.position.x < 0)
            {
                //�̹����� ������ ���ϰ� �Ѵ�
                transform.localScale = new Vector3(-8f, 8f, 1);
            }

            //�̵� ������ ���� ��ġ�� ����ؼ� 0���� Ŭ ��� �������� ���� �����̴� ���̱� ������
            else if (endPos.x - transform.position.x > 0)
            {
                //�̹����� �������� ���ϰ� �Ѵ�
                transform.localScale = new Vector3(8f, 8f, 1);
            }

            //�ش� ��ġ�� �̵��Ѵ�
            rigid.MovePosition(endPos);

            //�÷��̾�� �Ÿ��� ����Ѵ�
            attackDis = Vector3.Distance(target.transform.position, transform.position);

            //�÷��̾�� �Ÿ��� ���� ��ġ ���϶��
            if(attackDis <= 3)
            {
                //������ �ְ�
                rigid.MovePosition(transform.position);

                //������ �����Ѵ�
                StartCoroutine(Attack());

                //�̵� �ִϸ��̼��� �����
                anim.SetBool("walk",false);
            }
        }
    }

    //ó�� ���� ��ġ�� �ǵ����� �ڷ�ƾ
    public IEnumerator Return()
    {
        anim.SetBool("walk", false);

        //���� �ڸ� �̵��� ������ �� �����Ѵ�
        Invoke("PosReset", 7f);

        yield return new WaitForSeconds(1f);
    }

    public void PosReset()
    {
        //�ڱ� ��ġ�� ���� ��ġ�� �ٲ۴�
        transform.position = startPos;
        //�Ҹ����� ���� ����ߴٰ� ǥ���ϱ� ���� true�� ǥ��
        moving = true;
        randoming = true;
        //��ǥ������ ���� ��ġ�� �����Ѵ�
        endPos = startPos;
        //�ٽ� �̵� ��ƾ�� �Ͼ���� �Ҹ��� ����Ī�� ���ش�
        Switching();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, attackRange);
    }

    //�÷��̾ �����ϴ� ����� ����Ǵ� �ڵ�
    public IEnumerator Attack()
    {
        //������ ���� �̷������ �ʾ��� ���
        if(atkMotion == false)
        {
            //���� ����� �����ϰ�
            anim.SetTrigger("attack");

            //�÷��̾� ���ݿ� ���� �ڵ� �ۼ� �ʿ�
            Debug.Log("����");
            Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0);

            foreach(Collider2D collider in colliders)
            {
                if(collider.tag == "Player")
                {
                    Debug.Log("�÷��̾� ����");
                    //�÷��̾ ���� ����� ����?
                }
            }

            //������ �̷�������Ƿ� true�� �ٲ��ش�
            atkMotion = true;

            //�ٽ� ������ �� �� �ֵ��� 3���� �����̸� �ش�
            Invoke("atkDeliy", 3f);
        }

        yield return new WaitForSeconds(3f);
    }

    //3���� ��Ÿ�� ���� �ٽ� atkMotion�� false�� �ٲ۴�
    void atkDeliy()
    {
        //��Ÿ���� ���� �� �ٽ� ������ �̷���� �� �ְ� �����
        atkMotion = false;
    }

    //�Ҹ����� �ٲ��ִ� �Լ�
    public void Switching()
    {
        //�ٽ� ������ �� �ֵ��� false�� �ٲ��ش�
        moving = false;
        randoming = false;
    }

    //���� ���� ��� ���߰� �ٸ� ������ ���� ����
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //���� �浹�� ���� ���
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag=="Enemy")
        {
            anim.SetBool("walk", false);

            //�̵��� �Ϸ��ߴٰ� �Ǵ��� true�� �ٲٰ�
            moving = true;
            //��ǥ������ �ڽ��� ��ġ�� �ٲ۴�
            //endPos = transform.position;
            //�׸��� ��ǥ�������� �̵��Ѵ�
            //rigid.MovePosition(endPos);

            //�̵��� �ٽ� �̷���� �� �ֵ��� �����̸� �� �Լ��� �����Ѵ�
            Invoke("Switching", Random.Range(3, 6));
        }

        /*
        //���⿡ �¾Ƽ�
        if(collision.gameObject.tag == "����")
        {
            //� ������ ���ݷ��� 4 �ϰ��
            damage = 4f;
            //�� ���ݷ¸�ŭ ü���� �����Ѵ�
            hp -= damage;
        }
        */
    }
}
