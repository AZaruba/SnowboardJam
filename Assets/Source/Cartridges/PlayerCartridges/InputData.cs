using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData : MonoBehaviour
{
    [SerializeField] private KeyCode Trick1;
    [SerializeField] private KeyCode Trick2;
    [SerializeField] private KeyCode Trick3;
    [SerializeField] private KeyCode Trick4;
    [SerializeField] private KeyCode JumpButton;
    [SerializeField] private KeyCode TuckButton;

    [SerializeField] private string HorizMoveAxisName;
    [SerializeField] private string VertMoveAxisName;

    [SerializeField] private string TuckAxisName;
    [SerializeField] private string JumpAxisName;

    public KeyCode k_trick1
    {
        get { return Trick1; }
        set { Trick1 = value; }
    }

    public KeyCode k_trick2
    {
        get { return Trick2; }
        set { Trick2 = value; }
    }

    public KeyCode k_trick3
    {
        get { return Trick3; }
        set { Trick3 = value; }
    }

    public KeyCode k_trick4
    {
        get { return Trick4; }
        set { Trick4 = value; }
    }

    public string a_hMove
    {
        get { return HorizMoveAxisName; }
    }

    public string a_vMove
    {
        get { return VertMoveAxisName; }
    }

    public string a_tuck
    {
        get { return TuckAxisName; }
    }
    public KeyCode k_jump
    {
        get { return JumpButton; }
        set { JumpButton = value; }
    }

    public string a_jump
    {
        get { return JumpAxisName; }
    }
    public KeyCode k_tuck
    {
        get { return TuckButton; }
        set { TuckButton = value; }
    }
}
