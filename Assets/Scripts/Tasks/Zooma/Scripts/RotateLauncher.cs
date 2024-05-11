using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateLauncher : MonoBehaviour
{
    public GameObject dummyBall;
    public float ballSpeed = 10;
    public GameObject instanceBall;
    public float cooldown = 2.0f;
    bool isCoolingDown = false;
    private Vector3 lookPos;
    public Joystick joystick;
    public PlayerDataUI playerData;


    private void Start()
    {
        playerData = GameObject.FindAnyObjectByType<PlayerDataUI>();
        playerData.catUI.sayAbouteTask();
        CreateBall();

        DateTime StartTime = DateTime.Now;
        SQLiteDbConnection Conn = new SQLiteDbConnection();
        int TaskID = SceneManager.GetActiveScene().buildIndex;
        Conn.executeSetQuery("UPDATE Task SET StartTime ='" + StartTime.ToLongTimeString() + "' WHERE Id=" + TaskID);
        Debug.Log(StartTime);
    }

    // Update is called once per frame
    private void Update()
    {
        RotatePlayerAlongMousePosition();
       
    }
    private void FixedUpdate()
    {
        SetBallPostion();
    }


    // Rotate the launcher along the mouse position
    private void RotatePlayerAlongMousePosition()
    {
        Vector3 moveVector = (Vector3.left * joystick.Horizontal + Vector3.back * joystick.Vertical);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.LookAt(moveVector+ transform.position);
            //transform.rotation = rotation;
            //transform.LookAt(transform.position + moveVector, Vector3.left);
        }
           
    }

    // Set balls postions and forward w.r.t to the launcher
    private void SetBallPostion()
    {
        instanceBall.transform.forward = transform.forward;
        instanceBall.transform.position = transform.position + transform.forward * transform.localScale.z;
    }

    public void ShootBall()
    {
        if (!isCoolingDown)
        {
            // Выполнить действие при нажатии кнопки
            instanceBall.GetComponent<Rigidbody>().AddForce(instanceBall.transform.forward * ballSpeed);
            CreateBall();
            // Запустить восстановление
            StartCoroutine(CooldownCoroutine());
        }
       
    }
    private IEnumerator CooldownCoroutine()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldown); // Ждать время восстановления
        isCoolingDown = false;
    }

    private void CreateBall()
    {
        instanceBall = Instantiate(dummyBall, transform.position, Quaternion.identity);
        instanceBall.SetActive(true);

        instanceBall.tag = "NewBall";
        instanceBall.gameObject.layer = LayerMask.NameToLayer("Default");

        SetBallColor(instanceBall);
    }

    private void SetBallColor(GameObject go)
    {
        BallColor randColor = MoveBalls.GetRandomBallColor();
        Ball instanceBallComponent = go.GetComponentInChildren<Ball>();

        switch (randColor)
        {
            case BallColor.red:
                
                instanceBallComponent.Initialise(0, "Ы");
                break;

            case BallColor.green:
                
                instanceBallComponent.Initialise(1, "И");
                break;

        }
    }
}
