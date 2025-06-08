using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotsMoveWork : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform posWork;
    private Transform posStorage;
    private Transform shopUpGradePos;

    public int bag;
    private int capacity;
    private float duration;
    private RobotsSpawn robotsSpawn;
    private MainManager mainManagerScripts;
    private PauseManager pauseManager;
    private bool isActiveFullingBug;
    private bool isActiveEmptyBug;
    private bool isEmpty;
    private bool isMining;
    public bool isUpgrade;
    private void Start()
    {
        GameObject mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            robotsSpawn = mainManager.GetComponent<RobotsSpawn>();
            mainManagerScripts = mainManager.GetComponent<MainManager>();
            pauseManager = mainManager.GetComponent<PauseManager>();
        }
        posWork = robotsSpawn.posWork;
        posStorage = robotsSpawn.posStorage;
        shopUpGradePos = robotsSpawn.shopPos;
        capacity = robotsSpawn.bagSize;
        duration = robotsSpawn.speedWork;
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
                if (bag < capacity)
                {
                    if (!isActiveFullingBug)
                    {
                        MoveToBuild(posWork);
                        //agent.SetDestination(posWork.position);
                    }
                    else if (isActiveFullingBug && !isMining)
                    {
                        FullingBag();
                    }
                }
                else if (bag >= capacity)
                {
                    if (!isActiveEmptyBug)
                    {
                        MoveToBuild(posStorage);
                        //agent.SetDestination(posStorage.position);
                    }
                    else if (isActiveEmptyBug && !isEmpty)
                    {
                        EmptyingBag();
                    }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Shop"))
        {
            Shop shop = collision.GetComponent<Shop>();
            duration = shop.duration;
            capacity = shop.bagSize;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pauseManager.isPause)
        {
            if (collision != null && collision.CompareTag("Factory"))
            {
                isActiveFullingBug = true;
            }
            if (collision != null && collision.CompareTag("Storage"))
            {
                isActiveEmptyBug = true;
            }
            if (collision != null && collision.CompareTag("Shop"))
            {
                isUpgrade = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!pauseManager.isPause)
        {
            if (collision != null && collision.CompareTag("Factory"))
            {
                isActiveFullingBug = false;
            }
            if (collision != null && collision.CompareTag("Storage"))
            {
                isActiveEmptyBug = false;
            }
        }
        else
        {
            isActiveFullingBug = false;
            isActiveEmptyBug = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paws") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void FullingBag()
    {
        isMining = true;
        StartCoroutine(Mining());

    }

    private IEnumerator Mining()
    {
        for (int i = 0; i <= capacity; i++)
        {
            if (bag == capacity)
            {
                isMining = false;
                break;
            }
            if (bag > capacity)
            {
                int value = bag - capacity;
                bag -= value;
                isMining = false;
                break;
            }
            bag += mainManagerScripts.valueProduction;

            yield return new WaitForSeconds(duration);
        }
        isMining = false;
    }

    private void EmptyingBag()
    {
        isEmpty = true;
        StartCoroutine(Emptying());

    }
    private IEnumerator Emptying()
    {
        for (int i = 0; i < capacity; i++)
        {
            bag--;
            mainManagerScripts.moneys += 1;
            mainManagerScripts.countMoneys += 1;
            yield return new WaitForSeconds(duration);
        }
        isEmpty = false;
    }

}
