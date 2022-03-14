using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GhostController : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Ghost ghost;
    public Ghost AppliedGhost { set { ghost = value; } }
    [SerializeField] private SteamVR_Action_Vector2 joystick;
    [SerializeField] private SteamVR_Behaviour_Pose controller;

    void Awake()
    {
        controller = GetComponent<SteamVR_Behaviour_Pose>();
    }
    private void Update()
    {
        if(ghost != null)
        {
            ghost.SetHorizontalAxis(-joystick.GetAxis(controller.inputSource).x);
        }
    }
}
