using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseChek : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("letter"))
        {
            Debug.Log("Letter enter");
        }
    }
}
