using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum species
{
    cat, dog, frog, bear, panda
}

public struct animalType
{
    public species specie;
    public Sprite[] walkCycle;
    public Sprite[] working;
}


public class Animal : MonoBehaviour
{
    public List<animalType> animalTypes = new List<animalType>();




}
