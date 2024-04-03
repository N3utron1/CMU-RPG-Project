using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
//using UnityEngine.Object;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	public List<Question> questionDB;

	//public int maxIteration = questionDB.length

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

	public Button trueButton;
	public Text trueButtonText;

	public Button falseButton;
	public Text falseButtonText;

	//public Button AButton;
	//public Text AButtonText;

	//public Button BButton;
	//public Text BButtonText;

	//public Button CButton;
	//public Text CButtonText;

	//public Button DButton;
	//public Text DButtonText;

	//public bool trueFalse;

	public Vector2 truePosition;
	public Vector2 aPosition;

	public Vector2 falsePosition;
	public Vector2 cPosition;

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

		//AButton.enabled = false;
		//AButtonText.enabled = false;

		//BButton.enabled = false;
		//BButtonText.enabled = false;

		//CButton.enabled = false;
		//CButtonText.enabled = false;

		//DButton.enabled = false;
		//DButtonText.enabled = false;

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
		//Code for if it is a true/false question, or a multiple choice question.
		/*
		if(questionDB[iteration].type == "t_f"){
			trueFalse = true;
		}else{
			trueFalse = false;
		}
		*/
		//trueFalse = true;
/*
		if(trueFalse){
			//Renames
			trueButtonText.text = "True";
			falseButtonText.text = "False";

			//Positioning
			RectTransform trueButtonRectTransform = trueButton.GetComponent<RectTransform>();
			trueButtonRectTransform.anchoredPosition = truePosition;
			RectTransform falseButtonRectTransform = falseButton.GetComponent<RectTransform>();
			falseButtonRectTransform.anchoredPosition = falsePosition;

			//B and D disenabling
			BButton.enabled = false;
			BButtonText.enabled = false;
			DButton.enabled = false;
			DButtonText.enabled = false;
		}else{
			//Renames
			trueButtonText.text = "A";
			falseButtonText.text = "C";

			//Suffle Mechanism?

			trueButtonText.text = questionDB[iteration].nonAnswers[0];
			BButtonText.text = questionDB[iteration].nonAnswers[1];
			falseButtonText.text = questionDB[iteration].nonAnswers[2];
			DButtonText.text = questionDB[iteration].nonAnswers[3];

			trueButtonText.text = questionDB[iteration].questions[0];
			

			//Positioning
			RectTransform trueButtonRectTransform = trueButton.GetComponent<RectTransform>();
			trueButtonRectTransform.anchoredPosition = aPosition;
			RectTransform falseButtonRectTransform = falseButton.GetComponent<RectTransform>();
			falseButtonRectTransform.anchoredPosition = cPosition;

			//B and D enabling
			BButton.enabled = true;
			BButtonText.enabled = true;
			DButton.enabled = true;
			DButtonText.enabled = true;


		}		
*/

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
		/*
		if(trueFalse){
			PressButton("True");
		}else{
			MultiPressButton(trueButtonText.text);
		}
		*/
	}

	public void OnFalseButton()
	{
		PressButton("False");
		/*
		if(trueFalse){
			PressButton("False");
		}else{
			MultiPressButton(falseButtonText.text);
		}
		*/
	}
/*
	public void OnB_Button()
	{
		MultiPressButton(BButtonText.text);
	}

	public void OnD_Button()
	{
		MultiPressButton(DButtonText.text);
	}
*/
//True/False Button function
	public void PressButton(string buttonValue)
	{
		if (state != BattleState.PLAYERTURN)
			return;
//Add to new lines.
		if(answers[iteration] == buttonValue){
			iteration = iteration + 1;
			StartCoroutine(PlayerAttack());
		}else{
			iteration = iteration + 1;
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
	}
//Multichoice Button function
/*
		public void MultiPressButton(string buttonValue)
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
*/

/*
  if(questionDB[iteration].answer == buttonValue){
	iteration = iteration + 1;
	StartCoroutine(PlayerAttack());
  }
*/

}
