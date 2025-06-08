using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform attackPos;
    private PauseManager pausedManager;
    private WaveManager waveManager;
    private Transform currentTurret;
    private List<GameObject> currentTurrets;
    private EnemyStats enemyStats;
    private void Start()
    {
        GameObject mainManager = GameObject.FindWithTag("MainManager");
        currentTurrets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Turret"));
        if (currentTurrets.Count != 0)
        {
            SelectTurret();
        }
        if (mainManager != null)
        {
            waveManager = mainManager.GetComponent<WaveManager>();
            pausedManager = mainManager.GetComponent<PauseManager>();
        }
        enemyStats = gameObject.GetComponent<EnemyStats>();
        attackPos = waveManager.attackPos;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = waveManager.speedEnemy;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void SelectTurret()
    {
        Turret turret = null;

        // Проверяем, есть ли доступные турели
        while (currentTurrets.Count > 0)
        {
            int i = Random.Range(0, currentTurrets.Count);

            if (currentTurrets[i] != null)
            {
                turret = currentTurrets[i].GetComponent<Turret>();

                // Если турель не завершила работу, выбираем её
                if (!turret.isDonWork)
                {
                    currentTurret = currentTurrets[i].transform;
                    return; // Выходим из метода, так как нашли подходящую турель
                }
            }

            // Удаляем текущую турель из списка, если она завершила работу
            currentTurrets.RemoveAt(i);
        }

        // Если не нашли ни одной подходящей турели
        if (turret == null)
        {
            Debug.Log("Нет доступных турелей для работы.");
        }
    }

    private void Update()
    {
        if (!pausedManager.isPause)
        {
            if (!enemyStats.isDeath)
            {

                if (currentTurret != null)
                {
                    Turret turret = currentTurret.GetComponent<Turret>();
                    if (!turret.isDonWork)
                    {
                        MoveToBuild(currentTurret);
                    }
                    else
                    {
                        currentTurret = null;
                    }
                    //agent.SetDestination(currentTurret.position);
                }
                else
                {
                    MoveToBuild(attackPos);
                    //agent.SetDestination(attackPos.position);
                }
            }
            else
            {
                MoveToBuild(transform);
                //agent.SetDestination(transform.position);
            }
        }
        else
        {
            MoveToBuild(transform);
            //agent.SetDestination(transform.position);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paws") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
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

}
