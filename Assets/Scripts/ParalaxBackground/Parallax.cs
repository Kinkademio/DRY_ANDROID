using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float _length;
    private float _startPosition;
    private Camera _playerCamera;

    public float parallaxEffectSetting;


    private void Awake()
    {
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _playerCamera = Camera.main;
    }

    public void FixedUpdate()
    {
        float distance = _playerCamera.gameObject.transform.position.x * parallaxEffectSetting;
        transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);
    }

}
