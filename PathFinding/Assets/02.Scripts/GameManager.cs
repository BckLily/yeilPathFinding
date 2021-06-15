using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Transform> crossRoadTr;

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
        // 지금은 이미 오브젝트가 있기 때문에 여기서 오브젝트를 찾아서 처리하지만
        // 나중에는 생성되는 오브젝트(차)가 이미 설정된 도로 위를 돌아다닐 것이라서 이 처리를 안해주고
        // Path를 설정할 때(도로가 새로 생성되거나, 삭제되거나 등등..) 처리하면 된다.

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
