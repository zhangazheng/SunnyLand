using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float MoveRate;
    Vector2 startPos;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.position = new Vector2(startPos.x + camera.transform.position.x * MoveRate, transform.position.y);
    }
}
