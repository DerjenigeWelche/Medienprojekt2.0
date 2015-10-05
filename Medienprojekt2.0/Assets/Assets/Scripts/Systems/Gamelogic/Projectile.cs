﻿using UnityEngine;
using System.Collections;

//Beinhaltet das Verhalten eines Projektils, wird auch nur an Projektile angehangen. Dient zb. zu Trefferabfrage bei schüssen.

public class Projectile : MonoBehaviour {

    public enum Shooting_Type
    {
        NORMAL,
        SPECIAL
    }

    //Allgemeine Kugel Eigenschaften
    GameObject prObject; //GameObject des Projektils
	Rigidbody2D rigid;
	BoxCollider2D collid;
	ProjectilePoolingSystem PPS;

	//Schussspezifische Eigenschaften
	GameObject owner;
	float projectileSpeed = 10.0f;
	float damage;
	float range;
	float timeLeft;
	bool inAir = false;


	// Use this for initialization
	void Start () {
		collid = (BoxCollider2D)GetComponent (typeof(BoxCollider2D));
		rigid = (Rigidbody2D)GetComponent (typeof(Rigidbody2D));
		prObject = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (inAir) {
			if(timeLeft <= 0)
			{
				inAir = false;
				PPS.storeProjectile(prObject);
			}
			timeLeft-=Time.deltaTime;
		}
	}

	public void shoot(float range,bool facingRight, Shooting_Type type)
	{
		this.range = range;
		timeLeft = range / projectileSpeed;
		Physics2D.IgnoreCollision ((Collider2D)owner.GetComponent(typeof(Collider2D)), collid);
		AttributeComponent ac = (AttributeComponent)owner.GetComponent (typeof(AttributeComponent));
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        
        if(type == Shooting_Type.NORMAL)
        {
            damage = ac.getDamage();
        }
        else
        {
            damage = ac.getDamage() * 2;
            sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/02_items") as Sprite;
        }
		setDamage (damage);

		inAir = true;
		rigid.transform.position = new Vector2(owner.transform.position.x,owner.transform.position.y +.5f);


		if (facingRight) 
			rigid.velocity = new Vector2 (projectileSpeed, 0);
		else
			rigid.velocity = new Vector2 (-projectileSpeed, 0);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		PPS.storeProjectile (prObject);
		HealthSystem HS = (HealthSystem)coll.gameObject.GetComponent (typeof(HealthSystem));
		if (HS != null)
			HS.lowerHealth (getDamage());
		inAir = false;
	}

	//Übergibt Schaden des Projektils(Genutzt in HealthSystem)
	public float getDamage()
	{
		return damage;
	}

	//setzt Schaden des Projektils
	public void setDamage(float newDamage)
	{
		damage = newDamage;
	}

	public void setOwner(GameObject owner)
	{
		this.owner = owner;
		PPS = (ProjectilePoolingSystem) owner.GetComponent(typeof(ProjectilePoolingSystem));
	}
}