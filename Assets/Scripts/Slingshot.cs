using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _velocityMult = 8f;

    [Header("Set Dinamically")]
    [SerializeField] private Transform _launchPoint;
    [SerializeField] private Vector3 _launchPos;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private bool _isAiming;

    private Rigidbody _projectileRigidbody;

    private void Awake()
    {
        _launchPoint = transform.Find("LaunchPoint"); //need better variant
        _launchPoint.gameObject.SetActive(false); //or just realize in in own class

        _launchPos = _launchPoint.position;
    }

    private void OnMouseEnter()
    {
        _launchPoint.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        _launchPoint.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        _isAiming = true;
        _projectile = Instantiate(_projectilePrefab);
        _projectile.transform.position = _launchPos;
        _projectileRigidbody = _projectile.GetComponent<Rigidbody>();
        _projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (!_isAiming)
            return;
        
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - _launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = _launchPos + mouseDelta;
        _projectile.transform.position = projPos;

        if(Input.GetMouseButtonUp(0))
        {
            _isAiming = false;
            _projectileRigidbody.isKinematic = false;
            _projectileRigidbody.velocity = -mouseDelta * _velocityMult;
            FollowCam.POI = _projectile.transform;
            _projectile = null;
        }
    }
}
