using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This struct contains information regarding a course's records
/// </summary>
public struct CourseRecordInfo
{
    public double BestTime;
    public CharacterSelection bestTimeCharacter;

    public int BestScore;
    public CharacterSelection bestScoreCharacter;
}

public struct ControlSettings
{
    public int JumpBtn;
    public int CrouchBtn;

    public int DownGrabBtn;
    public int LeftGrabBtn;
    public int RightGrabBtn;
    public int UpGrabBtn;

    public int ConfirmBtn;
    public int BackBtn;
    public int PauseBtn;

    public string SpinAxis;
    public string FlipAxis;

    public string SpinAxis_Controller;
    public string FlipAxis_Controller;
}

/// <summary>
/// This struct encompasses all video settings, including graphics quality and resolution
/// </summary>
public struct VideoSettings
{
    public int ScreenResIndex;
    public int RenderMultiplier;
    public FullScreenMode ScreenMode;

    public int Quality;
}

/// <summary>
/// This struct contains stored values for all aduio settings
/// </summary>
public struct AudioSettings
{
    public bool AudioEnabled;

    public int MasterAudioLevel;
    public int MusicLevel;
    public int VoiceLevel;
    public int SfxLevel;
}

/// <summary>
/// This struct encompasses all data that will be saved to disk
/// and be persistent across game sessions
/// </summary>
public struct PersistentSaveData
{
    public bool courseTwoUnlocked;
    public bool courseThreeUnlocked;

    public bool characterFiveUnlocked;
    public bool characterSixUnlocked;

    public CourseRecordInfo courseOneRecords;
    public CourseRecordInfo courseTwoRecords;
    public CourseRecordInfo courseThreeRecords;
}

public static class PlayerPrefsNames
{
    // Video settings
    public static string ScreenResIdx = "ScreenResIdx";
    public static string RenderMult = "RenderMultiplier";
    public static string VQuality = "VideoQuality";
    public static string FSMode = "FullScreenMode";

    // Audio settings
    public static string AudioEn = "AudioEnabled";

    public static string MasterAudioL = "MasterAudio";
    public static string MusicL = "MusicLevel";
    public static string VoiceL = "VoiceLevel";
    public static string SfxL = "SfxLevel";

    // keyboard settings
    public static string JumpBtn = "Jump";
    public static string CrouchBtn = "Crouch";

    public static string DownGrabBtn = "DownGrab";
    public static string LeftGrabBtn = "LeftGrab";
    public static string RightGrabBtn = "RightGrab";
    public static string UpGrabBtn = "UpGrab";

    public static string ConfirmBtn = "Confirm";
    public static string BackBtn = "Back";
    public static string PauseBtn = "Pause";

    public static string SpinAxis = "SpinAx";
    public static string FlipAxis = "FlipAx";

    // controller settings
    public static string JumpBtn_Controller = "Jump_Controller";
    public static string CrouchBtn_Controller = "Crouch_Controller";

    public static string DownGrabBtn_Controller = "DownGrab_Controller";
    public static string LeftGrabBtn_Controller = "LeftGrab_Controller";
    public static string RightGrabBtn_Controller = "RightGrab_Controller";
    public static string UpGrabBtn_Controller = "UpGrab_Controller";

    public static string ConfirmBtn_Controller = "Confirm_Controller";
    public static string BackBtn_Controller = "Back_Controller";
    public static string PauseBtn_Controller = "Pause_Controller";

    public static string SpinAxis_Controller = "SpinAx_Controller";
    public static string FlipAxis_Controller = "FlipAx_Controller";
}

/// <summary>
/// This static class contains all data that will be propagated across scenes.
/// Included are getters/setters as well as systems for serializing and saving
/// data.
/// </summary>
public static class GlobalGameData
{
    public static CharacterSelection playerOneCharacter;
    public static StyleSelection playerOneStyle;
    public static CourseSelection selecctedCourse; // redundant as we can identify courses as scenes?
    public static GameMode selectedMode;

    public static PersistentSaveData currentSaveData;
    public static ControlSettings controlSettings;
    public static ControlSettings controlSettings_controller;
    public static VideoSettings videoSettings;
    public static AudioSettings audioSettings;

    public static int GetSettingsInt(DataTarget targetIn)
    {
        switch (targetIn)
        {
            case DataTarget.MASTER_AUDIO_LEVEL:
                return audioSettings.MasterAudioLevel;
            case DataTarget.MUSIC_LEVEL:
                return audioSettings.MusicLevel;
            case DataTarget.VOICE_LEVEL:
                return audioSettings.VoiceLevel;
            case DataTarget.SFX_LEVEL:
                return audioSettings.SfxLevel;
            case DataTarget.RENDER_MULTIPLIER:
                return videoSettings.RenderMultiplier;
            case DataTarget.VIDEO_QUALITY:
                return (int)videoSettings.Quality;
            case DataTarget.RESOLUTION_IDX:
                return videoSettings.ScreenResIndex;
            case DataTarget.SCREEN_MODE:
                return (int)videoSettings.ScreenMode;
        }
        return default;
    }

    public static bool GetSettingsBool(DataTarget targetIn)
    {
        return false;
    }

    public static float GetSettingsFloat(DataTarget targetIn)
    {
        return default;
    }


    public static bool SetActionSetting(ControlAction actIn, KeyCode keyIn, InputType inputType = InputType.KEYBOARD_GENERIC)
    {
        if (inputType == InputType.KEYBOARD_GENERIC)
        {
            switch (actIn)
            {
                case ControlAction.BACK:
                    controlSettings.BackBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.CONFIRM:
                    controlSettings.ConfirmBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.PAUSE:
                    controlSettings.PauseBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.JUMP:
                    controlSettings.JumpBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.CROUCH:
                    controlSettings.CrouchBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.DOWN_GRAB:
                    controlSettings.DownGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.LEFT_GRAB:
                    controlSettings.LeftGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.RIGHT_GRAB:
                    controlSettings.RightGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.UP_GRAB:
                    controlSettings.UpGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
            }
        }
        else
        {
            switch (actIn)
            {
                case ControlAction.BACK:
                    controlSettings_controller.BackBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.CONFIRM:
                    controlSettings_controller.ConfirmBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.PAUSE:
                    controlSettings_controller.PauseBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.JUMP:
                    controlSettings_controller.JumpBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.CROUCH:
                    controlSettings_controller.CrouchBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.DOWN_GRAB:
                    controlSettings_controller.DownGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.LEFT_GRAB:
                    controlSettings_controller.LeftGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.RIGHT_GRAB:
                    controlSettings_controller.RightGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
                case ControlAction.UP_GRAB:
                    controlSettings_controller.UpGrabBtn = (int)keyIn;
                    SaveControlSettings();
                    return true;
            }
        }
        return false;
    }

    public static bool SetActionSetting(ControlAction actIn, string axName)
    {
        switch (actIn)
        {
            case ControlAction.SPIN_AXIS:
                controlSettings.SpinAxis = axName;
                SaveControlSettings();
                return true;
            case ControlAction.FLIP_AXIS:
                controlSettings.FlipAxis = axName;
                SaveControlSettings();
                return true;
        }
        return false;
    }

    // TODO: implementing mouse input will mean that there will always be a way to reassign keys, so "safety" keys are unnecessary
    public static KeyCode GetActionSetting(ControlAction actIn, InputType typeIn = InputType.KEYBOARD_GENERIC)
    {
        bool controllerSet = typeIn != InputType.KEYBOARD_GENERIC;

        switch (actIn)
        {
            case ControlAction.BACK:
                return controllerSet ? (KeyCode)controlSettings_controller.BackBtn : (KeyCode)controlSettings.BackBtn;
            case ControlAction.CONFIRM:
                return controllerSet ? (KeyCode)controlSettings_controller.ConfirmBtn : (KeyCode)controlSettings.ConfirmBtn;
            case ControlAction.PAUSE:
                return controllerSet ? (KeyCode)controlSettings_controller.PauseBtn : (KeyCode)controlSettings.PauseBtn;
            case ControlAction.JUMP:
                return controllerSet ? (KeyCode)controlSettings_controller.JumpBtn : (KeyCode)controlSettings.JumpBtn;
            case ControlAction.CROUCH:
                return controllerSet ? (KeyCode)controlSettings_controller.CrouchBtn : (KeyCode)controlSettings.CrouchBtn;
            case ControlAction.DOWN_GRAB:
                return controllerSet ? (KeyCode)controlSettings_controller.DownGrabBtn : (KeyCode)controlSettings.DownGrabBtn;
            case ControlAction.LEFT_GRAB:
                return controllerSet ? (KeyCode)controlSettings_controller.LeftGrabBtn : (KeyCode)controlSettings.LeftGrabBtn;
            case ControlAction.RIGHT_GRAB:
                return controllerSet ? (KeyCode)controlSettings_controller.RightGrabBtn : (KeyCode)controlSettings.RightGrabBtn;
            case ControlAction.UP_GRAB:
                return controllerSet ? (KeyCode)controlSettings_controller.UpGrabBtn : (KeyCode)controlSettings.UpGrabBtn;
        }
        return KeyCode.None;
    }

    public static string GetAnalogActionSetting(ControlAction actIn)
    {
        switch (actIn)
        {
            case ControlAction.SPIN_AXIS:
                return controlSettings.SpinAxis;
            case ControlAction.FLIP_AXIS:
                return controlSettings.FlipAxis;
        }
        return "";
    }

    public static bool SetSettingsValue(DataTarget target, int value)
    {
        switch(target)
        {
            case DataTarget.RESOLUTION_IDX:
                videoSettings.ScreenResIndex = value;
                Resolution resOut = Screen.resolutions[videoSettings.ScreenResIndex];
                Screen.SetResolution(resOut.width, resOut.height, videoSettings.ScreenMode);
                SaveVideoSettings();
                break;
            case DataTarget.SCREEN_MODE:
                videoSettings.ScreenMode = (FullScreenMode)value;
                resOut = Screen.resolutions[videoSettings.ScreenResIndex];
                Screen.SetResolution(resOut.width, resOut.height, videoSettings.ScreenMode);
                SaveVideoSettings();
                break;
            case DataTarget.RENDER_MULTIPLIER:
                videoSettings.RenderMultiplier = value;
                SaveVideoSettings();
                break;
            case DataTarget.VIDEO_QUALITY:
                videoSettings.Quality = value;
                QualitySettings.SetQualityLevel(videoSettings.Quality);
                SaveVideoSettings();
                break;
            case DataTarget.MASTER_AUDIO_LEVEL:
                audioSettings.MasterAudioLevel = value;
                SaveAudioSettings();
                break;
            case DataTarget.MUSIC_LEVEL:
                audioSettings.MusicLevel = value;
                SaveAudioSettings();
                break;
            case DataTarget.VOICE_LEVEL:
                audioSettings.VoiceLevel = value;
                SaveAudioSettings();
                break;
            case DataTarget.SFX_LEVEL:
                audioSettings.SfxLevel = value;
                SaveAudioSettings();
                break;
        }
        return true;
    }
    public static bool SetSettingsValue(DataTarget target, float value)
    {
        switch (target)
        {
            // no floats currently
        }
        return true;
    }
    public static bool SetSettingsValue(DataTarget target, bool value)
    {
        switch (target)
        {
            case DataTarget.AUDIO_ENABLED:
                audioSettings.AudioEnabled = value;
                SaveAudioSettings();
                break;
        }
        return true;
    }


    public static bool SaveData()
    {
        return false;
    }

    public static bool LoadData()
    {
        return false;
    }

    public static string GetSavePath()
    {
        return Application.persistentDataPath + "saveData.shred";
    }

    public static bool SaveVideoSettings()
    {
        PlayerPrefs.SetInt(PlayerPrefsNames.ScreenResIdx, videoSettings.ScreenResIndex);
        PlayerPrefs.SetInt(PlayerPrefsNames.RenderMult, videoSettings.RenderMultiplier);
        PlayerPrefs.SetInt(PlayerPrefsNames.VQuality, (int)videoSettings.Quality);
        PlayerPrefs.SetInt(PlayerPrefsNames.FSMode, (int)videoSettings.ScreenMode);
        PlayerPrefs.Save();
        return true;
    }

    public static bool LoadVideoSettings()
    {
        bool status = true;
        int intVal;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.ScreenResIdx);
        if (intVal == default || intVal >= Screen.resolutions.Length || intVal < 0)
        {
            status = false;
            videoSettings.ScreenResIndex = -1;
        }
        else
        {
            videoSettings.ScreenResIndex = intVal;
            Resolution resOut = Screen.resolutions[videoSettings.ScreenResIndex];
            Screen.SetResolution(resOut.width, resOut.height, videoSettings.ScreenMode);
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.RenderMult);
        if (intVal == default)
        {
            status = false;
            videoSettings.RenderMultiplier = 1;
        }
        else
        {
            videoSettings.RenderMultiplier = intVal;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.VQuality);
        if (intVal == default || intVal >= QualitySettings.names.Length)
        {
            status = false;
            videoSettings.Quality = 0;
        }
        else
        {
            videoSettings.Quality = intVal;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.FSMode);
        if (intVal == default || intVal > 3 || intVal < 0)
        {
            status = false;
            videoSettings.ScreenMode = (int)FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            videoSettings.ScreenMode = (FullScreenMode)intVal;
        }
            

        return status;
    }


    public static bool SaveAudioSettings()
    {
        PlayerPrefs.SetInt(PlayerPrefsNames.AudioEn, audioSettings.AudioEnabled ? 1 : 0);
        PlayerPrefs.SetInt(PlayerPrefsNames.MasterAudioL, audioSettings.MasterAudioLevel);
        PlayerPrefs.SetInt(PlayerPrefsNames.MusicL, audioSettings.MusicLevel);
        PlayerPrefs.SetInt(PlayerPrefsNames.VoiceL, audioSettings.VoiceLevel);
        PlayerPrefs.SetInt(PlayerPrefsNames.SfxL, audioSettings.SfxLevel);
        PlayerPrefs.Save();
        return true;
    }

    public static bool LoadAudioSettings()
    {
        bool status = true;
        int intVal;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.AudioEn);
        if (intVal == default)
        {
            status = false;
            audioSettings.AudioEnabled = true;
        }
        else
        {
            audioSettings.AudioEnabled = intVal == 1 ? true : false;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.MasterAudioL);
        if (intVal == default)
        {
            status = false;
            audioSettings.MasterAudioLevel = 50;
        }
        else
        {
            audioSettings.MasterAudioLevel = intVal;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.MusicL);
        if (intVal == default)
        {
            status = false;
            audioSettings.MusicLevel = 50;
        }
        else
        {
            audioSettings.MusicLevel = intVal;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.VoiceL);
        if (intVal == default)
        {
            status = false;
            audioSettings.VoiceLevel = 50;
        }
        else
        {
            audioSettings.VoiceLevel = intVal;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.SfxL);
        if (intVal == default)
        {
            status = false;
            audioSettings.SfxLevel = 50;
        }
        else
        {
            audioSettings.SfxLevel = intVal;
        }

        return status;
    }

    // TODO: create separate controller/keyboard bindings
    private static bool SaveControlSettings()
    {
        PlayerPrefs.SetInt(PlayerPrefsNames.JumpBtn, controlSettings.JumpBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.CrouchBtn, controlSettings.CrouchBtn);

        PlayerPrefs.SetInt(PlayerPrefsNames.DownGrabBtn, controlSettings.DownGrabBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.LeftGrabBtn, controlSettings.LeftGrabBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.RightGrabBtn, controlSettings.RightGrabBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.UpGrabBtn, controlSettings.UpGrabBtn);

        PlayerPrefs.SetInt(PlayerPrefsNames.ConfirmBtn, controlSettings.ConfirmBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.BackBtn, controlSettings.BackBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.PauseBtn, controlSettings.PauseBtn);

        PlayerPrefs.SetString(PlayerPrefsNames.FlipAxis, controlSettings.FlipAxis);
        PlayerPrefs.SetString(PlayerPrefsNames.SpinAxis, controlSettings.SpinAxis);

        PlayerPrefs.SetInt(PlayerPrefsNames.JumpBtn_Controller, controlSettings_controller.JumpBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.CrouchBtn_Controller, controlSettings_controller.CrouchBtn);

        KeyCode confirmSettings = (KeyCode)controlSettings.ConfirmBtn;
        KeyCode playerPrefsConfirm = (KeyCode)PlayerPrefs.GetInt(PlayerPrefsNames.ConfirmBtn);

        if (confirmSettings == playerPrefsConfirm)
        {

        }

        PlayerPrefs.SetInt(PlayerPrefsNames.DownGrabBtn_Controller, controlSettings_controller.DownGrabBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.LeftGrabBtn_Controller, controlSettings_controller.LeftGrabBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.RightGrabBtn_Controller, controlSettings_controller.RightGrabBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.UpGrabBtn_Controller, controlSettings_controller.UpGrabBtn);

        PlayerPrefs.SetInt(PlayerPrefsNames.ConfirmBtn_Controller, controlSettings_controller.ConfirmBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.BackBtn_Controller, controlSettings_controller.BackBtn);
        PlayerPrefs.SetInt(PlayerPrefsNames.PauseBtn_Controller, controlSettings_controller.PauseBtn);

        PlayerPrefs.SetString(PlayerPrefsNames.FlipAxis_Controller, controlSettings_controller.FlipAxis);
        PlayerPrefs.SetString(PlayerPrefsNames.SpinAxis_Controller, controlSettings_controller.SpinAxis);
        return true;
    }

    private static bool LoadControlSettings()
    {
        bool status = true;

        DefaultControls.InitializeLayouts();

        // decide layout based on 
        DefaultControlLayout layoutIn = DefaultControls.KeyboardLayout;
        DefaultControlLayout gamepadLayoutIn = DefaultControls.XboxLayout;
        GlobalInputController.SetActiveControllerType(InputType.KEYBOARD_GENERIC);

        int intVal;

        string[] joystickNames = Input.GetJoystickNames();
        if (joystickNames.Length > 0)
        {
            if (joystickNames[0].ToLower().Contains("dualshock"))
            {
                gamepadLayoutIn = DefaultControls.PSLayout;
            }
            // other controller types?
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.JumpBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.JumpBtn = (int)layoutIn.DefaultJump;
        }
        else
        {
            controlSettings.JumpBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.CrouchBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.CrouchBtn = (int)layoutIn.DefaultCrouch;
        }
        else
        {
            controlSettings.CrouchBtn = intVal;
        }


        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.DownGrabBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.DownGrabBtn = (int)layoutIn.DefaultTrickD;
        }
        else
        {
            controlSettings.DownGrabBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.LeftGrabBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.LeftGrabBtn = (int)layoutIn.DefaultTrickL;
        }
        else
        {
            controlSettings.LeftGrabBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.RightGrabBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.RightGrabBtn = (int)layoutIn.DefaultTrickR;
        }
        else
        {
            controlSettings.RightGrabBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.UpGrabBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.UpGrabBtn = (int)layoutIn.DefaultTrickU;
        }
        else
        {
            controlSettings.UpGrabBtn = intVal;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.BackBtn);
        if(intVal == default)
        {
            status = false;
            controlSettings.BackBtn = (int)layoutIn.DefaultBack;
        }
        else
        {
            controlSettings.BackBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.ConfirmBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.ConfirmBtn = (int)layoutIn.DefaultConfirm;
        }
        else
        {
            controlSettings.ConfirmBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.PauseBtn);
        if (intVal == default)
        {
            status = false;
            controlSettings.PauseBtn = (int)layoutIn.DefaultPause;
        }
        else
        {
            controlSettings.PauseBtn = intVal;
        }

        string strVal;
        strVal = PlayerPrefs.GetString(PlayerPrefsNames.FlipAxis);
        if (strVal.Length == 0)
        {
            status = false;
            controlSettings.FlipAxis = layoutIn.DefaultLVerti;
        }
        else
        {
            controlSettings.FlipAxis = strVal;
        }
        strVal = PlayerPrefs.GetString(PlayerPrefsNames.SpinAxis);
        if (strVal.Length == 0)
        {
            status = false;
            controlSettings.SpinAxis = layoutIn.DefaultLHoriz;
        }
        else
        {
            controlSettings.SpinAxis = strVal;
        }

        // Controller
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.JumpBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.JumpBtn = (int)gamepadLayoutIn.DefaultJump;
        }
        else
        {
            controlSettings_controller.JumpBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.CrouchBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.CrouchBtn = (int)gamepadLayoutIn.DefaultCrouch;
        }
        else
        {
            controlSettings_controller.CrouchBtn = intVal;
        }


        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.DownGrabBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.DownGrabBtn = (int)gamepadLayoutIn.DefaultTrickD;
        }
        else
        {
            controlSettings_controller.DownGrabBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.LeftGrabBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.LeftGrabBtn = (int)gamepadLayoutIn.DefaultTrickL;
        }
        else
        {
            controlSettings_controller.LeftGrabBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.RightGrabBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.RightGrabBtn = (int)gamepadLayoutIn.DefaultTrickR;
        }
        else
        {
            controlSettings_controller.RightGrabBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.UpGrabBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.UpGrabBtn = (int)gamepadLayoutIn.DefaultTrickU;
        }
        else
        {
            controlSettings_controller.UpGrabBtn = intVal;
        }

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.BackBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.BackBtn = (int)gamepadLayoutIn.DefaultBack;
        }
        else
        {
            controlSettings_controller.BackBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.ConfirmBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.ConfirmBtn = (int)gamepadLayoutIn.DefaultConfirm;
        }
        else
        {
            controlSettings_controller.ConfirmBtn = intVal;
        }
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.PauseBtn_Controller);
        if (intVal == default)
        {
            status = false;
            controlSettings_controller.PauseBtn = (int)gamepadLayoutIn.DefaultPause;
        }
        else
        {
            controlSettings_controller.PauseBtn = intVal;
        }

        strVal = PlayerPrefs.GetString(PlayerPrefsNames.FlipAxis_Controller);
        if (strVal.Length == 0)
        {
            status = false;
            controlSettings_controller.FlipAxis = gamepadLayoutIn.DefaultLVerti;
        }
        else
        {
            controlSettings_controller.FlipAxis = strVal;
        }
        strVal = PlayerPrefs.GetString(PlayerPrefsNames.SpinAxis_Controller);
        if (strVal.Length == 0)
        {
            status = false;
            controlSettings.SpinAxis_Controller = gamepadLayoutIn.DefaultLHoriz;
        }
        else
        {
            controlSettings.SpinAxis_Controller = strVal;
        }

        return status;
    }

    public static void CheckAndSetDefaults()
    {
        LoadAudioSettings();
        LoadVideoSettings();
        LoadControlSettings();
    }

    private static void SetControlGlobal(KeyCode keyIn, ControlAction actIn, InputType typeIn)
    {
        MessageServer.SendMessage(MessageID.EDIT_RESET, new Message((int)keyIn, (uint)actIn).withInputType(typeIn));
    }

    public static void SetControlsToDefault(InputType typeIn)
    {
        if (typeIn == InputType.KEYBOARD_GENERIC)
        {
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultJump, ControlAction.JUMP, typeIn);
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultCrouch, ControlAction.CROUCH, typeIn);
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultTrickU, ControlAction.UP_GRAB, typeIn);
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultTrickL, ControlAction.LEFT_GRAB, typeIn);
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultTrickR, ControlAction.RIGHT_GRAB, typeIn);
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultTrickD, ControlAction.DOWN_GRAB, typeIn);
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultPause, ControlAction.PAUSE, typeIn);

            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultBack, ControlAction.BACK, typeIn);
            SetControlGlobal(DefaultControls.KeyboardLayout.DefaultConfirm, ControlAction.CONFIRM, typeIn);
        }
        else if (typeIn == InputType.CONTROLLER_GENERIC)
        {
            SetControlGlobal(DefaultControls.XboxLayout.DefaultJump, ControlAction.JUMP, typeIn);
            SetControlGlobal(DefaultControls.XboxLayout.DefaultCrouch, ControlAction.CROUCH, typeIn);
            SetControlGlobal(DefaultControls.XboxLayout.DefaultTrickU, ControlAction.UP_GRAB, typeIn);
            SetControlGlobal(DefaultControls.XboxLayout.DefaultTrickL, ControlAction.LEFT_GRAB, typeIn);
            SetControlGlobal(DefaultControls.XboxLayout.DefaultTrickR, ControlAction.RIGHT_GRAB, typeIn);
            SetControlGlobal(DefaultControls.XboxLayout.DefaultTrickD, ControlAction.DOWN_GRAB, typeIn);
            SetControlGlobal(DefaultControls.XboxLayout.DefaultPause, ControlAction.PAUSE, typeIn);

            SetControlGlobal(DefaultControls.XboxLayout.DefaultBack, ControlAction.BACK, typeIn);
            SetControlGlobal(DefaultControls.XboxLayout.DefaultConfirm, ControlAction.CONFIRM, typeIn);
        }
    }

}
