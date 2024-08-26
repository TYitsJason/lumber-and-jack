using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForOneSec());
        
    }

    IEnumerator WaitForOneSec()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
