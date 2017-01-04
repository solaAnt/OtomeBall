using UnityEngine;
using System.Collections;

public class SolaButtonNative : MonoBehaviour
{
		public delegate void ClickedDelegate (GameObject src);

		public ClickedDelegate onClicked;
		private const int ANDROID = 0;
		private const int IOS = 1;
		private const int WIN = 2;
		private int _platform;
		private Vector3 _beginPos;

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

		void Start ()
		{
				onClicked += init;
		}
	
		void Update ()
		{
				switch (_platform) {
				case ANDROID:
				case IOS:
						if (Input.touchCount > 0) {
//				Touch point = Input.GetTouch(0);
								if (Input.GetTouch (0).phase == TouchPhase.Began)
										_beginPos = Input.mousePosition;
				
								if (Input.GetTouch (0).phase == TouchPhase.Moved)
										print ("TouchPhase.Moved");
				
								if (Input.GetTouch (0).phase == TouchPhase.Ended) {
										Vector3 curPos = Input.mousePosition;
										rayTest (curPos);
								}
						}  
						break;

				case WIN:
						if (Input.GetMouseButtonDown (0)) {
								_beginPos = Input.mousePosition;
								if (rayTest (_beginPos))
										transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
						}

			
						if (Input.GetMouseButtonUp (0)) {
								transform.localScale = new Vector3 (1, 1, 1);
								Vector3 curPos = Input.mousePosition;
								if (rayTest (curPos))
										onClicked (gameObject);
						}
						break;
				}
		}

		private bool rayTest (Vector3 curPos)
		{
				float distance=Vector3.Distance(curPos,_beginPos);
				if(distance>50)
					return false;

				RaycastHit2D[] hits = Physics2D.RaycastAll (Camera.main.ScreenToWorldPoint (_beginPos), Vector2.zero); 

				foreach (RaycastHit2D hit in hits) {
						if (hit.collider.gameObject == gameObject)
								return true;
				}
				return false;
		}

		private void init (GameObject gameObject)
		{
				print ("!!!!!!!!!!!!!!!!!!!!!!init");
		}

}

