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
        i_activeMenuItemIndex = 0;
        c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
        c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") > 0 && i_activeMenuItemIndex < l_menuItems.Count - 1)
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);
            i_activeMenuItemIndex++;
            c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        }
        else if (Input.GetAxis("Vertical") < 0 && i_activeMenuItemIndex > 0)
        {
            c_activeMenuItem.ExecuteStateMachineCommand(Command.UNSELECT);
            i_activeMenuItemIndex--;
            c_activeMenuItem = l_menuItems[i_activeMenuItemIndex];
            c_activeMenuItem.ExecuteStateMachineCommand(Command.SELECT);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            c_activeMenuItem.ExecuteMenuCommand();
        }
    }
}

/* TODO / Improving the menu controller
 * 1. Get rid of hardcoded axes and input
 * 2. implement cartridges (as the confusing position bug proved they ARE generally a good idea
 * 3. Get rid of constants
 * 4. Ensure menus can scale
 * 5. Bi-directional/grid based menus
 */
 