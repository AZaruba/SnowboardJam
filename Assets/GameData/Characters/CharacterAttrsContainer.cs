using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttrsContainer : MonoBehaviour
{
    [SerializeField] private CharacterAttributeData CharacterAttributes;

    public CharacterAttributeData c_attrs
    {
        get { return CharacterAttributes; }
    }
}
