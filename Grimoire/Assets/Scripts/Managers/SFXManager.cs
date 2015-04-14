using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
	public static SFXManager instance = null;
	public AudioClip[] whooshEffects;
	public AudioClip[] hitEffects;
	public AudioClip[] mineSfx;
	public AudioClip jumpSFX;

	private static AudioClip[] whoosh;
	private static AudioClip[] hits;
	private static AudioClip[] mineSFX;
	private static AudioClip jumpClip;



	void Awake()
	{
		if ( instance == null )
			instance = this;
		else if ( instance != this )
			Destroy( gameObject );

		DontDestroyOnLoad( this );
	}

	void Start()
	{
		whoosh		= whooshEffects;
		hits				= hitEffects;
		mineSFX		= mineSfx;
		jumpClip		= jumpSFX;
	}


	void Update()
	{

	}

	public static AudioClip GetWhooshEffect(){	return whoosh[Random.Range( 0, whoosh.Length )]; }
	public static AudioClip GetHitEffect(){	return hits[Random.Range( 0, hits.Length )];}
	public static AudioClip GetMinePlace() { return mineSFX[0]; }
	public static AudioClip GetMineExplode() { return mineSFX[1]; }
	public static AudioClip GetJump() { return jumpClip; }

}
