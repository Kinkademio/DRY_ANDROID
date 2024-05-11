using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int type;
    public string letter;
    [SerializeField] private TMPro.TextMeshPro m_TextMeshPro;


    public void Initialise(int type, string letter)
    {
        this.type = type; 
        this.letter = letter;

        m_TextMeshPro.text = letter;
    }

}
