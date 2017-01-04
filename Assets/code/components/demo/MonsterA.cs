using UnityEngine;
using System.Collections;

public class MonsterA : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.localPosition;
		if (pos.y > 1280) {
			Destroy(gameObject);
			return;
		}

		pos.y += 10;
		transform.localPosition=pos;
	}
}
