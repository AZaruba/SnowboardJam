public class StateData
{
    public StateData()
    {
        b_preStarted = true;
    }

    private bool UpdateState;
    private bool CourseFinished;
    public bool b_preStarted;

    public bool b_updateState
    {
        get { return UpdateState; }
        set { UpdateState = value; }
    }

    public bool b_courseFinished
    {
        get { return CourseFinished; }
        set { CourseFinished = value; }
    }
}