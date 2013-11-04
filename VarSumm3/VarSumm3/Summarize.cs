using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace SummarizeVars{

	class VariableSummarizer {

		static void Main() {
			//NOTE THIS FILE LOC!!!!!!!!!!!!!!!!!
			StreamReader reader = new StreamReader(File.OpenRead("C:\\ABM_repo\\Daysim\\ChoiceModels\\H\\HTransitPassOwnershipModel.cs"));
			string fileContent = reader.ReadToEnd();

			int[] indices_alts = StringExtender.IndexesOf(fileContent, "alternative.Choice");
			int[] indices_vars = StringExtender.IndexesOf(fileContent, "AddUtilityTerm(");


			// an array of dictionaries holding the variable names, ids specific to each alternative
			Dictionary<string, string>[] vars_choice = GetUtils(fileContent, indices_vars, indices_alts);

			using (StreamWriter writer = new StreamWriter("C:\\soundcat\\soundcat\\outputs\\HTransitPassOwnershipModel.csv")) {

				for (int a = 0; a < indices_alts.Length; a++){

					if (vars_choice[a] != null) {
						foreach (KeyValuePair<string, string> variable in vars_choice[a]) {

							if (variable.Value != "-1" && variable.Value != "0") {

								writer.WriteLine(a.ToString() + ',' + variable.Key + ',' + variable.Value);

							}

						}
					}
			}
			}
		}


	


		public static Dictionary<string, string>[] GetUtils(string fileContent, int[] indices_vars, int[] indices_alts)
        {
            int const_ctr = 1;
            Dictionary<string, string>[] vars_choice = new Dictionary<string, string>[indices_alts.Length];

            for (int a = 0; a < indices_alts.Length; a++)
            {
                int ind_a = indices_alts[a];
                int ind_a1 =100000;

					if( a < indices_alts.Length -1){
						 ind_a1 = indices_alts[a + 1];}

                Dictionary<string, string> names_nums = new Dictionary<string, string>();

                foreach (int i in indices_vars)
                {
							
                    if (i > ind_a && i < ind_a1)
                    //the variable belongs to the alternative
                    {
                        string num = "-1";
                        string name = "none";

                        //skipping 15 characters forward for AddUtilityTerm(

                        int begin_index = i + 15;
                        int end_num = begin_index;
                        int end_name;

                        while (fileContent[end_num - 1] != ',')
                        {
                            if (fileContent[end_num] == ',')
                            {
                                num = Convert.ToString(fileContent.Substring(begin_index, end_num - begin_index));
                            }

                            end_num++;

                            end_name = end_num;

                            while (fileContent[end_name - 1] != ';')
                            {

                                if (fileContent[end_name] == ';')
                                {
                                    name = Convert.ToString(fileContent.Substring(end_num, end_name - end_num - 1));
                                }

                                end_name++;

                            }

                            if (name.Length < 3)
                            {

                                name = "constant_" + const_ctr.ToString();
                                const_ctr++;

                            }

									if(num != "-1" && !names_nums.ContainsKey(name))
									{
                            names_nums.Add(name, num);
									}
			
                            

                        }
							  
                    }
                    }

					 vars_choice[a] = names_nums;

                }
                return vars_choice;

            }

	}

	public static class StringExtender {

		public static int[] IndexesOf(this string str, string sub) {

			int[] result = new int[0];



			for (int i = 0; i < str.Length; ++i) {

				if (i + sub.Length > str.Length)

					break;

				if (str.Substring(i, sub.Length).Equals(sub)) {

					Array.Resize(ref result, result.Length + 1);

					result[result.Length - 1] = i;

				}

			}

			return result;

		}


	}


}