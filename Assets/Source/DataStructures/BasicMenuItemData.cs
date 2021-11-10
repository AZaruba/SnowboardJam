using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuItemData", menuName = "Menu Item Data")]
public class BasicMenuItemData : ScriptableObject
{
    #region PUBLIC_VARIABLES
    public float TransitionSpeed;
    public float TransitionOffset;
    public Vector2 TransitionDirection;
    public Color SelectedColor;
    public Color UnselectedColor;
    #endregion
}
