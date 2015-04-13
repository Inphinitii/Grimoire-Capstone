using System.Collections;

public class Page
{
	public enum Type
	{
		STANDING_NEUTRAL,
		STANDING_DIRECTIONAL,
		AIR_NEUTRAL,
		AIR_DIRECTIONAL
	};

	public AbstractAttack standingNeutral;
	public AbstractAttack standingDirectional;
	public AbstractAttack airNeutral;
	public AbstractAttack airDirectional;

	private AbstractAttack[] m_attacks;


	public virtual void UsePage()
	{

	}

    public virtual void OnPageUse()
    {

    }

    public virtual void OnPageRelease()
    {

    }
}
