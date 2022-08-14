using UnityEngine;
using Moonvalk.Animation;
using Moonvalk.Utility;

namespace Moonvalk
{
    /// <summary>
    /// Main game manager behavior.
    /// </summary>
    public class MVGame : MonoBehaviour
    {
        #region Data Fields
        /// <summary>
        /// Singleton instance of MVGame.
        /// </summary>
        protected static MVGame _instance;
        #endregion

        #region Public Getters/Setters
        /// <summary>
        /// Gets the MVGame Instance.
        /// </summary>
        /// <value>Returns the MVGame Singleton.</value>
        public static MVGame Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Unity Events
        /// <summary>
        /// Unity Event - Called when this GameObject is first created.
        /// </summary>
        private void Awake()
        {
            this.initialize();
            this.registerSystems();
        }

        /// <summary>
        /// Unity Event - Called each game tick.
        /// </summary>
        private void Update()
        {
            Global.Systems.Update(Time.deltaTime);
        }
        #endregion

        /// <summary>
        /// Initializes this Component as a singleton which shall persist through Scenes.
        /// </summary>
        protected void initialize()
        {
            if (!_instance)
            {
                _instance = this;
                DontDestroyOnLoad(this);
                return;
            }
            Destroy(this);
        }

        /// <summary>
        /// Registers all MVSystems that will be executed by this manager per game tick.
        /// </summary>
        protected void registerSystems()
        {
            new MVTweenSystem();
            new MVTimerSystem();
        }
    }
}
