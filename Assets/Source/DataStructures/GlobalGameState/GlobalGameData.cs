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

    public Resolution ScreenRes;
    public int RenderMultiplier;
    public FullScreenMode ScreenMode;

    public VideoQuality Quality;
}

public enum VideoQuality
{
    ERROR_QUALITY = -1,
    LOWEST,
    LOW,
    MEDIUM,
    HIGH,
    HIGHEST,
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
    public static string VertRes = "VerticalResolution";
    public static string HoriRes = "HorizontalResolution";
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
            case DataTarget.VERT_RESOLUTION:
                videoSettings.VerticalResolution = value;
                Screen.SetResolution(videoSettings.VerticalResolution, videoSettings.HorizontalResolution, videoSettings.ScreenMode);
                SaveVideoSettings();
                break;
            case DataTarget.RENDER_MULTIPLIER:
                videoSettings.RenderMultiplier = value;
                SaveVideoSettings();
                break;
            case DataTarget.VIDEO_QUALITY:
                videoSettings.Quality = (VideoQuality)value;
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
        Debug.Log("data updated int");
        return true;
    }
    public static bool SetSettingsValue(DataTarget target, float value)
    {
        switch (target)
        {
            // no floats currently
        }
        Debug.Log("data updated float");
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
        Debug.Log("data updated bool");
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
        PlayerPrefs.SetInt(PlayerPrefsNames.VertRes, videoSettings.ScreenRes.height);
        PlayerPrefs.SetInt(PlayerPrefsNames.HoriRes, videoSettings.ScreenRes.width);
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

        Resolution resOut = new Resolution();
        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.VertRes);
        if (intVal == default)
        {
            status = false;
        }
       resOut.height = intVal;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.HoriRes);
        if (intVal == default)
        {
            status = false;
        }
        resOut.width = intVal;
        videoSettings.ScreenRes = resOut;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.RenderMult);
        if (intVal == default)
        {
            status = false;
        }
        videoSettings.RenderMultiplier = intVal;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.VQuality);
        if (intVal == default)
        {
            status = false;
        }
        videoSettings.Quality = (VideoQuality)intVal;

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
        }
        audioSettings.AudioEnabled = intVal == 1 ? true : false;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.MasterAudioL);
        if (intVal == default)
        {
            status = false;
        }
        audioSettings.MasterAudioLevel = intVal;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.MusicL);
        if (intVal == default)
        {
            status = false;
        }
        audioSettings.MusicLevel = intVal;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.VoiceL);
        if (intVal == default)
        {
            status = false;
        }
        audioSettings.VoiceLevel = intVal;

        intVal = PlayerPrefs.GetInt(PlayerPrefsNames.SfxL);
        if (intVal == default)
        {
            status = false;
        }
        audioSettings.SfxLevel = intVal;

        return status;
    }

    public static void CheckAndSetDefaults()
    {
        LoadAudioSettings();
        LoadVideoSettings();
    }
}
