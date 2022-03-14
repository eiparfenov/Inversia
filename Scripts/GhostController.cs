using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Ghost ghost;
    public Ghost AppliedGhost => ghost;

    private void Update()
    {
        
    }
}
