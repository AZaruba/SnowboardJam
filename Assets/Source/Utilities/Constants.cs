
static class Constants
{
    public const float ZERO_F = 0.0f;
    public const float VECTOR_2_TOLERANCE = 0.1f;
    public const float LERP_DEFAULT = 0.5f;

    public const float PERPENDICULAR_F = 90f;

    public const int ZERO = 0;
}

public enum DataTarget
{
    // VideoSettings
    ERROR_TARGET = -1,
    VERT_RESOLUTION,
    ASPECT_RATIO,
    RENDER_MULTIPLIER,
    VIDEO_QUALITY,

    AUDIO_ENABLED,
    MASTER_AUDIO_LEVEL,
    MUSIC_LEVEL,
    VOICE_LEVEL,
    SFX_LEVEL,
}