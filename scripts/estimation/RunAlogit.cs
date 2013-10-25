using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace RunAlogit {
	class AlogitRunner {
		static void Main(string[] args) {
			string exe_loc = "C:\\Program Files (x86)\\HCG Software\\ALOGIT 4\\Alo4nec.exe";
			string directory = "C:\\M4\\soundcast\\outputs";
		   string fileAppend = "_psrcper1";
		


			List<string> modelNames = new List<string>();
			modelNames.Add("AutoOwnershipModel"+fileAppend);
			modelNames.Add("WorkLocationModel");
			modelNames.Add("SchoolLocationModel");
			modelNames.Add("TransitPassOwnershipModel");
		   modelNames.Add("IndividualPersonDayPatternModel"+fileAppend);
			modelNames.Add("PersonExactNumberOfToursModel"+fileAppend);
			modelNames.Add("WorkTourDestinationModel"+fileAppend);
			modelNames.Add("OtherTourDestinationModel"+fileAppend);
			modelNames.Add("WorkBasedSubtourGenerationModel"+fileAppend);
			modelNames.Add("WorkTourModeModel"+fileAppend);
			modelNames.Add("SchoolTourModeModel"+fileAppend);
			modelNames.Add("EscortTourModeModel"+fileAppend);
			modelNames.Add("WorkBasedSubtourModeModel"+fileAppend);
			modelNames.Add("OtherHomeBasedTourModeModel"+fileAppend);
			modelNames.Add("WorkTourTimeModel"+fileAppend);
			modelNames.Add("SchoolTourTimeModel"+fileAppend);
			modelNames.Add("OtherHomeBasedTourTimeModel"+fileAppend);
			modelNames.Add("WorkBasedSubtourTimeModel"+fileAppend);
			modelNames.Add("IntermediateStopGenerationModel"+fileAppend);
			modelNames.Add("IntermediateStopLocationModel"+fileAppend);
			modelNames.Add("TripModeModel"+fileAppend);
			modelNames.Add("TripTimeModel"+fileAppend);

			foreach (string model in modelNames) {
				Console.WriteLine("Running alogit for {0}", model);
				if(File.Exists(directory+"\\"+model+".F12"))
				{
					File.Delete(directory+"\\"+model+".F12");
				}
				Process alogit = new Process();
				alogit.StartInfo.FileName = exe_loc;
				alogit.StartInfo.WorkingDirectory=directory;
				alogit.StartInfo.Arguments = model+".alo";
				alogit.Start();
				alogit.WaitForExit();
			}


		}
	}
}
