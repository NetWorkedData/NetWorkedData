using UnityEngine;

using BasicToolBox;

using System.Threading;

//=====================================================================================================================
namespace NetWorkedData
{
	/// <summary>
	/// NWD game data manager.
	/// The GameObject monobehaviour connexion
	/// Use in game to acces to specific method and create a simulate singleton instance 
	/// The sigleton instance was connect to :
	/// Data Manager
	/// App Configuration
	/// Notification Center
	/// </summary>
	[ExecuteInEditMode] // We use this only in playmode so don't attribut ExecuteInEditMode
	public partial class NWDGameDataManager : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SharedInstanceAsSingleton. Class to create a ShareInstance use as Singleton by All NWDGameDataManager
		/// Declare as private, all NWDGameDataManager instance call this class's shared instance.
		/// So, instances connected to private sharedinstance, if I don't create otehr instance, nobody can : I have a singleton simulation
		/// Of course it's a patch, but it's working!
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[ExecuteInEditMode] // I do that to run this object in edit mode too (on scene)
		//-------------------------------------------------------------------------------------------------------------
		private class SharedInstanceAsSingleton 
		{
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// The instance's counter.
			/// </summary>
			static int InstanceCounter = 0;
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Log the counter of instance of this Class.
			/// </summary>
			static void InstanceCounterLog ()
			{
				BTBDebug.LogVerbose ("(NWDGameDataManager SharedInstanceAsSingleton number of instance : "+InstanceCounter+")");
			}
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// The shared instance as singleton.
			/// </summary>
			static public SharedInstanceAsSingleton kSharedInstaceAsSingleton = new SharedInstanceAsSingleton ();
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// The data manager.
			/// </summary>
			public NWDDataManager DataManager;
			/// <summary>
			/// The app configuration.
			/// </summary>
			public NWDAppConfiguration AppConfiguration;
			/// <summary>
			/// The notification center.
			/// </summary>
			public BTBNotificationManager NotificationCenter;
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// The initialization state.
			/// </summary>
			private bool Initialized = false;
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// The data need update state.
			/// </summary>
			public bool DataWasUpdated;
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Initializes a new instance of the <see cref="NetWorkedData.NWDGameDataManager+SharedInstanceAsSingleton"/> class.
			/// </summary>
			public SharedInstanceAsSingleton ()
			{
				Interlocked.Increment(ref InstanceCounter);
				if (Initialized == false) {
					// memorize the shared instance
					DataManager = NWDDataManager.SharedInstance;
					AppConfiguration = NWDAppConfiguration.SharedInstance;
					NotificationCenter = BTBNotificationManager.SharedInstance;
					// ready to launch database
					DataManager.ConnectToDatabase ();
					// finish init
					Initialized = true;
					//BTBDebug.LogVerbose ("NWDGameDataManager SharedInstanceAsSingleton InitInstance finished ("+InstanceCounter+")");
				}
				InstanceCounterLog ();
			}
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Releases unmanaged resources and performs other cleanup operations before the
			/// <see cref="NetWorkedData.NWDGameDataManager+SharedInstanceAsSingleton"/> is reclaimed by garbage collection.
			/// </summary>
			~SharedInstanceAsSingleton()
			{
				Interlocked.Decrement(ref InstanceCounter);
				//BTBDebug.LogVerbose ("NWDGameDataManager SharedInstanceAsSingleton destroy ... IT NEVER POSSIBLE IN RUNTIME");
				if (NotificationCenter != null) {
					NotificationCenter.RemoveAll ();
					NotificationCenter = null;
				}
				InstanceCounterLog ();
			}
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Determines if Singleton is active.
			/// </summary>
			/// <returns><c>true</c> if is active; otherwise, <c>false</c>.</returns>
			public static bool IsActive ()
			{
				return (kSharedInstaceAsSingleton != null);
			}
			//-------------------------------------------------------------------------------------------------------------
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the data manager.
		/// </summary>
		/// <value>The data manager.</value>
		public NWDDataManager DataManager { get; private set;}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the app configuration.
		/// </summary>
		/// <value>The app configuration.</value>
		public NWDAppConfiguration AppConfiguration { get; private set;}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the notification center.
		/// </summary>
		/// <value>The notification center.</value>
		public BTBNotificationManager NotificationCenter { get; private set;}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDGameDataManager"/> class.
		/// </summary>
		public NWDGameDataManager () {
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="NetWorkedData.NWDGameDataManager"/> is reclaimed by garbage collection.
		/// </summary>
		~NWDGameDataManager()
		{
			//BTBDebug.LogVerbose ("NWDGameDataManager Destroyed");
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set needs the synchronize data to true.
		/// </summary>
		public void NeedSynchronizeData ()
		{
			SharedInstanceAsSingleton.kSharedInstaceAsSingleton.DataWasUpdated = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Player's account reference.
		/// </summary>
		/// <returns>The account reference.</returns>
		public string PlayerAccountReference() {
			return AppConfiguration.SelectedEnvironment().PlayerAccountReference;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
