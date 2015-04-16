using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameSpellPanel : MonoBehaviour {

	public Grimoire referenceObject;

	private PageUI[] m_spellImages;

	private int selection, previous;
	// Use this for initialization
	void Start () 
	{
		previous = -1;
		m_spellImages = transform.GetComponentsInChildren<PageUI>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		selection = referenceObject.GetPageIndex() - 1;
		if ( selection != previous )
		{
			m_spellImages[selection].transform.SetAsLastSibling();
			m_spellImages[selection].Select();

			if ( previous != -1 )
			{
				m_spellImages[previous].Exit();
			}
			if ( previous == 2 )
			{
				m_spellImages[previous].transform.SetAsFirstSibling();
			}
		}
		previous = selection;
	}
}
