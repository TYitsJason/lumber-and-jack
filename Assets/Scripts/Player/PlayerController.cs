using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LoadSaveManager.GameStateData;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float gravity = -9.81f;
    public float jumpSpeed = 2.0f;
    public int health = 10;
    public Camera playerCamera;
    private Vector3 playerVelocity;
    public LayerMask enemyCheck;
    public bool hasMainAttack = false;
    public bool hasSecondaryAttack = false;
    public GameObject axe;
    public CinemachineFreeLook cinemachineFreeLook;

    CharacterController cc;
    Animator anim;

    public Text healthDisplay;

    public PauseMenu pauseMenu;

    public Collider hitbox;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            cc = gameObject.GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            if (speed < 0)
            {
                speed = 5;
                throw new ArgumentException("Default value has been set for speed");
            }
            healthDisplay.text = "Health: " + health.ToString();
            axe.SetActive(hasMainAttack);
        }
        catch(NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
        catch(ArgumentException e)
        {
            Debug.Log(e.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("Dead"))
            return;
        if (pauseMenu.IsPaused())
            return;
        float hInput = Input.GetAxisRaw("Horizontal");
        float fInput = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        if (cameraForward.magnitude > 0)
            cameraForward.Normalize();
        if (cameraRight.magnitude > 0)
            cameraRight.Normalize();

        Vector3 desiredMoveDir = cameraForward * fInput + cameraRight * hInput;


        playerVelocity.y += gravity * Time.deltaTime;

        if (desiredMoveDir.magnitude != 0 && !pauseMenu.IsPaused())
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDir), 0.1f);

        if (!hitbox.enabled && !anim.GetBool("isAttacking"))
            cc.Move(desiredMoveDir * Time.deltaTime * speed + playerVelocity * Time.deltaTime);

        Vector3 dir = new Vector3(hInput, 0, fInput);
        
        anim.SetFloat("Speed", dir.magnitude);

        if (Input.GetButton("Fire1") && !pauseMenu.IsPaused())
        {
            mainAttack();
        }
        
        if (Input.GetButton("Fire2") && !pauseMenu.IsPaused())
        {
            secondaryAttack();
        }
        
    }

    private void mainAttack()
    {
        if (hasMainAttack)
        {
            anim.SetInteger("Damage", 10);
            anim.SetBool("isAttacking", true);
        }
    }

    private void secondaryAttack()
    {
        if (hasSecondaryAttack) {
            anim.SetInteger("Damage", 5);
            anim.SetBool("isKicking", true);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            anim.SetBool("Dead", true);
        }
        healthDisplay.text = "Health: " + health.ToString();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
            TakeDamage(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slow"))
            speed *= 0.2f;
        if (other.CompareTag("Explosion"))
        {
            TakeDamage(100);
        }
        if (other.CompareTag("PowerUp"))
        {
            if (!hasMainAttack)
            {
                hasMainAttack = true;
                axe.SetActive(true);
            }
            else if (!hasSecondaryAttack)
            {
                hasSecondaryAttack = true;
            }
            else TakeDamage(-5);
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slow"))
            speed *= 5;
    }

    // Save & Load
    public void SaveGamePrepare()
    {
        Debug.Log("Preparing PlayerData");
        DataPlayer data = GameManager.StateManager.gameState.player;
        data.health = health;
        data.hasMainAttack = hasMainAttack;
        data.hasSecondaryAttack = hasSecondaryAttack;
        data.trans.posX = transform.position.x;
        data.trans.posY = transform.position.y;
        data.trans.posZ = transform.position.z;
        data.trans.rotX = transform.rotation.x;
        data.trans.rotY = transform.rotation.y;
        data.trans.rotZ = transform.rotation.z;
        data.trans.scale = transform.localScale.x;
    }

    public void LoadGameComplete()
    {
        DataPlayer dataPlayer = GameManager.StateManager.gameState.player;
        Vector3 newPos = new Vector3(dataPlayer.trans.posX, dataPlayer.trans.posY, dataPlayer.trans.posZ);
        Quaternion newRot = Quaternion.Euler(dataPlayer.trans.rotX, dataPlayer.trans.rotY, dataPlayer.trans.rotZ);
        cc.enabled = false;
        transform.position = newPos;
        transform.rotation = newRot;
        transform.localScale = new Vector3(dataPlayer.trans.scale, dataPlayer.trans.scale, dataPlayer.trans.scale);
        cc.enabled = true;
        cinemachineFreeLook.ForceCameraPosition(newPos, newRot);
        health = dataPlayer.health;
        hasMainAttack = dataPlayer.hasMainAttack;
        hasSecondaryAttack = dataPlayer.hasSecondaryAttack;
    }
}
