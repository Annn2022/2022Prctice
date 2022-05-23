using System;
using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	[HideInInspector]
	public bool menu = false;
	private bool itemMenu = true;
	private bool equipMenu = false;
	
	//道具包
	public int[] itemSlot;
	//道具包的数量
	public int[] itemQuantity;
	//装备包
	public int[] equipment;

	public bool autoSetToAtkTrigger = false;
	public bool allowWeaponUnequip = false;
	
	public int weaponEquip = 0;
	public int subWeaponEquip = 0;
	public int armorEquip = 0;
	public int hatEquip = 0;
	public int glovesEquip = 0;
	public int bootsEquip = 0;
	public int accessoryEquip = 0;
	
	//玩家装备的武器组
	public GameObject[] weapon = new GameObject[1];
	
	//道具
	public GameObject database;
	public GameObject fistPrefab;
	public GameObject InventeryUI;
	
	public int cash = 500;
	
	public GUISkin skin;
	public Rect windowRect = new Rect(260 ,140 ,280 ,385);

	private PlayerData _playerData;
	private ItemDataC dataItem;
	private AttackTriggerC attackTrigger;
	
	private StarterAssetsInputs _input;
	
	//private string hover = ""; 

	private void Awake()
	{
		itemSlot = new int[16];
		itemQuantity = new int[16];
		equipment = new int[10];
		
		_playerData = GetComponent<PlayerData>();
		dataItem = database.GetComponent<ItemDataC>();
		attackTrigger = GetComponent<AttackTriggerC>();
		_input = GetComponent<StarterAssetsInputs>();
		
		
	}

	void Start(){
		//Reset Power of Current Weapon & Armor
		//设定玩家的装备的数值
		SettingEquipmentStatus();
		StartCoroutine(DelayUpdateUI());
		if(autoSetToAtkTrigger){
			int tempEq = weaponEquip;
			weaponEquip = 0;
			EquipItem(tempEq , 9999);
		}
		
		GameObject.Find("EquipmentTab").SetActive(false);
		
	}

	IEnumerator DelayUpdateUI(){
		yield return new WaitForSeconds(0.05f);
		UpdateAmmoUI();
	}
	
	void Update(){
		if(_input.openBag){
			OnOffMenu();
			_input.openBag = false;
			//AutoSortItem();
		}
		
		CheckMenu();
	}
	
	/// <summary>
	/// 使用道具
	/// </summary>
	/// <param name="slot"></param>
	public void UseItem(int slot)
	{
		int id = itemSlot[slot];
		if(dataItem.usableItem[id].unusable){
			return;
		}
		
		_playerData.Heal(dataItem.usableItem[id].hpRecover , dataItem.usableItem[id].mpRecover);
		_playerData.atk += dataItem.usableItem[id].atkPlus;
		_playerData.def += dataItem.usableItem[id].defPlus;
		
		if(dataItem.usableItem[id].sendMsg != ""){
			SendMessage(dataItem.usableItem[id].sendMsg , SendMessageOptions.DontRequireReceiver);
		}
		itemQuantity[slot]--;
		if(itemQuantity[slot] <= 0){
			itemSlot[slot] = 0;
			itemQuantity[slot] = 0;
		}
		AutoSortItem();
	}

	/// <summary>
	/// 设置玩家的装备数值
	/// </summary>
	void SettingEquipmentStatus()
	{
		if(!GetComponent<PlayerData>()){
			return;
		}
		
		//Reset Power of Current Weapon & Armor
		//武器
		_playerData.weaponAtk = dataItem.equipment[weaponEquip].attack;
		_playerData.addDef = dataItem.equipment[weaponEquip].defense;
		_playerData.weaponMatk = dataItem.equipment[weaponEquip].magicAttack;
		
		_playerData.addHP = dataItem.equipment[weaponEquip].hpBonus;
		
		//盔甲
		_playerData.weaponAtk += dataItem.equipment[armorEquip].attack;
		_playerData.addDef += dataItem.equipment[armorEquip].defense;
		_playerData.addHP += dataItem.equipment[armorEquip].hpBonus;
		//头饰
		_playerData.weaponAtk += dataItem.equipment[hatEquip].attack;
		_playerData.addDef += dataItem.equipment[hatEquip].defense;
		_playerData.weaponMatk += dataItem.equipment[hatEquip].magicAttack;
		
		_playerData.addHP += dataItem.equipment[hatEquip].hpBonus;
		
		//手套
		_playerData.weaponAtk += dataItem.equipment[glovesEquip].attack;
		_playerData.addDef += dataItem.equipment[glovesEquip].defense;
		_playerData.weaponMatk += dataItem.equipment[glovesEquip].magicAttack;
		_playerData.addHP += dataItem.equipment[glovesEquip].hpBonus;
		
		//鞋子
		_playerData.weaponAtk += dataItem.equipment[bootsEquip].attack;
		_playerData.addDef += dataItem.equipment[bootsEquip].defense;
		_playerData.weaponMatk += dataItem.equipment[bootsEquip].magicAttack;
		_playerData.addHP += dataItem.equipment[bootsEquip].hpBonus;
		
		//Set New Variable of 饰品
		_playerData.weaponAtk += dataItem.equipment[accessoryEquip].attack;
		_playerData.addDef += dataItem.equipment[accessoryEquip].defense;
		_playerData.weaponMatk += dataItem.equipment[accessoryEquip].magicAttack;
		_playerData.addHP += dataItem.equipment[accessoryEquip].hpBonus;
		
		//各种抗性
		_playerData.eqResist.poisonResist = dataItem.equipment[weaponEquip].statusResist.poisonResist + dataItem.equipment[armorEquip].statusResist.poisonResist + dataItem.equipment[accessoryEquip].statusResist.poisonResist + dataItem.equipment[hatEquip].statusResist.poisonResist + dataItem.equipment[glovesEquip].statusResist.poisonResist + dataItem.equipment[bootsEquip].statusResist.poisonResist;
		_playerData.eqResist.stunResist = dataItem.equipment[weaponEquip].statusResist.stunResist + dataItem.equipment[armorEquip].statusResist.stunResist + dataItem.equipment[accessoryEquip].statusResist.stunResist + dataItem.equipment[hatEquip].statusResist.stunResist + dataItem.equipment[glovesEquip].statusResist.stunResist + dataItem.equipment[bootsEquip].statusResist.stunResist;
		_playerData.eqResist.silenceResist = dataItem.equipment[weaponEquip].statusResist.silenceResist + dataItem.equipment[armorEquip].statusResist.silenceResist + dataItem.equipment[accessoryEquip].statusResist.silenceResist + dataItem.equipment[hatEquip].statusResist.silenceResist + dataItem.equipment[glovesEquip].statusResist.silenceResist + dataItem.equipment[bootsEquip].statusResist.silenceResist;
		_playerData.eqResist.webResist = dataItem.equipment[weaponEquip].statusResist.webResist + dataItem.equipment[armorEquip].statusResist.webResist + dataItem.equipment[accessoryEquip].statusResist.webResist + dataItem.equipment[hatEquip].statusResist.webResist + dataItem.equipment[glovesEquip].statusResist.webResist + dataItem.equipment[bootsEquip].statusResist.webResist;
		
		//隐藏能力 二段跳设计
		_playerData.hiddenStatus.doubleJump = false;
		if(dataItem.equipment[weaponEquip].canDoubleJump){
			_playerData.hiddenStatus.doubleJump = true;
		}
		else if(dataItem.equipment[armorEquip].canDoubleJump){
			_playerData.hiddenStatus.doubleJump = true;
		}
		else if(dataItem.equipment[hatEquip].canDoubleJump){
			_playerData.hiddenStatus.doubleJump = true;
		}
		else if(dataItem.equipment[glovesEquip].canDoubleJump){
			_playerData.hiddenStatus.doubleJump = true;
		}
		else if(dataItem.equipment[bootsEquip].canDoubleJump){
			_playerData.hiddenStatus.doubleJump = true;
		}
		else if(dataItem.equipment[accessoryEquip].canDoubleJump){
			_playerData.hiddenStatus.doubleJump = true;
		}
		_playerData.hiddenStatus.autoGuard = dataItem.equipment[weaponEquip].autoGuard + dataItem.equipment[armorEquip].autoGuard + dataItem.equipment[accessoryEquip].autoGuard + dataItem.equipment[hatEquip].autoGuard + dataItem.equipment[glovesEquip].autoGuard + dataItem.equipment[bootsEquip].autoGuard;
		_playerData.hiddenStatus.drainTouch = dataItem.equipment[weaponEquip].drainTouch + dataItem.equipment[armorEquip].drainTouch + dataItem.equipment[accessoryEquip].drainTouch + dataItem.equipment[hatEquip].drainTouch + dataItem.equipment[glovesEquip].drainTouch + dataItem.equipment[bootsEquip].drainTouch;
		_playerData.hiddenStatus.mpReduce = dataItem.equipment[weaponEquip].mpReduce + dataItem.equipment[armorEquip].mpReduce + dataItem.equipment[accessoryEquip].mpReduce + dataItem.equipment[hatEquip].mpReduce + dataItem.equipment[glovesEquip].mpReduce + dataItem.equipment[bootsEquip].mpReduce;

		_playerData.CalculateStatus();
	}
	
	/// <summary>
	/// 穿上装备
	/// </summary>
	/// <param name="id">装备id</param>
	/// <param name="slot">背包格子</param>
	public void EquipItem(int id , int slot){
		GameObject wea = new GameObject();
		if(id == 0){
			return;
		}
		
		//Backup Your Current Equipment before Unequip
		int tempEquipment = 0;
		
		if((int)dataItem.equipment[id].EquipmentType == 0)
		{
			//Equipment = Weapon
			tempEquipment = weaponEquip;
			weaponEquip = id;

			AttackTriggerC attackTrigger = GetComponent<AttackTriggerC>();
			//设定攻击预制体及攻击的各种参数
			if(dataItem.equipment[id].attackPrefab)
			{
				attackTrigger.attackPrefab = dataItem.equipment[id].attackPrefab.transform;
			}
			attackTrigger.weaponType = dataItem.equipment[id].weaponType;
			int reqId = dataItem.equipment[id].requireItemId;
			attackTrigger.requireItemId = reqId;
			attackTrigger.requireItemName = dataItem.usableItem[reqId].itemName;
			attackTrigger.attackSoundEffect = dataItem.equipment[id].soundEffect;
			
			//Update Show Ammo UI
			// if(reqId > 0 && ShowAmmoC.showAmmo){
			// 	ShowAmmoC.showAmmo.OnOffShowing(true);
			// 	int sl = FindItemSlot(reqId);
			// 	int am = 0;
			// 	Sprite spr = dataItem.usableItem[reqId].iconSprite;
			// 	if(sl < itemQuantity.Length){
			// 		am = itemQuantity[sl];
			// 	}	
			// 	ShowAmmoC.showAmmo.UpdateSprite(spr);
			// 	ShowAmmoC.showAmmo.UpdateAmmo(am);
			// }else if(ShowAmmoC.showAmmo){
			// 	ShowAmmoC.showAmmo.OnOffShowing(false);
			// }

			
			//Change Weapon Mesh
			if(dataItem.equipment[id].model && weapon.Length > 0 && weapon[0] != null)
			{
				int allweapenCount = weapon.Length;
				int a = 0;
				
				//分配到所有武器
				if(allweapenCount > 0 && dataItem.equipment[id].assignAllWeapon)
				{
					//遍历武器库
					while(a < allweapenCount && weapon[a]){
						weapon[a].SetActive(true);
						wea = Instantiate(dataItem.equipment[id].model, weapon[a].transform.position, weapon[a].transform.rotation) as GameObject;
						wea.transform.parent = weapon[a].transform.parent;
						Destroy(weapon[a].gameObject);
						weapon[a] = wea;
						a++;
					}
				}
				else if(allweapenCount > 0)
				{
					while(a < allweapenCount && weapon[a]){
						if(a == 0){
							weapon[a].SetActive(true);
							wea = Instantiate(dataItem.equipment[id].model,
								weapon[a].transform.position, weapon[a].transform.rotation) as GameObject;
							wea.transform.parent = weapon[a].transform.parent;
							Destroy(weapon[a].gameObject);
							weapon[a] = wea;
						}
						else
						{
							weapon[a].SetActive(false);
						}
						a++;
					}
				}
			}
		}
		else if((int)dataItem.equipment[id].EquipmentType == 1){
			//Armor Type
			tempEquipment = armorEquip;
			armorEquip = id;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 2){
			//Accessory Type
			tempEquipment = accessoryEquip;
			accessoryEquip = id;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 3){
			//Headgear Type
			tempEquipment = hatEquip;
			hatEquip = id;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 4){
			//Gloves Type
			tempEquipment = glovesEquip;
			glovesEquip = id;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 5){
			//Boots Type
			tempEquipment = bootsEquip;
			bootsEquip = id;
		}
		//将装备从装备包中移除
		if(slot <= equipment.Length){
			equipment[slot] = 0;
		}
		//设定武器的攻击动画及相关各项数值
		AssignWeaponAnimation(id);
		//设定玩家的装备数值
		SettingEquipmentStatus();
		//装备自动排序
		AutoSortEquipment();
		//将卸下的装备添加到装备栏
		AddEquipment(tempEquipment);
	}
	
	/// <summary>
	/// 切换武器
	/// </summary>
	public void SwapWeapon(){
		int tempEq = weaponEquip; //Store Main Weapon Data

		if(subWeaponEquip == 0){
			//Use Unequip Instead if no Sub Weapon equipped
			weaponEquip = 0; // Set to 0 because we didn't want to add it to inventory after swap.
			UnEquip(0);
			subWeaponEquip = tempEq;
			return;
		}
		weaponEquip = 0; // Set to 0 because we didn't want to add it to inventory after swap.
		EquipItem(subWeaponEquip , equipment.Length + 10);
		subWeaponEquip = tempEq;
	}

	
	/// <summary>
	/// 移除装备中的武器的显示
	/// </summary>
	public void RemoveWeaponMesh(){
		if(weapon.Length > 0 && weapon[0] != null){
			int allWeapon = weapon.Length;
			int a = 0;
			if(allWeapon > 0){
				while(a < allWeapon && weapon[a]){
					weapon[a].SetActive(false);
					//Destroy(weapon[a].gameObject);
					a++;
				}
			}
		}
	}
	
	/// <summary>
	/// 卸下装备
	/// </summary>
	/// <param name="id"></param>
	public void UnEquip(int id)
	{
		bool full = false;

		if((int)dataItem.equipment[id].EquipmentType == 0)
		{
			full = AddEquipment(weaponEquip);
		}
		else if((int)dataItem.equipment[id].EquipmentType == 1)
		{
			full = AddEquipment(armorEquip);
		}
		else if((int)dataItem.equipment[id].EquipmentType == 2)
		{
			full = AddEquipment(accessoryEquip);
		}
		else if((int)dataItem.equipment[id].EquipmentType == 3)
		{
			full = AddEquipment(hatEquip);
		}
		else if((int)dataItem.equipment[id].EquipmentType == 4)
		{
			full = AddEquipment(glovesEquip);
		}
		else if((int)dataItem.equipment[id].EquipmentType == 5)
		{
			full = AddEquipment(bootsEquip);
		}
		
		
		if(full)
			return;
			
		if((int)dataItem.equipment[id].EquipmentType == 0)
		{
			weaponEquip = 0;

			attackTrigger.weaponType = 0;
			int reqId = 0;
			attackTrigger.requireItemId = reqId;
			attackTrigger.requireItemName = "";
			UpdateAmmoUI();
			//GetComponent<AttackTriggerC>().canBlock = false;

			attackTrigger.attackPrefab = fistPrefab.transform;
			
			//显示层
			if(weapon.Length > 0 && weapon[0] != null){
				int allWeapon = weapon.Length;
				int a = 0;
				if(allWeapon > 0){
					while(a < allWeapon && weapon[a]){
						weapon[a].SetActive(false);
						//Destroy(weapon[a].gameObject);
						a++;
					}
				}
			}
			
			AssignWeaponAnimation(0);
			attackTrigger.attackSoundEffect = dataItem.equipment[0].soundEffect;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 1)
		{
			armorEquip = 0;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 2)
		{
			accessoryEquip = 0;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 3)
		{
			hatEquip = 0;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 4)
		{
			glovesEquip = 0;
		}
		else if((int)dataItem.equipment[id].EquipmentType == 5)
		{
			bootsEquip = 0;
		}
		//重新设定玩家的装备数值
		SettingEquipmentStatus();
		
	}
	
	
	//TODO ui界面及触发事件的重写

	void CheckMenu()
	{
		InventeryUI.SetActive(menu);
	}
	// void OnGUI(){
	// 	GUI.skin = skin;
	// 	if(menu && itemMenu){
	// 		windowRect = GUI.Window (1, windowRect, ItemWindow, "Items");
	// 	}
	// 	if(menu && equipMenu){
	// 		windowRect = GUI.Window (1, windowRect, ItemWindow, "Equipment");
	// 	}
	// 	
	// 	if(menu){
	// 		if (GUI.Button ( new Rect(windowRect.x -50, windowRect.y +105,50,100), "Item")) {
	// 			//Switch to Item Tab
	// 			itemMenu = true;
	// 			equipMenu = false;
	// 		}
	// 		if (GUI.Button ( new Rect(windowRect.x -50, windowRect.y +225,50,100), "Equip")) {
	// 			//Switch to Equipment Tab
	// 			equipMenu = true;
	// 			itemMenu = false;	
	// 		}
	// 	}
	// 	//hover = GUI.tooltip;
	// }
	//
	//-----------Item Window-------------
	/// <summary>
	/// 道具UI的事件
	/// </summary>
	/// <param name="windowID"></param>
	// void ItemWindow(int windowID){
	// 	
	// 	if(menu && itemMenu){
	// 		//GUI.Box ( new Rect(260,140,280,385), "Items");
	// 		//Close Window Button
	// 		if (GUI.Button ( new Rect(250,2,30,30), "X")) {
	// 			OnOffMenu();
	// 		}
	// 		//Items Slot
	// 		if (GUI.Button(new Rect(30,115,50,50),new GUIContent (dataItem.usableItem[itemSlot[0]].icon, dataItem.usableItem[itemSlot[0]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[0]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[0]].unusable){
	// 				UseItem(0);
	// 			}
	// 		}
	// 		if(itemQuantity[0] > 0){
	// 			GUI.Label(new Rect(70, 150, 20, 20), itemQuantity[0].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(90,115,50,50),new GUIContent (dataItem.usableItem[itemSlot[1]].icon, dataItem.usableItem[itemSlot[1]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[1]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[1]].unusable){
	// 				UseItem(1);
	// 			}
	// 		}
	// 		if(itemQuantity[1] > 0){
	// 			GUI.Label(new Rect(130, 150, 20, 20), itemQuantity[1].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(150,115,50,50),new GUIContent (dataItem.usableItem[itemSlot[2]].icon, dataItem.usableItem[itemSlot[2]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[2]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[2]].unusable){
	// 				UseItem(2);
	// 			}
	// 		}
	// 		if(itemQuantity[2] > 0){
	// 			GUI.Label(new Rect(190, 150, 20, 20), itemQuantity[2].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(210,115,50,50),new GUIContent (dataItem.usableItem[itemSlot[3]].icon, dataItem.usableItem[itemSlot[3]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[3]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[3]].unusable){
	// 				UseItem(3);
	// 			}
	// 		}
	// 		if(itemQuantity[3] > 0){
	// 			GUI.Label(new Rect(250, 150, 20, 20), itemQuantity[3].ToString()); //Quantity
	// 		}
	// 		
	// 		//-----------------------------
	// 		if (GUI.Button(new Rect(30,175,50,50),new GUIContent (dataItem.usableItem[itemSlot[4]].icon, dataItem.usableItem[itemSlot[4]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[4]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[4]].unusable){
	// 				UseItem(4);
	// 			}
	// 		}
	// 		if(itemQuantity[4] > 0){
	// 			GUI.Label(new Rect(70, 210, 20, 20), itemQuantity[4].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(90,175,50,50),new GUIContent (dataItem.usableItem[itemSlot[5]].icon, dataItem.usableItem[itemSlot[5]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[5]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[5]].unusable){
	// 				UseItem(5);
	// 			}
	// 		}
	// 		if(itemQuantity[5] > 0){
	// 			GUI.Label ( new Rect(130, 210, 20, 20), itemQuantity[5].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(150,175,50,50),new GUIContent (dataItem.usableItem[itemSlot[6]].icon, dataItem.usableItem[itemSlot[6]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[6]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[6]].unusable){
	// 				UseItem(6);
	// 			}
	// 		}
	// 		if(itemQuantity[6] > 0){
	// 			GUI.Label(new Rect(190, 210, 20, 20), itemQuantity[6].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(210,175,50,50),new GUIContent (dataItem.usableItem[itemSlot[7]].icon, dataItem.usableItem[itemSlot[7]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[7]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[7]].unusable){
	// 				UseItem(7);
	// 			}
	// 		}
	// 		if(itemQuantity[7] > 0){
	// 			GUI.Label(new Rect(250, 210, 20, 20), itemQuantity[7].ToString()); //Quantity
	// 		}
	// 		//-----------------------------
	// 		if (GUI.Button ( new Rect(30,235,50,50),new GUIContent (dataItem.usableItem[itemSlot[8]].icon, dataItem.usableItem[itemSlot[8]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[8]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[8]].unusable){
	// 				UseItem(8);
	// 			}
	// 		}
	// 		if(itemQuantity[8] > 0){
	// 			GUI.Label(new Rect(70, 270, 20, 20), itemQuantity[8].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(90,235,50,50),new GUIContent (dataItem.usableItem[itemSlot[9]].icon, dataItem.usableItem[itemSlot[9]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[9]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[9]].unusable){
	// 				UseItem(9);
	// 			}
	// 		}
	// 		if(itemQuantity[9] > 0){
	// 			GUI.Label(new Rect(130, 270, 20, 20), itemQuantity[9].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(150,235,50,50),new GUIContent (dataItem.usableItem[itemSlot[10]].icon, dataItem.usableItem[itemSlot[10]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[10]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[10]].unusable){
	// 				UseItem(10);
	// 			}
	// 		}
	// 		if(itemQuantity[10] > 0){
	// 			GUI.Label ( new Rect(190, 270, 20, 20), itemQuantity[10].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(210,235,50,50),new GUIContent (dataItem.usableItem[itemSlot[11]].icon, dataItem.usableItem[itemSlot[11]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[11]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[11]].unusable){
	// 				UseItem(11);
	// 			}
	// 		}
	// 		if(itemQuantity[11] > 0){
	// 			GUI.Label(new Rect(250, 270, 20, 20), itemQuantity[11].ToString()); //Quantity
	// 		}
	// 		//-----------------------------
	// 		if (GUI.Button ( new Rect(30,295,50,50),new GUIContent (dataItem.usableItem[itemSlot[12]].icon, dataItem.usableItem[itemSlot[12]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[12]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[12]].unusable){
	// 				UseItem(12);
	// 			}
	// 		}
	// 		if(itemQuantity[12] > 0){
	// 			GUI.Label(new Rect(70, 330, 20, 20), itemQuantity[12].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(90,295,50,50),new GUIContent (dataItem.usableItem[itemSlot[13]].icon, dataItem.usableItem[itemSlot[13]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[13]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[13]].unusable){
	// 				UseItem(13);
	// 			}
	// 		}
	// 		if(itemQuantity[13] > 0){
	// 			GUI.Label ( new Rect(130, 330, 20, 20), itemQuantity[13].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(150,295,50,50),new GUIContent (dataItem.usableItem[itemSlot[14]].icon, dataItem.usableItem[itemSlot[14]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[14]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[14]].unusable){
	// 				UseItem(14);
	// 			}
	// 		}
	// 		if(itemQuantity[14] > 0){
	// 			GUI.Label(new Rect(190, 330, 20, 20), itemQuantity[14].ToString()); //Quantity
	// 		}
	// 		
	// 		if (GUI.Button(new Rect(210,295,50,50),new GUIContent (dataItem.usableItem[itemSlot[15]].icon, dataItem.usableItem[itemSlot[15]].itemName + "\n" + "\n" + dataItem.usableItem[itemSlot[15]].description ))){
	// 			if(!dataItem.usableItem[itemSlot[15]].unusable){
	// 				UseItem(15);
	// 			}
	// 			
	// 		}
	// 		if(itemQuantity[15] > 0){
	// 			GUI.Label(new Rect(250, 330, 20, 20), itemQuantity[15].ToString()); //Quantity
	// 		}
	// 		GUI.Label ( new Rect(20, 355, 150, 50), "$ " + cash.ToString());
	// 		GUI.Box ( new Rect(20,30,240,60), GUI.tooltip);
	// 		//---------------------------
	// 	}
	// 	
	// 	//---------------Equipment Tab----------------------------
	// 	if(menu && equipMenu){
	// 		//Close Window Button
	// 		if(GUI.Button ( new Rect(250,2,30,30), "X")) {
	// 			OnOffMenu();
	// 		}
	// 		//Weapon
	// 		GUI.Label ( new Rect(20, 130, 150, 50), "Weapon");			
	// 		if(GUI.Button ( new Rect(100,115,50,50),new GUIContent (dataItem.equipment[weaponEquip].icon, dataItem.equipment[weaponEquip].itemName + "\n" + "\n" + dataItem.equipment[weaponEquip].description ))){
	// 			if(!allowWeaponUnequip || weaponEquip == 0){
	// 				return;
	// 			}
	// 			UnEquip(weaponEquip);
	// 		}
	// 		//Armor
	// 		GUI.Label ( new Rect(20, 190, 150, 50), "Armor");
	// 		if (GUI.Button ( new Rect(100,175,50,50),new GUIContent (dataItem.equipment[armorEquip].icon, dataItem.equipment[armorEquip].itemName + "\n" + "\n" + dataItem.equipment[armorEquip].description ))){
	// 			if(armorEquip == 0){
	// 				return;
	// 			}
	// 			UnEquip(armorEquip);
	// 		}
	// 		
	// 		//--------Equipment Slot---------
	// 		if(GUI.Button ( new Rect(30,235,50,50),new GUIContent (dataItem.equipment[equipment[0]].icon, dataItem.equipment[equipment[0]].itemName + "\n" + "\n" + dataItem.equipment[equipment[0]].description ))){
	// 			EquipItem(equipment[0] , 0);
	// 		}
	// 		
	// 		if(GUI.Button ( new Rect(90,235,50,50),new GUIContent (dataItem.equipment[equipment[1]].icon, dataItem.equipment[equipment[1]].itemName + "\n" + "\n" + dataItem.equipment[equipment[1]].description ))){
	// 			EquipItem(equipment[1] , 1);
	// 		}
	// 		
	// 		if(GUI.Button ( new Rect(150,235,50,50),new GUIContent (dataItem.equipment[equipment[2]].icon, dataItem.equipment[equipment[2]].itemName + "\n" + "\n" + dataItem.equipment[equipment[2]].description ))){
	// 			EquipItem(equipment[2] , 2);
	// 		}
	// 		
	// 		if(GUI.Button ( new Rect(210,235,50,50),new GUIContent (dataItem.equipment[equipment[3]].icon, dataItem.equipment[equipment[3]].itemName + "\n" + "\n" + dataItem.equipment[equipment[3]].description ))){
	// 			EquipItem(equipment[3] , 3);
	// 		}
	// 		//-----------------------------
	// 		if(GUI.Button ( new Rect(30,295,50,50),new GUIContent (dataItem.equipment[equipment[4]].icon, dataItem.equipment[equipment[4]].itemName + "\n" + "\n" + dataItem.equipment[equipment[4]].description ))){
	// 			EquipItem(equipment[4] , 4);
	// 		}
	// 		
	// 		if(GUI.Button ( new Rect(90,295,50,50),new GUIContent (dataItem.equipment[equipment[5]].icon, dataItem.equipment[equipment[5]].itemName + "\n" + "\n" + dataItem.equipment[equipment[5]].description ))){
	// 			EquipItem(equipment[5] , 5);
	// 		}
	// 		
	// 		if(GUI.Button ( new Rect(150,295,50,50),new GUIContent (dataItem.equipment[equipment[6]].icon, dataItem.equipment[equipment[6]].itemName + "\n" + "\n" + dataItem.equipment[equipment[6]].description ))){
	// 			EquipItem(equipment[6] , 6);
	// 		}
	// 		
	// 		if(GUI.Button ( new Rect(210,295,50,50),new GUIContent (dataItem.equipment[equipment[7]].icon, dataItem.equipment[equipment[7]].itemName + "\n" + "\n" + dataItem.equipment[equipment[7]].description ))){
	// 			EquipItem(equipment[7] , 7);
	// 		}
	// 		GUI.Label ( new Rect(20, 355, 150, 50), "$ " + cash.ToString());
	// 		GUI.Box ( new Rect(20,30,240,60), GUI.tooltip);
	// 	}
	// 	GUI.DragWindow (new Rect (0,0,10000,10000)); 
	// }
	
	/// <summary>
	/// 向背包添加道具，如果背包满了，则返回true
	/// </summary>
	/// <param name="id"></param>
	/// <param name="quan">数量</param>
	/// <returns></returns>
	public bool AddItem(int id , int quan)
	{
		bool full = false;
		bool geta = false;
		
		int pt = 0;
		while(pt < itemSlot.Length && !geta){
			if(itemSlot[pt] == id){
				itemQuantity[pt] += quan;
				geta = true;
			}
			else if(itemSlot[pt] == 0)
			{
				itemSlot[pt] = id;
				itemQuantity[pt] = quan;
				geta = true;
			}
			else
			{
				pt++;
				if(pt >= itemSlot.Length){
					full = true;
					print("Full");
				}
			}
		}
		UpdateAmmoUI();

		int slot = FindItemSlot(id);
		if(slot < itemSlot.Length){
			if(itemQuantity[slot] <= 0){
				itemSlot[slot] = 0;
				itemQuantity[slot] = 0;
				AutoSortItem();
			}
		}
		return full;
	}
	
	/// <summary>
	/// 将装备添加到装备栏中，返回装备栏是否已满
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public bool AddEquipment(int id)
	{
		bool full = false;
		bool geta = false;

		int pt = 0;
		while(pt < equipment.Length && !geta){
			if(equipment[pt] == 0){
				equipment[pt] = id;
				geta = true;
			}else{
				pt++;
				if(pt >= equipment.Length){
					full = true;
					print("Full");
				}
			}
		}
		return full;
	}
	
	
	//------------AutoSort----------
	public void AutoSortItem(){
		int pt = 0;
		int nextp = 0;
		bool clear = false;
		while(pt < itemSlot.Length)
		{
			if(itemSlot[pt] == 0){
				nextp = pt + 1;
				while(nextp < itemSlot.Length && !clear){
					if(itemSlot[nextp] > 0){
						//Fine Next Item and Set
						itemSlot[pt] = itemSlot[nextp];
						itemQuantity[pt] = itemQuantity[nextp];
						itemSlot[nextp] = 0;
						itemQuantity[nextp] = 0;
						clear = true;
					}
					else
						nextp++;
					
				}
				//Continue New Loop
				clear = false;
				pt++;
			}
			else
				pt++;
			
		}
		UpdateAmmoUI();
	}
	
	public void AutoSortEquipment(){
		int pt = 0;
		int nextp = 0;
		bool  clearr = false;
		while(pt < equipment.Length){
			if(equipment[pt] == 0){
				nextp = pt + 1;
				while(nextp < equipment.Length && !clearr){
					if(equipment[nextp] > 0){
						//Fine Next Item and Set
						equipment[pt] = equipment[nextp];
						equipment[nextp] = 0;
						clearr = true;
					}else{
						nextp++;
					}
					
				}
				//Continue New Loop
				clearr = false;
				pt++;
			}else{
				pt++;
			}
		}
	}

	
	/// <summary>
	/// menu菜单栏的开关
	/// </summary>
	public void OnOffMenu(){
		//Freeze Time Scale to 0 if Window is Showing
		if(!menu && Time.timeScale != 0.0f){
			menu = true;
			Time.timeScale = 0.0f;
			//Screen.lockCursor = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			ResetPosition();
		}
		else if(menu)
		{
			menu = false;
			Time.timeScale = 1.0f;
			//Screen.lockCursor = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
	
	
	/// <summary>
	/// 设定武器的动画
	/// </summary>
	/// <param name="id"></param>
	void AssignWeaponAnimation(int id){
		
		// PlayerAnimationC playerAnim = GetComponent<PlayerAnimationC>();
		// if(!playerAnim){
		// 	//If use Mecanim
		// 	AssignMecanimAnimation(id);
		// 	return;
		// }
		
		//设定该武器的所有combo动画
		if(dataItem.equipment[id].attackCombo.Length > 0 && 
		   dataItem.equipment[id].attackCombo[0] != null && dataItem.equipment[id].EquipmentType == 0)
		{
			int allPrefab = dataItem.equipment[id].attackCombo.Length;
			attackTrigger.attackCombo = new AnimationClip[allPrefab];
			
			int a = 0;
			if(allPrefab > 0){
				while(a < allPrefab){
					attackTrigger.attackCombo[a] = dataItem.equipment[id].attackCombo[a];
					attackTrigger.mainModel.GetComponent<Animation>()[dataItem.equipment[id].attackCombo[a].name].layer = 15;
					a++;
				}
			}
			int watk = (int)dataItem.equipment[id].whileAttack;
			attackTrigger.WhileAttackSet(watk);
			//依据动画来设定攻击的各项速度
			attackTrigger.attackSpeed = dataItem.equipment[id].attackSpeed;
			attackTrigger.atkDelay1 = dataItem.equipment[id].attackDelay;
			attackTrigger.blockingAnimation = dataItem.equipment[id].blockingAnimation;
			attackTrigger.canBlock = dataItem.equipment[id].canBlock;
		}

		// if(dataItem.equipment[id].idleAnimation){
		// 	playerAnim.idle = dataItem.equipment[id].idleAnimation;
		// }
		// if(dataItem.equipment[id].runAnimation){
		// 	playerAnim.run = dataItem.equipment[id].runAnimation;
		// }
		// if(dataItem.equipment[id].rightAnimation){
		// 	playerAnim.right = dataItem.equipment[id].rightAnimation;
		// }
		// if(dataItem.equipment[id].leftAnimation){
		// 	playerAnim.left = dataItem.equipment[id].leftAnimation;
		// }
		// if(dataItem.equipment[id].backAnimation){
		// 	playerAnim.back = dataItem.equipment[id].backAnimation;
		// }
		// if(dataItem.equipment[id].jumpAnimation){
		// 	playerAnim.jump = dataItem.equipment[id].jumpAnimation;
		// }
		// if(dataItem.equipment[id].jumpUpAnimation){
		// 	playerAnim.jumpUp = dataItem.equipment[id].jumpUpAnimation;
		// }
		// playerAnim.AnimationSpeedSet();
	}
	
	
	/// <summary>
	/// 重新设定menu的窗口
	/// </summary>
	void ResetPosition(){
		//Reset GUI Position when it out of Screen.
		if(windowRect.x >= Screen.width -30 || windowRect.y >= Screen.height -30 || windowRect.x <= -70 || windowRect.y <= -70 )
		{
			windowRect = new Rect (260 ,140 ,280 ,385);
		}
	}

	// void AssignMecanimAnimation(int id)
	// {
	// 	if(dataItem.equipment[id].EquipmentType == 0){
	// 		int watk = (int)dataItem.equipment[id].whileAttack;
	// 		GetComponent<AttackTriggerC>().WhileAttackSet(watk);
	// 		//Assign Attack Speed
	// 		GetComponent<AttackTriggerC>().attackSpeed = dataItem.equipment[id].attackSpeed;
	// 		GetComponent<AttackTriggerC>().atkDelay1 = dataItem.equipment[id].attackDelay;
	// 		GetComponent<AttackTriggerC>().blockingAnimation = dataItem.equipment[id].blockingAnimation;
	// 		GetComponent<AttackTriggerC>().canBlock = dataItem.equipment[id].canBlock;
	// 		//Set Weapon Type ID to Mecanim Animator and Set New Idle
	// 		GetComponent<PlayerMecanimAnimationC>().SetWeaponType(dataItem.equipment[id].weaponType);
	// 		
	// 		int allPrefab = dataItem.equipment[id].attackCombo.Length;
	// 		GetComponent<AttackTriggerC>().attackCombo = new AnimationClip[allPrefab];
	// 		
	// 		//Set Attack Animation
	// 		int a = 0;
	// 		if(allPrefab > 0){
	// 			while(a < allPrefab){
	// 				GetComponent<AttackTriggerC>().attackCombo[a] = dataItem.equipment[id].attackCombo[a];
	// 				a++;
	// 			}
	// 		}
	// 	}
	// }
	//--------------------------------------------

	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="type"></param>
	/// <param name="qty"></param>
	/// <returns></returns>
	public bool CheckItem(int id , int type, int qty){
		bool having = false;
		bool geta = false;
		//type 0 = Usable , 1 = Equipment
		
		int pt = 0;
		
		//================Usable==================
		if(type == 0){
			while(pt < itemSlot.Length && !geta){
				if(itemSlot[pt] == id){
					if(itemQuantity[pt] >= qty){
						having = true;
					}
					geta = true;
				}else{
					pt++;
				}
				//--------------------------
			}
		}
		//=================Equipment=================
		if(type == 1){
			while(pt < equipment.Length && !geta){
				if(equipment[pt] == id){
					having = true;
					geta = true;
				}else{
					pt++;
				}
				//--------------------------
			}
		}
		return having;
	}
	
	/// <summary>
	/// 找到道具所在的格子
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public int FindItemSlot(int id){
		bool geta = false;
		int pt = 0;
		while(pt < itemSlot.Length && !geta){
			if(itemSlot[pt] == id){
				geta = true;
			}
			else
			{
				pt++;
				if(pt >= itemSlot.Length)
				{
					pt = itemSlot.Length + 50;//No Item
					print("No Item");
				}
			}
		}
		return pt;
	}
	
	/// <summary>
	/// 找到装备所在的格子
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public int FindEquipmentSlot(int id){
		bool geta = false;
		int pt = 0;
		while(pt < equipment.Length && !geta){
			if(equipment[pt] == id){
				geta = true;
			}else{
				pt++;
				if(pt >= equipment.Length){
					pt = equipment.Length + 50;//No Item
					print("No Item");
				}
			}
		}
		return pt;
	}
	
	/// <summary>
	/// 移除道具
	/// </summary>
	/// <param name="id">道具id</param>
	/// <param name="amount">移除的数量</param>
	/// <returns></returns>
	public bool RemoveItem(int id , int amount){
		bool haveItem = false;
		int slot = FindItemSlot(id);
		if(slot < itemSlot.Length){
			if(itemQuantity[slot] > amount){
				itemQuantity[slot] -= amount;
				haveItem = true;
			}
			if(itemQuantity[slot] <= 0){
				itemSlot[slot] = 0;
				itemQuantity[slot] = 0;
				AutoSortItem();
			}
		}
		UpdateAmmoUI();
		return haveItem;
	}

	
	/// <summary>
	/// 更新武器栏UI
	/// </summary>
	public void UpdateAmmoUI(){
		//Update Show Ammo UI
		if(!attackTrigger){
			return;
		}
		int reqId = attackTrigger.requireItemId;

		if(reqId > 0 && ShowAmmoC.showAmmo){
			ShowAmmoC.showAmmo.OnOffShowing(true);
			int sl = FindItemSlot(reqId);
			int am = 0;
			//Sprite spr = dataItem.usableItem[reqId].iconSprite;
			if(sl < itemQuantity.Length){
				am = itemQuantity[sl];
			}			
			//ShowAmmoC.showAmmo.UpdateSprite(spr);
			ShowAmmoC.showAmmo.UpdateAmmo(am);
		}else if(ShowAmmoC.showAmmo){
			ShowAmmoC.showAmmo.OnOffShowing(false);
		}
	}
	
	
	/// <summary>
	/// 删除装备，只能找装备背包里的
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public bool RemoveEquipment(int id ){
		bool haveItem = false;
		int slot = FindEquipmentSlot(id);
		if(slot < equipment.Length){
			equipment[slot] = 0;
			AutoSortEquipment();
			haveItem = true;
		}
		return haveItem;
	}

	
}
