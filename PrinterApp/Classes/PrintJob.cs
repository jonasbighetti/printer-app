namespace PrinterApp.Classes
{
    public class PrintJob
    {
        private readonly string _jobName;
        private readonly int _numberOfPages;

        public PrintJob(string jobName, int numberOfPages)
        {
            _jobName = jobName;
            _numberOfPages = numberOfPages;
        }

        public string GetJobName()
        {
            return _jobName;
        }

        public int GetNumberOfPages()
        {
            return _numberOfPages;
        }
    }
}
