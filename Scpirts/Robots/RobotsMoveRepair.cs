using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotsMoveRepair : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform posRepairFactory;
    private Transform posRepairTurret;

    private Transform posWait;
    private Transform shopUpGradePos;


    private RobotsSpawn robotsSpawn;
    private MainManager mainManagerScripts;
    private HealthManager healthManager;

    private bool isRepairFactory;
    private bool isRepairTurret;
    private bool isRepair;
    private bool isRepaing;
    public bool isUpgrade;

    private GameObject currentTurret;
    private GameObject mainManager;
    public int repairForce;
    public float durationRepair;

    private Coroutine repair;

    private PauseManager pauseManager;
    private void Start()
    {
        mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            robotsSpawn = mainManager.GetComponent<RobotsSpawn>();
            mainManagerScripts = mainManager.GetComponent<MainManager>();
            healthManager = mainManager.GetComponent<HealthManager>();
            pauseManager = mainManager.GetComponent<PauseManager>();

        }
        StartCoroutine(UpdateInformation());
        StartCoroutine(CheckHealthBuild());


        posRepairFactory = robotsSpawn.posWork;
        posWait = robotsSpawn.waitPos;
        shopUpGradePos = robotsSpawn.shopPos;
        repairForce = robotsSpawn.repairForce;
        durationRepair = robotsSpawn.durationRepair;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void Update()
    {
        if (!pauseManager.isPause)
        {

            if (isUpgrade)
            {
                MoveToBuild(shopUpGradePos);
                //agent.SetDestination(shopUpGradePos.position);
            }
            else
            {
                if (!isRepair)
                {
                    MoveToBuild(posWait);
                    //agent.SetDestination(posWait.position);
                }
                else
                {
                    if (isRepairFactory)
                        MoveToBuild(posRepairFactory);
                        //agent.SetDestination(posRepairFactory.position);
                    if (isRepairTurret && posRepairTurret != null)
                        MoveToBuild(posRepairTurret);
                    //agent.SetDestination(posRepairTurret.position);
                }
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }
    private void MoveToBuild(Transform transformToMove)
    {
        Vector3 currentPosition = transform.position;
        Vector3 movement = transformToMove.position - currentPosition;
        movement.Normalize();
        agent.SetDestination(transformToMove.position);
        if (movement.x > 0)
        {
            // Двигается вправо
            transform.localScale = new Vector3(-0.5f, 0.5f, 1f); // Обычный масштаб (вправо)
        }
        else if (movement.x < 0)
        {
            // Двигается влево
            transform.localScale = new Vector3(0.5f, 0.5f, 1f); // Отзеркаленный масштаб (влево)
        }
        else
        {
            // Если не движется по оси X, можно оставить масштаб прежним или установить его в нейтральное состояние
            transform.localScale = new Vector3(0.5f, 0.5f, 1f); // Нейтральный масштаб
        }
    }
    private IEnumerator UpdateInformation()
    {
        while (true)
        {
            if (currentTurret == null)
            {
                GetCountTurret();
            }
            yield return new WaitForSeconds(1);

        }
    }
    private IEnumerator CheckHealthBuild()
    {
        while (true)
        {
            if (!isRepair)
            {
                if (healthManager.currentHealth < healthManager.maxHealth)
                {
                    isRepairFactory = true;
                    isRepair = true;
                }
                if (currentTurret != null)
                {
                    Turret turret = currentTurret.GetComponent<Turret>();
                    if (turret.health < turret.maxHealth)
                    {
                        posRepairTurret = currentTurret.transform;
                        isRepairTurret = true;
                        isRepair = true;
                    }
                }
            }
            yield return new WaitForSeconds(2);
        }
    }
    private void GetCountTurret()
    {
        GameObject[] currentTurrets = GameObject.FindGameObjectsWithTag("Turret");
        if (currentTurrets.Length != 0)
        {
            foreach (GameObject currentTurretFor in currentTurrets)
            {
                Turret turret = currentTurretFor.GetComponent<Turret>();
                if (turret != null)
                {
                    if (turret.health < turret.maxHealth)
                    {
                        currentTurret = currentTurretFor; // Сохраняем текущую турель, если она нуждается в лечении
                        break; // Если вы хотите остановиться на первой найденной турели, которая нуждается в лечении
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Shop"))
        {
            Shop shop = collision.GetComponent<Shop>();
            repairForce = shop.repairForce;
            durationRepair = shop.durationRepair;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Factory"))
        {
            if (healthManager.currentHealth >= healthManager.maxHealth)
            {
                isRepairFactory = false;
                isRepair = false;
                isRepaing = false;
                if (repair != null)
                    StopCoroutine(repair);
            }
            else
            {
                if (!isRepaing)
                    repair = StartCoroutine(RepairBuild(mainManager));
            }
        }
        if (collision != null && collision.CompareTag("Turret"))
        {

        }
        if (collision != null && collision.CompareTag("Shop"))
        {
            isUpgrade = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Turret"))
        {
            Turret turret = collision.gameObject.GetComponent<Turret>();
            if (turret.health >= turret.maxHealth)
            {
                isRepairTurret = false;
                isRepair = false;
                isRepaing = false;
                currentTurret = null;
                if (repair != null)
                    StopCoroutine(repair);
            }
            else
            {
                if (!isRepaing && currentTurret != null)
                    repair = StartCoroutine(RepairBuild(currentTurret));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paws") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
    private IEnumerator RepairBuild(GameObject repair)
    {
        isRepaing = true;
        HealthManager healthManager = repair.GetComponent<HealthManager>();
        Turret turret = repair.GetComponent<Turret>();
        while (true)
        {
            if (turret != null)
            {
                if (turret.health < turret.maxHealth)
                {
                    turret.health += repairForce;
                    turret.UpdateHealthSlider();
                }
            }
            if (healthManager != null)
            {
                if (healthManager.currentHealth < healthManager.maxHealth)
                {

                    healthManager.currentHealth += repairForce;
                    healthManager.UpdateHealthSlider();
                }
            }
            yield return new WaitForSeconds(durationRepair);

        }
    }

}
