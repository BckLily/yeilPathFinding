using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    public List<Transform> crossRoadTr;
    [SerializeField]
    public List<Transform> endRoadTr;

    Transform endTr;
    Transform startTr;

    public GameObject car;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ������ �̹� ������Ʈ�� �ֱ� ������ ���⼭ ������Ʈ�� ã�Ƽ� ó��������
        // ���߿��� �����Ǵ� ������Ʈ(��)�� �̹� ������ ���� ���� ���ƴٴ� ���̶� �� ó���� �����ְ�
        // Path�� ������ ��(���ΰ� ���� �����ǰų�, �����ǰų� ���..) ó���ϸ� �ȴ�.

        PathFinding cubePathFinding = GameObject.Find("Cube").GetComponent<PathFinding>();
        cubePathFinding.setCrossRoadDist();


        //endTr = GameObject.FindGameObjectWithTag("END").GetComponent<Transform>();
        //startTr = GameObject.FindGameObjectWithTag("START").GetComponent<Transform>();

        //car = GameObject.FindGameObjectWithTag("CAR");

        //Instantiate(car, startTr.position, startTr.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}