    /// <summary>
    /// Parses the version number of the calling assembly and provides properties.
    /// </summary>
    public class ConnectVersion
    {
        private readonly System.Version _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectVersion"/> class.
        /// </summary>
        public ConnectVersion()
        {
            AssemblyName assemblyName = Assembly.GetCallingAssembly().GetName();
            _version = assemblyName.Version;            
        }

        /// <summary>
        /// Gets the full version
        /// </summary>
        public string FullVersion
        {
            get { return "{0}.{1}.{2}.{3}".FormatWith(_version.Major, _version.Minor, _version.Build, _version.Revision); }
        }
    }