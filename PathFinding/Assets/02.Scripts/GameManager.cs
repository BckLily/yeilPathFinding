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

        endTr = GameObject.FindGameObjectWithTag("END").GetComponent<Transform>();
        startTr = GameObject.FindGameObjectWithTag("START").GetComponent<Transform>();

        car = GameObject.FindGameObjectWithTag("CAR");

        Instantiate(car, startTr.position, startTr.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
