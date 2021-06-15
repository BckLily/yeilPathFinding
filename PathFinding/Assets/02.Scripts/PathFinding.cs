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
     * Dictionary Serialize �ϴ� ���.
     */

    List<Transform> saveTr;


    void Start()
    {
        gameManager = GameManager.instance;
        //endTr = GameObject.FindGameObjectWithTag("END").GetComponent<Transform>();
        //startTr = GameObject.FindGameObjectWithTag("START").GetComponent<Transform>();
        //car = GameObject.FindGameObjectWithTag("CAR");
        //Instantiate(car, startTr.position, startTr.rotation);

        // ���� ������Ʈ(�̵��ϴ� ������Ʈ) ��ġ
        // ���� ���� �ٴ� ������Ʈ�� ���ؼ� �� ĭ ���� ��ġ�� �ִ�.
        startTr = GetComponent<Transform>();
        // ������ ������Ʈ(�ǹ� ������Ʈ) ��ġ
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
            float hereToDestDiffX = Mathf.Abs(destiTr.position.x) - Mathf.Abs(hereTr.position.x);
            float hereToDestdiffZ = Mathf.Abs(destiTr.position.z) - Mathf.Abs(hereTr.position.z);

            float hereToNextDiffX = Mathf.Abs(nextRoadTr.position.x) - Mathf.Abs(hereTr.position.x);
            float hereToNextDiffZ = Mathf.Abs(nextRoadTr.position.z) - Mathf.Abs(hereTr.position.z);

            // Find Road 210615
            // �������� �Ÿ��� �������� �Ÿ����� �ְ�, ���� ������ ��(X)
            if (Mathf.Abs(hereToDestDiffX) > Mathf.Abs(hereToNextDiffX) && hereToDestDiffX * hereToNextDiffX > 0)
            {

            }
            // �������� �Ÿ��� �������� �Ÿ����� �ְ�, ���� ������ ��(Z)
            // �׿�(�ٸ� �����̿��� �������� �ִ� ���)
            // �׿�(�������� ������ �������� �ִ� ���)

        }



    }

    // Update is called once per frame
    void Update()
    {


    }


}
