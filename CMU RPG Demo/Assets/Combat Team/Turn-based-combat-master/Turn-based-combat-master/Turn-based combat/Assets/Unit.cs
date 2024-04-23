using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

	public string unitName;
	public int unitLevel;

	public int damage;

	public int maxHP;
	public int currentHP;

	//Scripts for question.
	/*
	public int numberOfQuestions;
	public string[] enemyQuestions;
	public string[] enemyAnswers;
	*/

	public ShakerScript shakerScript;

	public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		//Shaking code
		shakerScript.Shake();

		//Victory state check
		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	public void Heal(int amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
	}
}
