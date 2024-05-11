using UnityEngine;

public class MoveCameraOnToutch : MonoBehaviour
{
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;
    public float time;

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector3 point = Camera.main.ScreenToViewportPoint(touch.position);
            Vector3 newPoint = point * 2;

             newPoint.z = Camera.main.transform.position.z;
             smoothMove(newPoint);
        }
       
    }

    private void smoothMove(Vector3 toTarget)
    {
        transform.position = Vector3.SmoothDamp(transform.position, toTarget, ref _currentVelocity, smoothTime);
    }
}
