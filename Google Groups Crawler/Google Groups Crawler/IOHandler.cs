using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Google_Groups_Crawler
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

        public string loadError()
        {
            Settings l_Settings = Settings.Instance;

            string l_Error = "";

            try
            {
                TextReader l_TrError = new StreamReader(l_Settings.DebugDir + "error.txt");
                l_Error = l_TrError.ReadToEnd();
                l_TrError.Close();
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    debug("Exception in loadError(): " + e.Message);
            }
            return l_Error;
        }

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

        public void exportTopic(string p_ID, Topic p_Topic)
        {
            Settings l_Settings = Settings.Instance;
            //File overwritten when already existing
            XmlTextWriter l_Writer = new XmlTextWriter(l_Settings.TopicsDir + p_ID + ".xml", Encoding.UTF8);
            l_Writer.Formatting = Formatting.Indented;

            try
            {
                l_Writer.WriteStartDocument();
                l_Writer.WriteComment("ggplot2 topic.");
                l_Writer.WriteStartElement("Topic");
                
                l_Writer.WriteStartElement("Title");
                l_Writer.WriteString(p_Topic.Title);
                l_Writer.WriteEndElement();
                l_Writer.WriteStartElement("ID");
                l_Writer.WriteString(p_Topic.ID);
                l_Writer.WriteEndElement();
                l_Writer.WriteStartElement("Messages");

                for (int j = 0; j < p_Topic.Messages.Count; j++)
                {
                    l_Writer.WriteStartElement("Message");
                    l_Writer.WriteStartElement("User");
                    l_Writer.WriteStartElement("Name");
                    l_Writer.WriteString(p_Topic.Messages[j].UserName);
                    l_Writer.WriteEndElement();
                    l_Writer.WriteStartElement("Avatar");
                    l_Writer.WriteString(p_Topic.Messages[j].Avatar);
                    l_Writer.WriteEndElement();
                    l_Writer.WriteStartElement("Email");
                    l_Writer.WriteString(p_Topic.Messages[j].Email);
                    l_Writer.WriteEndElement();
                    l_Writer.WriteEndElement();//~User
                    l_Writer.WriteStartElement("date");
                    l_Writer.WriteString(p_Topic.Messages[j].Date);
                    l_Writer.WriteEndElement();
                    l_Writer.WriteStartElement("content");
                    l_Writer.WriteString(p_Topic.Messages[j].Content);
                    l_Writer.WriteEndElement();
                    l_Writer.WriteEndElement();//~Message
                }

                l_Writer.WriteEndElement();//~Messages
                l_Writer.WriteEndElement();//~Topic

                l_Writer.WriteEndDocument();
                l_Writer.Flush();
                l_Writer.Close();
            }
            catch (Exception e)
            {
                l_Writer.Flush();
                l_Writer.Close();
                if (l_Settings.DebugMode)
                    debug("Exception in exportTopic(): " + e.Message);
            }
        }

        public void exportTopicList(List<Topic> p_Topics)
        {
            Settings l_Settings = Settings.Instance;

            try
            {
                TextWriter l_TwTopics = new StreamWriter(l_Settings.OutputDir + "GG_topiclist.txt", false);
                for (int i = 0; i < p_Topics.Count; i++)
                {
                    l_TwTopics.WriteLine(p_Topics[i].ID + ";" + p_Topics[i].Title);
                }
                l_TwTopics.Close();
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    debug("Exception in exportTopicList(): " + e.Message);
            }
        }

        public void saveServerResponse(string p_State, string p_Response)
        {
            Settings l_Settings = Settings.Instance;

            try
            {
                TextWriter l_TwResponse = new StreamWriter(l_Settings.DebugDir + System.DateTime.Now.Ticks.ToString() + "_" + p_State + ".txt", false);
                l_TwResponse.Write(p_Response);
                l_TwResponse.Close();
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    debug("Exception in saveServerResponse(): " + e.Message);
            }
        }
    }
}
