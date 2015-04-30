using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageScrollMenu : ScrollMenuObject {

	Image m_parentImage;

	void Start()
	{
		m_parentImage = GetComponent<Image>();
	}

	public override void Update()
	{
		if(!parent.GetComponent<CustomizationMenu>().lockedIn)
			base.Update();
	}
	public override void UpdateSelection()
	{
		Debug.Log( "Update Image" );
		m_parentImage.sprite = m_currentlySelected.GetComponent<Page>().imageForUI;
	}
}
