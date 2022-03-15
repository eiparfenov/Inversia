using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadLevelButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private LevelData level;

    private LevelManager _levelManager;

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }
    private void ClickHandler()
    {
        print("CLICK!!!");
        _levelManager.LoadLevel(level);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ClickHandler();
    }
}
