using UnityEngine;
using System.Collections;

public class Grimoire : MonoBehaviour
{

	public int maxPages;
	public Page[]	grimoirePages;


	private int m_PageAmount;
	private int m_SelectedPageIndex;

	private Page[] internalPages;
	private Page		m_SelectedPage;

	void Start()
	{
		if ( grimoirePages.Length > 0 )
		{
			m_PageAmount	= grimoirePages.Length;
			internalPages		= new Page[m_PageAmount];

			for ( int i = 0; i < m_PageAmount; i++ )
			{
				internalPages[i]								= Instantiate( grimoirePages[i], this.gameObject.transform.position, Quaternion.identity ) as Page;
				internalPages[i].transform.parent	= this.gameObject.transform;
				internalPages[i].parent					= this.gameObject;
			}

			m_SelectedPage = internalPages[0];
		}
		else
		{
			m_PageAmount	= 0;
			m_SelectedPage = null;
		}
	}

	public AbstractAttack UseCurrentPage(Page.Type _type)
	{
		return m_SelectedPage.UsePage( _type );
	}

	public float GetRefreshRate()
	{
		return m_SelectedPage.ChargeRefresh;
	}

	public void AddPage( Page _page )
	{
		internalPages[m_PageAmount] = _page;
		m_PageAmount++;
	}

	public void RemovePage( int _index )
	{
		internalPages[_index] = null;
		m_PageAmount--;
	}

	public void SetPage( int _index, Page _page )
	{
		internalPages[_index] = _page;
	}

	public Page GetPage( int _index )
	{
		return internalPages[_index];
	}

	void Update()
	{

	}
}
