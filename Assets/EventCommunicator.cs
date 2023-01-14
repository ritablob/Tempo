using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCommunicator : MonoBehaviour
{
    [SerializeField] PlayerMovement playerScriptRef;

    public void StartAttack() { playerScriptRef.StartAttack(); }
    public void CanCancelAttack() { playerScriptRef.CanCancelAttack(); }
    public void EndAttack() { playerScriptRef.EndAttack(); }
    public void CanMove() { playerScriptRef.CanMove(); }
    public void SnapToOpponent() { playerScriptRef.SnapToOpponent(); }
    public void LaunchPlayer(float units) { playerScriptRef.LaunchPlayer(units); }
    public void EndLaunch() { playerScriptRef.EndLaunch(); }
    public void BeatsForNextAttack(int numOfBeats) { playerScriptRef.BeatsForNextAttack(numOfBeats); } //Use eighth notes for calculations
    public void SpawnShadowClone(float _fadeSpeed) { playerScriptRef.SpawnShadowClone(_fadeSpeed); }
}
