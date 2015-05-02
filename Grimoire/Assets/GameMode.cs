using UnityEngine;
using System.Collections;

public class GameMode {

    public GameObject _uiPrefab;
    protected float _timer;
    protected Actor[] _actorReference;
    protected GameObject m_uiReference;

    public Actor winnerActor;


    public GameMode()
    {

    }

    public GameMode( Actor[] _actors )
    {
        _actorReference     = _actors;
        _timer              = GameManager.gameModifiers.GameTime;

    }

    public virtual void Initialize() { }
	
	// Update is called once per frame
    public virtual void Update() { }

    public virtual void SpawnUI()
    {
        m_uiReference = (GameObject)GameObject.Instantiate( _uiPrefab, Vector3.zero, Quaternion.identity );
    }

    public virtual string GetDescription()
    {
        return "Not a Mode";
    }

    public float GetTimer()
    {
        return _timer;
    }

    public virtual bool GameEnd()
    {
        return false;
    }
}
