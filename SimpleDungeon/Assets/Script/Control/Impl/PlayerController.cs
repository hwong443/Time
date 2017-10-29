using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
	private Hashtable keyCodeStatus = new Hashtable();

	// Use this for initialization
	void Start () {

		keyCodeStatus.Add (KeyCode.A,0);
		keyCodeStatus.Add (KeyCode.S,0);
		keyCodeStatus.Add (KeyCode.W,0);
		keyCodeStatus.Add (KeyCode.D,0);
		keyCodeStatus.Add (KeyCode.Q,0);
		keyCodeStatus.Add (KeyCode.E,0);
		keyCodeStatus.Add (KeyCode.J,0);
		keyCodeStatus.Add (KeyCode.K,0);
		keyCodeStatus.Add (KeyCode.L,0);
		keyCodeStatus.Add (KeyCode.U,0);
		keyCodeStatus.Add (KeyCode.I,0);
		keyCodeStatus.Add (KeyCode.O,0);

		if(characterObject == null)
			characterObject = "Player1";

		SetReceiver(characterObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.receiver != null) {
			UpdateKeyCodeStatus ();

			if (Input.GetKeyDown (KeyCode.A)) {
				receiver.pressA();
			} else if (Input.GetKeyUp (KeyCode.A)) {
				if (keyCodeStatus [KeyCode.D].Equals (1))
					receiver.pressD();
				else
					receiver.releaseA();
			}
			else if (Input.GetKeyDown (KeyCode.D)) {
				receiver.pressD();
			} else if (Input.GetKeyUp (KeyCode.D)) {
				if (keyCodeStatus [KeyCode.A].Equals (1))
					receiver.pressA();
				else
					receiver.releaseD();
			}
			else{
			}

			/*
			if (keyCodeStatus [KeyCode.A].Equals (0) && keyCodeStatus [KeyCode.D].Equals (0)){
				receiver.releaseA();
				receiver.releaseD();
			}
			else {
				if (Input.GetKeyDown (KeyCode.A)) {
					receiver.pressA();
				} else if (Input.GetKeyUp (KeyCode.A)) {
					if (keyCodeStatus [KeyCode.D].Equals (1))
						receiver.pressD();
					else
						receiver.releaseA();
				}
				else if (Input.GetKeyDown (KeyCode.D)) {
					receiver.pressD();
				} else if (Input.GetKeyUp (KeyCode.D)) {
					if (keyCodeStatus [KeyCode.A].Equals (1))
						receiver.pressA();
					else
						receiver.releaseD();
				}
			}
			*/

			if (Input.GetKeyDown (KeyCode.W)) {
				receiver.pressW();
			} else if (Input.GetKeyDown (KeyCode.S)) {
				receiver.pressS();
			} 
			else if(Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S)){
				receiver.releaseS();
			}

			if (Input.GetKeyDown (KeyCode.Q)) {
				receiver.pressQ();
			} else if (Input.GetKeyDown (KeyCode.E)) {
				receiver.pressE();
			} 

			if (Input.GetKeyDown (KeyCode.K)) {
				receiver.pressK();
			}

			if(Input.GetKeyDown (KeyCode.J)){
				receiver.pressJ();
			}
			else if(Input.GetKeyUp (KeyCode.J)){
				receiver.releaseJ();
			}

			if(Input.GetKeyDown (KeyCode.L)){
				receiver.pressL();
			}
			else if(Input.GetKeyUp (KeyCode.L)){
				receiver.releaseL();
			}
		}
	}

	void UpdateKeyCodeStatus(){
		if (Input.GetKeyDown (KeyCode.A)) {
			keyCodeStatus [KeyCode.A] = 1;
		} else if(Input.GetKeyUp (KeyCode.A)){
			keyCodeStatus [KeyCode.A] = 0;
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			keyCodeStatus [KeyCode.D] = 1;
		} else if(Input.GetKeyUp (KeyCode.D)){
			keyCodeStatus [KeyCode.D] = 0;
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			keyCodeStatus [KeyCode.S] = 1;
		} else if(Input.GetKeyUp (KeyCode.S)){
			keyCodeStatus [KeyCode.S] = 0;
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			keyCodeStatus [KeyCode.W] = 1;
		} else if(Input.GetKeyUp (KeyCode.W)){
			keyCodeStatus [KeyCode.W] = 0;
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			keyCodeStatus [KeyCode.Q] = 1;
		} else if(Input.GetKeyUp (KeyCode.Q)){
			keyCodeStatus [KeyCode.Q] = 0;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			keyCodeStatus [KeyCode.E] = 1;
		} else if(Input.GetKeyUp (KeyCode.E)){
			keyCodeStatus [KeyCode.E] = 0;
		}
		if (Input.GetKeyDown (KeyCode.J)) {
			keyCodeStatus [KeyCode.J] = 1;
		} else if(Input.GetKeyUp (KeyCode.J)){
			keyCodeStatus [KeyCode.J] = 0;
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			keyCodeStatus [KeyCode.K] = 1;
		} else if(Input.GetKeyUp (KeyCode.K)){
			keyCodeStatus [KeyCode.K] = 0;
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			keyCodeStatus [KeyCode.L] = 1;
		} else if(Input.GetKeyUp (KeyCode.L)){
			keyCodeStatus [KeyCode.L] = 0;
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			keyCodeStatus [KeyCode.U] = 1;
		} else if(Input.GetKeyUp (KeyCode.U)){
			keyCodeStatus [KeyCode.U] = 0;
		}
		if (Input.GetKeyDown (KeyCode.I)) {
			keyCodeStatus [KeyCode.I] = 1;
		} else if(Input.GetKeyUp (KeyCode.I)){
			keyCodeStatus [KeyCode.I] = 0;
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			keyCodeStatus [KeyCode.O] = 1;
		} else if(Input.GetKeyUp (KeyCode.O)){
			keyCodeStatus [KeyCode.O] = 0;
		}

	}
}
