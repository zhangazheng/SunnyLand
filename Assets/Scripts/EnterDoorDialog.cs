using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoorDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialog;
    // 收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialog.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialog.SetActive(false);
        }
    }
}
