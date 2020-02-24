using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemController : MonoBehaviour, iEntityController
{
    [SerializeField] private BasicMenuItemData ItemData;
    [SerializeField] private RectTransform t_itemTransform;
    [SerializeField] private Text t_itemText;
    private MenuItemActiveData c_itemActiveData;

    private StateMachine sm_menuItem;

    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
        InitializeStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        EnginePull();
        UpdateStateMachine();
        sm_menuItem.Act();
        EngineUpdate();
    }

    public void EngineUpdate()
    {
        t_itemTransform.anchoredPosition = c_itemActiveData.v_itemPosition;
        t_itemText.color = c_itemActiveData.c_currentColor;
    }

    public void EnginePull()
    {

    }

    public void UpdateStateMachine()
    {
        // state machine only updates on parent call
        if (Vector2.Distance(c_itemActiveData.v_targetItemPosition, c_itemActiveData.v_itemPosition) < Constants.VECTOR_2_TOLERANCE)
        {
            sm_menuItem.Execute(Command.END_TRANSITION);
        }
    }

    public void ExecuteStateMachineCommand(Command cmd)
    {
        sm_menuItem.Execute(cmd);
    }

    public void ExecuteMenuCommand()
    {

    }

    private void InitializeStateMachine()
    {
        SelectedState s_selected = new SelectedState(ref ItemData, ref c_itemActiveData);
        PostselectedState s_postSelected = new PostselectedState(ref ItemData, ref c_itemActiveData);
        UnselectedState s_unselected = new UnselectedState(ref ItemData, ref c_itemActiveData);
        PreselectedState s_preselected = new PreselectedState(ref ItemData, ref c_itemActiveData);

        sm_menuItem = new StateMachine(s_unselected, StateRef.ITEM_UNSELECTED);
        sm_menuItem.AddState(s_postSelected, StateRef.ITEM_POSTSELECTED);
        sm_menuItem.AddState(s_selected, StateRef.ITEM_SELECTED);
        sm_menuItem.AddState(s_preselected, StateRef.ITEM_PRESELECTED);
    }

    private void InitializeData()
    {
        c_itemActiveData = new MenuItemActiveData();
        c_itemActiveData.v_itemPosition = t_itemTransform.anchoredPosition;
        c_itemActiveData.v_targetItemPosition = t_itemTransform.anchoredPosition;
        c_itemActiveData.v_origin = t_itemTransform.anchoredPosition;
        c_itemActiveData.c_currentColor = ItemData.UnselectedColor;
    }
}
