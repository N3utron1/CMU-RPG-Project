using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	public List<Question> questionDB;

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public string[] questions = {"1 + 1 = 2", "2 + 2 = 5", "3 + 3 = 6"};
	public string[] answers = {"True", "False", "True"};

	public int iteration = 0;
	//public int listSize;
	public int maxIteration = 2;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

    void Start()
    {
		state = BattleState.START;
		//questionDB = DatabaseManager.dbmInstance.questionDB;
		StartCoroutine(SetupBattle());
    }

//Start the battle
	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

//Player attack function
	IEnumerator PlayerAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "Correct! The attack is successful!";

		yield return new WaitForSeconds(2f);

		if(isDead)
		{
			state = BattleState.WON;
			EndBattle();
		} else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();

			//state = BattleState.ENEMYTURN;
			//StartCoroutine(EnemyTurn());
		}
	}

//Enemy turn function
	IEnumerator EnemyTurn()
	{
		dialogueText.text = "Incorrect! " + enemyUnit.unitName + " attacks!";

		yield return new WaitForSeconds(1f);

		bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		} else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

//Battle end check function
	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		} else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

//Player turn function for allowing button use
	void PlayerTurn()
	{
		if(iteration > maxIteration){
			iteration = 0;
		}
		//dialogueText.text = "Choose an action:";
		dialogueText.text = questions[iteration];
	}

//Not currently used.
	IEnumerator PlayerHeal()
	{
		playerUnit.Heal(5);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

//Button controls
	public void OnTrueButton()
	{
		PressButton("True");
	}

	public void OnFalseButton()
	{
		PressButton("False");
	}

	public void OnA_Button()
	{
		PressButton("A");
	}

	public void OnB_Button()
	{
		PressButton("B");
	}

	public void OnC_Button()
	{
		PressButton("C");
	}

	public void OnD_Button()
	{
		PressButton("D");
	}

//Button function
	public void PressButton(string buttonValue)
	{
		if (state != BattleState.PLAYERTURN)
			return;

		if(answers[iteration] == buttonValue){
			iteration = iteration + 1;
			StartCoroutine(PlayerAttack());
		}else{
			iteration = iteration + 1;
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
	}

}
