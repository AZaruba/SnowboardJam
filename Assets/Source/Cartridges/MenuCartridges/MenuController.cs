using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<MenuItemController> l_menuItems;
    private MenuItemController c_activeMenuItem;
    private int i_activeMenuItemIndex;

    void Start()
    {
        SetMenuDefaults();

        i_activeMenuItemIndex = 0;
        c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
        c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);

            // bug fix: modulo negative numbers produces an incorrect result
            if (i_activeMenuItemIndex == 0)
            {
                i_activeMenuItemIndex = l_menuItems.Count - 1;
            }
            else
            {
                i_activeMenuItemIndex = Mathf.Abs((i_activeMenuItemIndex - 1) % l_menuItems.Count);
            }
            c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);
            i_activeMenuItemIndex = Mathf.Abs((i_activeMenuItemIndex + 1) % l_menuItems.Count);
            c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            c_activeMenuItem.ExecuteMenuCommand();
        }
    }

    private void SetMenuDefaults()
    {
        MenuSelectionData.SetNextScene(1);
    }
}

/* TODO / Improving the menu controller
 * 1. Get rid of hardcoded axes and input
 * 3. Get rid of constants
 * 5. grid based menus
 */
 