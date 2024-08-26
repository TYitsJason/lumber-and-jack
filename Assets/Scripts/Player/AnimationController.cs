using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Collider hitbox;
    public Animator anim;
    public Enemy parentReference;

    public void enableHitbox()
    {
        hitbox.enabled = true;
    }

    public void disableHitbox()
    {
        hitbox.enabled = false;
        anim.SetBool("isAttacking", false);
        anim.SetBool("isKicking", false);
    }
    public void explode()
    {
        parentReference.spawnExplosion();
    }
}
