using UnityEngine;
using System.Collections;

public class MenuSystem : MonoBehaviour {

    //This script will handle the flow of menus within the UIMainMenu scene

    //Main Menu References


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void test()
    {

    }
    public Vector2 lerp(Vector2 _starting, Vector2 _ending, float _time)
    {
        float x, y;
        x = (1 - _time) * _starting.x + _time * _ending.x;
        y = (1 - _time) * _starting.y + _time * _ending.y;

        return new Vector2(x, y);
    }
}
