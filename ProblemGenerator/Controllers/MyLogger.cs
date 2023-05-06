using System;


namespace ProblemGenerator.Controllers
{   
    public class MyLogger
	{
		public List<string> Logs { get; set; }

		public MyLogger()
		{
			List<string> logs = new List<string>();
			Logs = logs;
		}
		public void addLog(string log)
		{
			Logs.Add($"{DateTime.Now.ToString("yyyy.MM.dd HH:mm")}: {log}");
            
        }

		public void writeLogs()
		{
            // string docPath = "C:\\logs"; //update for production! FrostPc
            Directory.CreateDirectory("C:\\Websites\\Usterka\\wwwroot\\logs"); //hardcoded!
            string docPath = "C:\\Websites\\Usterka\\wwwroot\\logs";            //hardcoded! update for production!
            /*
            Directory.CreateDirectory("C:\\Users\\user\\Desktop\\TestowyBuild\\wwwroot\\logs"); 
            string docPath = "C:\\Users\\user\\Desktop\\TestowyBuild\\wwwroot\\logs";
			*/

            //ensure log folder exists and if the filename is not used

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, $"{DateTime.Now.AddDays(-1).Date.ToString("yyyy.MM.dd")}_Log.txt")))
			{
				foreach (string item in Logs)
				{
					outputFile.WriteLine(item);
				}
			}
			//Logs.Clear();
		}

		

        
    }
}
