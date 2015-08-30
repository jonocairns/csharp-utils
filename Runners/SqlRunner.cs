public class SqlRunner : ISqlRunner
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlRunner"/> class.
        /// </summary>
        public SqlRunner(string connectionString)
        {
            Argument.CheckIfNull(connectionString, "connectionString");
            _connectionString = connectionString;
        }

        /// <summary>
        /// Performs the db operation in a transaction.
        /// </summary>
        public void Execute(Action<dynamic> dbAction)
        {
            try
            {
                dynamic db = Database.OpenConnection(_connectionString);
                using (dynamic transaction = db.BeginTransaction())
                {
                    dbAction(transaction);
                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                throw HandleAdoException(exception);
            }
        }

        /// <summary>
        /// Performs the db operation.
        /// </summary>
        public void ExecuteNonTransacted(Action<dynamic> dbAction)
        {
            try
            {
                dynamic db = Database.OpenConnection(_connectionString);
                dbAction.Invoke(db);
            }
            catch (Exception exception)
            {
                throw HandleAdoException(exception);
            }
        }

        /// <summary>
        /// Executes the versioned save.
        /// </summary>
        public SimpleResultSet ExecuteVersionedSave(Func<dynamic, SimpleResultSet> dbAction, string outOfDateError)
        {
            SimpleResultSet result = Execute(dbAction);

            if (result.SingleOrDefault() == null)
            {
                throw new ConcurrencyException(outOfDateError);
            }

            return result;
        }

        /// <summary>
        /// Executes the versioned save.
        /// </summary>
        public SimpleResultSet ExecuteNonTransactedVersionedSave(Func<dynamic, SimpleResultSet> dbAction, string outOfDateError)
        {
            SimpleResultSet result = ExecuteNonTransacted(dbAction);

            if (result.SingleOrDefault() == null)
            {
                throw new ConcurrencyException(outOfDateError);
            }

            return result;
        }

        /// <summary>
        /// Performs the db operation.
        /// </summary>
        public TResult ExecuteNonTransacted<TResult>(Func<dynamic, TResult> dbAction)
        {
            try
            {
                dynamic db = Database.OpenConnection(_connectionString);
                return dbAction.Invoke(db);
            }
            catch (Exception exception)
            {
                throw HandleAdoException(exception);
            }
        }

        /// <summary>
        /// Performs the db operation in a transaction.
        /// </summary>
        public TResult Execute<TResult>(Func<dynamic, TResult> dbAction)
        {
            try
            {
                dynamic db = Database.OpenConnection(_connectionString);
                using (dynamic transaction = db.BeginTransaction())
                {
                    dynamic result = dbAction(transaction);
                    transaction.Commit();
                    return result;
                }
            }
            catch (Exception exception)
            {
                throw HandleAdoException(exception);
            }
        }

        private static Exception HandleAdoException(Exception exception)
        {
            throw new DatabaseException("There was an error performing the requested operation", exception);
        }
    }