using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damageToPlayer;
    public int moneyOnDeath;
    public float moveSpeed;

    private Transform[] path;
    private int currentPathWaypoint;

    public GameObject healthBarPrefab;

    public static event UnityAction OnDestroyed;

    private void Start()
    {
        path = GameManager.instance.enemyPath.waypoints;

        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject healthBar = Instantiate(healthBarPrefab, canvas.transform);
        healthBar.GetComponent<EnemyHealthBar>().Intitialize(this);
    }

    private void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if(currentPathWaypoint < path.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[currentPathWaypoint].position, moveSpeed * Time.deltaTime);

            if (transform.position == path[currentPathWaypoint].position)
                currentPathWaypoint++;
        }else
        {
            GameManager.instance.TakeDamage(damageToPlayer);
            OnDestroyed.Invoke();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            GameManager.instance.ChangeMoney(moneyOnDeath, false);
            OnDestroyed.Invoke();
            Destroy(gameObject);
        }
    }
}
