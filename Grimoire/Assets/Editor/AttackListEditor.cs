using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AttackList))]
public class AttackListEditor : Editor 
{
	string Test;
	GUIStyle listStyle;
	void Awake(){
		listStyle = new GUIStyle(GUI.skin.label);
		listStyle.margin = new RectOffset(0,20,0,0);
	
	}
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(20.0f));
		EditorGUILayout.TextField("Action", Test, listStyle);
		EditorGUILayout.TextField("Time", Test,listStyle );
		
		EditorGUILayout.EndHorizontal();
	}
}