using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    public float duration; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
