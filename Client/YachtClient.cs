using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Handlers;
using Client.Helpers;

namespace Client
{
    /// <summary>
    /// Extend your Client class with BaseClient class and add your modules into the constructor.
    /// </summary>
    public abstract class YachtClient : BaseScript
    {
        /// <summary>
        /// Extend your Client class with BaseClient class and add your modules into the constructor.
        /// </summary>
        public YachtClient()
        {
            ResourceName = API.GetCurrentResourceName();

            Debug.WriteLine("============================================================");
            Log($"Initializing...");

            Tick += initialize;
            Tick += garbageCollect;
            Log($"Initialized!");
            Debug.WriteLine("============================================================");
        }

        #region VARIABLES

        /// <summary>
        /// This current client resource name
        /// </summary>
        public static string ResourceName { get; private set; }

        /// <summary>
        /// This current client resource name
        /// </summary>
        public string Name { get { return ResourceName; } }

        /// <summary>
        /// **Experimental**
        /// Cleanup the garbage once a minutes to keep memory usage low.
        /// </summary>
        protected bool UseGarbageCollector { get; set; } = false;

        /// <summary>
        /// **Experimental**
        /// Enabled the process that watches for players who die and also resurect.
        /// Enable if you need to use OnPlayerDied and OnPlayerResurect base events in your modules.
        /// </summary>
        protected bool UsePlayerDeadResurectWatcher
        {
            get
            {
                return _usePlayerDeadResurectWatcher;
            }
            set
            {
                if (value == false)
                {
                    deadPlayers.Clear();
                }
                _usePlayerDeadResurectWatcher = value;
            }
        }
        private bool _usePlayerDeadResurectWatcher = false;
        private readonly List<Player> deadPlayers;

        #endregion
        #region BASE EVENTS

        private async Task initialize()
        {
            Tick -= initialize; 

            await Delay(0);

            Log($"Initialized!");
            TriggerServerEvent($"internal:{Name}:onPlayerClientInitialized");
        }
          

        private async Task garbageCollect()
        {
            if (!UseGarbageCollector)
            {
                await Delay(1);
                return;
            }

            await Delay(60000);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
           
        /// <summary>
        /// Get the player list.
        /// </summary>
        /// <returns>Return a PlayerList object.</returns>
        public PlayerList GetPlayers()
        {
            return Players;
        }

        /// <summary>
        /// Add an event.
        /// </summary>
        /// <param name="eventName">Event name to add.</param>
        /// <param name="action">Event delegate to add.</param>
        protected void AddEvent(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
            Log($"Event {eventName} added!");
        }

        /// <summary>
        /// Remove an event.
        /// </summary>
        /// <param name="eventName">Event name to remove.</param>
        /// <param name="action">Event delegate to remove.</param>
        protected void RemoveEvent(string eventName, Delegate action)
        {
            EventHandlers[eventName] -= action;
            Log($"Event {eventName} removed!");
        }

        /// <summary>
        /// Add a tick.
        /// </summary>
        /// <param name="action">Tick delegate to add.</param>
        protected void AddTick(Func<Task> action)
        {
            Tick += action;
            Log($"Added Tick {action.Method.Name}!");
        }

        /// <summary>
        /// Remove a tick.
        /// </summary>
        /// <param name="action">Tick delegate to remove.</param>
        protected void RemoveTick(Func<Task> action)
        {
            Tick -= action;
            Log($"Removed Tick {action.Method.Name}!");
        }

        /// <summary>
        /// Get the ExportDictionary.
        /// </summary>
        /// <returns>Return the ExportDictionary.</returns>
        protected ExportDictionary GetExports()
        {
            return Exports;
        }

        /// <summary>
        /// Get a resource Export.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns>Return a dynamic export.</returns>
        protected dynamic GetExport(string resourceName)
        {
            return Exports[resourceName];
        }

        /// <summary>
        /// Export a delegate method.
        /// </summary>
        /// <param name="name">Name of the Export.</param>
        /// <param name="method">Delegate of the Export.</param>
        protected void SetExport(string name, Delegate method)
        {
            Exports.Add(name, method);
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [INFO] Lunapark: {message}");
        }
        
        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected static void sLog(string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [INFO] Lunapark: {message}");
        }

        /// <summary>
        /// Log an exception.
        /// </summary>
        /// <param name="exception">The Exception to log.</param>
        /// <param name="prefix">Some text to add before the log message.</param>
        protected void LogError(Exception exception, string prefix = "")
        {
            string pre = (prefix != "") ? prefix : "";
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [ERROR] Lunapark: {exception.Message}\n{exception.StackTrace}");
        }
        
        /// <summary>
        /// Log an exception.
        /// </summary>
        /// <param name="exception">The Exception to log.</param>
        /// <param name="prefix">Some text to add before the log message.</param>
        protected static void sLogError(Exception exception, string prefix = "")
        {
            string pre = (prefix != "") ? prefix : "";
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [ERROR] Lunapark: {exception.Message}\n{exception.StackTrace}");
        }

        #endregion
    }
}
