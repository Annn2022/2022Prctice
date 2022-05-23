using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof (PlayerData))]
//[RequireComponent(typeof (UiMasterC))]
[RequireComponent(typeof (Inventory))]
//[RequireComponent(typeof (QuestStatC))]
//[RequireComponent(typeof (SkillWindowC))]
[RequireComponent(typeof (DontDestroyOnloadC))]
//[RequireComponent(typeof (SaveLoadC))]
//[RequireComponent(typeof (HpMpRegenC))]

[AddComponentMenu("Action-RPG Kit(C#)/Create Player(No Controller)")]

public class AttackTriggerC : MonoBehaviour {
	//private bool  masterNetwork = false;
	public GameObject mainModel;
	public Transform attackPoint;
	public Transform attackPrefab;
	public bool notActive = false;
	public bool useMecanim = false;

	[System.Serializable]
	public class AtkMecanim{
		//Enable this if you want to use Set Trigger for Attack Animation instead of Play from Animation's name
		//Script will send triggerName and combo(int) to Animator Controller
		public bool useSetTriggerAtk = false;
		public string triggerName = "attack";
	}
	public AtkMecanim mecanimSetting;

	public whileAtk whileAttack = whileAtk.MeleeFwd;

	public AimType aimingType = AimType.Raycast;

	private bool atkDelay = false;
	public bool freeze = false;

	public float attackSpeed = 0.15f;
	private float nextFire = 0.0f;
	public float atkDelay1 = 0.1f;

	public AnimationClip[] attackCombo = new AnimationClip[3];
	public AnimationClip blockingAnimation;
	public float attackAnimationSpeed = 1.0f;
	public AudioClip attackSoundEffect;
	public SkillSetting[] skill = new SkillSetting[8];

	private AnimationClip hurt;

	private bool meleefwd = false;
	[HideInInspector]
	public bool isCasting = false;

	private int c = 0;

	public static Transform Maincam;
	public GameObject MaincamPrefab;

	private int str = 0;
	private int matk = 0;

	public Texture2D aimIcon;
	public int aimIconSize = 40;

	[HideInInspector]
	public bool flinch = false;
	private Vector3 knock = Vector3.zero;

	//----------Sounds-------------
	[System.Serializable]
	public class AtkSound {
		public AudioClip[] attackComboVoice = new AudioClip[3];
		public AudioClip magicCastVoice;
		public AudioClip hurtVoice;
	}
	public AtkSound sound;

	[HideInInspector]
	public GameObject pet;
	private GameObject castEff;

	//Icon for Buffs
	public Texture2D braveIcon;
	public Texture2D barrierIcon;
	public Texture2D faithIcon;
	public Texture2D magicBarrierIcon;
	public bool drawGUI = false;
	public bool mobileMode = false;

	[HideInInspector]
	public int[] skillCoolDown = new int[9];
	private float[] wait = new float[9];

	[HideInInspector]
	public bool skillAim = false;
	public Transform skillAimProjector;
	private Transform skillAimProj;
	private float dist;
	private Vector3 skillSpawnPos;
	public float skillRange = 25.0f;
	private int skSelect = 0;

	public bool canBlock = true;
	public int blockStamina = 30;
	public bool raycastAiming = true;

	[HideInInspector]
	public GameObject actvateObj;
	[HideInInspector]
	public string actvateMsg = "";
	public Texture2D button;
	[HideInInspector]
	public string buttonText = "";
	[HideInInspector]
	public bool showButton = false;
	public GUIStyle buttonTextStyle;
	//[HideInInspector]
	public int weaponType = 0;
	//[HideInInspector]
	public int requireItemId = 0;
	[HideInInspector]
	public string requireItemName = "";
	private PlayerData playerData;
	private CharacterController controller;
	[HideInInspector]
	public bool onAttacking = false; // For Mecanim disable jumping animation

	[System.Serializable]
	public class CanvasObj{
		public bool useCanvas = false;
		public GameObject activatorButton;
		public Text activatorText;
		public GameObject aimIcon;
	}
	public CanvasObj canvasElement;

	void Awake(){
		if(!mainModel){
			mainModel = gameObject;
		}
		playerData = GetComponent<PlayerData>();
		GlobalCondition.mainPlayer = gameObject;
		//Assign This mainModel to Status Script
		playerData.mainModel = mainModel;
		if(mainModel.GetComponent<Animator>()){
			useMecanim = true;
		}
		playerData.useMecanim = useMecanim;
		//Set tag to Player.
		gameObject.tag = "Player";

		skillCoolDown = new int[skill.Length];
		wait = new float[skill.Length];
		
		TryGetComponent(out controller);

		str = playerData.addAtk;
		
		//--------------------------------
		//设置一个攻击点
		if(!attackPoint){
			GameObject newAtkPoint = new GameObject();
			newAtkPoint.transform.position = transform.position;
			newAtkPoint.transform.rotation = transform.rotation;
			newAtkPoint.transform.parent = this.transform;
			attackPoint = newAtkPoint.transform;	
		}

		if(skillAimProjector){
			skillAimProj = Instantiate(skillAimProjector , transform.position , skillAimProjector.rotation) as Transform;
			skillAimProj.parent = this.transform;
			skillAimProj.gameObject.SetActive(false);
		}
		if(!GetComponent<AudioSource>()){
			gameObject.AddComponent<AudioSource>();
			GetComponent<AudioSource>().playOnAwake = false;
		}

		//小地图
		//GameObject minimap = GameObject.FindWithTag("Minimap");
		// if(minimap){
		// 	GameObject mapcam = minimap.GetComponent<MinimapOnOffC>().minimapCam;
		// 	mapcam.GetComponent<MinimapCameraC>().target = this.transform;
		// }
	}

	void Update(){
		if(Input.GetKeyDown("e")){
			Activator();
		}
		//技能冷却
		// for(int s = 0; s < skill.Length; s++){
		// 	if(skillCoolDown[s] > 0){
		// 		if(wait[s] >= 1){
		// 			skillCoolDown[s]--;
		// 			wait[s] = 0;
		// 		}
		// 		else
		// 			wait[s] += Time.deltaTime;
		// 	}
		// }
		
		if(canvasElement.useCanvas && showButton){
			if(!canvasElement.activatorButton.activeSelf){
				canvasElement.activatorButton.SetActive(true);
			}
			if(GlobalCondition.freezeAll || Time.timeScale == 0 || !actvateObj || GlobalCondition.interacting){
				if(canvasElement.activatorButton.activeSelf){
					canvasElement.activatorButton.SetActive(false);
				}
			}
		}
		if(canvasElement.useCanvas && !showButton && canvasElement.activatorButton && canvasElement.activatorButton.activeSelf){
			canvasElement.activatorButton.SetActive(false);
		}

		if(freeze || atkDelay || Time.timeScale == 0.0f || playerData.freeze || GlobalCondition.freezeAll || GlobalCondition.freezePlayer || playerData.block){
			return;
		}
		
		//玩家的两种状态的速度变化
		// if(controller){
		// 	if(flinch){
		// 		controller.Move(knock * 6 * Time.deltaTime);
		// 		return;
		// 	}
		// 	if(meleefwd){
		// 		Vector3 dir = transform.TransformDirection(Vector3.forward);
		// 		controller.Move(dir * 5 * Time.deltaTime);
		// 	}
		// }
		//if(Maincam && Maincam.GetComponent<ARPGcameraC>()){
		
		//设置瞄准的攻击点
		if(aimingType == AimType.Raycast){
			Aiming();
		}
		//}
		//----------------------------
		if(notActive){
			return;
		}
		//普通攻击的触发
		if(Input.GetButton("Fire1") && Time.time > nextFire && !isCasting && !mobileMode && !skillAim){
			if(Time.time > (nextFire + 0.5f)){
				c = 0;
			}
			//Attack Combo
			if(attackCombo.Length >= 1){
				StartCoroutine(AttackCombo());
			}
		}

		//Blocking Damage
		// if(GetComponent<PlayerInputControllerC>() && canBlock && Input.GetButton("Fire2") && Time.time > nextFire && !isCasting && !skillAim && !GetComponent<PlayerData>().block && !mobileMode){
		// 	if(GetComponent<PlayerInputControllerC>().stamina > blockStamina && !GetComponent<PlayerInputControllerC>().dodging){
		// 		GetComponent<PlayerInputControllerC>().stamina -= blockStamina;
		// 		string anim = "";
		// 		if(blockingAnimation){
		// 			anim = blockingAnimation.name;
		// 		}
		// 		GetComponent<playerData>().StartCoroutine("GuardUp" , anim);
		// 		//EnterCombat();
		// 		GetComponent<PlayerInputControllerC>().recover = true;
		// 		GetComponent<PlayerInputControllerC>().recoverStamina = 0.0f;
		// 	}
		// }

		// if(skillAim){
		// 	//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		// 	if(raycastAiming){
		// 		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0.0f));
		// 		RaycastHit hit;
		// 		if(Physics.Raycast(ray, out hit)) {
		// 			skillSpawnPos = hit.point;
		//
		// 			if(dist > skillRange){
		// 				skillAimProj.GetComponent<Projector>().material.color = Color.red;
		// 			}
		// 			else
		// 			{
		// 				skillAimProj.GetComponent<Projector>().material.color = Color.green;
		// 			}
		//
		// 			skillAimProj.position = new Vector3(skillSpawnPos.x , skillSpawnPos.y + 10.5f , skillSpawnPos.z);
		// 			dist = Vector3.Distance(hit.point, transform.position);
		// 		}
		// 	}
		// 	else
		// 	{
		// 		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// 		RaycastHit hit;
		// 		if(Physics.Raycast(ray, out hit)) {
		// 			skillSpawnPos = hit.point;
		//
		// 			if(dist > skillRange){
		// 				skillAimProj.GetComponent<Projector>().material.color = Color.red;
		// 			}else{
		// 				skillAimProj.GetComponent<Projector>().material.color = Color.green;
		// 			}
		//
		// 			skillAimProj.position = new Vector3(skillSpawnPos.x , skillSpawnPos.y + 10.5f , skillSpawnPos.z);
		// 			dist = Vector3.Distance(hit.point, transform.position);
		// 		}
		// 	}
		//
		// 	if(Input.GetButton("Fire1") && Time.time > nextFire && !isCasting) {
		// 		if(dist <= skillRange){
		// 			StartCoroutine(MagicSkill(skSelect));
		// 		}else{
		// 			skillAim = false;
		// 			skillAimProj.gameObject.SetActive(false);
		// 		}
		// 	}
		//
		// 	if(Input.GetButton("Fire2") || Input.GetKeyDown("e")){
		// 		skillAim = false;
		// 		skillAimProj.gameObject.SetActive(false);
		// 	}
		// }

		// if(skill.Length >= 1 && Input.GetKeyDown("1") && !isCasting && skill[0].skillPrefab){
		// 	skSelect = 0;
		// 	TriggerSkill(skSelect);
		// }
		// if(skill.Length >= 2 && Input.GetKeyDown("2") && !isCasting && skill[1].skillPrefab){
		// 	skSelect = 1;
		// 	TriggerSkill(skSelect);
		// }
		// if(skill.Length >= 3 && Input.GetKeyDown("3") && !isCasting && skill[2].skillPrefab){
		// 	skSelect = 2;
		// 	TriggerSkill(skSelect);
		// }
		// //-----------------------
		// if(skill.Length >= 4 && Input.GetKeyDown("4") && !isCasting && skill[3].skillPrefab){
		// 	skSelect = 3;
		// 	TriggerSkill(skSelect);
		// }
		// if(skill.Length >= 5 && Input.GetKeyDown("5") && !isCasting && skill[4].skillPrefab){
		// 	skSelect = 4;
		// 	TriggerSkill(skSelect);
		// }
		// if(skill.Length >= 6 && Input.GetKeyDown("6") && !isCasting && skill[5].skillPrefab){
		// 	skSelect = 5;
		// 	TriggerSkill(skSelect);
		// }
		// if(skill.Length >= 7 && Input.GetKeyDown("7") && !isCasting && skill[6].skillPrefab){
		// 	skSelect = 6;
		// 	TriggerSkill(skSelect);
		// }
		// if(skill.Length >= 8 && Input.GetKeyDown("8") && !isCasting && skill[7].skillPrefab){
		// 	skSelect = 7;
		// 	TriggerSkill(skSelect);
		// }
		// //Swap Weapon
		// if(Input.GetKeyDown("r") && !isCasting && GetComponent<Inventory>()){
		// 	GetComponent<Inventory>().SwapWeapon();
		// }
	}

	void OnGUI(){
		if(canvasElement.useCanvas){
			return;
		}
		/*if(aimingType == AimType.Normal){
			GUI.DrawTexture(new Rect(Screen.width/2 - 16,Screen.height/2 - 90,aimIconSize,aimIconSize), aimIcon);
		}*/
		if(aimingType == AimType.Raycast){
			GUI.DrawTexture(new Rect(Screen.width/2 - 20,Screen.height/2 - 20,40,40), aimIcon);
		}

		if(drawGUI){
			GUI.Box(new Rect(Screen.width - 630, Screen.height - 105,60,60), skill[0].icon);
			if(skillCoolDown[0] > 0){
				GUI.Box(new Rect(Screen.width - 630, Screen.height - 105,60,60), skillCoolDown[0].ToString());
			}
			//----------------------
			GUI.Box(new Rect(Screen.width - 570, Screen.height - 105,60,60), skill[1].icon);
			if(skillCoolDown[1] > 0){
				GUI.Box(new Rect(Screen.width - 570, Screen.height - 105,60,60), skillCoolDown[1].ToString());
			}
			//----------------------
			GUI.Box(new Rect(Screen.width - 510, Screen.height - 105,60,60), skill[2].icon);
			if(skillCoolDown[2] > 0){
				GUI.Box(new Rect(Screen.width - 510, Screen.height - 105,60,60), skillCoolDown[2].ToString());
			}
			//----------------------
			if(skill.Length >= 4){
				GUI.Box(new Rect(Screen.width - 450, Screen.height - 105,60,60), skill[3].icon);
				if(skillCoolDown[3] > 0){
					GUI.Box(new Rect(Screen.width - 450, Screen.height - 105,60,60), skillCoolDown[3].ToString());
				}
			}
			//----------------------
			if(skill.Length >= 5){
				GUI.Box(new Rect(Screen.width - 390, Screen.height - 105,60,60), skill[4].icon);
				if(skillCoolDown[4] > 0){
					GUI.Box(new Rect(Screen.width - 390, Screen.height - 105,60,60), skillCoolDown[4].ToString());
				}
			}
			//----------------------
			if(skill.Length >= 6){
				GUI.Box(new Rect(Screen.width - 330, Screen.height - 105,60,60), skill[5].icon);
				if(skillCoolDown[5] > 0){
					GUI.Box(new Rect(Screen.width - 330, Screen.height - 105,60,60), skillCoolDown[5].ToString());
				}
			}
			//----------------------
			if(skill.Length >= 7){
				GUI.Box(new Rect(Screen.width - 270, Screen.height - 105,60,60), skill[6].icon);
				if(skillCoolDown[6] > 0){
					GUI.Box(new Rect(Screen.width - 270, Screen.height - 105,60,60), skillCoolDown[6].ToString());
				}
			}
			//----------------------
			if(skill.Length >= 8){
				GUI.Box(new Rect(Screen.width - 210, Screen.height - 105,60,60), skill[7].icon);
				if(skillCoolDown[7] > 0){
					GUI.Box(new Rect(Screen.width - 210, Screen.height - 105,60,60), skillCoolDown[7].ToString());
				}
			}
		}

		if(showButton && !GlobalCondition.freezeAll && Time.timeScale != 0 && actvateObj){
			GUI.depth = -100;
			if(button)
				GUI.DrawTexture(new Rect(Screen.width / 2 - 130, Screen.height - 180, 260, 80), button);

			GUI.Label(new Rect(Screen.width / 2 - 140, Screen.height - 180, 260, 80), "[E] " + buttonText , buttonTextStyle);
		}
		
		//Show Buffs Icon
		if(playerData.brave){
			GUI.DrawTexture(new Rect(30,200,60,60), braveIcon);
		}
		if(playerData.barrier){
			GUI.DrawTexture(new Rect(30,260,60,60), barrierIcon);
		}
		if(playerData.faith){
			GUI.DrawTexture(new Rect(30,320,60,60), faithIcon);
		}
		if(playerData.mbarrier){
			GUI.DrawTexture(new Rect(30,380,60,60), magicBarrierIcon);
		}
	}

	public void TriggerAttack(){
		if(freeze || atkDelay || Time.timeScale == 0.0f || GetComponent<PlayerData>().freeze){
			return;
		}
		if(Time.time > nextFire && !isCasting && !skillAim){
			if(Time.time > (nextFire + 0.5f)){
				c = 0;
			}
			//Attack Combo
			if(attackCombo.Length >= 1){
				StartCoroutine(AttackCombo());
			}
		}
	}
	
	//触发技能
	// public void TriggerSkill(int sk){
	// 	if(freeze || atkDelay || Time.timeScale == 0.0f || GetComponent<PlayerData>().freeze || isCasting || !skill[sk].skillPrefab){
	// 		return;
	// 	}
	// 	if(skill[sk].skillSpawn == BSpawnType.FromPlayer){
	// 		StartCoroutine(MagicSkill(sk));
	// 	}
	// 	if(skill[sk].skillSpawn == BSpawnType.AtMouse && skillAimProj){
	// 		if(skillAim && skSelect == sk){
	// 			skillAim = false;
	// 			skillAimProj.gameObject.SetActive(false);
	// 		}else{
	// 			//skCombo = 0;
	// 			skillAim = true;
	// 			skillAimProj.gameObject.SetActive(true);
	// 		}
	// 	}
	// }

	IEnumerator AttackCombo(){
		if(c >= attackCombo.Length){
			c = 0;
		}
		if(playerData.dodge){
			//yield return new WaitForSeconds(0.1f);
			yield break;
		}
		
		//判断是否需要消耗道具
		if(requireItemId > 0){
			bool have = GetComponent<Inventory>().RemoveItem(requireItemId , 1);
			if(!have){
				print("Require " + requireItemName);
				//yield return new WaitForSeconds(0.1f);
				yield break;
			}
		}
		
		float wait = 0.0f;
		if(attackCombo[c] || useMecanim){
			str = playerData.addAtk;
			//Transform bulletShootout;
			isCasting = true;
			onAttacking = true;
			// If 混乱的冲刺
			if(whileAttack == whileAtk.MeleeFwd){
				playerData.canControl = false;
				//MeleeDash();
				StartCoroutine(MeleeDash());
			}
			// 被禁锢了，无法操作
			if(whileAttack == whileAtk.Immobile){
				GetComponent<PlayerData>().canControl = false;
			}
			
			
			//播放攻击音效
			if(sound.attackComboVoice.Length > c && sound.attackComboVoice[c]){
				GetComponent<AudioSource>().PlayOneShot(sound.attackComboVoice[c]);
			}
			if(attackSoundEffect){
				GetComponent<AudioSource>().PlayOneShot(attackSoundEffect);
			}
			

			yield return new WaitForSeconds(atkDelay1);
			c++;

			nextFire = Time.time + attackSpeed;
			// bulletShootout = Instantiate(attackPrefab, attackPoint.transform.position , attackPoint.transform.rotation) as Transform;
			// bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
			// if(playerData.hiddenStatus.drainTouch > 0){
			// 	bulletShootout.GetComponent<BulletStatusC>().drainHp += playerData.hiddenStatus.drainTouch;
			// }
			
			if(c >= attackCombo.Length){
				c = 0;
				atkDelay = true;
				yield return new WaitForSeconds(wait);
				atkDelay = false;
			}
			else
			{
				yield return new WaitForSeconds(attackSpeed);
			}

			isCasting = false;
			onAttacking = false;
			playerData.canControl = true;
		}
		else
			print("Please assign attack animation in Attack Combo");
		
	}

	IEnumerator MeleeDash(){
		meleefwd = true;
		yield return new WaitForSeconds(0.2f);
		meleefwd = false;
	}

	//施放魔法的协程
	// IEnumerator MagicSkill(int skillID){
	// 	skillAim = false;
	// 	if(skillAimProj){
	// 		skillAimProj.gameObject.SetActive(false);
	// 	}
	// 	if(skill[skillID].requireWeapon && weaponType != skill[skillID].requireWeaponType){
	// 		//Check Weapon Type for Use Skill
	// 		print("Cannot Use Skill with this Weapon");
	// 		yield break;
	// 	}
	// 	if(skillCoolDown[skillID] > 0){
	// 		yield break;
	// 	}
	// 	c = 0;
	// 	int cost = skill[skillID].manaCost;
	// 	if(GetComponent<PlayerData>().hiddenStatus.mpReduce > 0){
	// 		//Calculate MP Reduce
	// 		int per = 100 - GetComponent<PlayerData>().hiddenStatus.mpReduce;
	// 		if(per < 0){
	// 			per = 0;
	// 		}
	// 		cost *= per;
	// 		cost /= 100;
	// 	}
	// 	if(GetComponent<PlayerData>().mana >= cost){
	// 		if(skill[skillID].sendMsg != ""){
	// 			SendMessage(skill[skillID].sendMsg , SendMessageOptions.DontRequireReceiver);
	// 		}
	// 		GetComponent<PlayerData>().mana -= cost;
	//
	// 		if(skill[skillID].skillAnimation){
	// 			str = GetComponent<PlayerData>().addAtk;
	// 			matk = GetComponent<PlayerData>().addMatk;
	//
	// 			if(!GetComponent<PlayerData>().silence){
	// 				if(sound.magicCastVoice){
	// 					GetComponent<AudioSource>().clip = sound.magicCastVoice;
	// 					GetComponent<AudioSource>().Play();
	// 				}
	// 				isCasting = true;
	// 				// If Melee Dash
	// 				if(skill[skillID].whileAttack == whileAtk.MeleeFwd){
	// 					GetComponent<PlayerData>().canControl = false;
	// 					//MeleeDash();
	// 					meleefwd = true;
	// 				}
	// 				// If Immobile
	// 				if(skill[skillID].whileAttack == whileAtk.Immobile){
	// 					GetComponent<PlayerData>().canControl = false;
	// 				}
	// 				if(!useMecanim){
	// 					//For Legacy Animation
	// 					mainModel.GetComponent<Animation>()[skill[skillID].skillAnimation.name].layer = 15;
	// 					mainModel.GetComponent<Animation>().PlayQueued(skill[skillID].skillAnimation.name , QueueMode.PlayNow);
	// 				}else{
	// 					//For Mecanim Animation
	// 					if(mainModel.GetComponent<Animator>() && skill[skillID].mecanimTriggerName != ""){
	// 						mainModel.GetComponent<Animator>().SetTrigger(skill[skillID].mecanimTriggerName);
	// 					}else if(GetComponent<PlayerMecanimAnimationC>()){
	// 						GetComponent<PlayerMecanimAnimationC>().AttackAnimation(skill[skillID].skillAnimation.name);
	// 					}
	// 				}
	// 				if(skill[skillID].castEffect){
	// 					castEff = Instantiate(skill[skillID].castEffect , transform.position , transform.rotation) as GameObject;
	// 					castEff.transform.parent = this.transform;
	// 				}
	// 				nextFire = Time.time + skill[skillID].skillDelay;
	// 				/*if(Camera.main.GetComponent<ARPGcameraC>())
	// 					Maincam.GetComponent<ARPGcameraC>().lockOn = true;*/
	// 				//Transform bulletShootout;
	//
	// 				yield return new WaitForSeconds(skill[skillID].castTime);
	// 				/*if(Camera.main.GetComponent<ARPGcameraC>()){
	// 					Maincam.GetComponent<ARPGcameraC>().lockOn = false;
	// 				}*/
	// 				if(castEff){
	// 					Destroy(castEff);
	// 				}
	// 				//onAttacking = true;
	// 				if(skill[skillID].skillSpawn == BSpawnType.FromPlayer){
	// 					Transform bulletShootout = Instantiate(skill[skillID].skillPrefab, attackPoint.transform.position , attackPoint.transform.rotation) as Transform;
	// 					bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
	// 				}else{
	// 					Transform bulletShootout = Instantiate(skill[skillID].skillPrefab, skillSpawnPos , transform.rotation) as Transform;
	// 					bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
	// 				}
	// 				if(skill[skillID].soundEffect){
	// 					GetComponent<AudioSource>().PlayOneShot(skill[skillID].soundEffect);
	// 				}
	// 				yield return new WaitForSeconds(skill[skillID].skillDelay);
	//
	// 				//Addition Hit
	// 				for(int m = 0; m < skill[skillID].multipleHit.Length; m++){
	// 					if(skill[skillID].multipleHit[m].skillAnimation){
	// 						if(!useMecanim){
	// 							//For Legacy Animation
	// 							mainModel.GetComponent<Animation>()[skill[skillID].multipleHit[m].skillAnimation.name].layer = 15;
	// 							mainModel.GetComponent<Animation>().PlayQueued(skill[skillID].multipleHit[m].skillAnimation.name , QueueMode.PlayNow);
	// 						}else{
	// 							//For Mecanim Animation
	// 							GetComponent<PlayerMecanimAnimationC>().AttackAnimation(skill[skillID].multipleHit[m].skillAnimation.name);
	// 						}
	// 					}
	// 					yield return new WaitForSeconds(skill[skillID].multipleHit[m].castTime);
	//
	// 					if(skill[skillID].skillSpawn == BSpawnType.FromPlayer){
	// 						Transform bulletShootout = Instantiate(skill[skillID].multipleHit[m].skillPrefab, attackPoint.transform.position , attackPoint.transform.rotation) as Transform;
	// 						bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
	// 					}else{
	// 						Transform bulletShootout = Instantiate(skill[skillID].multipleHit[m].skillPrefab, skillSpawnPos , transform.rotation) as Transform;
	// 						bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
	// 					}
	// 					if(skill[skillID].multipleHit[m].soundEffect){
	// 						GetComponent<AudioSource>().PlayOneShot(skill[skillID].multipleHit[m].soundEffect);
	// 					}
	// 					yield return new WaitForSeconds(skill[skillID].multipleHit[m].skillDelay);
	// 				}
	//
	// 				skillCoolDown[skillID] = skill[skillID].coolDown;
	// 				isCasting = false;
	// 				//onAttacking = false;
	// 				meleefwd = false;
	// 				GetComponent<PlayerData>().canControl = true;
	// 			}
	// 		}else{
	// 			print("Please assign skill animation in Skill Animation");
	// 		}
	// 	}
	// }

	
	/// <summary>
	/// 玩家受击
	/// </summary>
	/// <param name="dir"></param>
	public void Flinch(Vector3 dir){
		if(sound.hurtVoice && playerData.health >= 1){
			GetComponent<AudioSource>().PlayOneShot(sound.hurtVoice);
		}
		if(GlobalCondition.freezePlayer){
			return;
		}
		knock = dir;
		if(isCasting && !playerData.canControl){
			return;
		}
		playerData.canControl = false;
		//KnockBack();
		StartCoroutine(KnockBack());
		if(!useMecanim && hurt){
			//For Legacy Animation
			mainModel.GetComponent<Animation>().PlayQueued(hurt.name, QueueMode.PlayNow);
		}else if(mainModel.GetComponent<Animator>()){
			mainModel.GetComponent<Animator>().SetTrigger("hurt");
		}
		playerData.canControl = true;
	}

	IEnumerator KnockBack (){
		flinch = true;
		yield return new WaitForSeconds(0.2f);
		flinch = false;
	}

	/// <summary>
	/// 攻击时的状态设定
	/// </summary>
	/// <param name="watk">1无敌，2自由移动</param>
	public void WhileAttackSet(int watk){
		if(watk == 2)
			whileAttack = whileAtk.WalkFree;
		else if (watk == 1)
			whileAttack = whileAtk.Immobile;
		else
			whileAttack = whileAtk.MeleeFwd;
	}

	
	/// <summary>
	/// 设定远程攻击的攻击方向
	/// </summary>
	void Aiming(){
		if (Camera.main == null)
			return;
		
		Ray ray = Camera.main.ViewportPointToRay (new Vector3(0.5f,0.5f,0.0f));
			// Do a raycast
		RaycastHit hit;
		//射线检测不是玩家也不是自己
		if (Physics.Raycast(ray, out hit) && !hit.transform.CompareTag("Player") && hit.transform != transform)
		{
			attackPoint.LookAt(hit.point);
			if (Vector3.Distance(hit.point, Camera.main.transform.position) <
			    Vector3.Distance(Camera.main.transform.position, transform.position))
			{
				attackPoint.LookAt(Camera.main.ViewportToWorldPoint(new Vector3(0.52f, 0.54f, 100.0f)));
			}
		}
		else
		{
			attackPoint.LookAt(Camera.main.ViewportToWorldPoint(new Vector3(0.52f, 0.54f, 100.0f)));
		}
		
	}

	public void GetActivator(GameObject obj , string msg , string btn){
		actvateObj = obj;
		actvateMsg = msg;
		buttonText = btn;
		showButton = true;
		if(canvasElement.activatorText){
			canvasElement.activatorText.text = "[E] " + btn;
		}
	}

	public void RemoveActivator(GameObject obj){
		if(obj == actvateObj){
			actvateObj = null;
			actvateMsg = "";
			buttonText = "";
			showButton = false;
			if(canvasElement.activatorText){
				canvasElement.activatorText.text = "";
			}
		}
	}

	/// <summary>
	/// 触发器发送消息
	/// </summary>
	public void Activator()
	{
		//if(!actvateObj || actvateMsg == ""){
		if(!actvateObj || actvateMsg == "" || playerData.freeze){
			return;
		}
		actvateObj.SendMessage(actvateMsg , SendMessageOptions.DontRequireReceiver);
	}

	
	// 产生远程攻击的预制体
	// public void SpawnAttackPrefab(){
	// 	if(!attackPrefab){
	// 		return;
	// 	}
	// 	str = playerData.addAtk;
	// 	matk = playerData.addMatk;
	// 	Transform bulletShootout = Instantiate(attackPrefab, attackPoint.transform.position , attackPoint.transform.rotation) as Transform;
	// 	bulletShootout.GetComponent<BulletStatusC>().Setting(str , matk , "Player" , this.gameObject);
	// 	if(playerData.hiddenStatus.drainTouch > 0){
	// 		bulletShootout.GetComponent<BulletStatusC>().drainHp += playerData.hiddenStatus.drainTouch;
	// 	}
	// }
}
public enum whileAtk{
	MeleeFwd = 0,
	Immobile = 1,
	WalkFree = 2
}

public enum AimType{
	Normal = 0,
	Raycast = 1
}
