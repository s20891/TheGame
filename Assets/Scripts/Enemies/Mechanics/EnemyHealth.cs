﻿using Scripts.Interfaces;
using Scripts.Tools;
using System.Collections;
using UnityEngine;

namespace Scripts.Enemies
{
	public class EnemyHealth : ExtendedMonoBehaviour, IHit, IRiposte
	{
		private EnemyManager manager;
		[SerializeField] private int hp;

		private void Awake()
		{
			manager = GetComponent<EnemyManager>();
		}

		public void Hit(GameObject attacker, int damage, IHit.HitWeapon weapon = IHit.HitWeapon.OTHER)
		{
			hp -= damage;

			if (hp <= 0) {
				manager.Animator.SetTrigger("Die");
			}
		}

		public void Stun(GameObject attacker, float time, float strength = 1, IHit.HitWeapon weapon = IHit.HitWeapon.OTHER)
		{
			if(strength >= 1) {
				// Enemy is stuned
				manager.Animator.SetBool("Stuned", true);
				StartCoroutine(WaitAndDo(time, () => manager.Animator.SetBool("Stuned", false)));
			} else {
				// Enemy is frozen, decrease speed
				manager.Data.speedMultiplier -= strength;
				StartCoroutine(WaitAndDo(time, () => manager.Data.speedMultiplier += strength));
			}
		}

		public void StunHit(GameObject attacker, int damage, float stunTime, IHit.HitWeapon weapon = IHit.HitWeapon.OTHER)
		{
			Hit(attacker, damage, weapon);
			Stun(attacker, stunTime, 1, weapon);
		}

		public void Riposte(GameObject sender)
		{
			Debug.LogError("Enemy riposeted no implemented");
		}
	}
}