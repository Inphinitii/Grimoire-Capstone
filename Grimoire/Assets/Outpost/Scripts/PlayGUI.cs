using UnityEngine;
using System.Collections;

public class PlayGUI : MonoBehaviour {
	public Transform[] transforms;
	
	public GUIContent[] GUIContents;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUILayout.BeginVertical("box");
		for (int i = 0; i < GUIContents.Length; i++) {
			if (GUILayout.Button(GUIContents[i])) {
				for (int j = 0; j < transforms.Length; j++) {
					transforms[j].animation.Play(GUIContents[i].text);
				}
			}
		}
		GUILayout.EndVertical();
	}
	
	

}
