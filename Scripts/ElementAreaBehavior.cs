using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAreaBehavior : MonoBehaviour
{
    public GameObject combinedElement1;
    public string combinedElement1Tag;

    public GameObject combinedElement2;
    public string combinedElement2Tag;

    public string cancellingElementTag; // Tag of the element that cancels this area
    public string cancellingAreaTag; // Tag of the area that cancels this area

    public string combiningElement1Tag; // Tag of the first element this element can be combined with
    public string combiningElement2Tag; // Tag of the second element this element can be combined with
    public string combiningArea1Tag; // Tag of the first area this area ca be combined with
    public string combiningArea2Tag; // Tag of the secon area this are ca be combined with

    public float duration; // Duration of the area, after this duration it gets destroyed

    public int Damage; // Damage this area makes

    public Vector3 offsetCombine1;
    public Vector3 offsetCombine2;

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
        if (other.tag == combiningArea1Tag) // Checks if the other gameobject is an element this element can combine with
        {
            Destroy(gameObject);
            Instantiate(combinedElement1, DetectGroundHeight(transform.position.x, transform.position.z) - offsetCombine1, combinedElement1.transform.rotation); // Spawns the first combined element
        }
        else if (other.tag == combiningArea2Tag)
        {
            Destroy(gameObject);
            Instantiate(combinedElement2, DetectGroundHeight(transform.position.x, transform.position.z) - offsetCombine2, combinedElement2.transform.rotation); // Spawns the second combined element
        }
        else if (other.tag == cancellingElementTag || other.tag == cancellingAreaTag || other.tag == combiningElement1Tag || other.tag == combiningElement2Tag)
        {
            Destroy(gameObject);
        }
    }

    // Destroying the area after the duration
    IEnumerator DestroyElement()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public Vector3 DetectGroundHeight(float x, float y)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, y);
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity);
        return hit.point;
    }
}
