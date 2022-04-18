using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    public void Collected()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
        GetComponent<Animator>().SetTrigger("got");
    }
    void Death()
    {
        Destroy(gameObject);
    }

}
