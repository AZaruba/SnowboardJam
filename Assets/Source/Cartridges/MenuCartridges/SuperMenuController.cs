using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuperMenuController : MonoBehaviour
{
    [SerializeField] private List<MenuController> MenuControllers;
    [SerializeField] private int PreviousSceneID;
    [SerializeField] private bool EmptyStackReturnsToPrevious;

    private iMessageClient c_client;
    private MenuController DefaultMenuController;

    private MenuController m_activeMenuController;
    private int m_activeMenuControllerIndex;
    private Stack<int> s_controllerStack;
    private bool m_bMenuInputActive;

    // Start is called before the first frame update
    void Start()
    {
        DefaultMenuController = MenuControllers[0];
        m_activeMenuController = DefaultMenuController;
        m_activeMenuControllerIndex = 0;
        c_client = new SuperMenuMessageClient(this);
        s_controllerStack = new Stack<int>();
        m_bMenuInputActive = false;

        for (int i = 0; i < MenuControllers.Count; i++)
        {
            MenuControllers[i].SetSuperMenuIndex(i);
        }

        MessageServer.Subscribe(ref c_client, MessageID.MENU_BACK);
        MessageServer.Subscribe(ref c_client, MessageID.MENU_FORWARD);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // call update function for active Menu Controller
        if (!m_bMenuInputActive)
        {
            if (GlobalInputController.GetInputAction(ControlAction.CONFIRM, KeyValue.IDLE) &&
                GlobalInputController.GetInputAction(ControlAction.BACK, KeyValue.IDLE))
            {
                m_bMenuInputActive = true;
            }
            else
            {
                return;
            }
        }
        m_activeMenuController.UpdateMenu();
    }

    /// <summary>
    /// Pushes the current menu's index on the menu stack, then
    /// Changes the active menu/index to the menu found at the
    /// given index in the menu controller list.
    ///
    /// Used to enter a "later" menu
    /// </summary>
    /// <param name="newIndex">The index of the target menu in the menu controller list</param>
    public void PushMenuStack(int newIndex)
    {
        if (newIndex < 0 || newIndex >= MenuControllers.Count)
        {
            return;
        }

        m_bMenuInputActive = false;

        s_controllerStack.Push(m_activeMenuControllerIndex);

        m_activeMenuController.HideMenu();
        m_activeMenuController = MenuControllers[newIndex];
        m_activeMenuControllerIndex = newIndex;
        m_activeMenuController.ShowMenu();
        m_activeMenuController.UpdateHelpTextOnBack();
    }

    /// <summary>
    /// Pops the value of the menu stack and then takes the associated value
    /// and activates it. Used when returning to an "earlier" menu.
    /// </summary>
    public void PopMenuStack()
    {
        m_bMenuInputActive = false;
        if (s_controllerStack.Count == 0)
        {
            if (EmptyStackReturnsToPrevious)
            {
                MessageServer.OnSceneChange();
                SceneManager.LoadScene(PreviousSceneID, LoadSceneMode.Single);
            }
            return;
        }

        int newIndex = s_controllerStack.Pop();

        m_activeMenuController.HideMenu();
        m_activeMenuControllerIndex = newIndex;
        m_activeMenuController = MenuControllers[newIndex];
        m_activeMenuController.ShowMenu();
        m_activeMenuController.UpdateHelpTextOnBack();

    }

    // what's needed is a message client watching for a "menu back" message to
    // update the current active menu controller to the new menu.
    // when we update that menu, we should only preserve the state going "OUT"
}
