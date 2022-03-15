using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevelData;
    private Ghost _ghost;
    private GameObject _currentEnvironment;
    private GameObject _currentBlocks;
    private Transform _currentLightSource;

    [Header("LevelLoadingOptions")] 
    [SerializeField] private float loadTime;
    [SerializeField] private float altitude;
    [SerializeField] private AnimationCurve liftAnimationCurve; 
    [SerializeField] private float ghost3DTime;
    [SerializeField] private Transform ghost3DSpawnField;

    [Header("Prefabs")] 
    [SerializeField] private GameObject ghost3DPref;
    [SerializeField] private GameObject ghost2DPref;


    public void LoadLevel(LevelData level)
    {
        currentLevelData = level;
        ClearLevel();
        StartCoroutine(LoadLevel());
    }
    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        _currentBlocks = Instantiate(currentLevelData.Blocks, Vector3.up * altitude, Quaternion.identity);
        _currentEnvironment = Instantiate(currentLevelData.Environment);

        float progress = 0f;
        AddShadowCastingUnions();
        while (progress < 1f)
        {
            progress += Time.deltaTime / loadTime;
            _currentBlocks.transform.position = Vector3.up * 
                                                Mathf.Lerp(10f, 0f, liftAnimationCurve.Evaluate(progress));
            yield return null;
        }
        
        _currentBlocks.transform.position = Vector3.zero;
        
        
        GameObject ghost3D = Instantiate(ghost3DPref, ghost3DSpawnField);
        yield return new WaitForSeconds(ghost3DTime);
        Destroy(ghost3D);

        Vector3 startPosition = _currentEnvironment.GetComponentInChildren<LevelStart>().transform.position;
        _ghost =  Instantiate(ghost2DPref, startPosition, Quaternion.identity).GetComponent<Ghost>();
        _ghost.onGhostFall.AddListener(GhostFallHandler);
        FindObjectOfType<GhostController>().AppliedGhost = _ghost;
        _currentEnvironment.GetComponentInChildren<LevelFinish>().onLevelFinished.AddListener(LevelFinishHandler);
    }

    private void AddShadowCastingUnions()
    {
        _currentLightSource = new GameObject("LightSource").transform;
        _currentLightSource.position = currentLevelData.LightSourcePosition;
        _currentLightSource.parent = _currentEnvironment.transform;

        _currentBlocks.AddComponent<Rigidbody>().isKinematic = true;
        int childCount = _currentBlocks.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            ShadowCastingUnion union = _currentBlocks.transform.GetChild(i).gameObject.AddComponent<ShadowCastingUnion>();
            union.LightSource = _currentLightSource;
            union.Create();
        }
    }

    private void ClearLevel()
    {
        StopAllCoroutines();
        if(_currentLightSource)
            Destroy(_currentLightSource.gameObject);
        Destroy(_currentBlocks);
        Destroy(_currentEnvironment);
        if(_ghost)
            Destroy(_ghost.gameObject);
    }

    private void LevelFinishHandler()
    {
        ClearLevel();
        StartCoroutine(LoadLevel());
    }
    private void GhostFallHandler()
    {
        ClearLevel();
        StartCoroutine(LoadLevel());
    }
}