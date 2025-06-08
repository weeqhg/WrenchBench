using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotsSpawn : MonoBehaviour
{
    public GameObject robotWork;
    public GameObject robotRepair;
    public Transform spawnPos;
    private MainManager mainManager;
    public int coastRobotWork;
    public int coastRobotRepair;
    public Text coastWork;
    public Text coastRepair;

    public Transform posWork;
    public Transform posStorage;
    public Transform shopPos;
    public Transform waitPos;

    public float speedWork;
    public int bagSize;

    public int repairForce;
    public float durationRepair;
    private void Start()
    {
        mainManager = gameObject.GetComponent<MainManager>();
    }
    private void Update()
    {
        coastWork.text = "Цена: " + coastRobotWork;
        coastRepair.text = "Цена: " + coastRobotRepair;
    }
    public void SpawnRobotWork()
    {
        if (mainManager.moneys >= coastRobotWork)
        {
            Instantiate(robotWork, spawnPos.position, Quaternion.identity);
            mainManager.moneys -= coastRobotWork;
            coastRobotWork *= 3;
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
    public void SpawnRobotRepair()
    {
        if (mainManager.moneys >= coastRobotRepair)
        {
            Instantiate(robotRepair, spawnPos.position, Quaternion.identity);
            mainManager.moneys -= coastRobotRepair;
            coastRobotRepair *= 2;
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
}
