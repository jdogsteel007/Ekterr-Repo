using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class so we can easily access things we want to from anywhere in the scene
/// </summary>
public class Globals : MonoBehaviour {

    public static Globals Inst;

    //Any global variables that we might want to access from anywhere can be put in here
    public PlayerController Player;
    public cubController Cub;
    public GameObject DefaultBulletPrefab, InputFocus, MainCamera, MainCanvas;
    public static bool DidPlayerSwitchThisFrame = false;

	// Use this for initialization
	void Start () {
        Inst = this;
	}

    private void Awake()
    {
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(Cub);
        DontDestroyOnLoad(MainCamera);
        DontDestroyOnLoad(MainCanvas);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
