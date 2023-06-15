using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactSpwner : MonoBehaviour
{ 
    //생성할 몬스터 프리팹을 받는 변수들
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    //랜덤하게 몬스터를 꺼내기 위한 변수와 리스트
    List<GameObject> monsters = new List<GameObject>();
    int randomMonster;

    //스폰 범위 안에 랜덤한 위치에서 생성되도록 만들어주는 좌표 변수들
    private float xPos;
    private float yPos;
    private Vector3 randomVector3;

    //스포너의 위치를 저장하는 변수
    private Vector3 spawnerPos;

    //스폰이 진행중인지 알려주는 변수
    bool nowSpawning;

    //총 몇 마리까지 스폰시킬건지 최대치를 정하는 변수
    public int monsterCount;

    //다시 몬스터를 스폰시키는데 걸리는 시간을 정하는 변수
    public float respawnTime;

    //자식 오브젝트가 추적을 하게 만들지 판단하는 불리언
    public bool chase;

    public Transform target;

    //스크립트 안의 코드들을 담아두는 변수
    SpawnerContact parent;

    // Start is called before the first frame update
    void Start()
    {
        //부모의 스크립트를 받아온다
        parent=GetComponentInParent<SpawnerContact>();

        //자신의 위치를 저장
        spawnerPos = transform.position;

        //각 변수마다 오브젝트가 있는지 확인하여 있을 경우 리스트에 추가
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
        //스포너가 멈춰야할때
        // 1. 플레이어가 인식 범위에서 벗어났을 때
        // 2. 정해진 수량 만큼 스폰이 끝났을 때.
        // 인식 범위에서 벗어나면 수량만큼 스폰이 안 되어도 스폰은 멈춤.

        if(nowSpawning == true)
        {
            //정해진 수량 만큼 스폰이 됐으면
            if (transform.childCount == monsterCount)
            {
                //코루틴을 멈춘다
                StopCoroutine(SpawnMonster());
                //적이 죽으면 다시 스폰이 일어날 수 있게 false로 바꿔준다
                nowSpawning = false;
                return;
            }
        }

        //플레이어가 인식 범위에 벗어나면
        if (parent.onPlayer == false)
        {
            StopCoroutine(SpawnMonster());
            //자식 오브젝트가 추격이 불가능하게 거짓으로 만든다
            chase = false;
            return;
        }

        

        //플레이어가 인식 범위 내에 있을 경우
        if (parent.onPlayer == true)
        {
            //자식 오브젝트가 추격이 가능하게 참으로 만든다
            chase = true;

            target = parent.target;
            
            //스폰이 이루어지지 않았을 경우
            if (nowSpawning == false)
            {
                //자식 오브젝트 수가 제한 수량보다 적을 경우
                if (transform.childCount < monsterCount)
                {
                    //코루틴을 실행한다.
                    StartCoroutine(SpawnMonster());
                }
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        //현재 스폰이 진행중이므로 true로 표시한다
        nowSpawning = true;

        //자식 오브젝트 수가 4개 미만일 경우
        while (transform.childCount < monsterCount)
        {
            //스폰이 현재 플레이어가 맞닿았을 때만 이루어지게 만든다
            if (parent.onPlayer == true)
            {
                //리스트의 위치 값을 지정하는 변수 값을 무작위로 지정하고
                randomMonster = Random.Range(0, monsters.Count);

                //해당 몬스터가 나올 위치도 무작위로 지정해준다
                xPos = Random.Range(spawnerPos.x - 5, spawnerPos.x + 6);
                yPos = Random.Range(spawnerPos.y - 5, spawnerPos.y + 6);
                randomVector3 = new Vector3(xPos, yPos, 0);

                //해당 위치에 있는 몬스터 프리팹을 만든다
                //몬스터 프리팹들이 들어있는 monsters 리스트에서 위에서 정한 랜덤한 위치값 지정 변수로 무작위로 프리팹을 지정하고
                //범위 내에서 랜덤한 위치에 생성되게 하며
                //Quaternion.identity는 회전이 없이 생성되게 해준다
                //몬스터를 생성하고 해당 프리팹을 가진 오브젝트를 변수에 저장하고 오브젝트를 자식 오브젝트로 만든다.
                GameObject childs = Instantiate(monsters[randomMonster], randomVector3, Quaternion.identity);
                childs.transform.SetParent(transform);

                //곧바로 생성되지 않도록 딜레이를 걸어준다
                yield return new WaitForSeconds(respawnTime);
            }

            //아닐 경우 스폰 반복을 멈춘다.
            else
            {
                //다시 스폰이 이루어지게 만든다
                nowSpawning = false;
                break;
            }
        }
    }
}
