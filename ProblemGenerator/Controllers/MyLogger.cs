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
            // string docPath = "C:\\logs"; //update for production! FrostPc
            //Directory.CreateDirectory("C:\\Websites\\Usterka\\wwwroot\\logs"); 
            //string docPath = "C:\\Websites\\Usterka\\wwwroot\\logs";
            string docPath = _config.GetValue<string>("LogStorage");
            Directory.CreateDirectory(docPath);


            /*
            Directory.CreateDirectory("C:\\Users\\user\\Desktop\\TestowyBuild\\wwwroot\\logs"); 
            string docPath = "C:\\Users\\user\\Desktop\\TestowyBuild\\wwwroot\\logs";

			string path = Path.Combine(config.GetValue<string>("FileStorage")!, $"{DateTime.Today.ToString("d")}", newFileName);
                string relativePath = Path.Combine($"{DateTime.Today.ToString("d")}", newFileName);
                Directory.CreateDirectory(Path.Combine(config.GetValue<string>("FileStorage")!, $"{DateTime.Today.ToString("d")}"));

                await using FileStream fs = new(path, FileMode.Create);
                await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

                string imgPathPart = Path.Combine(config.GetValue<string>("WebStorageRoot")!, relativePath);
                

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
