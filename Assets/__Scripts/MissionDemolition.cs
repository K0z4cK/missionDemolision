using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode { 
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition INSTANCE;

    [Header("Set in Inspector")]
    [SerializeField] private Text _uitLevel;
    [SerializeField] private Text _uitShots;
    [SerializeField] private Text _uitButton;
    [SerializeField] private Vector3 _castlePos;
    [SerializeField] private Transform[] _castles;

    [Header("Set Dinamically")]
    private int _level;
    private int _maxLevel;
    private int _shotsTaken;
    private Transform _currCastle;
    private GameMode _mode = GameMode.idle;
    private string _showing = "Show Slingshot";

    private void Start()
    {
        INSTANCE = this;

        _level = 0;
        _maxLevel = _castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        if(_currCastle != null)
            Destroy(_currCastle.gameObject);

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject projectile in projectiles)
            Destroy(projectile);

        _currCastle = Instantiate(_castles[_level]);
        _currCastle.transform.position = _castlePos;
        _shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLIne.INSTANCE.Clear();

        Goal.GOAL_MET = false;

        UpdateUI();

        _mode = GameMode.playing;
    }

    private void UpdateUI()
    {
        _uitLevel.text = "Level: " + (_level + 1) + " of " + _maxLevel;
        _uitShots.text = "Shots Taken: " + _shotsTaken;
    }
    private void NextLevel()
    {
        _level++;
        if(_level == _maxLevel)
            _level = 0;
        StartLevel();
    }
    private void Update()
    {
        UpdateUI();

        if (_mode == GameMode.playing && Goal.GOAL_MET)
        {
            _mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }
    public void SwitchView(string eView = "")
    {
        if (eView == "")
            eView = _uitButton.text;
        _showing = eView;
        switch (_showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                _uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = INSTANCE._currCastle;
                _uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth").transform;
                _uitButton.text = "Show Slingshot";
                break;
        }
    }
    public static void ShotFired()
    { 
        INSTANCE._shotsTaken++;
    }

}
