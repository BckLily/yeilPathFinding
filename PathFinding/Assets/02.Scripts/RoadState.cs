using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoadState : MonoBehaviour
{
    Transform roadTr;

    [SerializeField]
    List<eBlockState> nearRoadsState;




    // normal >> 일반적인 직선 도로. 연결부가 직선으로 2개가 있는 도로
    // cross >> 교차로 등의 꺽이는 도로가 있는 도로. 연결부의 도로가 적어도 하나 90도를 이루고 있는 도로.
    // end >> 도로의 끝 지점. 연결부가 하나밖에 없는 도로
    public enum eState
    {
        normalRoad, crossRoad, endRoad,
    }

    [SerializeField]
    eState _blockState;

    public eState blockState
    {
        get
        {
            return _blockState;
        }
    }

    float root2 = 1.41f;

    enum eBlockState
    {
        Field, Road, 
    }


    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        roadTr = GetComponent<Transform>();
        
        nearRoadsState = new List<eBlockState>();

        for(int i = 0; i < 4; i++)
        {
            nearRoadsState.Add(eBlockState.Field);
        }
        /*
         *        O x + 1
         *z - 1 O X O z + 1
         *        O x - 1
         * O >> 저장하는 위치
         * X >> 현재 위치
         */

        // 현재 위치 저장.

        NearRoadCheck();
    }


    // 추가적인 건설이 발생하면, 건설된 Road 근처의 Road 만 불러와서 그 Road들의 NearRoadCheck를 진행하여 상태 변화 확인하면 된다.
    // 그건 RoadGameManager 같은거 만들어서 처리할 수 있으면 하면 될 것 같은데 아니면
    // BuildScript를 실행하는 도중에 처리하려면 할 수 있을거고.
    public void NearRoadCheck()
    {
        // X: +1, Z: +1, X: -1, Z: -1
        // 현재 Road 블럭의 어떤 방향에 Road Block이 있는지 확인.
        Collider[] nearRoads = Physics.OverlapSphere(roadTr.position, root2, 1 << 6);
        int roadsCount = 0;

        //print("nearRoadsLength: " + nearRoads.Length);

        if (nearRoads.Length == 0)
            return;


        foreach (var road in nearRoads)
        {
            var nearRoadTr = road.GetComponent<Transform>();
            if (Mathf.Round(nearRoadTr.position.x) == (Mathf.Round(roadTr.position.x) + 1) && Mathf.Round(nearRoadTr.position.z) == Mathf.Round(roadTr.position.z))
            {
                //print("nearRoadsState[0]\n" + roadTr.position);
                nearRoadsState[0] = eBlockState.Road;
                roadsCount++;
            }
            else if (Mathf.Round(nearRoadTr.position.x) == (Mathf.Round(roadTr.position.x)) && Mathf.Round(nearRoadTr.position.z) == Mathf.Round(roadTr.position.z) - 1)
            {
                //print("nearRoadsState[1]\n" + roadTr.position);
                nearRoadsState[1] = eBlockState.Road;
                roadsCount++;
            }
            else if (Mathf.Round(nearRoadTr.position.x) == (Mathf.Round(roadTr.position.x)) && Mathf.Round(nearRoadTr.position.z) == Mathf.Round(roadTr.position.z) + 1)
            {
                //print("nearRoadsState[2]\n" + roadTr.position);
                nearRoadsState[2] = eBlockState.Road;
                roadsCount++;
            }
            else if (Mathf.Round(nearRoadTr.position.x) == (Mathf.Round(roadTr.position.x) - 1) && Mathf.Round(nearRoadTr.position.z) == Mathf.Round(roadTr.position.z))
            {
                //print("nearRoadsState[3]\n" + roadTr.position);
                nearRoadsState[3] = eBlockState.Road;
                roadsCount++;
            }
        }

        //print("roadsCount: " + roadsCount);

        ThisRoadState(roadsCount);
        roadsCount = 0;
    }
    
    void ThisRoadState(int roadsCount)
    {

        if (roadsCount >= 3)
        {
            _blockState = eState.crossRoad;
            GameManager.instance.crossRoadTr.Add(this.transform);
            
        }
        else if (roadsCount == 1)
        {
            _blockState = eState.endRoad;
            GameManager.instance.endRoadTr.Add(this.transform);
        }
        else if (roadsCount == 2)
        {
            if(nearRoadsState[0] == nearRoadsState[3] && nearRoadsState[1] == nearRoadsState[2])
            {
                _blockState = eState.normalRoad;
            }
            else
            {
                _blockState = eState.crossRoad;
                GameManager.instance.crossRoadTr.Add(this.transform);
            }
        }
        else
        {
            _blockState = eState.endRoad;
            GameManager.instance.endRoadTr.Add(this.transform);
        }
    }

    // direct (0~3)
    public bool GetNearRoadState(int direct)
    {
        //     0
        //   1   2
        //     3
        if (nearRoadsState[direct] == eBlockState.Road)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
