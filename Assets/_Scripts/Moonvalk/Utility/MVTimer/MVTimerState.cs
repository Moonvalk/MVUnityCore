
namespace Moonvalk.Utility
{
    /// <summary>
    /// Provides an index for each available MVTimer state.
    /// </summary>
    public enum MVTimerState
    {
        /// <summary> This MVTimer is idle and waiting for instruction. </summary>
        Idle,
        /// <summary> This MVTimer has just begun. </summary>
        Start,
        /// <summary> This MVTimer is actively updating. </summary>
        Update,
        /// <summary> This MVTimer has just completed. </summary>
        Complete,
        /// <summary> This MVTimer is idle and is now considered stopped. </summary>
        Stopped,
    }
}
