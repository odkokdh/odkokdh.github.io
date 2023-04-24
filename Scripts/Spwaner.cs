using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 스폰을 위한 코드

public class Spwaner : MonoBehaviour
{
    //생성할 몬스터 프리팹을 받는 변수들
    public GameObject melee;
    public GameObject ranged;

    //랜덤하게 몬스터를 꺼내기 위한 변수와 리스트
    List<GameObject> monsters = new List<GameObject>();
    int randomMonster;

    //스폰 범위 안에 랜덤한 위치에서 생성되도록 만들어주는 좌표 변수들
    public float xPos;
    public float yPos;
    private Vector3 randomVector3;

    //스포너의 위치를 저장하는 변수
    private Vector3 spawnerPos;

    //스폰이 한번씩 이루어지게 만들어주는 불리언
    private bool spawning;

    // Start is called before the first frame update
    void Start()
    {
        //자신의 위치를 저장
        spawnerPos = transform.position;

        //리스트 안에 몬스터 프리팹들을 추가
        monsters.Add(melee);
        monsters.Add(ranged);

        //코루틴을 실행하여 몬스터 스폰이 시작된다
        StartCoroutine(SpawnMonster());
    }

    private void Update()
    {
        //스폰이 시작하기 전 상태이면
        if (spawning == false)
        {
            //스폰을 실행하기 위해 코루틴을 실행해 몬스터 스폰이 시작된다
            StartCoroutine(SpawnMonster());
        }
    }

    IEnumerator SpawnMonster()
    {
        //자식 오브젝트 수가 4개 미만일 경우
        while(transform.childCount < 4)
        {
            //스폰 상태를 트루로 바꿔 준다
            spawning = true;

            if (spawning == true)
            {
                //리스트의 위치 값을 지정하는 변수 값을 무작위로 지정하고
                randomMonster = Random.Range(0, 2);

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
            }
            //곧바로 생성되지 않도록 딜레이를 걸어준다
            yield return new WaitForSeconds(5f);
        }
        //스폰이 끝났기에 false로 지정한다
        spawning = false;
    }
}
