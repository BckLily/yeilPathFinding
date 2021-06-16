using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    // 코드 자체를 Transform이 아니라 Vector3로 해서 진행했으면
    // 편했을 건데 Transform으로 해서 개판났다.
    // 미친다 진짜 ㅋㅋㅋ

    GameManager gameManager;

    //Transform endTr;
    //Transform startTr;
    //GameObject car;

    // Start is called before the first frame update
    [SerializeField]
    Transform startTr;
    [SerializeField]
    Transform destiTr;

    //[SerializeField]
    //List<float> crossRoadDist;

    //Distionary Key & Value Check
    [SerializeField]
    List<float> _key;
    [SerializeField]
    List<Transform> _value;

    public bool isFind = false;

    SortedDictionary<float, Transform> crossRoadDict;
    /*
     * https://redforce01.tistory.com/243
     * Dictionary Serialize 하는 방법.
     */

    [SerializeField]
    List<Transform> saveTr;

    int count = 0;
    int checkCount = 0;
    bool isAdd = false;

    void Start()
    {
        gameManager = GameManager.instance;
        //endTr = GameObject.FindGameObjectWithTag("END").GetComponent<Transform>();
        //startTr = GameObject.FindGameObjectWithTag("START").GetComponent<Transform>();
        //car = GameObject.FindGameObjectWithTag("CAR");
        //Instantiate(car, startTr.position, startTr.rotation);

        // 현재 오브젝트(이동하는 오브젝트) 위치
        // 받은 값은 바닥 오브젝트에 비해서 0.5 칸 높은 위치에 있다.
        var thisObjectTr = GetComponent<Transform>();

        var road = Physics.OverlapSphere(thisObjectTr.position, 0.51f, 1 << 6);

        startTr = road[0].GetComponent<Transform>();

        Vector3 pos = new Vector3();
        pos.x = startTr.position.x;
        pos.y = (float)(startTr.position.y - 0.5);
        pos.z = startTr.position.z;


        // 목적지 오브젝트(건물 오브젝트) 위치
        destiTr = Physics.OverlapSphere(GameObject.FindGameObjectWithTag("END").GetComponent<Transform>().position, 0.51f, 1 << 6)[0].GetComponent<Transform>();


        //crossRoadDist = new List<float>();
        crossRoadDict = new SortedDictionary<float, Transform>();
        saveTr = new List<Transform>();

        //Key & Value
        _key = new List<float>();
        _value = new List<Transform>();

        saveTr.Add(startTr);


        if (gameManager.crossRoadTr.Count > 0)
            setCrossRoadDist();
    }

    public void setCrossRoadDist()
    {

        Dictionary<Transform, float> roadDict = new Dictionary<Transform, float>();
        //List<float> crossRoadDist = new List<float>();
        foreach (var roadTr in gameManager.crossRoadTr)
        {
            //crossRoadDist.Add(Vector3.SqrMagnitude(roadTr.position - startTr.position));
            try
            {
                crossRoadDict.Add(Vector3.SqrMagnitude(roadTr.position - startTr.position), roadTr);
                // Key & Value
                _key.Add(Vector3.SqrMagnitude(roadTr.position - startTr.position));
                _value.Add(roadTr);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Exception " + ex + " is Occure");
            }
        }
        _key.Sort();
    }


    IEnumerator CreatePath()
    {


        //startTr
        //destiTr

        while (!isFind)
        {
            if (checkCount >= crossRoadDict.Count)
            {
                Debug.Log("Check Count Reset");
                checkCount = 0;

                //saveTr.Remove(hereTr);
                //checkCount = _value.IndexOf(hereTr);
                //count--;
            }

            yield return 0.1f;

            //Debug.Log("Check Road...");

            Transform nextRoadTr = crossRoadDict[_key[checkCount]];
            Transform hereTr = saveTr[count];

            Debug.Log("check Count: " + checkCount + " count: " + count);
            Debug.Log("hereTr: " + hereTr + " nextRoadTr: " + nextRoadTr);

            float hereToDestDiffX = Mathf.Abs(destiTr.position.x) - Mathf.Abs(hereTr.position.x);
            float hereToDestDiffZ = Mathf.Abs(destiTr.position.z) - Mathf.Abs(hereTr.position.z);

            float hereToNextDiffX = Mathf.Abs(nextRoadTr.position.x) - Mathf.Abs(hereTr.position.x);
            float hereToNextDiffZ = Mathf.Abs(nextRoadTr.position.z) - Mathf.Abs(hereTr.position.z);

            // Find Road 210615
            // 목적지의 거리가 교차점의 거리보다 멀고, 같은 방향일 때(X)

            //Debug.Log("hereToDestDiffX: " + hereToDestDiffX + " hereToNextDiffX" + hereToNextDiffX);
            //Debug.Log("hereToDestDiffZ: " + hereToDestDiffZ + " hereToNextDiffZ" + hereToNextDiffZ);
            //Debug.Log("End Road Count: " + GameManager.instance.endRoadTr.Count);

            // 중간 조건이 망한 것 같은데 중간 조건 처리만 잘 되면 좋을 것  같다.
            // 중간 조건을 어떻게 처리해야 할지는 감이 안오지만....

            //if (Mathf.Abs(hereToDestDiffX) > Mathf.Abs(hereToNextDiffX) && hereToDestDiffX * hereToNextDiffX >= 0)
            if (hereToDestDiffX * hereToNextDiffX >= 0)
            {
                Debug.Log("1. Start X...!");
                foreach (var tr in GameManager.instance.endRoadTr)
                {
                    //Debug.Log("tr.position.z: " + Mathf.Round(tr.position.z) + " hereTr.position.z: " + Mathf.Round(hereTr.position.z));
                    if (tr.position == hereTr.position)
                    {
                        Debug.LogError("tr == hereTr");
                        continue;
                    }
                    // Z축이 같은데, hereTr과 NextTr 사이에 endRoadTr이 없는 경우
                    if (Mathf.Round(nextRoadTr.position.z) == Mathf.Round(hereTr.position.z) &&
                        !(((Mathf.Round(tr.position.x) > Mathf.Round(hereTr.position.x)) && (Mathf.Round(tr.position.x) < Mathf.Round(nextRoadTr.position.x))) ||
                        ((Mathf.Round(tr.position.x) < Mathf.Round(hereTr.position.x)) && (Mathf.Round(tr.position.x) > Mathf.Round(nextRoadTr.position.x)))))
                    {
                        Debug.Log("X isAdd Set true");
                        isAdd = true;
                        break;
                    }
                    else
                    {
                        Debug.Log("X isAdd Set false");
                        isAdd = false;
                        continue;
                    }
                }
                Debug.Log("X isAdd: " + isAdd);
            }
            // 목적지의 거리가 교차점의 거리보다 멀고, 같은 방향일 때(Z)
            //if (Mathf.Abs(hereToDestDiffZ) > Mathf.Abs(hereToNextDiffZ) && hereToDestDiffZ * hereToNextDiffZ >= 0)
            else if (hereToDestDiffZ * hereToNextDiffZ >= 0)
            {
                Debug.Log("2. Start Z...!");
                foreach (var tr in GameManager.instance.endRoadTr)
                {
                    //Debug.Log("tr.position.x: " + Mathf.Round(tr.position.x) + " hereTr.position.x: " + Mathf.Round(hereTr.position.x));
                    if (tr.position == hereTr.position)
                    {
                        Debug.LogError("tr == hereTr");
                        continue;
                    }
                    // Z축이 같은데, hereTr과 NextTr 사이에 endRoadTr이 없는 경우
                    if (Mathf.Round(nextRoadTr.position.x) == Mathf.Round(hereTr.position.x) && 
                        !(((Mathf.Round(tr.position.z) > Mathf.Round(hereTr.position.z)) && (Mathf.Round(tr.position.z) < Mathf.Round(nextRoadTr.position.z))) || 
                        ((Mathf.Round(tr.position.z) < Mathf.Round(hereTr.position.z)) && (Mathf.Round(tr.position.z) > Mathf.Round(nextRoadTr.position.z)))))
                    {
                        Debug.Log("Z isAdd Set true");
                        isAdd = true;
                        break;
                    }
                    else
                    {
                        Debug.Log("Z isAdd Set false");
                        isAdd = false;
                        continue;
                    }
                }

                Debug.Log("Z isAdd: " + isAdd);

            }
            // 그외(다른 방향이여도 교차점이 있는 경우)
            //else if (Mathf.Abs(hereToDestDiffX) > Mathf.Abs(hereToNextDiffX) && hereToDestDiffX * hereToNextDiffX <= 0)
            else if (hereToDestDiffX * hereToNextDiffX <= 0)
            {
                Debug.Log("3. Start -X...!");
                foreach (var tr in GameManager.instance.endRoadTr)
                {
                    //Debug.Log("tr.position.z: " + Mathf.Round(tr.position.z) + " hereTr.position.z: " + Mathf.Round(hereTr.position.z));
                    if (tr.position == hereTr.position)
                    {
                        Debug.LogError("tr == hereTr");
                        continue;
                    }
                    // Z축이 같은데, hereTr과 NextTr 사이에 endRoadTr이 없는 경우
                    if (Mathf.Round(nextRoadTr.position.z) == Mathf.Round(hereTr.position.z) && 
                        !(((Mathf.Round(tr.position.x) > Mathf.Round(hereTr.position.x)) && (Mathf.Round(tr.position.x) < Mathf.Round(nextRoadTr.position.x)))||
                        ((Mathf.Round(tr.position.x) < Mathf.Round(hereTr.position.x)) && (Mathf.Round(tr.position.x) > Mathf.Round(nextRoadTr.position.x)))))
                    {
                        Debug.Log("-X isAdd Set true");
                        isAdd = true;
                        break;
                    }
                    else
                    {
                        Debug.Log("-X isAdd Set false");
                        isAdd = false;
                        continue;
                    }
                }
                Debug.Log("-X isAdd: " + isAdd);
            }
            //else if (Mathf.Abs(hereToDestDiffZ) > Mathf.Abs(hereToNextDiffZ) && hereToDestDiffZ * hereToNextDiffZ <= 0)
            else if (hereToDestDiffZ * hereToNextDiffZ <= 0)
            {
                Debug.Log("4. Start -Z...!");
                foreach (var tr in GameManager.instance.endRoadTr)
                {
                    //Debug.Log("tr.position.x: " + Mathf.Round(tr.position.x) + " hereTr.position.x: " + Mathf.Round(hereTr.position.x));
                    if (tr.position == hereTr.position)
                    {
                        Debug.LogError("tr == hereTr");
                        continue;
                    }
                    // Z축이 같은데, hereTr과 NextTr 사이에 endRoadTr이 없는 경우
                    if (Mathf.Round(nextRoadTr.position.x) == Mathf.Round(hereTr.position.x) && 
                        !(((Mathf.Round(tr.position.z) > Mathf.Round(hereTr.position.z)) && (Mathf.Round(tr.position.z) < Mathf.Round(nextRoadTr.position.z)))||
                        ((Mathf.Round(tr.position.z) < Mathf.Round(hereTr.position.z)) && (Mathf.Round(tr.position.z) > Mathf.Round(nextRoadTr.position.z)))))
                    {
                        Debug.Log("-Z isAdd Set true");
                        isAdd = true;
                        break;
                    }
                    else
                    {
                        Debug.Log("-Z isAdd Set false");
                        isAdd = false;
                        continue;
                    }
                }
                Debug.Log("-Z isAdd: " + isAdd);
            }
            // 그외(교차점이 없지만 목적지가 있는 경우)
            //else if()


            // Cross Road 와 Cross Road 사이에 EndRoad 즉, 길에 끊김이 있는지 없는지 확인을 해야한다.

            // 목적지가 현재 saveTr에 없는 경우, 목적지를 NextRoadTr로 추가, count 1증가, checkCount 0으로 초기화,
            // isAdd 추가할거? 라는 변수가 true false.
            // true이면 추가한다.
            // 목적지를 추가하는 경우, 다음 목적지를 추가하며, checkCount는 0으로, count는 1증가한다. 
            // 다음 목적지를 추가할 수 없는 경우, 이전 목적지로 돌아가며, 현재 목적지를 제거하고, 현재 목적지의 count를 기본 count로 설정한다.
            // 다음 목적지를 이미 가지고 있는 경우, 추가하지 않는다.(일반적으로 다음 목적지를 가지고 있는것은 이전 위치와 다음 위치가 같은 경우일 확률이 높다.

            //Debug.Log("Out Of Check...");

            if (saveTr.Contains(nextRoadTr))
            {
                Debug.Log("Contains..." + nextRoadTr);

                isAdd = false;
                try
                {
                    int index = saveTr.IndexOf(nextRoadTr);

                    Debug.Log("RemoveRange("+(index+1)+" ," + (saveTr.Count-index)+") ");
                    saveTr.RemoveRange(index + 1, saveTr.Count - index);
                    checkCount = _value.IndexOf(nextRoadTr);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Exception: " + ex);
                }

                checkCount++;
                continue;

            }
            //Debug.Log("Before Add..");

            float x = Mathf.Round(nextRoadTr.position.x - hereTr.position.x);
            float z = Mathf.Round(nextRoadTr.position.z - hereTr.position.z);

            int goX = x > 0 ? 0 : 3;
            int goZ = z > 0 ? 2 : 1;

            checkCount++;
            Debug.Log("check Count is: " + checkCount);



            GameObject hereRoad = Physics.OverlapSphere(hereTr.position, 0.5f, 1 << 6)[0].gameObject;
            Debug.Log("GameObject is: " + hereRoad);
            Debug.Log("hereTr.position: " + hereTr.position);
            Debug.Log("isAdd: " + isAdd);


            Debug.Log("x: " + x + " goX: " + goX + " z: " + z + " goZ: " + goZ);
            Debug.Log("x_Block_State: " + hereRoad.GetComponent<RoadState>().GetNearRoadState(goX) + " z_Block_State: " + hereRoad.GetComponent<RoadState>().GetNearRoadState(goZ));

            if (isAdd)
            {
                Vector3 checkVector = nextRoadTr.position - hereTr.position;
                Debug.Log("Vector3: " + checkVector);
                if (Mathf.Round(checkVector.x) * Mathf.Round(checkVector.z) != 0)
                {
                    Debug.Log("Out 1");
                    isAdd = false;
                }
                else if (x != 0)
                {
                    // 이동 경로가 아닌 경우 확인.
                    // 현재 Road의 nearRoad의 상황으로 진행 방향(다음 crossRoad)이 Road가 아닐 경우.
                    if (!hereRoad.GetComponent<RoadState>().GetNearRoadState(goX))
                    {
                        Debug.Log("Out 2");
                        isAdd = false;
                    }

                }
                else if (z != 0)
                {
                    if (!hereRoad.GetComponent<RoadState>().GetNearRoadState(goZ))
                    {
                        Debug.Log("Out 3");
                        isAdd = false;
                    }
                }

                if(isAdd)
                {
                    Debug.Log("Add Road..!");
                    saveTr.Add(nextRoadTr);
                    checkCount = 0;
                    count++;

                    isAdd = false;
                }
            }

            //Debug.Log("end isAdd: " + isAdd);



        }



    }



    // Update is called once per frame
    void Update()
    {
        if (!isFind)
            StartCoroutine(CreatePath());
        //else
        //    StopCoroutine(CreatePath());

    }


}
