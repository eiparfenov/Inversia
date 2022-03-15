using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputSystem : BaseInputModule
{
    [SerializeField] private Camera pointerCamera;
    [SerializeField] private SteamVR_Input_Sources inputSource;
    [SerializeField] private SteamVR_Action_Boolean buttonPress;

    private GameObject _currentObject;
    private PointerEventData _data;

    protected override void Awake()
    {
        base.Awake();
        _data = new PointerEventData(eventSystem);
    }
    public override void Process()
    {
        _data.Reset();
        _data.position = new Vector2(pointerCamera.pixelWidth / 2, pointerCamera.pixelHeight / 2);

        eventSystem.RaycastAll(_data, m_RaycastResultCache);
        _data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        _currentObject = _data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(_data, _currentObject);

        if (buttonPress.GetStateDown(inputSource))
        {
            _data.pointerPressRaycast = _data.pointerCurrentRaycast;

            GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(_currentObject, _data, ExecuteEvents.pointerDownHandler);

            if (newPointerPress == null)
                newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_currentObject);

            _data.pressPosition = _data.position;
            _data.pointerPress = newPointerPress;
            _data.rawPointerPress = _currentObject;
        }
        if (buttonPress.GetStateUp(inputSource))
        {
            ExecuteEvents.Execute(_data.pointerPress, _data, ExecuteEvents.pointerUpHandler);
            
            GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_currentObject);

            if(_data.pointerPress == pointerUpHandler)
            {
                ExecuteEvents.Execute(_data.pointerPress, _data, ExecuteEvents.pointerUpHandler);
            }
            
            eventSystem.SetSelectedGameObject(null);
            
            _data.pressPosition = Vector2.zero;
            _data.pointerPress = null;
            _data.rawPointerPress = null;
        }
    }
}
