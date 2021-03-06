﻿using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour {
    
    //ac = AttributeComponent von Player, bc = BoxCollider von Player, pbc = BoxCollider von Projectile
	AttributeComponent ac;
    MeleeSystem meleeSys;
	BoxCollider2D bc;
	BoxCollider2D pbc;
    Animator anim;

	// Use this for initialization
	void Start () {
        ac = this.gameObject.GetComponent<AttributeComponent>();
        bc = this.gameObject.GetComponent<BoxCollider2D>();
		pbc = GameObject.FindWithTag ("Projectile").GetComponent <BoxCollider2D>();
        anim = (Animator)GetComponent(typeof(Animator));
        meleeSys = (MeleeSystem)GetComponent(typeof(MeleeSystem));
	}

	// Update is called once per frame
	void Update () {

	}

    //Verringert HP des Spielers/Gegners und gibt Todesanzeige. MIN HP = 0
    public void lowerHealth(float damage)
    {
        if (meleeSys != null && meleeSys.blocking)
        {
            damage = ac.reduceStamina(damage);
        }
        if (damage >= 0)
        {
            ac.setHealth(ac.getHealth() - damage);
            if (anim != null)
                anim.SetTrigger("MeleeInterrupt");
        }
        if (ac.getHealth() <= 0)
        {
			if(this.gameObject.tag != "Player")
			{
				Destroy (this.gameObject);
			}
            if(this.gameObject.name == "Boss")
            {
                SpawnToad td = (SpawnToad)GameObject.Find("Toad").GetComponent(typeof(SpawnToad));
                CharacterMovement cm = (CharacterMovement)GameObject.Find("Player").GetComponent(typeof(CharacterMovement));
                td.spawn();
                cm.unableToMove = false;
            }
            ac.setHealth(0);
        }
	}

	//Erhöht HP des Spielers/Gegners. MAX HP = 100
	public void raiseHealth(float hp)
	{
		ac.setHealth(ac.getHealth() + hp);
		if (ac.getHealth () > 100)
			ac.setHealth (100);
	}


}