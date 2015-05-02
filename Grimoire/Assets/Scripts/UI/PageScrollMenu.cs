using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageScrollMenu : ScrollMenuObject {

    public Page m_current;
    public Page defaultPage;
	Image m_parentImage;

	void Start()
	{
		m_parentImage = GetComponent<Image>();
        m_current = defaultPage;
        m_parentImage.sprite = defaultPage.imageForUI;
	}

	public override void Update()
	{
		if(!parent.GetComponent<CustomizationMenu>().lockedIn)
			base.Update();
	}
	public override void UpdateSelection()
	{
        m_current            = m_currentlySelected.GetComponent<Page>();
		m_parentImage.sprite = m_currentlySelected.GetComponent<Page>().imageForUI;
	}
}
