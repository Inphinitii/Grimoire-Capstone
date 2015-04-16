using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
	public static SFXManager instance = null;
	public AudioClip[] whooshEffects;
	public AudioClip[] hitEffects;
	public AudioClip[] mineSfx;
	public AudioClip jumpSFX;
	public AudioClip pageFlipSFX;


	private static AudioClip[] whoosh;
	private static AudioClip[] hits;
	private static AudioClip[] mineSFX;
	private static AudioClip jumpClip;
	private static AudioClip pageFlip;



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
		pageFlip		= pageFlipSFX;
	}


	void Update()
	{

	}

	/// <summary>
	/// Wrapper for AudioSources to have their volume changed according to the setting in the GameManager.
	/// </summary>
	/// <param name="_source">AudioSource Component</param>
	/// <param name="_clip">Audio Clip to be played.</param>
	public static void PlayOneShot(AudioSource _source, AudioClip _clip)
	{
		_source.volume = GameManager.sfxVolume;
		_source.PlayOneShot( _clip );
	}

	/// <summary>
	/// Returns the given sound from the static equivilent of the Whoosh array.
	/// </summary>
	/// <returns>AudioClip</returns>
	public static AudioClip GetWhooshEffect(){	return whoosh[Random.Range( 0, whoosh.Length )]; }

	/// <summary>
	/// Returns the given sound from the static equivilent of the Hit array.
	/// </summary>
	/// <returns>AudioClip</returns>
	public static AudioClip GetHitEffect(){	return hits[Random.Range( 0, hits.Length )];}

	/// <summary>
	/// Returns the given sound from the static equivilent of the Mine Placement clip.
	/// </summary>
	/// <returns>AudioClip</returns>
	public static AudioClip GetMinePlace() { return mineSFX[0]; }

	/// <summary>
	/// Returns the given sound from the static equivilent of the Mine Explode clip.
	/// </summary>
	/// <returns>AudioClip</returns>
	public static AudioClip GetMineExplode() { return mineSFX[1]; }

	/// <summary>
	/// Returns the given sound from the static equivilent of the Jumping clip.
	/// </summary>
	/// <returns>AudioClip</returns>
	public static AudioClip GetJump() { return jumpClip; }

	/// <summary>
	/// Returns the given sound from the static equivilent of the Jumping clip.
	/// </summary>
	/// <returns>AudioClip</returns>
	public static AudioClip GetPageFlip() { return pageFlip; }

}
