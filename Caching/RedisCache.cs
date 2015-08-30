public class RedisCache : ICache, IDisposable
    {
        private readonly ConfigurationOptions _configurationOptions;
        private IDatabase _cache;
        private ConnectionMultiplexer _connectionMultiplexer;
        private static object _lock = new object();

        public RedisCache(ConfigurationOptions configurationOptions)
        {
            Argument.CheckIfNull(configurationOptions, "configurationOptions");
            _configurationOptions = configurationOptions;
        }

        private IDatabase Cache
        {
            get
            {
                if (_cache == null)
                {
                    Init();
                }

                return _cache;
            }
        }

        private void Init()
        {
            if (_connectionMultiplexer == null)
            {
                lock (_lock)
                {
                    if (_connectionMultiplexer == null)
                    {
                        _connectionMultiplexer = ConnectionMultiplexer.Connect(_configurationOptions);
                        _cache = _connectionMultiplexer.GetDatabase();
                    }
                }
            }
        }


        public async Task<T> TryGet<T>(string key, Func<Task<T>> ifCacheMissAction, CacheOptions cacheOption)
        {
            Argument.CheckIfNull(ifCacheMissAction, "ifCacheMissAction");
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            RedisValue redisValue = await Cache.StringGetAsync(key);
                // the following is a bit yolo
            if (redisValue.HasValue && redisValue != "{}")
            {
                var redisVal = default(T);
                JsonExceptionWrapper(() => redisVal = JsonConvert.DeserializeObject<T>(redisValue), redisValue, key);
                return redisVal;
            }
            T item = await ifCacheMissAction();
            SetValue(key, item, cacheOption.Expiry);
            return item;
        }

        public void ClearItem(string key)
        {
            Cache.KeyDelete(key);
        }

        public void ClearCache()
        {
            Init();
            EndPoint[] endPoints = _connectionMultiplexer.GetEndPoints();
            foreach (EndPoint point in endPoints)
            {
                IServer server = _connectionMultiplexer.GetServer(point);
                server.FlushDatabase();                
            }
        }

        public async Task<IEnumerable<T>> TryGetList<T>(string key, Func<Task<IEnumerable<T>>> ifCacheMissAction, CacheOptions cacheOption)
        {
            Argument.CheckIfNull(ifCacheMissAction, "ifCacheMissAction");
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            RedisValue redisValue = await Cache.StringGetAsync(key);
            // the following is a bit yolo
            if (redisValue.HasValue && redisValue != "{}")
            {
                var list = Enumerable.Empty<T>();
                JsonExceptionWrapper(() => list = JsonConvert.DeserializeObject<IEnumerable<T>>(redisValue), redisValue, key);
                return list;
            }

            IEnumerable<T> item = await ifCacheMissAction();
            SetValue(key, item, cacheOption.Expiry);
            return item;
        }

        private async void SetValue(string key, object value, DateTime expireOn)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            Argument.CheckIfNull(value, "value");

            string serializeObject = string.Empty;
            JsonExceptionWrapper(() => serializeObject = JsonConvert.SerializeObject(value), value, key);

            await Cache.StringSetAsync(key, serializeObject, new TimeSpan(expireOn.Ticks - DateTime.UtcNow.Ticks), When.NotExists, CommandFlags.FireAndForget);
        }

        private static void JsonExceptionWrapper(Action action, object value, string key)
        {
            try
            {
                action.Invoke();
            }
            catch (ArgumentNullException ex)
            {
                throw new RedisCacheJsonConvertException(
                    "Redis Cache: There was an issue deserializing / serializing the object {0} with the key {1}. This may occur because a property name is not the same name as the private field (eg private '_name' should have the property name 'Name'). Check the inner exception for details"
                        .FormatWith(value.GetType().FullName, key), ex);
            }
            catch (JsonException ex)
            {
                throw new RedisCacheJsonConvertException("Redis Cache: There was an issue deserializing / serializing the object {0} with the key {1}. Check inner exception for more information.".FormatWith(value.GetType().FullName, key), ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connectionMultiplexer != null)
                {
                    _connectionMultiplexer.Close();
                    _connectionMultiplexer.Dispose();
                }

                _cache = null;
            }
        }
    }