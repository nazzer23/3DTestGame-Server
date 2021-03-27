using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Game.Handlers;

namespace Game
{
    public class Core
    {

        private static Core _instance = null;

        public static Core GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Core();
            }
            return _instance;
        }

        public void Init()
        {
            if (LogHandler.GetInstance() != null)
            {
                LogHandler.GetInstance().Log("LogHandler has initialized", LogType.SUCCESS);
            }
            else
            {
                LogHandler.GetInstance().Log("There was an error whilst initializing the LogHandler.", LogType.ERROR);
            }

            if (ConfigHandler.GetInstance().Initialize())
            {
                LogHandler.GetInstance().Log("ConfigHandler has initialized", LogType.SUCCESS);
            }
            else
            {
                LogHandler.GetInstance().Log("There was an error whilst initializing the ConfigHandler.", LogType.ERROR);
            }
        }

        public void Shutdown()
        {
            // Shutdown procedures happen here - tcp close, database end, etc
        }

    }
}