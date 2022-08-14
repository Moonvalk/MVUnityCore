using Moonvalk.Systems;

namespace Moonvalk
{
    /// <summary>
    /// Global class for accessing Systems and game engine components.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Global System manager for MVSystems.
        /// </summary>
        public static MVSystemManager Systems = new MVSystemManager();

        /// <summary>
        /// Gets a specific MVSystem found within the Global MVSystemManager.
        /// </summary>
        /// <typeparam name="T">The type of System being searched for.</typeparam>
        /// <returns>Returns the MVSystem matching type T searched for, if found.</returns>
        public static IMVSystem GetSystem<T>()
        {
            return Global.Systems.Get<T>();
        }

        /// <summary>
        /// Gets the MVGame component.
        /// </summary>
        /// <returns>The singleton instance of MVGame which assists in managing the game.</returns>
        public static MVGame GetGameManager()
        {
            return MVGame.Instance;
        }
    }
}
