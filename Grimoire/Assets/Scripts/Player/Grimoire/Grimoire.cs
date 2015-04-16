using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Grimoire
 *
 * Description: Data structure that keeps track of the current Pages equipped in the Grimoire.
 * Handles the use of spell charges when a page is used. 
 =========================================================*/

[RequireComponent(typeof(SpellCharges))]
public class Grimoire : MonoBehaviour
{

	public Page[]	grimoirePages;
	public int			maxPages;


	private SpellCharges	m_spellCharges;
	private Page[]				m_internalPages;
	private Page					m_SelectedPage	= null;
	private int					m_PageAmount	= 0;
	private int					m_pageIndex;



	void Start()
	{
		m_PageAmount	= 0;
		m_SelectedPage = null;
		m_spellCharges	= GetComponent<SpellCharges>();

		if ( grimoirePages.Length > 0 )
		{
			m_PageAmount	= grimoirePages.Length;
			m_internalPages = new Page[m_PageAmount];

			for ( int i = 0; i < m_PageAmount; i++ )
			{
				//grimoirePages[i].enabled = false;
				m_internalPages[i] = Instantiate( grimoirePages[i], this.gameObject.transform.position, Quaternion.identity ) as Page;
				m_internalPages[i].transform.parent = this.gameObject.transform;
				m_internalPages[i].parent = this.gameObject;
			}
			PostStart();
			m_SelectedPage = m_internalPages[0];
			m_pageIndex = 1;
		}
	}

	/// <summary>
	/// Run after the initial Start method to ensure the objects are initialized in the correct order. 
	/// </summary>
	void PostStart()
	{
		for ( int i = 0; i < m_PageAmount; i++ )
			m_internalPages[i].Init();
	}

	void Update()
	{
		if ( GetComponent<InputHandler>().SpellSwap().thisFrame && !GetComponent<InputHandler>().SpellSwap().lastFrame )
		{
			if ( m_pageIndex < m_PageAmount )
			{
				m_pageIndex++;
				m_SelectedPage = m_internalPages[m_pageIndex - 1];
			}
			else
			{
				m_pageIndex = 1;
				m_SelectedPage = m_internalPages[0];
			}
		}
	}

	/// <summary>
	/// Use the page currently set as selected. Requires a SpellCharge to use.
	/// </summary>
	/// <param name="_type">Which type of attack to use on the given page.</param>
	/// <returns>The AbstractAttack reference.</returns>
	public AbstractAttack UseCurrentPage(Page.Type _type)
	{
		if ( !m_SelectedPage.OnCooldown() )
		{
			if ( m_spellCharges.UseCharge( GetRefreshRate() ) )
				return m_SelectedPage.UsePage( _type );
		}
		return null;
	}

	/// <summary>
	/// Returns the refresh rate for this page.
	/// </summary>
	/// <returns>Currently selected page refresh rate.</returns>
	public float GetRefreshRate()
	{
		return m_SelectedPage.chargeRefresh;
	}

	/// <summary>
	/// Return the page at the given index
	/// </summary>
	/// <param name="_index">Index in the array</param>
	/// <returns>Page object</returns>
	public Page GetPage( int _index )
	{
		return m_internalPages[_index];
	}

	public int GetPageIndex()
	{
		return m_pageIndex;
	}

	/// <summary>
	/// Add a page to the Grimoire
	/// </summary>
	/// <param name="_page">Page Object</param>
	public void AddPage( Page _page )
	{
		m_internalPages[m_PageAmount] = _page;
		m_PageAmount++;
	}

	/// <summary>
	/// Remove a page from the Grimoire
	/// </summary>
	/// <param name="_page">Page Object</param>
	public void RemovePage( int _index )
	{
		m_internalPages[_index] = null;
		m_PageAmount--;
	}

	/// <summary>
	/// Set a page at the index
	/// </summary>
	///<param name="_index">Index in the array</param>
	///<param name="_page">Page Object</param>
	public void SetPage( int _index, Page _page )
	{
		m_internalPages[_index] = _page;
	}

}
