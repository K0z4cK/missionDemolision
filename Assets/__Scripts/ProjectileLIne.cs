using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLIne : MonoBehaviour
{
    static public ProjectileLIne INSTANCE;

    [Header("Set in Inspector")]
    [SerializeField] private float _minDist = 0.1f;

    private LineRenderer _line;
    private List<Vector3> _points;

    private Transform _poi;

    public Transform poi
    {
        get { return _poi; }
        set { 
            _poi = value;
            if (_poi != null)
            {
                _line.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public Vector3 lastPoint
    {
        get
        {
            if (_points == null)
            {
                return (Vector3.zero);
            }
            return (_points[_points.Count - 1]);
        }
    }

    private void Awake()
    {
        INSTANCE = this;
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _points = new List<Vector3>();
    }
    private void Clear()
    {
        _poi = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }
    private void AddPoint()
    {
        Vector3 point = _poi.position;
        if (_points.Count > 0 && (point - lastPoint).magnitude < _minDist)
            return;
        if(_points.Count == 0)
        {
            Vector3 launchPosDiff = point - Slingshot.LAUNCH_POS;
            _points.Add(point + launchPosDiff);
            _points.Add(point);
            _line.positionCount = 2;
            _line.SetPosition(0, _points[0]);
            _line.SetPosition(1, _points[1]);
            _line.enabled = true;
        }
        else
        {
            _points.Add(point);
            _line.positionCount = _points.Count;
            _line.SetPosition(_points.Count - 1, lastPoint);
            _line.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if(poi == null)
        {
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                    poi = FollowCam.POI;
                else
                    return;
            }
            else
                return;
        }
        AddPoint();
        if (FollowCam.POI == null)
            poi = null;
    }

}
