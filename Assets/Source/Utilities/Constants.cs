
static class Constants
{
    public const float ZERO_F = 0.0f;
    public const float VECTOR_2_TOLERANCE = 0.1f;
    public const float LERP_DEFAULT = 0.5f;

    public const float DATA_EDIT_TICK_TIME = 0.05f;
    public const float LONG_DATA_EDIT_TICK_TIME = 0.5f;

    public const float PERPENDICULAR_F = 90f;

    public const int ZERO = 0;
    public const int ONE = 1;

    public const string TIME_FORMAT_STRING = "{0:0}:{1:00}:{2:00}";
}

public enum DataTarget
{
    // VideoSettings
    ERROR_TARGET = -1,
    RESOLUTION_IDX,
    SCREEN_MODE,
    RENDER_MULTIPLIER,
    VIDEO_QUALITY,

    AUDIO_ENABLED,
    MASTER_AUDIO_LEVEL,
    MUSIC_LEVEL,
    VOICE_LEVEL,
    SFX_LEVEL,
    ANY_KEY,
}