using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeManager : MonoBehaviour
{
    private Shop shop;
    private Defense currentDefense;
    private RobotsSpawn robotsSpawn;
    private MainManager mainManager;
    private HealthManager healthManager;
    private DefenseManager defenseManager;

    public int[] coastUpGradeRobots;
    public Text[] coastUpGradeRobotsText;

    public int[] coastFactoryUpgrade;
    public Text[] coastFactoryUpgradeText;

    private void Start()
    {
        shop = GameObject.FindWithTag("Shop").GetComponent<Shop>();
        robotsSpawn = gameObject.GetComponent<RobotsSpawn>();
        mainManager = gameObject.GetComponent<MainManager>();
        healthManager = gameObject.GetComponent<HealthManager>();
        defenseManager = gameObject.GetComponent<DefenseManager>();
        if (robotsSpawn != null && shop != null)
        {
            shop.duration = robotsSpawn.speedWork;
            shop.bagSize = robotsSpawn.bagSize;
            shop.repairForce = robotsSpawn.repairForce;
            shop.durationRepair = robotsSpawn.durationRepair;
        }
        for (int i = 0; i < coastUpGradeRobots.Length; i++)
        {
            coastUpGradeRobotsText[i].text = "÷ÂÌ‡: " + coastUpGradeRobots[i];
        }
        for (int i = 0; i < coastFactoryUpgrade.Length; i++)
        {
            coastFactoryUpgradeText[i].text = "÷ÂÌ‡: " + coastFactoryUpgrade[i];
        }
    }
    public void UpGradeRobot()
    {
        GameObject[] pawsObjects = GameObject.FindGameObjectsWithTag("Paws");

        foreach (GameObject paw in pawsObjects)
        {
            RobotsMoveWork robotsMove = paw.GetComponent<RobotsMoveWork>();
            RobotsMoveRepair robotsRepair = paw.GetComponent<RobotsMoveRepair>();
            if (robotsMove != null) // œÓ‚ÂˇÂÏ, ˜ÚÓ ÍÓÏÔÓÌÂÌÚ ÒÛ˘ÂÒÚ‚ÛÂÚ
            {
                robotsMove.isUpgrade = true;
            }
            if (robotsRepair != null)
            {
                robotsRepair.isUpgrade = true;
            }
        }
    }
    public void UpBagSize(int i)
    {
        if (mainManager.moneys >= coastUpGradeRobots[i])
        {
            mainManager.moneys -= coastUpGradeRobots[i];
            coastUpGradeRobots[i] += coastUpGradeRobots[i] / 2;
            shop.bagSize++;
            coastUpGradeRobotsText[i].text = "÷ÂÌ‡: " + coastUpGradeRobots[i];
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
    public void UpSpeedWork(int i)
    {
        if (mainManager.moneys >= coastUpGradeRobots[i])
        {
            mainManager.moneys -= coastUpGradeRobots[i];
            coastUpGradeRobots[i] += coastUpGradeRobots[i] / 2;
            shop.duration -= 0.1f;
            coastUpGradeRobotsText[i].text = "÷ÂÌ‡: " + coastUpGradeRobots[i];
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
    public void UpForceRepair(int i)
    {
        if (mainManager.moneys >= coastUpGradeRobots[i])
        {
            mainManager.moneys -= coastUpGradeRobots[i];
            coastUpGradeRobots[i] += coastUpGradeRobots[i] / 2;
            shop.repairForce += 2;
            coastUpGradeRobotsText[i].text = "÷ÂÌ‡: " + coastUpGradeRobots[i];
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
    public void GetCurrentDefensePos(GameObject gameObject)
    {
        currentDefense = gameObject.GetComponent<Defense>();
    }
    public void UpGradeHealthBuild(int i)
    {
        if (mainManager.moneys >= coastFactoryUpgrade[i])
        {
            mainManager.moneys -= coastFactoryUpgrade[i];
            coastFactoryUpgrade[i] += coastFactoryUpgrade[i] / 2;
            healthManager.maxHealth += 20;
            defenseManager.maxHealth += 20;
            coastFactoryUpgradeText[i].text = "÷ÂÌ‡: " + coastFactoryUpgrade[i];
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
    public void UpGradeTurretDamage(int i)
    {
        if (currentDefense != null)
        {
            if (mainManager.moneys >= currentDefense.coastUpgrade[i])
            {
                mainManager.moneys -= currentDefense.coastUpgrade[i];
                currentDefense.coastUpgrade[i] *= 2;
                currentDefense.damage += 10;
                currentDefense.UpdateText();
            }
            else
            {
                mainManager.CallMessagesNoHaveMoney();
            }
        }
    }
    public void RepairTurret(int i)
    {
        if (currentDefense != null)
        {
            if (mainManager.moneys >= currentDefense.coastUpgrade[i])
            {
                mainManager.moneys -= currentDefense.coastUpgrade[i];
                currentDefense.coastUpgrade[i] *= 2;
                currentDefense.RepairTurret();
                currentDefense.UpdateText();
            }
            else
            {
                mainManager.CallMessagesNoHaveMoney();
            }
        }
    }
    public void UpGradeTurret—lipTurret(int i)
    {
        if (currentDefense != null)
        {
            if (mainManager.moneys >= currentDefense.coastUpgrade[i])
            {
                mainManager.moneys -= currentDefense.coastUpgrade[i];
                currentDefense.coastUpgrade[i] *=  2;
                currentDefense.clipTurret += 5;
                currentDefense.UpdateText();
            }
            else
            {
                mainManager.CallMessagesNoHaveMoney();
            }
        }
    }
    public void UpGradeTurretRecharge(int i)
    {
        if (currentDefense != null)
        {
            if (mainManager.moneys >= currentDefense.coastUpgrade[i])
            {
                mainManager.moneys -= currentDefense.coastUpgrade[i];
                currentDefense.coastUpgrade[i] += currentDefense.coastUpgrade[i] / 2;
                currentDefense.reloadTime -= 0.1f;
                currentDefense.UpdateText();
            }
            else
            {
                mainManager.CallMessagesNoHaveMoney();
            }
        }
    }
    public void UpGradeForceRepairPlayer(int i)
    {
        if (mainManager.moneys >= coastFactoryUpgrade[i])
        {
            mainManager.moneys -= coastFactoryUpgrade[i];
            coastFactoryUpgrade[i] += coastFactoryUpgrade[i];
            mainManager.repairForcePlayer += 2;
            coastFactoryUpgradeText[i].text = "÷ÂÌ‡: " + coastFactoryUpgrade[i];
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
    public void UpGradeproduction(int i)
    {
        if (mainManager.moneys >= coastFactoryUpgrade[i])
        {
            mainManager.moneys -= coastFactoryUpgrade[i];
            coastFactoryUpgrade[i] += coastFactoryUpgrade[i] / 2;
            mainManager.valueProduction += 1;
            coastFactoryUpgradeText[i].text = "÷ÂÌ‡: " + coastFactoryUpgrade[i];
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }

}
