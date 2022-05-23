using UnityEngine;
using System.Collections;

	public class CameraController : MonoBehaviour
	{

		public static CameraController instence;
	public float shakeValue = 0.3f;
	public float shakeDuration = 0.3f;
	
	public Transform target;
	
	RaycastHit hit;
	
	[HideInInspector]
	public bool onShaking = false;
	private float shakingv = 0.0f;
	public bool lockOn = false;
	
	public bool mobileMode = false;

	void Awake()
	{
		if (instence == null)
		{
			instence = this;
		}
		DontDestroyOnLoad(transform.gameObject);
		if(!target){
			target = GameObject.FindWithTag("Player").transform;
		}

		//Screen.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	void LateUpdate(){
		if(onShaking && GlobalCondition.freezeCam){
			
			shakeValue = Random.Range(-shakingv , shakingv)* 0.2f;
			transform.position += new Vector3(0,shakeValue,0);
		}
		if(!target || GlobalCondition.freezeCam){
			return;
		}
		
		if(Time.timeScale == 0.0f){
			return;
		}

		//y = ClampAngle(y, yMinLimit, yMaxLimit);

		if (onShaking)
		{
			shakeValue = Random.Range(-shakingv , shakingv)* 0.2f;
			transform.position += new Vector3(0,shakeValue,0);
		}
		
		
	}
	
	static float ClampAngle(float angle , float min , float max){
		if(angle < -360)
			angle += 360;
		if(angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
		
	}
	
	public void Shake(float val , float dur){
		if(onShaking){
			return;
		}
		shakingv = val;
		StartCoroutine(Shaking(dur));
	}
	
	public IEnumerator Shaking(float dur){
		onShaking = true;
		yield return new WaitForSeconds(dur);
		shakingv = 0;
		shakeValue = 0;
		onShaking = false;
	}
	
	public void SetNewTarget(Transform p){
		target = p;
	}
	
	void OnEnable(){
		shakingv = 0;
		shakeValue = 0;
		onShaking = false;
	}
}