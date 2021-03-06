using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean trigger;

    [SerializeField] private SteamVR_Behaviour_Pose controller;

    [SerializeField] private FixedJoint fixedJoint;

    [SerializeField] private List<Interactable> interactables = new List<Interactable>();

    [SerializeField] private Interactable currentInteractable;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<SteamVR_Behaviour_Pose>();
        fixedJoint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger.GetStateDown(controller.inputSource))
        {
            currentInteractable = GetClosest();

            if (!currentInteractable)
                return;

            fixedJoint.connectedBody = currentInteractable.GetComponent<Rigidbody>();
            currentInteractable.onVRDragStart();
        }
        if(currentInteractable)
        {
            currentInteractable.onVRDrag();
        }
        if (trigger.GetStateUp(controller.inputSource))
        {
            fixedJoint.connectedBody = null;
            if(currentInteractable)
            {
                currentInteractable.onVRDragFinish();
                currentInteractable = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponentInParent<Interactable>();
        if (interactable && !interactables.Contains(interactable))
        {
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponentInParent<Interactable>();
        if (interactable && interactables.Contains(interactable))
        {
            interactables.Remove(interactable);
        }
    }

    private Interactable GetClosest()
    {
        for (int i = 0; i < interactables.Count; i++)
        {
            if (interactables[i] == null)
                interactables.Remove(interactables[i]);
        }

        if (interactables.Count == 0)
            return null;

        float minDist = (transform.position - interactables[0].transform.position).magnitude;
        Interactable closest = interactables[0];

        foreach (var interactable in interactables)
        {
            float newDist = (transform.position - interactable.transform.position).magnitude;
            if (newDist < minDist)
            {
                minDist = newDist;
                closest = interactable;
            }
        }

        return closest;
    }
}