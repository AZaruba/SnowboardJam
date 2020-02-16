using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayController : MonoBehaviour, iEntityController
{
    [SerializeField] Text scoreText;

    private iMessageClient cl_score;
    private ScoreDisplayData data_scoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        data_scoreDisplay = new ScoreDisplayData();

        cl_score = new ScoreMessageClient();
        MessageServer.Subscribe(ref cl_score);
    }

    // Update is called once per frame
    void Update()
    {
        EnginePull();
        UpdateStateMachine();

        EngineUpdate();
    }

    public void EnginePull()
    {

    }

    // TODO: Add scoreDisplay data
    public void EngineUpdate()
    {
        // scoreText.text = scoreDisplayData.i_currentScore;
    }

    // TODO: Add state machine for adding score
    public void UpdateStateMachine()
    {

    }

}
