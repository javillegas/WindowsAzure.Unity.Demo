using Brisebois.WindowsAzure.TableStorage;

namespace Worker.Demo
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