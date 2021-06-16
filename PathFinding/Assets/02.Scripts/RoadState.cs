using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoadState : MonoBehaviour
{
    Transform roadTr;

    [SerializeField]
    List<eBlockState> nearRoadsState;




    // normal >> �Ϲ����� ���� ����. ����ΰ� �������� 2���� �ִ� ����
    // cross >> ������ ���� ���̴� ���ΰ� �ִ� ����. ������� ���ΰ� ��� �ϳ� 90���� �̷�� �ִ� ����.
    // end >> ������ �� ����. ����ΰ� �ϳ��ۿ� ���� ����
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
         * O >> �����ϴ� ��ġ
         * X >> ���� ��ġ
         */

        // ���� ��ġ ����.

        NearRoadCheck();
    }


    // �߰����� �Ǽ��� �߻��ϸ�, �Ǽ��� Road ��ó�� Road �� �ҷ��ͼ� �� Road���� NearRoadCheck�� �����Ͽ� ���� ��ȭ Ȯ���ϸ� �ȴ�.
    // �װ� RoadGameManager ������ ���� ó���� �� ������ �ϸ� �� �� ������ �ƴϸ�
    // BuildScript�� �����ϴ� ���߿� ó���Ϸ��� �� �� �����Ű�.
    public void NearRoadCheck()
    {
        // X: +1, Z: +1, X: -1, Z: -1
        // ���� Road ���� � ���⿡ Road Block�� �ִ��� Ȯ��.
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
