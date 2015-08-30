    /// <summary>
    /// Responsible for loading settings from the Web.Config file
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Loads a setting with the specified key.
        /// </summary>
        public static Setting Load(string key)
        {
            try
            {
                return new Setting(key, ConfigurationManager.AppSettings[key]);
            }
            catch (Exception error)
            {
                throw new ConfigurationErrorsException("App Setting with name {0} was not defined".FormatWith(key), error);
            }
        }
    }