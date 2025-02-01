using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        mainMenu.Init();
    }
}
