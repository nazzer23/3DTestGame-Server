using System;
using System.Collections.Generic;
using System.IO;

namespace Game.Handlers
{
    public class ConfigHandler
    {
        private static ConfigHandler _instance;

        private readonly Dictionary<string, object> _configValues = new Dictionary<string,object>();

        public static ConfigHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConfigHandler();
            }

            return _instance;
        }

        public bool Initialize()
        {
            try
            {
                if (File.Exists("config.cfg"))
                {
                    StreamReader streamReader = new StreamReader("config.cfg");
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        if (line != null && !line.StartsWith("#") && line.Contains("="))
                        {
                            string[] temp = line.Split('=');
                            this._configValues.Add(temp[0].ToLower(), temp[1]);
                        }
                    }
                    streamReader.Close();
                    LogHandler.GetInstance().Log("ConfigHandler initialized with " + this._configValues.Keys.Count + " Keys.", LogType.DEBUG);
                    return true;
                }
                else
                {
                    LogHandler.GetInstance().Log("The config.cfg file was not found.", LogType.ERROR);
                    throw new FileNotFoundException();
                }
            }
            catch (Exception ex)
            {
                LogHandler.GetInstance().Log(ex.Message, LogType.ERROR);
                return false;
            }
        }
        
    }
}