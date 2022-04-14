using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    public void Collected()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
        Destroy(gameObject);
    }
}
