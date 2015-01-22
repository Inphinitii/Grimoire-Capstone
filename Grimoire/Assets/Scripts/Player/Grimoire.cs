using UnityEngine;
using System.Collections;

public class Grimoire : MonoBehaviour {

    Page[] mGrimoirePages;
    Page   mSelectedPage;
    int    mPageAmount;

	void Start () 
    {
        mGrimoirePages = new Page[3];
        mPageAmount = 0;
        mSelectedPage = null;
	}

    public void AddPage(Page _page)
    {
        mGrimoirePages[mPageAmount] = _page;
        mPageAmount++;
    }

    public void RemovePage(int _index)
    {
        mGrimoirePages[_index] = null; // = Page.BlankPage
        mPageAmount--;
    }

    public void SetPage(int _index, Page _page)
    {
        mGrimoirePages[_index] = _page;
    }

    public Page GetPage(int _index)
    {
        return mGrimoirePages[_index];
    }
	
	void Update () {
	
	}
}
