namespace Task2
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class HouseChekT2 : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.CompareTag("letter"))
            {
                Debug.Log("Letter enter");
            }
        }
    }
}