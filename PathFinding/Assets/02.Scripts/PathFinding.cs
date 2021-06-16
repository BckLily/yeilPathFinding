using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    // �ڵ� ��ü�� Transform�� �ƴ϶� Vector3�� �ؼ� ����������
    // ������ �ǵ� Transform���� �ؼ� ���ǳ���.
    // ��ģ�� ��¥ ������

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
     * Dictionary Serialize �ϴ� ���.
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

        // ���� ������Ʈ(�̵��ϴ� ������Ʈ) ��ġ
        // ���� ���� �ٴ� ������Ʈ�� ���ؼ� 0.5 ĭ ���� ��ġ�� �ִ�.
        var thisObjectTr = GetComponent<Transform>();

        var road = Physics.OverlapSphere(thisObjectTr.position, 0.51f, 1 << 6);

        startTr = road[0].GetComponent<Transform>();

        Vector3 pos = new Vector3();
        pos.x = startTr.position.x;
        pos.y = (float)(startTr.position.y - 0.5);
        pos.z = startTr.position.z;


        // ������ ������Ʈ(�ǹ� ������Ʈ) ��ġ
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
            // �������� �Ÿ��� �������� �Ÿ����� �ְ�, ���� ������ ��(X)

            //Debug.Log("hereToDestDiffX: " + hereToDestDiffX + " hereToNextDiffX" + hereToNextDiffX);
            //Debug.Log("hereToDestDiffZ: " + hereToDestDiffZ + " hereToNextDiffZ" + hereToNextDiffZ);
            //Debug.Log("End Road Count: " + GameManager.instance.endRoadTr.Count);

            // �߰� ������ ���� �� ������ �߰� ���� ó���� �� �Ǹ� ���� ��  ����.
            // �߰� ������ ��� ó���ؾ� ������ ���� �ȿ�����....

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
                    // Z���� ������, hereTr�� NextTr ���̿� endRoadTr�� ���� ���
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
            // �������� �Ÿ��� �������� �Ÿ����� �ְ�, ���� ������ ��(Z)
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
                    // Z���� ������, hereTr�� NextTr ���̿� endRoadTr�� ���� ���
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
            // �׿�(�ٸ� �����̿��� �������� �ִ� ���)
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
                    // Z���� ������, hereTr�� NextTr ���̿� endRoadTr�� ���� ���
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
                    // Z���� ������, hereTr�� NextTr ���̿� endRoadTr�� ���� ���
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
            // �׿�(�������� ������ �������� �ִ� ���)
            //else if()


            // Cross Road �� Cross Road ���̿� EndRoad ��, �濡 ������ �ִ��� ������ Ȯ���� �ؾ��Ѵ�.

            // �������� ���� saveTr�� ���� ���, �������� NextRoadTr�� �߰�, count 1����, checkCount 0���� �ʱ�ȭ,
            // isAdd �߰��Ұ�? ��� ������ true false.
            // true�̸� �߰��Ѵ�.
            // �������� �߰��ϴ� ���, ���� �������� �߰��ϸ�, checkCount�� 0����, count�� 1�����Ѵ�. 
            // ���� �������� �߰��� �� ���� ���, ���� �������� ���ư���, ���� �������� �����ϰ�, ���� �������� count�� �⺻ count�� �����Ѵ�.
            // ���� �������� �̹� ������ �ִ� ���, �߰����� �ʴ´�.(�Ϲ������� ���� �������� ������ �ִ°��� ���� ��ġ�� ���� ��ġ�� ���� ����� Ȯ���� ����.

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
                    // �̵� ��ΰ� �ƴ� ��� Ȯ��.
                    // ���� Road�� nearRoad�� ��Ȳ���� ���� ����(���� crossRoad)�� Road�� �ƴ� ���.
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
