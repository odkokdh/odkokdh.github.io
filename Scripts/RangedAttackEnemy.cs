using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���Ÿ� ���� ���Ϳ� ���� ������ �ڵ�
//������ �� �߰� ���¿��� ���� ��ġ���� �ָ� ���� ��� ���� ��ġ�� ���ư��� �ڵ� ����
//Ʈ�� �޽��� ��� Ŀ���� ������ �ݺ��Ͽ� �������� ����

public class RangedAttackEnemy : RangedPlayerContect
{
    Rigidbody2D rigid;
    Animator anim;

    Vector3 startPos; //�ڽ��� ���� ��ġ�� �����ϱ� ���� ����
    float xPos; //���� �������� �������� ������ x��ǥ�� �����ϱ� ���� ����
    float yPos; //���� �������� �������� ������ y��ǥ�� �����ϱ� ���� ����
    Vector3 randomPos; //�������� ������ ��ǥ ���� �����ϴ� ����

    Coroutine returnMove; //�ڷ�ƾ ������ üũ�ϴ� ����

    float attackDis; //�÷��̾ �����ϴ� �Ÿ��� �����ϴ� ����

    public float patrolRanged; //�ڽ��� ��ȸ ������ ���ϴ� ����

    Vector3 endPos; //��ǥ ������ �������ִ� ����

    private bool moving; //�ڽ��� ���� �̵������� üũ�ϴ� �Ҹ���
    private bool randoming = false; //��ǥ�� ������ ������ false�� ���ȸ� �̷�������� ���� �Ҹ���

    public GameObject projectilePrefab; //���Ÿ� ���ݽ� ����ü�� �����ϰ� ������ִ� ����
    public GameObject projectile; //Instantiate()�޼���� ������ ����ü�� ��� ���� ������Ʈ 
    private float speed = 18f;
    private float runSpeed = 18f;

    private bool atkMotion; //������ �����ߴ��� Ȯ���ϴ� �Ҹ���

    Canvas canvas; //�ڽ� ������Ʈ�� Canvas�� ã�� ������ ����
    public GameObject hpBarBackground; //���� ü���� �������ִ� ����
    public Image hpBarFilled; //������ �޾��� ��� hp���Ҹ� �����ϱ� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startPos = transform.position; //�ڽ��� ���� ��ġ�� ����

        moving = false; //�̵����� �ʾұ⿡ false�� �����Ѵ�
        atkMotion = false; //������ ���� �ʾұ⿡ false�� �����Ѵ�

        //�ڽ� ������Ʈ�� Canvas�� ã�� ������ �����Ѵ�
        canvas=GetComponentInChildren<Canvas>();
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

        if (chase==false)
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

                //���� �ڽ��� ��ġ�� ��ǥ ��ġ�� ��������
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
            else if(distanceStartNow < 15)
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
            if(hpBarBackground.activeSelf == false)
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

            //�÷��̾ �ʹ� �����̿� ���� ���
            if (attackDis < 3)
            {
                //�̵� �ִϸ��̼��� �����
                anim.SetBool("walk", false);

                float dirX = target.transform.position.x - transform.position.x;
                float dirY = target.transform.position.y - transform.position.y;

                Vector3 run;

                //dirX�� �����̰� dirY�� ������ �÷��̾�� ���� �Ʒ��� ��ġ
                if (dirX<0 && dirY <0)
                {
                    run =  Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x + 5, transform.position.y + 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }

                //dirX�� ����̰� dirY�� ������ �÷��̾�� ������ �Ʒ��� ��ġ
                else if(dirX > 0 && dirY <0)
                {
                    run = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x -5, transform.position.y + 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }

                //dirX�� �����̰� dirY�� ����� �÷��̾�� ���� ���� ��ġ
                else if(dirX < 0 && dirY > 0)
                {
                    run = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x + 5, transform.position.y - 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }

                //dirX�� ����̰� dirY�� ����� �÷��̾�� ������ ���� ��ġ
                else if(dirX > 0 && dirY > 0)
                {
                    run = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x - 5, transform.position.y - 5), runSpeed * Time.deltaTime);
                    rigid.MovePosition(run);
                }
            }

            //�÷��̾ ������ ���� ������ ���� ���
            else if (attackDis >= 3 && attackDis<=7)
            {
                //������ �ְ�
                rigid.MovePosition(transform.position);

                //������ �����Ѵ�
                StartCoroutine(Attack());

                //�̵� �ִϸ��̼��� �����
                anim.SetBool("walk", false);
            }

            //�÷��̾ ������ ������ ���� �����ȿ� ���� ���
            else if(attackDis > 7 && attackDis <=10)
            {
                rigid.MovePosition(endPos);
            }

            //���� ���� ���� ���� ������� ���
            else if(attackDis>10)
            {
                //������ �׸��Ѵ�
                chase = false;

                //�� ������ ������ ���¿��ٰ� �ٽ� ���� ���·� ���ư��� ��츦 ����� �ڵ�
                //moving = false;
                //randoming = false;
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


    //�÷��̾ �����ϴ� ����� ����Ǵ� �ڵ�
    public IEnumerator Attack()
    {
        //������ ���� �̷������ �ʾ��� ���
        if (atkMotion == false)
        {
            //���� ����� �����ϰ�
            anim.SetTrigger("attack");

            //�÷��̾� ���ݿ� ���� �ڵ� �ۼ� �ʿ�

            //Instantiate()�� ����ü�� ���� ����
            projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

            //������ �̷�������Ƿ� true�� �ٲ��ش�
            atkMotion = true;

            //�ٽ� ������ �� �� �ֵ��� 3���� �����̸� �ش�
            Invoke("atkDelay", 3f);
        }
        yield return new WaitForSeconds(3f);
    }

    //�Ҹ����� �ٲ��ִ� �Լ�
    public void Switching()
    {
            //�ٽ� ������ �� �ֵ��� false�� �ٲ��ش�
            moving = false;
            randoming = false;
    }

    //3���� ��Ÿ�� ���� �ٽ� atkMotion�� false�� �ٲ۴�
    void atkDelay()
    {
        //��Ÿ���� ���� �� �ٽ� ������ �̷���� �� �ְ� �����
        atkMotion = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            //���̳� ���� �浹�� ���� ���
            if (collision.gameObject.tag == "Wall" || collision.gameObject.tag=="Enemy")
            {
                float dirX = collision.transform.position.x - transform.position.x;
                float dirY = collision.transform.position.y - transform.position.y;

                Vector3 backStep;

                //dirX�� ������ �ε��� ����� ���ʿ� ��ġ
                if (dirX < 0)
                {
                    backStep = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x + 5, transform.position.y), speed * Time.deltaTime);
                    rigid.MovePosition(backStep);
                }

                //dirX�� ����� �ε��� ����� �����ʿ� ��ġ
                else if (dirX > 0)
                {
                    backStep = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x - 5, transform.position.y), speed * Time.deltaTime);
                    rigid.MovePosition(backStep);
                }

                //dirY�� ����� �ε��� ����� ���� ��ġ
                else if (dirY > 0)
                {
                    backStep = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x, transform.position.y - 5), speed * Time.deltaTime);
                    rigid.MovePosition(backStep);
                }

                //dirY�� ������ �ε��� ����� �Ʒ��� ��ġ
                else if (dirY < 0)
                {
                    backStep = Vector3.MoveTowards(rigid.position, new Vector3(transform.position.x, transform.position.y + 5), speed * Time.deltaTime);
                    rigid.MovePosition(backStep);
                }

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
        
    }
}
