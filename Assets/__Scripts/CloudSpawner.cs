using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private GameObject _cloudPrefab;
    [SerializeField] private int _cloudsNum = 40;
    [SerializeField] private Vector3 _cloudPosMin = new Vector3(-50, -5, 10);
    [SerializeField] private Vector3 _cloudPosMax = new Vector3(150, 100, 10);
    [SerializeField] private float _cloudScaleMin = 1f;
    [SerializeField] private float _cloudScaleMax = 3f;
    [SerializeField] private float _cloudsSpeedMult = 0.5f;

    private GameObject[] _cloudInstances;

    private void Awake()
    {
        _cloudInstances = new GameObject[_cloudsNum];

        Transform anchor = GameObject.Find("CloudAnchor").transform;
        GameObject cloud;
        for(int i = 0; i < _cloudsNum; i++)
        {
            cloud = Instantiate(_cloudPrefab);
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(_cloudPosMin.x, _cloudPosMax.x);
            cPos.y = Random.Range(_cloudPosMin.y, _cloudPosMax.y);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(_cloudScaleMin, _cloudScaleMax, scaleU);

            cPos.y = Mathf.Lerp(_cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU; //make small clouds far

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor);
            _cloudInstances[i] = cloud;       
        }
    }

    private void Update()
    {
        foreach(GameObject cloud in _cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            cPos.x -= scaleVal * Time.deltaTime * _cloudsSpeedMult;
            if(cPos.x <= _cloudPosMin.x)
                cPos.x = _cloudPosMax.x;
            cloud.transform.position = cPos;   
        }        
    }
}
