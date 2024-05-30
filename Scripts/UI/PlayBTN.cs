using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBTN : MonoBehaviour
{
    public void Play()
    {
        GameSceneManager.instance.LoadGameScene();
    }
}
