using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAreaDestroy : MonoBehaviour
{
    public float duration; // Duration of the area, after this duration it gets destroyed

    public int Damage; // Damage this area makes

    public string ownElementTag;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyElement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Enemy" && other.tag != "Ground" && other.tag != "Platform" && other.tag != ownElementTag && other.tag != "Grandmaster") // Checks if the other gameobject is an element this element can combine with
        {
            StartCoroutine(Destroy());
        }
    }

    IEnumerator DestroyElement()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
