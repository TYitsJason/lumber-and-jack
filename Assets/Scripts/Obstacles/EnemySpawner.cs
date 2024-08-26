using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Tree1;
    public GameObject Tree2;
    public GameObject Tree3;
    public GameObject Tree4;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = gameObject.transform.position;
        int i = UnityEngine.Random.Range(1, 6);
        try
        {
            switch (i)
            {
                case 1:
                    Instantiate(Tree1, pos, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(Tree2, pos, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(Tree3, pos, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(Tree4, pos, Quaternion.identity);
                    break;
                case 5:
                    break;
            }
            Destroy(gameObject);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
