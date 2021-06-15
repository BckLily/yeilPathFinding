using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
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

    List<Transform> saveTr;


    void Start()
    {
        gameManager = GameManager.instance;
        //endTr = GameObject.FindGameObjectWithTag("END").GetComponent<Transform>();
        //startTr = GameObject.FindGameObjectWithTag("START").GetComponent<Transform>();
        //car = GameObject.FindGameObjectWithTag("CAR");
        //Instantiate(car, startTr.position, startTr.rotation);

        // 현재 오브젝트(이동하는 오브젝트) 위치
        // 받은 값은 바닥 오브젝트에 비해서 한 칸 높은 위치에 있다.
        startTr = GetComponent<Transform>();
        // 목적지 오브젝트(건물 오브젝트) 위치
        destiTr = GameObject.FindGameObjectWithTag("END").GetComponent<Transform>();



        //crossRoadDist = new List<float>();
        crossRoadDict = new SortedDictionary<float, Transform>();
        saveTr = new List<Transform>();

        //Key & Value
        _key = new List<float>();
        _value = new List<Transform>();

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

            crossRoadDict.Add(Vector3.SqrMagnitude(roadTr.position - startTr.position), roadTr);
            // Key & Value
            _key.Add(Vector3.SqrMagnitude(roadTr.position - startTr.position));
            _value.Add(roadTr);

        }
        _key.Sort();
    }

    private void OnRenderObject()
    {
    }

    IEnumerator CreatePath()
    {
        int count = 1;
        saveTr.Add(startTr);
        //startTr
        //destiTr

        while (!isFind)
        {
            yield return 0.1f;

            Transform nextRoadTr = crossRoadDict[_key[count]];
            Transform hereTr = crossRoadDict[_key[count - 1]];
            float hereToDestDiffX = destiTr.position.x - hereTr.position.x;
            float hereToDestdiffZ = destiTr.position.z - hereTr.position.z;

            float hereToCrossDiffX = nextRoadTr.position.x - hereTr.position.x;
            float hereToCrossDiffZ = nextRoadTr.position.z - hereTr.position.z;


            if (saveTr[count - 1].position.x == nextRoadTr.position.x)
            {

            }

        }



    }

    // Update is called once per frame
    void Update()
    {


    }


}
