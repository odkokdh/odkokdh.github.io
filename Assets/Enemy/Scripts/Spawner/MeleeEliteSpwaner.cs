using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEliteSpwaner : MonoBehaviour
{
    //어떤 몬스터를 소환할지 받는 변수
    public GameObject enemy;

    //스폰이 한번씩 이루어지게 만들어주는 불리언
    private bool spawning;

    //총 몇 마리까지 스폰시킬건지 최대치를 정하는 변수
    public int monsterCount;

    //다시 몬스터를 스폰시키는데 걸리는 시간을 정하는 변수
    public float respawnTime;

    // Update is called once per frame
    void Update()
    {
        //스폰이 시작됐으면
        if (spawning == true)
        {
            //정해진 수량 만큼 스폰이 됐으면
            if (transform.childCount == monsterCount)
            {
                //코루틴을 멈춘다
                StopCoroutine(SpawnMonster());
                //적이 죽으면 다시 스폰이 일어날 수 있게 false로 바꿔준다
                spawning = false;
                return;
            }
        }

        //스폰을 실행하지 않았으면
        if (spawning == false)
        {
            //자식 오브젝트 수가 제한 수량보다 적을 경우
            if (transform.childCount < monsterCount)
            {
                //코루틴을 실행한다.
                StartCoroutine(SpawnMonster());
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        //스폰 상태를 트루로 바꿔 준다
        spawning = true;
        
        //자식 오브젝트 수가 제한 수량 미만일 경우
        while (transform.childCount < monsterCount)
        {
            //Quaternion.identity는 회전이 없이 생성되게 해준다
            //몬스터를 생성하고 해당 프리팹을 가진 오브젝트를 변수에 저장하고 오브젝트를 자식 오브젝트로 만든다.
            GameObject childs = Instantiate(enemy, transform.position, Quaternion.identity);
            childs.transform.SetParent(transform);

            //곧바로 생성되지 않도록 딜레이를 걸어준다
            yield return new WaitForSeconds(respawnTime);
        }
    }
}
