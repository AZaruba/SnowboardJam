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

/// <summary>
/// This struct encompasses all video settings, including graphics quality and resolution
/// </summary>
public struct VideoSettings
{
    public int VerticalResolution; // eliminate
    public int HorizontalResolution; // eliminate

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

    public static string AudioEn = "AudioEnabled";

    public static string MasterAudioL = "MasterAudio";
    public static string MusicL = "MusicLevel";
    public static string VoiceL = "VoiceLevel";
    public static string SfxL = "SfxLevel";
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

    public static void CheckAndSetDefaults()
    {
        LoadAudioSettings();
        LoadVideoSettings();
    }
}
