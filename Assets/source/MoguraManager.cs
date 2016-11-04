using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MoguraManager: MonoBehaviour {

	[SerializeField]
	Text[] judgeText;

	//textの寿命設定
	const int lifeCount = 50;
	int[] textLife = new int[GameManager.moguraMax];

	enum STATUS{
		STAY,
		UP,
		DOWN
	}

	STATUS[] moguraStatus = new STATUS[GameManager.moguraMax];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < GameManager.moguraMax; i++) {
			moguraStatus [i] = STATUS.STAY;
			textLife [i] = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		TextUpdate ();
		switch (GameManager.Instance.scene) {
		case GameManager.SCENE.COUNT:
			break;
		case GameManager.SCENE.MAIN:
			MoguraUpdate ();
			KeyUpdate ();
			break;
		case GameManager.SCENE.END:
			MoguraUpdate ();
			break;
		}
	}
		

	void Judge(int instanceNo){
		//二重判定阻止の為
		if (string.IsNullOrEmpty (judgeText[instanceNo].text)) {
			//position.yは-1.0f~0.5f間で移行0.5fに近いほど高得点
			if (GameManager.Instance.moguraObjList [instanceNo].transform.position.y > 0.4f) {
				judgeText [instanceNo].text = "Perfect!";
				GameManager.Instance.score += 1000;
			} else if (GameManager.Instance.moguraObjList [instanceNo].transform.position.y > 0.2f) {
				judgeText [instanceNo].text = "Nice!";
				GameManager.Instance.score += 500;
			} else {
				judgeText [instanceNo].text = "Bad..";
				GameManager.Instance.score += 100;
			}
			textLife [instanceNo] = lifeCount;
			//ここじゃない感満載だけどボタン押下時はSTATUSをDOWNに変える。
			moguraStatus [instanceNo] = STATUS.DOWN;
		}
	}


	void MoguraUpdate(){
		//毎フレーム処理。モグラのステータスを回す。
		for (int i = 0; i < GameManager.moguraMax; i++) {
			//上に出てくる処理。上限まで来たら下がる処理へ移行
			if (moguraStatus [i] == STATUS.UP) {
				if (GameManager.Instance.moguraObjList [i].transform.position.y <= 0.5f) {
					Vector3 pos = GameManager.Instance.moguraObjList [i].transform.position;
					pos.y += 0.05f;
					GameManager.Instance.moguraObjList [i].transform.position = pos;
				} else {
					moguraStatus [i] = STATUS.DOWN;
				}
			}
			//下に降りる処理。下限まで来たら待ち状態へ移行
			if (moguraStatus [i] == STATUS.DOWN) {
				if (GameManager.Instance.moguraObjList [i].transform.position.y >= -1.0f) {
					Vector3 pos = GameManager.Instance.moguraObjList [i].transform.position;
					pos.y -= 0.05f;
					GameManager.Instance.moguraObjList [i].transform.position = pos;
				} else {
					moguraStatus [i] = STATUS.STAY;
				}
			}
			//乱数を使用して待ち状態のモグラを上がる状態に移行
			if (moguraStatus [i] == STATUS.STAY) {
				if (Random.value > 0.995f) {
					moguraStatus [i] = STATUS.UP;
				}
			}
		}
	}


	void TextUpdate(){
		//毎フレーム処理。Textの表示時間の管理。
		for (int i = 0; i < GameManager.moguraMax; i++) {
			if (textLife [i] > 0) {
				textLife [i]--;
			}
			else if (textLife [i] <= 0 && !(string.IsNullOrEmpty(judgeText [i].text))) {
				judgeText [i].text = "";
			}
		}
	}


	void KeyUpdate(){
		//キー入力の制御（ここじゃないかもだから一時的に？）
		if (Input.GetKeyDown (KeyCode.A)) {
			if (moguraStatus [0] != STATUS.STAY) {
				Judge(0);
			}
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			if (moguraStatus [1] != STATUS.STAY) {
				Judge(1);
			}
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			if (moguraStatus [2] != STATUS.STAY) {
				Judge(2);
			}
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			if (moguraStatus [3] != STATUS.STAY) {
				Judge(3);
			}
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			if (moguraStatus [4] != STATUS.STAY) {
				Judge(4);
			}
		}
	}
}