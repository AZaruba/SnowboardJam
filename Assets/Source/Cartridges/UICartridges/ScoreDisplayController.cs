﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayController : MonoBehaviour, iEntityController
{
    [SerializeField] private Text scoreText;
    [SerializeField] private ScoreDisplayData data_scoreDisplay;

    private iMessageClient cl_score;
    private StateMachine sm_scoring;

    // Start is called before the first frame update
    void Start()
    {
        SetDefaultData();
        InitializeStateMachine();
        InitializeMessageClient();
    }

    // Update is called once per frame
    void Update()
    {
        EnginePull();

        UpdateStateMachine();

        sm_scoring.Act();

        EngineUpdate();
    }

    public void EnginePull()
    {

    }

    // TODO: Add scoreDisplay data
    public void EngineUpdate()
    {
        scoreText.text = data_scoreDisplay.i_displayScore.ToString();
    }

    // TODO: Add state machine for adding score
    public void UpdateStateMachine()
    {
        if (data_scoreDisplay.i_displayScore == data_scoreDisplay.i_currentScore)
        {
            sm_scoring.Execute(Command.STOP_SCORE);
        }
        else
        {
            sm_scoring.Execute(Command.INCREMENT_SCORE);
        }
    }

    #region StartupFunctions
    private void InitializeStateMachine()
    {
        IncrementCartridge cart_increment = new IncrementCartridge();

        UIIncrementState s_increment = new UIIncrementState(ref data_scoreDisplay, ref cart_increment);
        UIRestState s_rest = new UIRestState();
        UIPausedState s_paused = new UIPausedState();

        sm_scoring = new StateMachine(s_rest, StateRef.SCORE_STAY);
        sm_scoring.AddState(s_increment, StateRef.SCORE_INCREASING);
        sm_scoring.AddState(s_paused, StateRef.SCORE_PAUSED);
    }

    private void SetDefaultData()
    {
        data_scoreDisplay.i_currentScore = 0;
        data_scoreDisplay.i_displayScore = 0;
    }

    private void InitializeMessageClient()
    {
        cl_score = new ScoreMessageClient(ref data_scoreDisplay);
        MessageServer.Subscribe(ref cl_score, MessageID.SCORE_EDIT);
    }
    #endregion
}
