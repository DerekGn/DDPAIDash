using System.Diagnostics.CodeAnalysis;

namespace DDPAIDash.Core.Types
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal enum QueryParameterKeys
    {
        wdr_enable,
        gsensor_mode,
        cycle_record_space,
        speaker_turn,
        default_user,
        ldc_switch,
        anti_fog,
        is_need_update,
        event_after_time,
        event_before_time,
        mic_switch,
        image_quality,
        display_mode,
        osd_switch,
        osd_speedswitch,
        start_sound_switch,
        delay_poweroff_time,
        edog_switch,
        parking_mode_switch,
        timelapse_rec_switch
    }
}
