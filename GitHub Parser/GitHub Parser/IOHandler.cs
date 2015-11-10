using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GitHub_Parser
{
    sealed class IOHandler
    {
        //Singleton
        private static readonly IOHandler m_Instance = new IOHandler();

//-->Constructor

        private IOHandler()
        {

        }

//-->Attributes

        public static IOHandler Instance
        {
            get
            {
                return m_Instance;
            }
        }

//--Methods

        public void exportStats(List<User> p_Users)
        {
            XmlTextWriter l_Writer = new XmlTextWriter("GH_stats.xml", Encoding.UTF8);
            TextWriter l_WriterCsv = new StreamWriter("GH_stats.csv", false, Encoding.UTF8);
            l_Writer.Formatting = Formatting.Indented;
            string l_Delimiter = ";";

            try
            {
                l_Writer.WriteStartDocument();
                l_Writer.WriteComment("GitHub " + p_Users.Count + " user stats.");
                l_Writer.WriteStartElement("Users");
                l_WriterCsv.WriteLine("Id" + l_Delimiter + "Answers" + l_Delimiter + "Questions" + l_Delimiter + "ZScore" + l_Delimiter + "InDegree" + l_Delimiter + "OutDegree" + l_Delimiter + "ZDegree" + l_Delimiter + "ExertiseRank");

                for (int i = 0; i < p_Users.Count; i++)
                {
                    l_Writer.WriteStartElement("User");

                    l_Writer.WriteAttributeString("Id", p_Users[i].Id);
                    l_Writer.WriteAttributeString("Answers", p_Users[i].Answers.ToString());
                    l_Writer.WriteAttributeString("Questions", p_Users[i].Questions.ToString());
                    l_Writer.WriteAttributeString("ZScore", p_Users[i].ZScore.ToString());
                    l_Writer.WriteAttributeString("InDegree", p_Users[i].InDegreeList.Count.ToString());
                    l_Writer.WriteAttributeString("OutDegree", p_Users[i].OutDegreeList.Count.ToString());
                    l_Writer.WriteAttributeString("ZDegree", p_Users[i].ZDegree.ToString());
                    l_Writer.WriteAttributeString("ExpertiseRank", p_Users[i].ExpertiseRank.ToString());

                    l_Writer.WriteEndElement();

                    l_WriterCsv.WriteLine(p_Users[i].Id + l_Delimiter + p_Users[i].Answers.ToString() + l_Delimiter + p_Users[i].Questions.ToString() + l_Delimiter + p_Users[i].ZScore.ToString() + l_Delimiter + p_Users[i].InDegreeList.Count.ToString() + l_Delimiter + p_Users[i].OutDegreeList.Count.ToString() + l_Delimiter + p_Users[i].ZDegree.ToString() + l_Delimiter + p_Users[i].ExpertiseRank.ToString());
                }

                l_Writer.WriteEndElement();//~Users
                l_Writer.WriteEndDocument();
                l_Writer.Flush();
                l_Writer.Close();
                l_WriterCsv.Close();
            }
            catch (Exception e)
            {
                l_Writer.Flush();
                l_Writer.Close();
                l_WriterCsv.Close();
                Console.WriteLine("---> Exception in exportStats(): " + e.Message);
            }
        }
    }
}
