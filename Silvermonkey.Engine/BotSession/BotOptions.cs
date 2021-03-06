﻿using Furcadia.IO;
using Furcadia.Net.Options;
using MonkeyCore.Logging;
using System.IO;

namespace Engine.BotSession
{
    /// <summary>
    /// Silver Monkey Bot settings
    /// </summary>
    /// <seealso cref="Furcadia.Net.Options.ProxyOptions" />
    public class BotOptions : ProxyOptions
    {
        #region Private Fields

        private bool _AutoConnect;

        private string _BiniFile;

        private string _DreamURL;

        private int _GoMap;

        private IniFile BotIni;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BotOptions"/> class.
        /// </summary>
        public BotOptions()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BotOptions"/> class.
        /// </summary>
        /// <param name="BFile">The *.bini file containing the Silver Monkey settings</param>
        /// <exception cref="FileNotFoundException">Bot settings file not found</exception>
        public BotOptions(string BFile)
        {
            Initialize();

            if (!File.Exists(BFile))
            {
                throw new FileNotFoundException("Silver Monkey settings file not found", BFile);
            }

            string dir = Path.GetDirectoryName(BFile);
            if (string.IsNullOrEmpty(dir))
            {
                BFile = Path.Combine(IO.Paths.SilverMonkeyBotPath, BFile);
            }

            if (File.Exists(IO.Paths.CheckBotFolder(BFile)))
            {
                string p = Path.GetDirectoryName(BFile);
                if (!string.IsNullOrEmpty(p))
                {
                    IO.Paths.SilverMonkeyBotPath = p;
                }

                BotIni.Load(BFile);
            }

            _BiniFile = BFile;
            string s = "";
            short.TryParse(BotIni.GetKeyValue("Main", "LogOption"), out short LogOption);
            bool.TryParse(BotIni.GetKeyValue("Main", "Log"), out bool log);
            string BaseFileName = BotIni.GetKeyValue("Main", "LogNameBase");
            string logPath = BotIni.GetKeyValue("Main", "LogNamePath");

            LogOptions = new LogStreamOptions()
            {
                LogNameBase = BaseFileName ?? "Default",
                LogPath = logPath ?? IO.Paths.SilverMonkeyLogPath,
                LogOption = LogOption,
                Enabled = log
            };

            s = BotIni.GetKeyValue("Bot", "BotIni");
            if (!string.IsNullOrEmpty(s))
            {
                CharacterIniFile = s;
            }

            s = BotIni.GetKeyValue("Bot", "LPort");
            if (!string.IsNullOrEmpty(s))
            {
                LocalhostPort = int.Parse(s);
            }

            s = BotIni.GetKeyValue("Bot", "MS_File");
            if (!string.IsNullOrEmpty(s))
            {
                MonkeySpeakEngineOptions.MonkeySpeakScriptFile = s;
            }

            s = BotIni.GetKeyValue("Bot", "MSEngineEnable");
            if (!string.IsNullOrEmpty(s))
            {
                MonkeySpeakEngineOptions.IsEnabled = bool.Parse(s);
            }

            s = BotIni.GetKeyValue("Bot", "BotController");
            if (!string.IsNullOrEmpty(s))
            {
                MonkeySpeakEngineOptions.BotController = s;
            }

            s = BotIni.GetKeyValue("Bot", "StandAlone");
            if (!string.IsNullOrEmpty(s))
            {
                Standalone = bool.Parse(s);
            }

            s = BotIni.GetKeyValue("Bot", "AutoConnect");
            if (!string.IsNullOrEmpty(s))
            {
                _AutoConnect = bool.Parse(s);
            }

            s = BotIni.GetKeyValue("Bot", "NoEndurance");
            s = BotIni.GetKeyValue("Bot", "ConnectTimeOut");
            if (!string.IsNullOrEmpty(s))
            {
                ConnectionTimeOut = int.Parse(s);
            }

            s = BotIni.GetKeyValue("Bot", "ConnectionRetries");
            if (!string.IsNullOrEmpty(s))
            {
                ConnectionRetries = int.Parse(s);
            }

            s = BotIni.GetKeyValue("GoMap", "IDX");
            if (!string.IsNullOrEmpty(s))
            {
                _GoMap = int.Parse(s);
            }

            s = BotIni.GetKeyValue("GoMap", "DreamURL");
            if (!string.IsNullOrEmpty(s))
            {
                _DreamURL = s;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to connect
        /// to the server when the Bini file is loaded.
        /// </summary>
        /// <value><c>true</c> if [automatic connect]; otherwise, <c>false</c>.</value>
        public bool AutoConnect
        {
            get => _AutoConnect;
            set => _AutoConnect = value;
        }

        /// <summary>
        /// Bot File (*.bini) path
        /// </summary>
        /// <returns></returns>
        public string BotPath => Path.GetDirectoryName(_BiniFile);

        /// <summary>
        /// Gets the bot settings file.
        /// </summary>
        /// <value>The bot settings file.</value>
        public string BotSettingsFile
        {
            get => _BiniFile;
            set => _BiniFile = value;
        }

        /// <summary>
        /// Dream Url to send the bot to when connected
        /// </summary>
        /// <value>
        /// The dream link.
        /// </value>
        public string DreamLink
        {
            get => _DreamURL;
            set => _DreamURL = value;
        }

        /// <summary>
        /// Gets or sets the index of the default map at logon.
        /// </summary>
        /// <value>The index of the go map.</value>
        public int GoMapIDX
        {
            get => _GoMap;
            set => _GoMap = value;
        }

        /// <summary>
        /// Gets or sets the log options.
        /// </summary>
        /// <value>
        /// The log options.
        /// </value>
        public LogStreamOptions LogOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the monkey speak engine options.
        /// </summary>
        /// <value>
        /// The monkey speak engine options.
        /// </value>
        public EngineOptoons MonkeySpeakEngineOptions { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => Path.GetFileName(_BiniFile);

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Saves the bot settings.
        /// </summary>
        public void SaveBotSettings()
        {
            if (File.Exists(IO.Paths.CheckBotFolder(_BiniFile)))
                BotIni.Load(_BiniFile);

            BotIni.SetKeyValue("Main", "Log", LogOptions.Enabled.ToString());
            BotIni.SetKeyValue("Main", "LogNameBase", LogOptions.LogNameBase);
            BotIni.SetKeyValue("Main", "LogOption", LogOptions.LogOption.ToString());
            BotIni.SetKeyValue("Main", "LogNamePath", LogOptions.LogPath);
            BotIni.SetKeyValue("Bot", "BotIni", CharacterIniFile);
            BotIni.SetKeyValue("Bot", "MS_File", MonkeySpeakEngineOptions.MonkeySpeakScriptFile);
            BotIni.SetKeyValue("Bot", "LPort", LocalhostPort.ToString());
            BotIni.SetKeyValue("Bot", "MSEngineEnable", MonkeySpeakEngineOptions.IsEnabled.ToString());
            BotIni.SetKeyValue("Bot", "BotController", MonkeySpeakEngineOptions.BotController);
            BotIni.SetKeyValue("Bot", "StandAlone", Standalone.ToString());
            BotIni.SetKeyValue("Bot", "AutoConnect", _AutoConnect.ToString());
            BotIni.SetKeyValue("Bot", "ConnectionRetries", ConnectionRetries.ToString());
            BotIni.SetKeyValue("Bot", "ConnectTimeOut", ConnectionTimeOut.ToString());
            BotIni.SetKeyValue("GoMap", "IDX", _GoMap.ToString());
            BotIni.SetKeyValue("GoMap", "DreamURL", _DreamURL);
            BotIni.Save(IO.Paths.CheckBotFolder(_BiniFile));
        }

        #endregion Public Methods

        #region Private Methods

        private void Initialize()
        {
            BotIni = new IniFile();
            MonkeySpeakEngineOptions = new EngineOptoons();
            LogOptions = new LogStreamOptions();
        }

        #endregion Private Methods
    }
}