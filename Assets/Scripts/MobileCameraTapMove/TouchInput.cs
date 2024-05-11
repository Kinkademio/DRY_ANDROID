using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    Debug.Log(touch.position);
                    smoothMove(touch.position);
                }
            }
        }
    }

    private void smoothMove(Vector3 toTarget)
    {
        transform.position = Vector3.SmoothDamp(transform.position, toTarget, ref _currentVelocity, smoothTime);
    }
}