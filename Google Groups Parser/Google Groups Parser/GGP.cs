using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Google_Groups_Parser
{
    public partial class GGP : Form
    {
        private ToolTip m_Tooltip = new ToolTip();
        private List<string> m_QuestionIdsTesting = new List<string>();
        //Custom classes
        private Group m_Group = new Group();

//-->Constructor

        public GGP()
        {
            InitializeComponent();
            initFolders();

            m_Tooltip.SetToolTip(checkBoxBase64, "Save message content as base64.");
            m_Tooltip.SetToolTip(radioButtonSortDefault, "Do not sort users.");
            m_Tooltip.SetToolTip(radioButtonSortActivity, "Sort users by activity.");
            m_Tooltip.SetToolTip(radioButtonSortZScore, "Sort users by Z-Score.");
            m_Tooltip.SetToolTip(radioButtonSortName, "Sort users by name.");
        }

//-->Methods

        private void initFolders()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                Directory.CreateDirectory(l_Settings.DebugDir);
                Directory.CreateDirectory(l_Settings.OutputDir);
                Directory.CreateDirectory(l_Settings.TopicsDir);
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in initFolders(): " + e.Message);
            }
        }

//-->Events

        private void buttonImportData_Click(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                buttonImportData.Enabled = false;
                buttonExportData.Enabled = false;

                //reset topics/users
                m_Group.Topics.Clear();
                m_Group.Users.Clear();

                //load GG_topiclist.txt
                TextReader l_TrTopiclist = new StreamReader(l_Settings.OutputDir + l_Settings.TopicListFile);
                string l_Topic = "";
                string l_ID = "";
                string l_Title = "";
                int l_Index = -1;

                while ((l_Topic = l_TrTopiclist.ReadLine()) != null)
                {
                    l_ID = l_Topic.Split(';')[0];
                    l_Title = l_Topic.Substring(l_ID.Length + 1);
                    //Add topic
                    m_Group.addTopic(l_Title, l_ID);
                }
                l_TrTopiclist.Close();

                //load topic data
                string[] l_Topicpaths = Directory.GetFiles(l_Settings.TopicsDir);
                for (int i = 0; i < l_Topicpaths.Length; i++)
                {
                    XmlDocument l_XmlDoc = new XmlDocument();
                    l_XmlDoc.Load(l_Topicpaths[i]);
                    XmlNodeList l_NodeList = l_XmlDoc.DocumentElement.SelectNodes("/Topic");
                    l_Title = l_NodeList[0].SelectSingleNode("Title").InnerText;
                    l_ID = l_NodeList[0].SelectSingleNode("ID").InnerText;
                    l_NodeList = l_XmlDoc.DocumentElement.SelectNodes("/Topic/Messages/Message");
                    string l_Date = "", l_Content = "", l_Name = "", l_Avatar = "", l_Email = "";
                    foreach (XmlNode l_Node in l_NodeList)
                    {
                        l_Date = l_Node.SelectSingleNode("date").InnerText;
                        l_Content = l_Node.SelectSingleNode("content").InnerText;
                        l_Name = l_Node.SelectSingleNode("User").SelectSingleNode("Name").InnerText;
                        l_Avatar = l_Node.SelectSingleNode("User").SelectSingleNode("Avatar").InnerText;
                        l_Email = l_Node.SelectSingleNode("User").SelectSingleNode("Email").InnerText;

                        l_Index = m_Group.getTopicIndex(l_ID);
                        if (l_Index != -1)
                        {
                            m_Group.Topics[l_Index].Messages.Add(new Message(l_Name, l_Avatar, l_Date, l_Content, l_Email));
                            if (m_Group.isUniqueUser(l_Name, l_Avatar, l_Email))
                                m_Group.Users.Add(new User(l_Name, l_Avatar, l_Email));
                        }
                    }
                }
                buttonImportData.Enabled = true;
                buttonExportData.Enabled = true;
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in buttonImportData_Click(): " + ex.Message);
            }
        }

        private void buttonExportData_Click(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                buttonImportData.Enabled = false;
                buttonExportData.Enabled = false;

                m_Group.resetUserStats();
                m_Group.countUserAnswers();
                m_Group.countUserQuestions();
                m_Group.calcUserZScore();
                
                if(radioButtonSortActivity.Checked)
                    m_Group.sortUsersByActivity();
                else if (radioButtonSortZScore.Checked)
                    m_Group.sortUsersByZScore();
                else if(radioButtonSortName.Checked)
                    m_Group.sortUsersByName();

                //In-, out-degree require sorted list to be consistent
                m_Group.calcInDegree();
                m_Group.calcOutDegree();
                m_Group.calcUserZDegree();//Call after in-, out-degree
                //m_Group.calcExpertiseRank();//Call after in-, out-degree
                m_Group.calcExpertiseRank2();//Call after in-, out-degree

                l_IOHandler.exportUserList(m_Group.Users);
                l_IOHandler.exportTopics(m_Group.Topics, checkBoxBase64.Checked);
                l_IOHandler.exportUserEmailList(m_Group.Users);
                l_IOHandler.exportStats(m_Group.Users);

                buttonImportData.Enabled = true;
                buttonExportData.Enabled = true;
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in buttonExportData_Click(): " + ex.Message);
            }
        }

        private void buttonSplit_Click(object sender, EventArgs e)
        {
            SplitData(1);
        }
        
        private void SplitData(int p_Section)
        {
            var l_BW = new BackgroundWorker();
            buttonSplit.Enabled = false;
            int l_Index = 0;
            m_QuestionIdsTesting.Clear();
            TextWriter l_TwQuestionListTraining = new StreamWriter("R/GG_questionlist_training_" + p_Section + ".csv", false);
            TextWriter l_TwQuestionListTesting = new StreamWriter("R/GG_questionlist_testing_" + p_Section + ".csv", false);
            TextWriter l_TwTopicIdListTraining = new StreamWriter("R/GG_topicIDlist_training_" + p_Section + ".csv", false);
            TextWriter l_TwTopicIdListTesting = new StreamWriter("R/GG_topicIDlist_testing_" + p_Section + ".csv", false);
            TextWriter l_TwUserListTraining = new StreamWriter("R/GG_userlist_training_" + p_Section + ".csv", false);
            TextWriter l_TwUserListTesting = new StreamWriter("R/GG_userlist_testing_" + p_Section + ".csv", false);
            TextWriter l_TwWordListTFIDFTraining = new StreamWriter("R/GG_wordlist_tfidf_training_" + p_Section + ".csv", false);
            TextWriter l_TwWordListTFIDFTesting = new StreamWriter("R/GG_wordlist_tfidf_testing_" + p_Section + ".csv", false);
            TextWriter l_TwPostsTraining = new StreamWriter("Google Groups/Output/GG_topics_training_" + p_Section + ".xml", false);
            TextWriter l_TwPostsTesting = new StreamWriter("Google Groups/Output/GG_topics_testing_" + p_Section + ".xml", false);
            l_TwPostsTraining.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            l_TwPostsTraining.WriteLine("<Topics>");
            l_TwPostsTesting.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            l_TwPostsTesting.WriteLine("<Topics>");

            try
            {
                l_BW.WorkerReportsProgress = true;
                l_BW.DoWork += delegate
                {
                    //Question list
                    l_BW.ReportProgress(0, "Splitting question list.");
                    using (StreamReader l_Reader = new StreamReader("R/GG_questionlist.csv"))
                    {
                        while (true)
                        {
                            string l_Line = l_Reader.ReadLine();
                            if (l_Line == null)
                            {
                                break;
                            }
                            if (l_Index % 10 == (p_Section - 1))
                            {
                                //10% testing
                                l_TwQuestionListTesting.WriteLine(l_Line);
                            }
                            else
                            {
                                //90% training
                                l_TwQuestionListTraining.WriteLine(l_Line);
                            }
                            l_Index++;
                        }
                        l_BW.ReportProgress(0, "Creating GG_questionlist_training_" + p_Section + ".csv and GG_questionlist_testing_" + p_Section + ".csv");
                    }
                    l_TwQuestionListTraining.Close();
                    l_TwQuestionListTesting.Close();

                    //TopicId list
                    l_Index = 0;
                    l_BW.ReportProgress(0, "Splitting topicId list.");
                    using (StreamReader l_Reader = new StreamReader("R/GG_topicIDlist.csv"))
                    {
                        while (true)
                        {
                            string l_Line = l_Reader.ReadLine();
                            if (l_Line == null)
                            {
                                break;
                            }
                            if (l_Index % 10 == (p_Section - 1))
                            {
                                //10% testing
                                l_TwTopicIdListTesting.WriteLine(l_Line);
                                m_QuestionIdsTesting.Add(l_Line.Substring(1, l_Line.Length - 2));
                            }
                            else
                            {
                                //90% training
                                l_TwTopicIdListTraining.WriteLine(l_Line);
                            }
                            l_Index++;
                        }
                        l_BW.ReportProgress(0, "Creating GG_topicIDlist_training_" + p_Section + ".csv and GG_topicIDlist_testing_" + p_Section + ".csv");
                    }
                    l_TwTopicIdListTraining.Close();
                    l_TwTopicIdListTesting.Close();

                    //User list
                    l_Index = 0;
                    l_BW.ReportProgress(0, "Splitting user list.");
                    using (StreamReader l_Reader = new StreamReader("R/GG_userlist.csv"))
                    {
                        while (true)
                        {
                            string l_Line = l_Reader.ReadLine();
                            if (l_Line == null)
                            {
                                break;
                            }
                            if (l_Index % 10 == (p_Section - 1))
                            {
                                //10% testing
                                l_TwUserListTesting.WriteLine(l_Line);
                            }
                            else
                            {
                                //90% training
                                l_TwUserListTraining.WriteLine(l_Line);
                            }
                            l_Index++;
                        }
                        l_BW.ReportProgress(0, "Creating GG_userlist_training_" + p_Section + ".csv and GG_userlist_testing_" + p_Section + ".csv");
                    }
                    l_TwUserListTraining.Close();
                    l_TwUserListTesting.Close();

                    //Word list TFIDF
                    l_Index = 0;
                    l_BW.ReportProgress(0, "Splitting word list tfidf.");
                    using (StreamReader l_Reader = new StreamReader("R/GG_wordlist_tfidf.csv"))
                    {
                        //Add column
                        string l_Line = l_Reader.ReadLine();
                        l_TwWordListTFIDFTesting.WriteLine(l_Line);
                        l_TwWordListTFIDFTraining.WriteLine(l_Line);

                        while (true)
                        {
                            l_Line = l_Reader.ReadLine();
                            if (l_Line == null)
                            {
                                break;
                            }
                            if (l_Index % 10 == (p_Section - 1))
                            {
                                //10% testing
                                l_TwWordListTFIDFTesting.WriteLine(l_Line);
                            }
                            else
                            {
                                //90% training
                                l_TwWordListTFIDFTraining.WriteLine(l_Line);
                            }
                            l_Index++;
                        }
                        l_BW.ReportProgress(0, "Creating GG_wordlist_tfidf_training_" + p_Section + ".csv and GG_wordlist_tfidf_testing_" + p_Section + ".csv");
                    }
                    l_TwWordListTFIDFTraining.Close();
                    l_TwWordListTFIDFTesting.Close();

                    //Posts
                    l_BW.ReportProgress(0, "Splitting posts.");
                    using (XmlReader l_Reader = XmlReader.Create("Google Groups/Output/GG_topics.xml"))
                    {
                        while (l_Reader.Read())
                        {
                            if (l_Reader.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader.Name == "Topic")
                                {
                                    XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                    if (l_Element != null)
                                    {
                                        if (l_Element.Element("ID") != null)
                                        {
                                            if (m_QuestionIdsTesting.Contains(l_Element.Element("ID").Value))
                                            {
                                                //Testing dataset
                                                l_TwPostsTesting.WriteLine(l_Element.ToString());
                                            }
                                            else
                                            {
                                                //Training dataset
                                                l_TwPostsTraining.WriteLine(l_Element.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    l_TwPostsTraining.WriteLine("</Topics>");
                    l_TwPostsTesting.WriteLine("</Topics>");
                    l_TwPostsTraining.Close();
                    l_TwPostsTesting.Close();
                    l_BW.ReportProgress(0, "Creating GG_Posts_filter_testing_" + p_Section + ".xml and GG_Posts_filter_testing_" + p_Section + ".xml");
                    l_BW.ReportProgress(0, "Ready");
                };
                l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
                {
                    toolStripStatusLabel1.Text = p_E.UserState.ToString();
                };
                l_BW.RunWorkerCompleted += delegate
                {
                    if (p_Section == 10)
                        buttonSplit.Enabled = true;
                    else
                    {
                        p_Section++;
                        SplitData(p_Section);
                    }
                };
                l_BW.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("---> " + ex.Message);
            }
        }
    }
}