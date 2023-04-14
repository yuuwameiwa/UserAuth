using DataHelper;

namespace UserAuth.Services
{
    /// <summary>
    /// This is a base service class. Inherit from this class if your service uses Databases (SQL, Redis).
    /// </summary>
    public abstract class BaseDatabaseService
    {
        public DataContainer _dataContainer { get; private set; }

        public BaseDatabaseService(DataContainer dataContainer)
        {
            _dataContainer = dataContainer;
        }
    }
}