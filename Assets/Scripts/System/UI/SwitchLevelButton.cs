using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLevelButton : MonoBehaviour
{
    public void OnClick(string scene_name)
    {
        LevelManager.instance.LoadScene(scene_name);
    }
}
