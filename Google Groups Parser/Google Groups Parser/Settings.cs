using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Groups_Parser
{
    sealed class Settings
    {
        //Singleton
        private static readonly Settings m_Instance = new Settings();

        private bool m_DebugMode = true;
        private string m_DebugFile = "debug.txt";
        private string m_SettingsFile = "settings.xml";
        private string m_UsersFile = "GG_users.xml";
        private string m_StatsFile = "GG_stats.xml";
        private string m_TopicsFile = "GG_topics.xml";
        private string m_TopicListFile = "GG_topiclist.txt";
        private string m_DebugDir = "Google Groups\\Debug\\";
        private string m_OutputDir = "Google Groups\\Output\\";
        private string m_TopicsDir = "Google Groups\\Output\\Topics\\";

//-->Constructor

        private Settings()
        {
            
        }

//-->Attributes

        //Singleton
        public static Settings Instance
        {
            get
            {
                return m_Instance;
            }
        }

        public bool DebugMode
        {
            get { return m_DebugMode; }
            set { m_DebugMode = value; }
        }

        public string DebugFile
        {
            get { return m_DebugFile; }
            set { m_DebugFile = value; }
        }

        public string SettingsFile
        {
            get { return m_SettingsFile; }
            set { m_SettingsFile = value; }
        }

        public string UsersFile
        {
            get { return m_UsersFile; }
            set { m_UsersFile = value; }
        }

        public string StatsFile
        {
            get { return m_StatsFile; }
            set { m_StatsFile = value; }
        }

        public string TopicsFile
        {
            get { return m_TopicsFile; }
            set { m_TopicsFile = value; }
        }

        public string TopicListFile
        {
            get { return m_TopicListFile; }
            set { m_TopicListFile = value; }
        }

        public string DebugDir
        {
            get { return m_DebugDir; }
            set { m_DebugDir = value; }
        }

        public string OutputDir
        {
            get { return m_OutputDir; }
            set { m_OutputDir = value; }
        }

        public string TopicsDir
        {
            get { return m_TopicsDir; }
            set { m_TopicsDir = value; }
        }

//-->Methods

    }
}
