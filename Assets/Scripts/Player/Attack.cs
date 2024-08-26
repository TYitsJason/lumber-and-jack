using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public PlayerController pc;
    public Animator anim;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(anim.GetInteger("Damage"));
        }
        else if (!collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Did not hit enemy");
            if (anim.GetBool("isAttacking"))
            {
                pc.TakeDamage(5);
            }
        }
    }
}
