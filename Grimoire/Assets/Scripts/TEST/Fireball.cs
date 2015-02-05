using UnityEngine;
using System.Collections;

public class Fireball : ISpell {

	// Use this for initialization
	public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}

    public override void OnUse() {
        base.OnUse(); 
        Instantiate(spellProjectile, this.transform.position, Quaternion.identity);
    }

    public override void OnRelease() {
        Debug.Log("On Release");
    }

    public override void OnHit() {

    }
}
