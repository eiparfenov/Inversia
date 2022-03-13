using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelFinish : MonoBehaviour
{
    public UnityEvent onLevelFinished = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Ghost>())
        {
            StartCoroutine(FinishAfterDelay());
        }
    }

    private IEnumerator FinishAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        onLevelFinished.Invoke();
    }

    private void OnDestroy()
    {
        onLevelFinished.RemoveAllListeners();
    }
}