using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCommunicator : MonoBehaviour
{
    [SerializeField] PlayerMovement playerScriptRef;
    [SerializeField] GameObject[] hitBoxes;
    [SerializeField] GameObject[] characterSpecificObjects;

    //General commands
    public void EndAttack() { playerScriptRef.EndAttack(); }
    public void CanMove() { playerScriptRef.CanMove(); }
    public void SnapToOpponent() { playerScriptRef.SnapToOpponent(); }
    public void LaunchPlayer(float units) { playerScriptRef.LaunchPlayer(units); }
    public void EndLaunch() { playerScriptRef.EndLaunch(); }
    public void LongAttack() { playerScriptRef.InLongCombo(); }
    public void SpawnShadowClone(int _index)
    {
        //Copy bone transforms
        Transform[] sourceBones = GetComponentsInChildren<Transform>();

        GameObject _shadowClone = Instantiate(characterSpecificObjects[_index], transform.parent.position, transform.parent.rotation);
        _shadowClone.transform.parent = transform.parent;

        _shadowClone.GetComponent<ShadowCloneInitializer>().InitializeBones(sourceBones);
    }
    public void SetSpeed(float newSpeed) { playerScriptRef.SetSpeed(newSpeed); }
    public void EnableHitbox(int hitBoxID) { hitBoxes[hitBoxID].SetActive(true); }
    public void DisableHitbox(int hitBoxID) { hitBoxes[hitBoxID].SetActive(false); }
    public void ResetLayers() { playerScriptRef.ResetLayers(); }


    //Character-specific commands
    public void PickUpSpear()
    {
        characterSpecificObjects[0].transform.SetParent(characterSpecificObjects[1].transform);
        characterSpecificObjects[0].transform.localPosition = new Vector3(0.0089f, 0.0222f, -0.0086f);
        characterSpecificObjects[0].transform.localEulerAngles = new Vector3(-100, 0, 0);
    }
    public void MoveToLocation(int positionIndex)
    {
        characterSpecificObjects[0].transform.SetParent(characterSpecificObjects[positionIndex].transform);
        characterSpecificObjects[0].GetComponent<Pole>().LerpPole(characterSpecificObjects[positionIndex].transform);
    }
}
