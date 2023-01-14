using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCommunicator : MonoBehaviour
{
    [SerializeField] PlayerMovement playerScriptRef;
    [SerializeField] GameObject[] hitBoxes;

    public void StartAttack() { playerScriptRef.StartAttack(); }
    public void CanCancelAttack() { playerScriptRef.CanCancelAttack(); }
    public void EndAttack() { playerScriptRef.EndAttack(); }
    public void CanMove() { playerScriptRef.CanMove(); }
    public void SnapToOpponent() { playerScriptRef.SnapToOpponent(); }
    public void LaunchPlayer(float units) { playerScriptRef.LaunchPlayer(units); }
    public void EndLaunch() { playerScriptRef.EndLaunch(); }
    public void BeatsForNextAttack(int numOfBeats) { playerScriptRef.BeatsForNextAttack(numOfBeats); } //Use eighth notes for calculations
    public void SpawnShadowClone(float _fadeSpeed) { playerScriptRef.SpawnShadowClone(_fadeSpeed); }
    public void SetSpeed(float newSpeed) { playerScriptRef.SetSpeed(newSpeed); }
    public void EnableHitbox(int hitBoxID) { hitBoxes[hitBoxID].SetActive(true); }
    public void DisableHitbox(int hitBoxID) { hitBoxes[hitBoxID].SetActive(false); }
}
