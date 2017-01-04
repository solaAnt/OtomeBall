using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]

public class SolaButtonUgui : MonoBehaviour
{
//	public AudioClip defaultAudioClip;
	public delegate void VoidDelegate (GameObject gameObject);
	public VoidDelegate onClicked;
	public VoidDelegate onDown;
	public VoidDelegate onUp;


	public void setDisable (bool isDisable)
	{
		_isDisable = isDisable;
	}

	void Awake ()
	{
		#if UNITY_ANDROID
		_platform=ANDROID;
		#endif
		
		#if UNITY_IPHONE
		_platform=IOS;
		#endif
		
		#if UNITY_STANDALONE_WIN
		_platform=WIN;
		#endif        
	}

	private const float ACTIVE_DISTANCE = 10F;
	private const float SCALE_RATE = 0.8F;
	private Vector3 INIT_BEGIN_POS = new Vector3 (0f, 0f, 0f);
	private Vector3 _beginPos;
	private const int ANDROID = 0;
	private const int IOS = 1;
	private const int WIN = 2;
	private int _platform;
	private bool _isDisable = false;
	private bool _isTouched = false;

	private AudioSource _audioSource;

	void Start ()
	{
//		EventTriggerListener.Get (gameObject).onDown += _onDown;
//		EventTriggerListener.Get (gameObject).onUp += _onUp;
//		EventTriggerListener.Get (gameObject).onDrag += _onDrag;
		_beginPos = INIT_BEGIN_POS;
		onClicked += initDelegate;
		onDown += initDelegate;
		onUp += initDelegate;
		
		if (!gameObject.GetComponent<AudioSource> ())
			gameObject.AddComponent<AudioSource> ();

		_audioSource=gameObject.GetComponent<AudioSource> ();
		_audioSource.playOnAwake = false;

		AudioClip defaultAudioClip = Resources.Load("audio/btn_01") as AudioClip;
		_audioSource.clip = defaultAudioClip;
	}
	
	void Update ()
	{
		if (_isDisable == true)
			return;

		switch (_platform) {
		case ANDROID:
		case IOS:
			if (Input.touchCount <= 0)
				return;

			Touch finger = Input.GetTouch (0);
			TouchPhase phase = finger.phase;

			if (phase == TouchPhase.Began) {
				if (IPointerOverUI.Instance.IsTouchObject (finger, gameObject)) {
					_isTouched = true;
					_beginPos = finger.position;
					transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);

					_onDown();
					return;
				}
			}

			if (phase == TouchPhase.Ended && _isTouched == true) {
				_isTouched = false;

				transform.localScale = new Vector3 (1, 1, 1);
				Vector3 curPos = finger.position;

				if (_beginPos == INIT_BEGIN_POS || Vector3.Distance (_beginPos, curPos) > ACTIVE_DISTANCE){
					onUp (gameObject);
					break;
				}
					
				_beginPos = INIT_BEGIN_POS;
				_onUp();
			}
			break;
			
		case WIN:
			if (Input.GetMouseButtonDown (0)) {
				if (IPointerOverUI.Instance.IsPointerOverUIObject (gameObject)) {
					_beginPos = Input.mousePosition;
					transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
					_onDown();
				}
			}
			
			if (Input.GetMouseButtonUp (0)) {
				transform.localScale = new Vector3 (1, 1, 1);
				Vector3 curPos = Input.mousePosition;

				if (_beginPos == INIT_BEGIN_POS || Vector3.Distance (_beginPos, curPos) > ACTIVE_DISTANCE){
					onUp (gameObject);
					break;
				}

				if (IPointerOverUI.Instance.IsPointerOverUIObject (gameObject)) {
					_beginPos = INIT_BEGIN_POS;
					_onUp();
				}
					
			}
			break;
		}
	}

	bool IsPointerOverGameObject (int fingerId)
	{
		EventSystem eventSystem = EventSystem.current;
		return eventSystem.IsPointerOverGameObject (fingerId);
	}

	private void initDelegate (GameObject gameObject){}

	private void _onDown(){
		_audioSource.Play ();
		onDown (gameObject);
	}

	private void _onUp(){
		onClicked (gameObject);
		onUp (gameObject);
	}
//	private void _onDown (GameObject go, PointerEventData pointEventData, BaseEventData baseEventData)
//	{
//		Vector3 wp = pointEventData.pressEventCamera.camera.ScreenToWorldPoint (pointEventData.position);
//		_beginPos = gameObject.transform.parent.transform.InverseTransformPoint (wp);
//		gameObject.transform.localScale = new  Vector3 (SCALE_RATE, SCALE_RATE, SCALE_RATE);
//	}
//
//	private void _onUp (GameObject go, PointerEventData pointEventData, BaseEventData baseEventData)
//	{
//		Vector3 wp = pointEventData.pressEventCamera.camera.ScreenToWorldPoint (pointEventData.position);
//		Vector3 np = gameObject.transform.parent.transform.InverseTransformPoint (wp);
//
//		float distance = Vector3.Distance (_beginPos, np);
//		if (distance <= ACTIVE_DISTANCE)
//			onClicked (gameObject);
//
//		transform.localScale = new  Vector3 (1, 1, 1);
//	}

//	private void _onDrag(GameObject go,PointerEventData pointEventData,BaseEventData baseEventData){
//		Vector3 wp = pointEventData.pressEventCamera.camera.ScreenToWorldPoint (pointEventData.position);
//		Vector3 np= gameObject.transform.parent.transform.InverseTransformPoint (wp);
//		np.z = 0;
//		gameObject.transform.localPosition = np;
//	}
}
