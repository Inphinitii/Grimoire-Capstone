using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameSpellPanel : MonoBehaviour {

	public Grimoire referenceObject;

	private PageUI[] m_spellImages;

	private int selection, previous;
	private bool m_updatePanel;

	// Use this for initialization
	public void StartUI () 
	{
		previous = -1;
		m_spellImages = transform.GetComponentsInChildren<PageUI>();

		for ( int i = 0; i < m_spellImages.Length; i++ )
		{
			m_spellImages[i].GetComponent<Image>().sprite = referenceObject.GetPage( i ).imageForUI;
			m_spellImages[i].Exit();
		}
		m_spellImages[m_spellImages.Length - 1].transform.SetAsFirstSibling();
		m_updatePanel = true;


	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_updatePanel )
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
}
