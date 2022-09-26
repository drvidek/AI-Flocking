using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    private EventSystem _thisEventSystem;

    private void Start()
    {
        _thisEventSystem = GetComponent<EventSystem>();
    }

    public void SetSelected(GameObject selected)
    {
        _thisEventSystem.SetSelectedGameObject(selected);
    }
}
