using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionDisplay : MonoBehaviour
{
    public GameObject from;
    public GameObject to;

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                to.SetActive(false);
                from.SetActive(true);
            }
            else if(other.gameObject.transform.position.x < transform.position.x)
            {
                from.SetActive(false);
                to.SetActive(true);
            }
            else
            {
                from.SetActive(false);
                to.SetActive(false);
            }
        }
    }
}
