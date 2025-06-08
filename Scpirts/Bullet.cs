using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject enemyPos;
    private Vector2 spawnPosition;
    private float speed = 15f;
    private int currentDamage;

    private float angle; // Угол стрельбы
    private Vector2 direction;
    private 
    void Start()
    {
        spawnPosition = transform.position;
        StartCoroutine(Death());
    }
    private void Update()
    {
        MovingBullet();
    }
    public void GetNeedValue(GameObject enemy, int damage)
    {
        enemyPos = enemy;
        currentDamage = damage;
    }
    public void Initialize(float angleInDegrees)
    {
        angle = angleInDegrees * Mathf.Deg2Rad; // Преобразование градусов в радианы
        transform.rotation = Quaternion.Euler(0f, 0f, 270f + angleInDegrees);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // Вычисляем направление
    }
    private void MovingBullet()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Enemy"))
        {
            if (enemyPos != null)
            {
                EnemyStats enemyStats = enemyPos.GetComponent<EnemyStats>();
                enemyStats.GetDamage(currentDamage);
            }
        }
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
