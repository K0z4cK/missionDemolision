using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public Transform POI;

    [Header("Set in Inspector")]
    [SerializeField] private float _easing = 0.05f;
    [SerializeField] private Vector2 _minXY = Vector2.zero;


    [Header("Set Dinamically")]
    [SerializeField] private float _camZ;

    private void Awake()
    {
        _camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null)
            return;

        Vector3 destination = POI.position;
        destination.x = Mathf.Max(_minXY.x, destination.x);
        destination.y = Mathf.Max(_minXY.y, destination.y);

        destination = Vector3.Lerp(transform.position, destination, _easing);
        destination.z = _camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }
}
