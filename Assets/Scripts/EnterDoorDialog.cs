using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoorDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialog;
    // �ռ���Ʒ
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
