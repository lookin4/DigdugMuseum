using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static GameManager instance;

	public static GameManager Instance{
		get{return instance;}
	}

	[SerializeField]
	GameObject moguraObj;

	[SerializeField]
	Text timeText;

	[SerializeField]
	Text scoreText;

	[SerializeField]
	Text countText;

	float countTime = 5.0f;

	public float time;
	public int score = 0;

	public enum SCENE{
		COUNT,
		MAIN,
		END
	}

	public SCENE scene = SCENE.COUNT;

	public static int moguraMax = 5;//現状奇数のみ

	public List<GameObject> moguraObjList = new List<GameObject>();

	void Awake(){
		instance = this;
	}

	private void CreateMogura(){
		int a = moguraMax / 2;
		for (int j = -a; j <= a; j += 1) {
			GameObject obj = Instantiate (moguraObj);
			Vector3 pos = new Vector3 (j*2.0f,-1.0f,-45.0f);
			obj.transform.position = pos;

			moguraObjList.Add (obj);
		}
	}

	// Use this for initialization
	void Start () {
		CreateMogura ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (scene) {
		case SCENE.COUNT:
			if (countTime > 3.0f) {
				countText.text = "Ready...";
			} else if (countTime > 0.0f) {
				countText.text = "" + ((int)countTime + 1);
			} else {
				countText.text = "";
				scene = SCENE.MAIN;
			}
			countTime -= 1.0f / 60.0f;
			break;
		case SCENE.MAIN:
			//切り上げ表示にする為+1
			timeText.text = "Time:" + (int)(time + 0.9f);
			scoreText.text = "score:" + score;
			time -= 1.0f / 60.0f;
			if (time <= 0.0f)
				scene = SCENE.END;
			break;
		case SCENE.END:
			countText.text = "Finish!";
			break;
		}
	}
}
