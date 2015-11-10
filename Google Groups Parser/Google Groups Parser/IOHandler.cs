using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Google_Groups_Parser
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

        public void debug(string p_Message)
        {
            Settings l_Settings = Settings.Instance;

            try
            {
                TextWriter l_TwDebug = new StreamWriter(l_Settings.DebugDir + l_Settings.DebugFile, true);
                l_TwDebug.WriteLine(DateTime.Now.ToLocalTime() + " " + p_Message);
                l_TwDebug.Close();
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    debug("Exception in debug(): " + e.Message);
            }
        }

        public void exportUserList(List<User> p_Users)
        {
            Settings l_Settings = Settings.Instance;
            XmlTextWriter l_Writer = new XmlTextWriter(l_Settings.OutputDir + l_Settings.UsersFile, Encoding.UTF8);
            l_Writer.Formatting = Formatting.Indented;

            try
            {
                l_Writer.WriteStartDocument();
                l_Writer.WriteComment("ggplot2 " + p_Users.Count + " users.");
                l_Writer.WriteStartElement("Users");

                for (int i = 0; i < p_Users.Count; i++)
                {
                    l_Writer.WriteStartElement("User");

                    l_Writer.WriteAttributeString("Id", i.ToString());
                    l_Writer.WriteAttributeString("Name", p_Users[i].Name);
                    l_Writer.WriteAttributeString("Avatar", p_Users[i].Avatar);
                    if(p_Users[i].Email.Length > 0)
                        l_Writer.WriteAttributeString("Email", p_Users[i].Email);
                    l_Writer.WriteAttributeString("Answers", p_Users[i].Answers.ToString());
                    l_Writer.WriteAttributeString("Questions", p_Users[i].Questions.ToString());
                    l_Writer.WriteAttributeString("ZScore", p_Users[i].ZScore.ToString());

                    l_Writer.WriteEndElement();
                }

                l_Writer.WriteEndElement();//~Users
                l_Writer.WriteEndDocument();
                l_Writer.Flush();
                l_Writer.Close();
            }
            catch (Exception e)
            {
                l_Writer.Flush();
                l_Writer.Close();
                if (l_Settings.DebugMode)
                    debug("Exception in exportUserList(): " + e.Message);
            }
        }

        public void exportStats(List<User> p_Users)
        {
            Settings l_Settings = Settings.Instance;
            XmlTextWriter l_Writer = new XmlTextWriter(l_Settings.OutputDir + l_Settings.StatsFile, Encoding.UTF8);
            l_Writer.Formatting = Formatting.Indented;
            TextWriter l_WriterCsv = new StreamWriter(l_Settings.OutputDir + "GG_stats.csv", false, Encoding.UTF8);
            string l_Delimiter = ";";

            try
            {
                l_Writer.WriteStartDocument();
                l_Writer.WriteComment("ggplot2 " + p_Users.Count + " user stats.");
                l_Writer.WriteStartElement("Users");
                l_WriterCsv.WriteLine("Id" + l_Delimiter + "Answers" + l_Delimiter + "Questions" + l_Delimiter + "ZScore" + l_Delimiter + "InDegree" + l_Delimiter + "OutDegree" + l_Delimiter + "ZDegree" + l_Delimiter + "ExertiseRank");

                for (int i = 0; i < p_Users.Count; i++)
                {
                    l_Writer.WriteStartElement("User");

                    l_Writer.WriteAttributeString("Id", i.ToString());
                    l_Writer.WriteAttributeString("Answers", p_Users[i].Answers.ToString());
                    l_Writer.WriteAttributeString("Questions", p_Users[i].Questions.ToString());
                    l_Writer.WriteAttributeString("ZScore", p_Users[i].ZScore.ToString());
                    l_Writer.WriteAttributeString("InDegree", p_Users[i].InDegreeList.Count.ToString());
                    l_Writer.WriteAttributeString("OutDegree", p_Users[i].OutDegreeList.Count.ToString());
                    l_Writer.WriteAttributeString("ZDegree", p_Users[i].ZDegree.ToString());
                    l_Writer.WriteAttributeString("ExpertiseRank", p_Users[i].ExpertiseRank.ToString());

                    l_Writer.WriteEndElement();

                    l_WriterCsv.WriteLine(i.ToString() + l_Delimiter + p_Users[i].Answers.ToString() + l_Delimiter + p_Users[i].Questions.ToString() + l_Delimiter + p_Users[i].ZScore.ToString() + l_Delimiter + p_Users[i].InDegreeList.Count.ToString() + l_Delimiter + p_Users[i].OutDegreeList.Count.ToString() + l_Delimiter + p_Users[i].ZDegree.ToString() + l_Delimiter + p_Users[i].ExpertiseRank.ToString());
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
                if (l_Settings.DebugMode)
                    debug("Exception in exportStats(): " + e.Message);
            }
        }

        public void exportTopics(List<Topic> p_Topics, bool p_Base64)
        {
            Settings l_Settings = Settings.Instance;
            XmlTextWriter l_Writer = new XmlTextWriter(l_Settings.OutputDir + l_Settings.TopicsFile, Encoding.UTF8);
            l_Writer.Formatting = Formatting.Indented;

            try
            {
                l_Writer.WriteStartDocument();
                l_Writer.WriteComment("ggplot2 " + p_Topics.Count + " topics.");
                l_Writer.WriteStartElement("Topics");

                for (int i = 0; i < p_Topics.Count; i++)
                {
                    l_Writer.WriteStartElement("Topic");
                        l_Writer.WriteStartElement("Title");
                        l_Writer.WriteString(p_Topics[i].Title);
                        l_Writer.WriteEndElement();
                        l_Writer.WriteStartElement("ID");
                        l_Writer.WriteString(p_Topics[i].ID);
                        l_Writer.WriteEndElement();
                        l_Writer.WriteStartElement("Messages");

                        for (int j = 0; j < p_Topics[i].Messages.Count; j++)
                        {

                            l_Writer.WriteStartElement("Message");
                                l_Writer.WriteStartElement("User");
                                    l_Writer.WriteStartElement("Name");
                                    l_Writer.WriteString(p_Topics[i].Messages[j].UserName);
                                    l_Writer.WriteEndElement();
                                    l_Writer.WriteStartElement("Avatar");
                                    l_Writer.WriteString(p_Topics[i].Messages[j].Avatar);
                                    l_Writer.WriteEndElement();
                                    l_Writer.WriteStartElement("Email");
                                    l_Writer.WriteString(p_Topics[i].Messages[j].Email);
                                    l_Writer.WriteEndElement();
                                l_Writer.WriteEndElement();//~User
                                l_Writer.WriteStartElement("date");
                                l_Writer.WriteString(p_Topics[i].Messages[j].Date);
                                l_Writer.WriteEndElement();
                                l_Writer.WriteStartElement("content");
                                if (p_Base64)
                                {
                                    l_Writer.WriteString(p_Topics[i].Messages[j].Content);
                                }
                                else
                                {
                                    //encode
                                    //System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(l_Contents[i]))));
                                    //decode
                                    //System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(p_Topics[i].Messages[j].Content));
                                    l_Writer.WriteString(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(p_Topics[i].Messages[j].Content)));
                                }
                                l_Writer.WriteEndElement();
                            l_Writer.WriteEndElement();//~Message
                        }

                        l_Writer.WriteEndElement();//~Messages
                    l_Writer.WriteEndElement();//~Topic
                }

                l_Writer.WriteEndElement();//~Topics
                l_Writer.WriteEndDocument();
                l_Writer.Flush();
                l_Writer.Close();
            }
            catch (Exception e)
            {
                l_Writer.Flush();
                l_Writer.Close();
                if (l_Settings.DebugMode)
                    debug("Exception in exportTopics(): " + e.Message);
            }
        }

        public void exportUserEmailList(List<User> p_Users)
        {
            Settings l_Settings = Settings.Instance;
            
            try
            {
                TextWriter l_TwTopics = new StreamWriter(l_Settings.OutputDir + "GG_useremaillist.txt", false);
                for (int i = 0; i < p_Users.Count; i++)
                {
                    l_TwTopics.WriteLine(p_Users[i].Name + ";" + p_Users[i].Email);
                }
                l_TwTopics.Close();
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    debug("Exception in exportUserEmailList(): " + e.Message);
            }
        }
    }
}
