using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace EstimationFileCreator {
	class CreateEstFiles {
		static void Main() {

			// this is assuming that your configuration_est.xml file starts set like this, 
			// so that these text strings are replaced
			//EstimationModel="AutoOwnershipModel"
			//OutputAlogitDataPath="AutoOwnershipModel_psrcper1.dat"
			//OutputAlogitControlPath="AutoOwnershipModel_psrcper1.alo"

			string exe_loc = "C:\\M4\\soundcat\\soundcat\\daysim\\Daysim.exe";
			string configuration = "C:\\M4\\soundcat\\soundcat\\configuration_M4.xml";
			
			// configuration_temp is just a copy of configuration for the next model to be estimated
			string configuration_temp = "C:\\person_based_est\\soundcast\\configuration_M4_temp.xml";
			string lettersToAppend = "_psrcper1";

			List<string> modelNames = new List<string>();
		   modelNames.Add("AutoOwnershipModel");
			// modelNames.Add("WorkLocationModel");
			// modelNames.Add("SchoolLocationModel");
			modelNames.Add("PayToParkAtWorkplaceModel");
			modelNames.Add("TransitPassOwnershipModel");
			modelNames.Add("IndividualPersonDayPatternModel");
		   modelNames.Add("PersonExactNumberOfToursModel");
			modelNames.Add("WorkTourDestinationModel");
			modelNames.Add("OtherTourDestinationModel");
			modelNames.Add("WorkBasedSubtourGenerationModel");
			modelNames.Add("WorkTourModeModel");
			modelNames.Add("SchoolTourModeModel");
			modelNames.Add("EscortTourModeModel");
			modelNames.Add("WorkBasedSubtourModeModel");
			modelNames.Add("OtherHomeBasedTourModeModel");
			modelNames.Add("WorkTourTimeModel");
			modelNames.Add("SchoolTourTimeModel");
			modelNames.Add("OtherHomeBasedTourTimeModel");
			modelNames.Add("WorkBasedSubtourTimeModel");
			modelNames.Add("IntermediateStopGenerationModel");
			modelNames.Add("IntermediateStopLocationModel");
			modelNames.Add("TripModeModel");
			modelNames.Add("TripTimeModel");

			string previous_name = "WorkBasedSubtourGenerationModel";

			foreach (string model in modelNames) {

				NameReplacer NReplace = new NameReplacer(model, previous_name, lettersToAppend, configuration, configuration_temp);
				NReplace.ReplaceFile();
				Console.WriteLine("Writing dat and alo files for {0}", model);
				Process daysim = new Process();
				daysim.StartInfo.FileName = exe_loc;
				daysim.StartInfo.Arguments = "-c" + configuration;
				daysim.Start();
				daysim.WaitForExit();
				previous_name = model;

			}


		}

		public class NameReplacer {

			public string modelName;
			public StreamReader reader;
			public StreamWriter writer;
			public string configuration;
			public string configuration_temp;
			public string previous_name;
			public string lettersToAppend;
			private string estModelToReplace;
			private string datModelToReplace;
			private string aloModelToReplace;
			private string estModel;
			private string datModel;
			private string aloModel;
			string line = "";

			private Dictionary<string, string> replacements = new Dictionary<string, string>();

			public NameReplacer(string modelName, string previous_name, string lettersToAppend, string configuration, string configuration_temp) {

				this.configuration = configuration;
				this.configuration_temp = configuration_temp;

				reader = new StreamReader(File.OpenRead(configuration));
				writer = new StreamWriter(configuration_temp);
				estModelToReplace = "EstimationModel=\"" + previous_name + "\"";
				datModelToReplace = "OutputAlogitDataPath=\"" + previous_name + lettersToAppend + ".dat\"";
				aloModelToReplace = "OutputAlogitControlPath=\"" + previous_name + lettersToAppend + ".alo\"";

				estModel = "EstimationModel=\"" + modelName + "\"";
				datModel = "OutputAlogitDataPath=\"" + modelName + lettersToAppend + ".dat\"";
				aloModel = "OutputAlogitControlPath=\"" + modelName + lettersToAppend + ".alo\"";
				replacements.Add(estModelToReplace, estModel);
				replacements.Add(datModelToReplace, datModel);
				replacements.Add(aloModelToReplace, aloModel);

			}


			public void ReplaceFile() {
				bool replacementMade;
				line = reader.ReadLine();
				while (!reader.EndOfStream) {
					replacementMade = false;
					foreach (var replacement in replacements) {
						if (line.StartsWith(replacement.Key)) {
							writer.WriteLine(replacement.Value);
							replacementMade = true;
							break;
						}
					}
					if (!replacementMade) {
						writer.WriteLine(line);
					}
					writer.Flush();
					line = reader.ReadLine();
				}


				writer.WriteLine(line);
				writer.Flush();

				reader.Close();
				writer.Close();
				File.Delete(configuration);
				File.Copy(configuration_temp, configuration);
			}
		}





	}
}







