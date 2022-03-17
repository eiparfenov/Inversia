using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Levels allLevels;
    private LevelData currentLevelData;
    private Ghost _ghost;
    private GameObject _currentEnvironment;
    private GameObject _currentBlocks;
    private Transform _currentLightSource;
    private int _currentLevelId = 0;

    [Header("LevelLoadingOptions")] 
    [SerializeField] private float loadTime;
    [SerializeField] private float altitude;
    [SerializeField] private AnimationCurve liftAnimationCurve; 
    [SerializeField] private float ghost3DTime;
    [SerializeField] private Transform ghost3DSpawnField;

    [Header("Prefabs")] 
    [SerializeField] private GameObject ghost3DPref;
    [SerializeField] private GameObject ghost2DPref;


    public void LoadLevel(int level)
    {
        _currentLevelId = level;
        currentLevelData = allLevels.GetLevel(level);
        ClearLevel();
        StartCoroutine(LoadLevel());
    }
    private void Start()
    {
        currentLevelData = allLevels.GetLevel(_currentLevelId);
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
        GameObject ghostObj = ghost3D.transform.GetChild(0).gameObject;
        GameObject vfxObj = ghost3D.transform.GetChild(1).gameObject;
        vfxObj.SetActive(true);
        vfxObj.GetComponent<VisualEffect>().Play();
        float shrinkSpeed = 0.1f;
        while (ghostObj.transform.localScale.x > 0.1f)
        {
            ghostObj.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f) * shrinkSpeed;
            ghostObj.transform.position += new Vector3(0, 0.001f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.5f);
        vfxObj.GetComponent<VisualEffect>().Stop();
        yield return new WaitForSeconds(2f);
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
        _currentLevelId += 1;
        currentLevelData = allLevels.GetLevel(_currentLevelId);
        StartCoroutine(LoadLevel());
    }
    private void GhostFallHandler()
    {
        ClearLevel();
        StartCoroutine(LoadLevel());
    }
}