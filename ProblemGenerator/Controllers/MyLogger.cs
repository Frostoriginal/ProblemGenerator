using System;


namespace ProblemGenerator.Controllers
{   
    public class MyLogger
	{
		public List<string> Logs { get; set; }
		private readonly IConfiguration _config;

		public MyLogger(IConfiguration config)
		{
			List<string> logs = new List<string>();
			Logs = logs;
			_config = config;
		}
		public void addLog(string log)
		{
			Logs.Add($"{DateTime.Now.ToString("yyyy.MM.dd HH:mm")}: {log}");
            
        }

		public void writeLogs()
		{            
            string docPath = _config.GetValue<string>("LogStorage");
            Directory.CreateDirectory(docPath);

            //ensure log folder exists and if the filename is not used

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, $"{DateTime.Now.AddDays(-1).Date.ToString("yyyy.MM.dd")}_Log.txt")))
			{
				foreach (string item in Logs)
				{
					outputFile.WriteLine(item);
				}
			}
			Logs.Clear();
		}

		

        
    }
}
