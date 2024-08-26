using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public Animator anim;
    public float moveSpeed = 5f;
    public GameObject model;
    public NavMeshAgent agent;
    public int health = 10;
    public GameObject explosionEffect;
    public GameObject powerUpPrefab;

    private bool isLooking;
    private MeshRenderer mr;
    public Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mr  = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAnim.GetBool("Dead"))
        {
            agent.isStopped = true;
            mr.enabled = true;
            model.SetActive(false);
            return;
        }
        if (new Vector3(-playerCamera.transform.position.x + transform.position.x, 0, -playerCamera.transform.position.z + transform.position.z).magnitude <= 5)
        {
            anim.SetBool("isAttacking", true);
        }
        else
        {
            isLooking = CheckIfPlayerIsLooking();
            moveTowardsPlayer();
        }
    }
    bool CheckIfPlayerIsLooking()
    {
        Vector3 directionToPlayer = new Vector3(-playerCamera.transform.position.x + transform.position.x, 0, -playerCamera.transform.position.z + transform.position.z).normalized;
        float angle = Vector3.Angle(playerCamera.transform.forward, directionToPlayer);
        return angle < 80.0f;
    }

    void moveTowardsPlayer()
    {
        agent.isStopped = isLooking;
        
        if (!isLooking)
        {
            agent.SetDestination(player.transform.position);
        }
        mr.enabled = isLooking;
        model.SetActive(!isLooking);
    }

    public void takeDamage(int damage)
    {
        if (mr.enabled)
            health -= damage;
        if (health <= 0)
        {
            killEnemy();
        }
    }

    public void spawnExplosion()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        StartCoroutine(WaitOneSecond());
        Debug.Log("Explosion spawn");
        killEnemy();
    }

    public void killEnemy()
    {
        GameManager.Instance.EnemyDefeated();
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Coroutine Waited");
    }
}
