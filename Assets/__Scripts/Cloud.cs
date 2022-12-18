using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private GameObject _cloudSpherePrefab;
    [SerializeField] private int _numSpheresMin = 6;
    [SerializeField] private int _numSpheresMax = 10;
    [SerializeField] private Vector3 _sphereOffsetScale = new Vector3(5, 2, 1);
    [SerializeField] private Vector2 _sphereScaleRangeX = new Vector3(4, 8);
    [SerializeField] private Vector2 _sphereScaleRangeY = new Vector3(3, 4);
    [SerializeField] private Vector2 _sphereScaleRangeZ = new Vector3(2, 4);
    [SerializeField] private float _scaleYMin = 2f;

    private List<GameObject> _spheres = new List<GameObject>();

    private void Start()
    {
        _spheres = new List<GameObject>();
        int numSpheres = Random.Range(_numSpheresMin, _numSpheresMax);
        for (int i = 0; i < numSpheres; i++)
        {
            GameObject newSphere = Instantiate(_cloudSpherePrefab);
            _spheres.Add(newSphere);

            Transform sphereTransform = newSphere.transform;
            sphereTransform.SetParent(this.transform);

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= _sphereOffsetScale.x;
            offset.y *= _sphereOffsetScale.y;
            offset.z *= _sphereOffsetScale.z;
            sphereTransform.localPosition = offset;

            Vector3 scale = Vector3.one;
            scale.x = Random.Range(_sphereScaleRangeX.x, _sphereScaleRangeX.x);
            scale.y = Random.Range(_sphereScaleRangeY.y, _sphereScaleRangeY.y);
            scale.z = Random.Range(_sphereScaleRangeZ.x, _sphereScaleRangeZ.y);

            scale.y *= 1 - (Mathf.Abs(offset.x) / _sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, _scaleYMin);
            
            sphereTransform.localScale = scale;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Restart();
    }

    private void Restart()
    {
        foreach(var sphere in _spheres)
            Destroy(sphere);
        Start();
    }
}
