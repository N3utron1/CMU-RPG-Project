using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

	//CloudSave variable
	private const string CLOUD_SAVE_LEVEL_KEY = "level";

	public List<Question> questionDB;

	//public int maxIteration = questionDB.length

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public string winSceneName;
	public string loseSceneName;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	//Base Questions
	//public string[] questions = {"1 + 1 = 2", "2 + 2 = 5", "3 + 3 = 6"};
	//public string[] answers = {"True", "False", "True"};

	public string[] questions;
	public string[] answers;

	public int iteration = 0;
	//public int listSize;
	public int maxIteration;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

	public Button trueButton;
	public Text trueButtonText;

	public Button falseButton;
	public Text falseButtonText;

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

		playerUnit.unitLevel = StateNameController.playerLevel;

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
			StateNameController.playerLevel = StateNameController.playerLevel + 1;

			SaveDataWithErrorHandling();

			StartCoroutine(Winner());

		} else if (state == BattleState.LOST)
		{
			StartCoroutine(Loser());
		}
	}

//Win function
	IEnumerator Winner()
	{
		dialogueText.text = "You won the battle! You Level up!";

		yield return new WaitForSeconds(2f);

		SceneManager.LoadScene(winSceneName);
	}

//Lose function
	IEnumerator Loser()
	{
		dialogueText.text = "You were defeated.";

		yield return new WaitForSeconds(2f);

		SceneManager.LoadScene(loseSceneName);
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

	IEnumerator PauseForSeconds()
	{
		yield return new WaitForSeconds(2f);
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

	async void SaveDataWithErrorHandling() {
        var data = new Dictionary<string, object>{ 
            {CLOUD_SAVE_LEVEL_KEY, StateNameController.playerLevel},  
        };
        try {
            Debug.Log("Attempting to save data...");
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log("Save data success!");
        } catch (ServicesInitializationException e) {
            // service not initialized
            Debug.LogError(e);
        } catch (CloudSaveValidationException e) {
            // validation error
            Debug.LogError(e);
        } catch (CloudSaveRateLimitedException e) {
            // rate limited
            Debug.LogError(e);
        } catch (CloudSaveException e) {
            Debug.LogError(e);
        }

    }

}
