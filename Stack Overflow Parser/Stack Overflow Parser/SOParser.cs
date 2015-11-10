using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Stack_Overflow_Parser
{
    public partial class SOParser : Form
    {
        //private List<Topic> m_Topics = new List<Topic>();
        //private List<User> m_Users = new List<User>();
        private List<int> m_QuestionIds = new List<int>();
        private List<string> m_QuestionIdsTesting = new List<string>();
        private List<int> m_UserIds = new List<int>();
        private Group m_Group = new Group();
        private string m_File = "";

        public SOParser()
        {
            InitializeComponent();
        }

        private void buttonParse_Click(object sender, EventArgs e)
        {
            buttonParse.Enabled = false;
            //parsePostLinq();
            findTaggedQuestions();
        }

        private void buttonParseUsers_Click(object sender, EventArgs e)
        {
            buttonParseUsers.Enabled = false;
            findUsersXMLNodes();
        }

        /*private void parsePostLinq()
        {
            openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            openFileDialog1.RestoreDirectory = true;
            DialogResult l_Result = openFileDialog1.ShowDialog();
            if (l_Result == DialogResult.OK)
            {
                string l_File = openFileDialog1.FileName;
                using (XmlReader l_Reader = XmlReader.Create(l_File))
                {
                    var l_Posts = from p in l_Reader.Posts()
                                  where p.Tags != null &&
                                          //p.Tags.Contains(textBoxTag.Text) &&
                                          p.PostTypeId == 2
                                  select p;
                    Console.WriteLine("{0} posts match query", l_Posts.Count());
                }
            }
            buttonParse.Enabled = true;
        }*/

        private void findTaggedQuestions()
        {
            var l_BW = new BackgroundWorker();
            openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            openFileDialog1.RestoreDirectory = true;
            DialogResult l_Result = openFileDialog1.ShowDialog();
            if (l_Result == DialogResult.OK)
            {
                m_File = openFileDialog1.FileName;
                try
                {
                    l_BW.WorkerReportsProgress = true;
                    l_BW.DoWork += delegate
                    {
                        l_BW.ReportProgress(0, "Searching");
                        using (XmlReader l_Reader = XmlReader.Create(m_File))
                        {
                            while (l_Reader.Read())
                            {
                                if (l_Reader.NodeType == XmlNodeType.Element)
                                {
                                    if (l_Reader.Name == "row")
                                    {
                                        XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                        if (l_Element != null)
                                        {
                                            if (l_Element.Attribute("Tags") != null)
                                            {
                                                if (l_Element.Attribute("Tags").Value.Contains(textBoxTag.Text))
                                                {
                                                    if (!m_QuestionIds.Contains(int.Parse(l_Element.Attribute("Id").Value)))
                                                    {
                                                        m_QuestionIds.Add(int.Parse(l_Element.Attribute("Id").Value));
                                                        l_BW.ReportProgress(0, "Tag found at post Id: " + l_Element.Attribute("Id").Value);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Parse xml
                        if (m_QuestionIds.Count > 0)
                        {
                            l_BW.ReportProgress(0, "Creating SO_Posts_filter_temp.xml");
                            parsePostXMLNodes();
                            l_BW.ReportProgress(0, "Creating SO_Posts_filter.xml");
                            removeAnonymousPosts();
                            l_BW.ReportProgress(0, "ready");
                        }
                        else
                            Console.WriteLine("---> No questions found for current tag");
                    };
                    l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
                    {
                        toolStripStatusLabel1.Text = p_E.UserState.ToString();
                    };
                    l_BW.RunWorkerCompleted += delegate
                    {
                        buttonParse.Enabled = true;
                    };
                    l_BW.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("---> " + ex.Message);
                }
            }
        }

        private void parsePostXMLNodes()
        {
            try
            {
                TextWriter l_TwPosts = new StreamWriter("Stack Overflow/SO_Posts_filter_temp.xml", false);
                l_TwPosts.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                l_TwPosts.WriteLine("<posts>");

                using (XmlReader l_Reader = XmlReader.Create(m_File))
                {
                    while (l_Reader.Read())
                    {
                        if (l_Reader.NodeType == XmlNodeType.Element)
                        {
                            if (l_Reader.Name == "row")
                            {
                                XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                if (l_Element != null)
                                {
                                    if (l_Element.Attribute("Tags") != null)
                                    {
                                        if (l_Element.Attribute("Tags").Value.Contains(textBoxTag.Text))
                                        {
                                            l_TwPosts.WriteLine(l_Element.ToString());
                                        }
                                    }
                                    else if (l_Element.Attribute("ParentId") != null)
                                    {
                                        if (m_QuestionIds.Contains(int.Parse(l_Element.Attribute("ParentId").Value)))
                                        {
                                            l_TwPosts.WriteLine(l_Element.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                l_TwPosts.WriteLine("</posts>");
                l_TwPosts.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("---> " + ex.Message);
            }
        }

        private void removeAnonymousPosts()
        {
            try
            {
                TextWriter l_TwPosts = new StreamWriter("Stack Overflow/SO_Posts_filter.xml", false);
                l_TwPosts.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                l_TwPosts.WriteLine("<posts>");

                using (XmlReader l_Reader = XmlReader.Create("Stack Overflow/SO_Posts_filter_temp.xml"))
                {
                    while (l_Reader.Read())
                    {
                        if (l_Reader.NodeType == XmlNodeType.Element)
                        {
                            if (l_Reader.Name == "row")
                            {
                                XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                if (l_Element != null)
                                {
                                    if (l_Element.Attribute("OwnerUserId") != null)
                                    {
                                        l_TwPosts.WriteLine(l_Element.ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                l_TwPosts.WriteLine("</posts>");
                l_TwPosts.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("---> " + ex.Message);
            }
        }

        private void findUsersXMLNodes()
        {
            var l_BW = new BackgroundWorker();
            openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            openFileDialog1.RestoreDirectory = true;
            DialogResult l_Result = openFileDialog1.ShowDialog();
            if (l_Result == DialogResult.OK)
            {
                m_File = openFileDialog1.FileName;
                try
                {
                    l_BW.WorkerReportsProgress = true;
                    l_BW.DoWork += delegate
                    {
                        l_BW.ReportProgress(0, "Searching");
                        using (XmlReader l_Reader = XmlReader.Create("Stack Overflow/SO_Posts_filter.xml"))
                        {
                            while (l_Reader.Read())
                            {
                                if (l_Reader.NodeType == XmlNodeType.Element)
                                {
                                    if (l_Reader.Name == "row")
                                    {
                                        XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                        if (l_Element != null)
                                        {
                                            if (l_Element.Attribute("OwnerUserId") != null)
                                            {
                                                if (!m_UserIds.Contains(int.Parse(l_Element.Attribute("OwnerUserId").Value)))
                                                {
                                                    m_UserIds.Add(int.Parse(l_Element.Attribute("OwnerUserId").Value));
                                                    l_BW.ReportProgress(0, "ggplot2 user found with Id: " + l_Element.Attribute("OwnerUserId").Value);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Parse xml
                        if (m_UserIds.Count > 0)
                        {
                            l_BW.ReportProgress(0, "Creating SO_Users_filter.xml");
                            parseUsersXMLNodes();
                            l_BW.ReportProgress(0, "Ready");
                        }
                        else
                            Console.WriteLine("---> No users found in post data set");
                    };
                    l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
                    {
                        toolStripStatusLabel1.Text = p_E.UserState.ToString();
                    };
                    l_BW.RunWorkerCompleted += delegate
                    {
                        buttonParseUsers.Enabled = true;
                    };
                    l_BW.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("---> " + ex.Message);
                }
            }
        }

        private void parseUsersXMLNodes()
        {

            TextWriter l_TwPosts = new StreamWriter("Stack Overflow/SO_Users_filter.xml", false);
            l_TwPosts.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            l_TwPosts.WriteLine("<users>");

            try
            {
                using (XmlReader l_Reader = XmlReader.Create(m_File))
                {
                    while (l_Reader.Read())
                    {
                        if (l_Reader.NodeType == XmlNodeType.Element)
                        {
                            if (l_Reader.Name == "row")
                            {
                                XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                if (l_Element != null)
                                {
                                    //if (l_Element.Attribute("AccountId") != null)
                                    if (l_Element.Attribute("Id") != null)
                                    {
                                        //if (m_UserIds.Contains(int.Parse(l_Element.Attribute("AccountId").Value)))
                                        if (m_UserIds.Contains(int.Parse(l_Element.Attribute("Id").Value)))
                                        {
                                            l_TwPosts.WriteLine(l_Element.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                l_TwPosts.WriteLine("</users>");
                l_TwPosts.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("---> " + ex.Message);
            }
        }

        private void buttonEmailHash_Click(object sender, EventArgs e)
        {
            var l_BW = new BackgroundWorker();
            buttonEmailHash.Enabled = false;
            //openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //openFileDialog1.RestoreDirectory = true;
            //DialogResult l_Result = openFileDialog1.ShowDialog();
            //if (l_Result == DialogResult.OK)
            //{
            m_File = "Stack Overflow/SO_Users_filter.xml";

            TextWriter l_TwUsers = new StreamWriter("Stack Overflow/SO_Users_filter_hash.xml", false);
            l_TwUsers.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            l_TwUsers.WriteLine("<users>");

            try
            {
                l_BW.WorkerReportsProgress = true;
                l_BW.DoWork += delegate
                {
                    l_BW.ReportProgress(0, "Searching");
                    string l_ID = "";
                    string l_Hash = "";
                    //open SO_Users_filter.xml
                    using (XmlReader l_Reader = XmlReader.Create(m_File))
                    {
                        while (l_Reader.Read())
                        {
                            if (l_Reader.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader.Name == "row")
                                {
                                    XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                    if (l_Element != null)
                                    {
                                        if (l_Element.Attribute("Id") != null)
                                        {
                                            l_ID = l_Element.Attribute("Id").Value;
                                            //Find email hash
                                            l_BW.ReportProgress(0, "Searching email hash for user: " + l_ID);
                                            Console.WriteLine("Current user: " + l_ID);
                                            l_Hash = findEmailHash2(l_ID);
                                            //Add email hash
                                            if (l_Hash.Length > 0)
                                                l_Element.SetAttributeValue("EmailHash", l_Hash);
                                            //Add user to xml
                                            l_TwUsers.WriteLine(l_Element.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }

                    l_TwUsers.WriteLine("</users>");
                    l_TwUsers.Close();
                    l_BW.ReportProgress(0, "Ready");
                };
                l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
                {
                    toolStripStatusLabel1.Text = p_E.UserState.ToString();
                };
                l_BW.RunWorkerCompleted += delegate
                {
                    buttonEmailHash.Enabled = true;
                };
                l_BW.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("---> " + ex.Message);
            }
        }

        /*private string findEmailHash(string p_ID)
        {
            string l_Hash = "";

            try
            {
                using (XmlReader l_Reader = XmlReader.Create("Users_2013-3.xml"))
                {
                    while (l_Reader.Read())
                    {
                        if (l_Reader.NodeType == XmlNodeType.Element)
                        {
                            if (l_Reader.Name == "row")
                            {
                                XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                if (l_Element != null)
                                {
                                    if (l_Element.Attribute("Id") != null)
                                    {
                                        if (l_Element.Attribute("Id").Value.Equals(p_ID))
                                        {
                                            if (l_Element.Attribute("EmailHash") != null)
                                            {
                                                l_Hash = l_Element.Attribute("EmailHash").Value;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---> " + ex.Message);
            }
            return l_Hash;
        }*/

        private string findEmailHash2(string p_ID)
        {
            string l_Hash = "";

            try
            {
                using (XmlReader l_Reader = XmlReader.Create("Stack Overflow/Users_2013-3.xml"))
                {
                    var l_Users = from u in l_Reader.Users()
                                  where u.Id == p_ID
                                  select u;
                    //if (l_Users.Count() > 0)
                    //    l_Hash = l_Users.ElementAt(0).EmailHash;
                    //if(l_Users.ElementAt(0) != null)
                    //    if (l_Users.ElementAt(0).EmailHash != null)
                    //       l_Hash = l_Users.ElementAt(0).EmailHash;
                    //if (l_Users.Count() > 0)
                    foreach (var i in l_Users)
                    {
                        l_Hash = i.EmailHash;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---> " + ex.Message);
            }
            if (l_Hash.Length > 0)
                Console.WriteLine("Hash found: " + l_Hash);
            return l_Hash;
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {
            IOHandler l_IOHandler = IOHandler.Instance;

            var l_BW = new BackgroundWorker();
            buttonStats.Enabled = false;
            int l_Index = -1;
            string l_UserID = "";
            string l_Date = "";
            string l_Content = "";
            string l_Id = "";
            string l_ParentID = "";

            try
            {
                l_BW.WorkerReportsProgress = true;
                l_BW.DoWork += delegate
                {
                    l_BW.ReportProgress(0, "Identifying questions and answers");
                    using (XmlReader l_Reader = XmlReader.Create("Stack Overflow/SO_Posts_filter.xml"))
                    {
                        while (l_Reader.Read())
                        {
                            if (l_Reader.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader.Name == "row")
                                {
                                    XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                    if (l_Element != null)
                                    {

                                        if (l_Element.Attribute("PostTypeId").Value == "1")
                                        {//Question
                                            m_Group.Topics.Add(new Topic(l_Element.Attribute("Title").Value, l_Element.Attribute("Id").Value));
                                            l_Index = m_Group.Topics.FindIndex(item => item.ID == l_Element.Attribute("Id").Value);

                                            if (l_Element.Attribute("OwnerUserId") != null)
                                                l_UserID = l_Element.Attribute("OwnerUserId").Value;
                                            else
                                                l_UserID = "Ignore this user!";
                                            l_Date = l_Element.Attribute("CreationDate").Value;
                                            l_Content = l_Element.Attribute("Body").Value;
                                            l_Id = l_Element.Attribute("Id").Value;
                                            l_ParentID = l_Element.Attribute("Id").Value;
                                            m_Group.Topics[l_Index].Messages.Add(new Message(l_UserID, l_Date, l_Content, l_Id, l_ParentID));
                                        }
                                        else
                                        {//Answer
                                            l_Index = m_Group.Topics.FindIndex(item => item.ID == l_Element.Attribute("ParentId").Value);

                                            if (l_Element.Attribute("OwnerUserId") != null)
                                                l_UserID = l_Element.Attribute("OwnerUserId").Value;
                                            else
                                                l_UserID = "Ignore this user!";
                                            l_Date = l_Element.Attribute("CreationDate").Value;
                                            l_Content = l_Element.Attribute("Body").Value;
                                            l_Id = l_Element.Attribute("Id").Value;
                                            l_ParentID = l_Element.Attribute("ParentId").Value;
                                            m_Group.Topics[l_Index].Messages.Add(new Message(l_UserID, l_Date, l_Content, l_Id, l_ParentID));
                                        }
                                        //Add if new user
                                        if (m_Group.Users.FindIndex(item => item.Id == l_UserID) < 0)
                                        {
                                            m_Group.Users.Add(new User(l_UserID));
                                        }
                                    }
                                }
                            }
                        }
                        //Calc stats
                        l_BW.ReportProgress(0, "Starting calculations");
                        m_Group.resetUserStats();
                        m_Group.countUserAnswers();
                        m_Group.countUserQuestions();
                        m_Group.calcUserZScore();
                        m_Group.calcInDegree();
                        m_Group.calcOutDegree();
                        m_Group.calcUserZDegree();//Call after in-, out-degree
                        m_Group.calcExpertiseRank();//Call after in-, out-degree

                        l_BW.ReportProgress(0, "Creating SO_stats.xml and SO_stats.csv");
                        l_IOHandler.exportStats(m_Group.Users);
                        l_BW.ReportProgress(0, "Ready");
                    }
                };
                l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
                {
                    toolStripStatusLabel1.Text = p_E.UserState.ToString();
                };
                l_BW.RunWorkerCompleted += delegate
                {
                    buttonStats.Enabled = true;
                };
                l_BW.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("---> " + ex.Message);
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
            TextWriter l_TwQuestionListTraining = new StreamWriter("R/SO_questionlist_training_" + p_Section + ".csv", false);
            TextWriter l_TwQuestionListTesting = new StreamWriter("R/SO_questionlist_testing_" + p_Section + ".csv", false);
            TextWriter l_TwTopicIdListTraining = new StreamWriter("R/SO_topicIDlist_training_" + p_Section + ".csv", false);
            TextWriter l_TwTopicIdListTesting = new StreamWriter("R/SO_topicIDlist_testing_" + p_Section + ".csv", false);
            TextWriter l_TwUserListTraining = new StreamWriter("R/SO_userlist_training_" + p_Section + ".csv", false);
            TextWriter l_TwUserListTesting = new StreamWriter("R/SO_userlist_testing_" + p_Section + ".csv", false);
            TextWriter l_TwWordListTFIDFTraining = new StreamWriter("R/SO_wordlist_tfidf_training_" + p_Section + ".csv", false);
            TextWriter l_TwWordListTFIDFTesting = new StreamWriter("R/SO_wordlist_tfidf_testing_" + p_Section + ".csv", false);
            TextWriter l_TwPostsTraining = new StreamWriter("Stack Overflow/SO_Posts_filter_training_" + p_Section + ".xml", false);
            TextWriter l_TwPostsTesting = new StreamWriter("Stack Overflow/SO_Posts_filter_testing_" + p_Section + ".xml", false);
            l_TwPostsTraining.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            l_TwPostsTraining.WriteLine("<posts>");
            l_TwPostsTesting.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            l_TwPostsTesting.WriteLine("<posts>");

            try
            {
                l_BW.WorkerReportsProgress = true;
                l_BW.DoWork += delegate
                {
                    //Question list
                    l_BW.ReportProgress(0, "Splitting question list.");
                    using (StreamReader l_Reader = new StreamReader("R/SO_questionlist.csv"))
                    {
                        while (true)
                        {
                            string l_Line = l_Reader.ReadLine();
                            if (l_Line == null)
                            {
                                break;
                            }
                            if (l_Index % 10 == (p_Section-1))
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
                        l_BW.ReportProgress(0, "Creating SO_questionlist_training_" + p_Section + ".csv and SO_questionlist_testing_" + p_Section + ".csv");
                    }
                    l_TwQuestionListTraining.Close();
                    l_TwQuestionListTesting.Close();

                    //TopicId list
                    l_Index = 0;
                    l_BW.ReportProgress(0, "Splitting topicId list.");
                    using (StreamReader l_Reader = new StreamReader("R/SO_topicIDlist.csv"))
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
                        l_BW.ReportProgress(0, "Creating SO_topicIDlist_training_" + p_Section + ".csv and SO_topicIDlist_testing_" + p_Section + ".csv");
                    }
                    l_TwTopicIdListTraining.Close();
                    l_TwTopicIdListTesting.Close();

                    //User list
                    l_Index = 0;
                    l_BW.ReportProgress(0, "Splitting user list.");
                    using (StreamReader l_Reader = new StreamReader("R/SO_userlist.csv"))
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
                        l_BW.ReportProgress(0, "Creating SO_userlist_training_" + p_Section + ".csv and SO_userlist_testing_" + p_Section + ".csv");
                    }
                    l_TwUserListTraining.Close();
                    l_TwUserListTesting.Close();

                    //Word list TFIDF
                    l_Index = 0;
                    l_BW.ReportProgress(0, "Splitting word list tfidf.");
                    using (StreamReader l_Reader = new StreamReader("R/SO_wordlist_tfidf.csv"))
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
                        l_BW.ReportProgress(0, "Creating SO_wordlist_tfidf_training_" + p_Section + ".csv and SO_wordlist_tfidf_testing_" + p_Section + ".csv");
                    }
                    l_TwWordListTFIDFTraining.Close();
                    l_TwWordListTFIDFTesting.Close();

                    //Posts
                    l_BW.ReportProgress(0, "Splitting posts.");
                    using (XmlReader l_Reader = XmlReader.Create("Stack Overflow/SO_Posts_filter.xml"))
                    {
                        while (l_Reader.Read())
                        {
                            if (l_Reader.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader.Name == "row")
                                {
                                    XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                    if (l_Element != null)
                                    {
                                        if (l_Element.Attribute("Id") != null)
                                        {
                                            if (m_QuestionIdsTesting.Contains(l_Element.Attribute("Id").Value))
                                            {
                                                //Testing dataset
                                                l_TwPostsTesting.WriteLine(l_Element.ToString());
                                            }
                                            else if (l_Element.Attribute("ParentId") != null)
                                            {
                                                if (m_QuestionIdsTesting.Contains(l_Element.Attribute("ParentId").Value))
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
                    l_TwPostsTraining.WriteLine("</posts>");
                    l_TwPostsTesting.WriteLine("</posts>");
                    l_TwPostsTraining.Close();
                    l_TwPostsTesting.Close();
                    l_BW.ReportProgress(0, "Creating SO_Posts_filter_testing_" + p_Section + ".xml and SO_Posts_filter_testing_" + p_Section + ".xml");
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
