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
            Interactable interactable = GetClosest();

            if (!interactable)
                return;

            fixedJoint.connectedBody = interactable.GetComponent<Rigidbody>();
        }
        if (trigger.GetStateUp(controller.inputSource))
        {
            fixedJoint.connectedBody = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Enter");
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
        foreach (var interactable in interactables)
        {
            if (interactable == null)
                interactables.Remove(interactable);
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