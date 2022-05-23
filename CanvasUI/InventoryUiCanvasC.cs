using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryUiCanvasC : MonoBehaviour 
{
	public GameObject player;
	
	public Text moneyText;
	
	public Image[] itemIcons;
	public Text[] itemQty;
	public Image[] equipmentIcons;
	
	public Image weaponIcons;
	public Image subWeaponIcons;
	public Image armorIcons;
	public Image accIcons;
	public Image helmIcons;
	public Image glovesIcons;
	public Image bootsIcons;
	
	public GameObject tooltip;
	public Image tooltipIcon;
	public Text tooltipName;
	public Text tooltipText1;
	public Text tooltipText2;
	public Text tooltipText3;
	
	public GameObject usableTab;
	public GameObject equipmentTab;
	
	public GameObject database;
	private ItemDataC itemData;
	private Inventory _inventory;

	private Button[] PlayerItems = new Button[16];
	private Button[] Equipments = new Button[16];
	private Button[] Equiped = new Button[16];


	private void Awake()
	{
		itemIcons = new Image[16];
		itemQty = new Text[16];
		equipmentIcons = new Image[10];
	
		player = transform.parent.gameObject;
		itemData = database.GetComponent<ItemDataC>();
		_inventory = player.GetComponent<Inventory>();

		for (int i = 0; i < usableTab.transform.childCount; i++)
		{
			Transform itemSlot = usableTab.transform.GetChild(i);

			int tep = i;
			itemSlot.GetComponent<Button>().onClick.AddListener(() => UseItem(tep));

			itemIcons[i] = itemSlot.GetChild(0).GetComponent<Image>();
			itemQty[i] = itemSlot.GetChild(1).GetComponent<Text>();
			
		}

		for (int i = 0; i < equipmentTab.transform.childCount; i++)
		{
			
			Transform equipSlot = equipmentTab.transform.GetChild(i);
			
			equipmentIcons[i] = equipSlot.GetChild(0).GetComponent<Image>();
		}
	}

	void Start()
	{
		
	}
	
	void Update(){
		//设置道具提示的坐标
		SetToolTipPos();
		
		if(!player)
			return;
		
		//itemIcons[0].GetComponent<Image>().sprite = itemData.usableItem[player.GetComponent<Inventory>().itemSlot[0]].iconSprite;
		SetIcon();

	}
	
	
	/// <summary>
	/// 设置道具提示的坐标
	/// </summary>
	void SetToolTipPos()
	{
		// if(tooltip && tooltip.activeSelf){
		// 	Vector2 tooltipPos = Input.mousePosition;
		// 	tooltipPos.x += 7;
		// 	tooltip.transform.position = tooltipPos;
		// }
	}

	void SetIcon()
	{
		if (!_inventory.menu)
			return;
		
		for(int a = 0; a < itemIcons.Length; a++)
		{
			itemIcons[a].sprite = itemData.usableItem[_inventory.itemSlot[a]].iconSprite;
			itemIcons[a].color = itemData.usableItem[_inventory.itemSlot[a]].spriteColor;
		}
		
		for(int q = 0; q < itemQty.Length; q++){
			string qty = _inventory.itemQuantity[q].ToString();
			if(qty == "0"){
				qty = "";
			}
			itemQty[q].text = qty;
		}
		
		for(int b = 0; b < equipmentIcons.Length; b++){
			equipmentIcons[b].sprite = itemData.equipment[_inventory.equipment[b]].iconSprite;
			equipmentIcons[b].color = itemData.equipment[_inventory.equipment[b]].spriteColor;
		}
		
		if(weaponIcons){
			weaponIcons.sprite = itemData.equipment[_inventory.weaponEquip].iconSprite;
			weaponIcons.color = itemData.equipment[_inventory.weaponEquip].spriteColor;
		}
		if(subWeaponIcons){
			subWeaponIcons.sprite = itemData.equipment[_inventory.subWeaponEquip].iconSprite;
			subWeaponIcons.color = itemData.equipment[_inventory.subWeaponEquip].spriteColor;
		}
		if(armorIcons){
			armorIcons.sprite = itemData.equipment[_inventory.armorEquip].iconSprite;
			armorIcons.color = itemData.equipment[_inventory.armorEquip].spriteColor;
		}
		if(accIcons){
			accIcons.sprite = itemData.equipment[_inventory.accessoryEquip].iconSprite;
			accIcons.color = itemData.equipment[_inventory.accessoryEquip].spriteColor;
		}
		if(helmIcons){
			helmIcons.sprite = itemData.equipment[_inventory.hatEquip].iconSprite;
			helmIcons.color = itemData.equipment[_inventory.hatEquip].spriteColor;
		}
		if(glovesIcons){
			glovesIcons.sprite = itemData.equipment[_inventory.glovesEquip].iconSprite;
			glovesIcons.color = itemData.equipment[_inventory.glovesEquip].spriteColor;
		}
		if(bootsIcons){
			bootsIcons.sprite = itemData.equipment[_inventory.bootsEquip].iconSprite;
			bootsIcons.color = itemData.equipment[_inventory.bootsEquip].spriteColor;
		}
		if(moneyText){
			moneyText.text = _inventory.cash.ToString();
		}
	}
	
	/// <summary>
	/// 显示道具的信息
	/// </summary>
	/// <param name="slot"></param>
	public void ShowItemTooltip(int slot){
		if(!tooltip || !player){
			return;
		}
		
		if(_inventory.itemSlot[slot] <= 0){
			HideTooltip();
			return;
		}
		
		tooltipIcon.sprite = itemData.usableItem[_inventory.itemSlot[slot]].iconSprite;
		tooltipName.text = itemData.usableItem[_inventory.itemSlot[slot]].itemName;
		
		tooltipText1.text = itemData.usableItem[_inventory.itemSlot[slot]].description;
		tooltipText2.text = itemData.usableItem[_inventory.itemSlot[slot]].description2;
		tooltipText3.text = itemData.usableItem[_inventory.itemSlot[slot]].description3;
		
		tooltip.SetActive(true);
	}
	
	
	/// <summary>
	/// 显示装备的信息
	/// </summary>
	/// <param name="slot"></param>
	public void ShowEquipmentTooltip(int slot){
		if(!tooltip || !player){
			return;
		}
		if(_inventory.equipment[slot] <= 0){
			HideTooltip();
			return;
		}
		
		tooltipIcon.GetComponent<Image>().sprite = itemData.equipment[_inventory.equipment[slot]].iconSprite;
		tooltipName.GetComponent<Text>().text = itemData.equipment[_inventory.equipment[slot]].itemName;
		
		tooltipText1.GetComponent<Text>().text = itemData.equipment[_inventory.equipment[slot]].description;
		tooltipText2.GetComponent<Text>().text = itemData.equipment[_inventory.equipment[slot]].description2;
		tooltipText3.GetComponent<Text>().text = itemData.equipment[_inventory.equipment[slot]].description3;
		
		tooltip.SetActive(true);
	}
	
	
	/// <summary>
	/// 显示装备栏的道具提示
	/// </summary>
	/// <param name="type"></param>
	public void ShowOnEquipTooltip(int type){
		if(!tooltip || !player){
			return;
		}
		//0 = Weapon, 1 = Armor, 2 = Accessories , 3 = Sub Weapon
		//4 = Headgear , 5 = Gloves , 6 = Boots
		int id = 0;
		if(type == 0){
			id = _inventory.weaponEquip;
		}
		else if(type == 1){
			id = _inventory.armorEquip;
		}
		else if(type == 2){
			id = _inventory.accessoryEquip;
		}
		else if(type == 3){
			id = _inventory.subWeaponEquip;
		}
		else if(type == 4){
			id = _inventory.hatEquip;
		}
		else if(type == 5){
			id = _inventory.glovesEquip;
		}
		else if(type == 6){
			id = _inventory.bootsEquip;
		}
		
		if(id <= 0){
			HideTooltip();
			return;
		}
		
		tooltipIcon.GetComponent<Image>().sprite = itemData.equipment[id].iconSprite;
		tooltipName.GetComponent<Text>().text = itemData.equipment[id].itemName;
		
		tooltipText1.GetComponent<Text>().text = itemData.equipment[id].description;
		tooltipText2.GetComponent<Text>().text = itemData.equipment[id].description2;
		tooltipText3.GetComponent<Text>().text = itemData.equipment[id].description3;
		
		tooltip.SetActive(true);
	}
	
	/// <summary>
	/// 隐藏道具提示
	/// </summary>
	public void HideTooltip(){
		if(!tooltip){
			return;
		}
		tooltip.SetActive(false);
	}
	
	/// <summary>
	/// 使用道具
	/// </summary>
	/// <param name="itemSlot"></param>
	public void UseItem(int itemSlot){
		if(!player){
			return;
		}
		_inventory.UseItem(itemSlot);
		ShowItemTooltip(itemSlot);
		
	}
	
	/// <summary>
	/// 装备道具
	/// </summary>
	/// <param name="itemSlot"></param>
	public void EquipItem(int itemSlot){
		if(!player){
			return;
		}
		_inventory.EquipItem(_inventory.equipment[itemSlot] , itemSlot);
		ShowEquipmentTooltip(itemSlot);
	}
	
	
	/// <summary>
	/// 卸下道具
	/// </summary>
	/// <param name="type">0 = Weapon, 1 = Armor, 2 = Accessories，3 = Headgear , 4 = Gloves , 5 = Boots</param>
	public void UnEquip(int type){
		//0 = Weapon, 1 = Armor, 2 = Accessories
		//3 = Headgear , 4 = Gloves , 5 = Boots
		if(!player){
			return;
		}
		int id = 0;
		if(type == 0){
			id = _inventory.weaponEquip;
		}
		if(type == 1){
			id = _inventory.armorEquip;
		}
		if(type == 2){
			id = _inventory.accessoryEquip;
		}
		if(type == 3){
			id = _inventory.hatEquip;
		}
		if(type == 4){
			id = _inventory.glovesEquip;
		}
		if(type == 5){
			id = _inventory.bootsEquip;
		}
		_inventory.UnEquip(id);
		ShowOnEquipTooltip(type);
	}

	public void SwapWeapon(){
		if(!player){
			return;
		}
		_inventory.SwapWeapon();
		ShowOnEquipTooltip(3);
	}

	public void CloseMenu(){
		Time.timeScale = 1.0f;
		//Screen.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		gameObject.SetActive(false);
	}
	
	public void OpenUsableTab(){
		usableTab.SetActive(true);
		equipmentTab.SetActive(false);
	}
	
	public void OpenEquipmentTab(){
		usableTab.SetActive(false);
		equipmentTab.SetActive(true);
	}
	
}