using Brisebois.WindowsAzure.TableStorage;

namespace Worker.Demo.Logging
{
    public class TableStorageLogger : ILogger
    {
        public void Write(string entry)
        {
            Logger.Add("demo","log",entry);
            Logger.Persist(true);
        }
    }
}