using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputMutex", menuName = "Input Mutex")]
public class InputMutex : ScriptableObject
{
public List<ControlAction> MutuallyExclusiveActions;
}
