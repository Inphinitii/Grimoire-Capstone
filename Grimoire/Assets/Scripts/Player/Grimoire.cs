using UnityEngine;
using System.Collections;

public class Grimoire : MonoBehaviour
{

	public int maxPages;

	Page[]	m_GrimoirePages;
	Page		m_SelectedPage;

	int		m_PageAmount;
	int		m_SelectedPageIndex;

	void Start()
	{
		m_GrimoirePages = new Page[maxPages];
		m_PageAmount = 0;
		m_SelectedPage = null;
	}

	public void UseCurrentPage()
	{
		m_SelectedPage.UsePage();
	}

	public void AddPage( Page _page )
	{
		m_GrimoirePages[m_PageAmount] = _page;
		m_PageAmount++;
	}

	public void RemovePage( int _index )
	{
		m_GrimoirePages[_index] = null; // = Page.BlankPage
		m_PageAmount--;
	}

	public void SetPage( int _index, Page _page )
	{
		m_GrimoirePages[_index] = _page;
	}

	public Page GetPage( int _index )
	{
		return m_GrimoirePages[_index];
	}

	void Update()
	{

	}
}
