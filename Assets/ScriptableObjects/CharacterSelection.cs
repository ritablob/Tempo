using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSelection", order = 1)]
public class CharacterSelection : ScriptableObject
{
    public GameObject characterPrefab;
    public string nameText;
    public Sprite nameSprite;
    public Sprite nameSelectedSprite;

    public Sprite characterImageSprite;
    public Sprite characterIconSprite;
}
