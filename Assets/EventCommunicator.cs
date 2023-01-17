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
    public void StartAttack() { playerScriptRef.StartAttack(); }
    public void CanCancelAttack() { playerScriptRef.CanCancelAttack(); }
    public void EndAttack() { playerScriptRef.EndAttack(); }
    public void CanMove() { playerScriptRef.CanMove(); }
    public void SnapToOpponent() { playerScriptRef.SnapToOpponent(); }
    public void LaunchPlayer(float units) { playerScriptRef.LaunchPlayer(units); }
    public void EndLaunch() { playerScriptRef.EndLaunch(); }
    public void BeatsForNextAttack(int numOfBeats) { playerScriptRef.BeatsForNextAttack(numOfBeats); } //Use eighth notes for calculations
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


    //Character-specific commands
    public void PlantSpear(int index) { characterSpecificObjects[index].GetComponent<Pole>().Plant(); }
    public void PickUpSpear(GameObject _objectToChild, int index)
    {
        _objectToChild.transform.SetParent(characterSpecificObjects[index].transform);
        _objectToChild.transform.localPosition = new Vector3(0.0089f, 0.0222f, -0.0086f);
        _objectToChild.transform.localEulerAngles = new Vector3(-100, 0, 0);
    }
}
