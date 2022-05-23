using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusWindowCanvasC : MonoBehaviour {
	public GameObject player;
	public Text charName;
	public Text lv;
	public Text atk;
	public Text def;
	public Text matk;
	public Text mdef;
	public Text exp;
	public Text nextLv;
	public Text stPoint;
	
	public Text totalAtk;
	public Text totalDef;
	public Text totalMatk;
	public Text totalMdef;
	
	public Button atkUpButton;
	public Button defUpButton;
	public Button matkUpButton;
	public Button mdefUpButton;

	private PlayerData playerData;


	private void Awake()
	{
		if(!player){
			player = GameObject.FindWithTag("Player");
		}
		
		playerData = player.GetComponent<PlayerData>();
	}

	void Start(){
		
	}
	
	void Update(){
		if(!player){
			Destroy(gameObject);
			return;
		}
		 
		if(charName){
			charName.text = playerData.characterName;
		}
		if(lv){
			lv.text = playerData.level.ToString();
		}
		if(atk){
			atk.text = playerData.atk.ToString();
		}
		if(def){
			def.text = playerData.def.ToString();
		}
		if(matk){
			matk.text = playerData.matk.ToString();
		}
		if(mdef){
			mdef.text = playerData.mdef.ToString();
		}
		
		if(exp){
			exp.text = playerData.exp.ToString();
		}
		if(nextLv){
			nextLv.text = (playerData.maxExp - playerData.exp).ToString();
		}
		if(stPoint){
			stPoint.text = playerData.statusPoint.ToString();
		}
		
		if(totalAtk){
			totalAtk.text = "(" + playerData.addAtk + ")";
		}
		if(totalDef){
			totalDef.text = "(" + (playerData.def + playerData.addDef + playerData.buffDef) + ")";
		}
		if(totalMatk){
			totalMatk.text = "(" + playerData.addMatk + ")";
		}
		if(totalMdef){
			totalMdef.text = "(" + (playerData.mdef + playerData.addMdef + playerData.buffMdef) + ")";
		}
		
		if(playerData.statusPoint > 0)
		{
			if(atkUpButton)
				atkUpButton.gameObject.SetActive(true);
			if(defUpButton)
				defUpButton.gameObject.SetActive(true);
			if(matkUpButton)
				matkUpButton.gameObject.SetActive(true);
			if(mdefUpButton)
				mdefUpButton.gameObject.SetActive(true);
		}
		else
		{
			if(atkUpButton)
				atkUpButton.gameObject.SetActive(false);
			if(defUpButton)
				defUpButton.gameObject.SetActive(false);
			if(matkUpButton)
				matkUpButton.gameObject.SetActive(false);
			if(mdefUpButton)
				mdefUpButton.gameObject.SetActive(false);
		}
		
	}
	
	public void UpgradeStatus(int statusId){
		//0 = Atk , 1 = Def , 2 = Matk , 3 = Mdef
		if(!player){
			return;
		}
		
		if(statusId == 0 && playerData.statusPoint > 0){
			playerData.atk += 1;
			playerData.statusPoint -= 1;
			playerData.CalculateStatus();
		}
		else if(statusId == 1 && playerData.statusPoint > 0){
			playerData.def += 1;
			playerData.maxHealth += 5;
			playerData.statusPoint -= 1;
			playerData.CalculateStatus();
		}
		else if(statusId == 2 && playerData.statusPoint > 0){
			playerData.matk += 1;
			playerData.maxMana += 3;
			playerData.statusPoint -= 1;
			playerData.CalculateStatus();
		}
		else if(statusId == 3 && playerData.statusPoint > 0){
			playerData.mdef += 1;
			playerData.statusPoint -= 1;
			playerData.CalculateStatus();
		}
	}
	
	public void CloseMenu(){
		// Time.timeScale = 1.0f;
		// //Screen.lockCursor = true;
		// Cursor.lockState = CursorLockMode.Locked;
		// Cursor.visible = false;
		// gameObject.SetActive(false);
	}
	
}
