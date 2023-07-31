using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum species
{
    bee, bunny, cat, chicken, creature, deer, monkey, shark, sloth
}
[Serializable]
public struct animalType
{
    public species specie;
    public Sprite[] walkCycleBack;
    public Sprite[] walkCycleForward;
    public Sprite[] workingForward;
    public Sprite[] workingBackward;
    public Sprite face;
}


public class Animal : MonoBehaviour
{
    public List<animalType> animalTypes = new List<animalType>();




}

