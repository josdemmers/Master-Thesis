using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace TopicAnalyser
{
    public partial class Form1 : Form
    {
        //string[] m_Words;
        //List<string> m_Words = new List<string>();
        //List<string[]> m_Values = new List<string[]>();
        //List<string> m_UserList = new List<string>();
        //List<User> m_Users = new List<User>();
        Controller m_Controller = new Controller();
        //Thread Handling
        public delegate void SetStatusBarCallback(object sender);

        public Form1()
        {
            InitializeComponent();
            initEventHandlers();
        }

        private void initEventHandlers()
        {
            m_Controller.statusBarUpdated += m_Controller_statusBarUpdated;
        }

        private void buttonTFIDF_Click(object sender, EventArgs e)
        {
            m_Controller.reset();
            buttonTFIDF.Enabled = false;
            buttonCustom.Enabled = false;
            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                l_BW.ReportProgress(0, "Read userlist");
                m_Controller.readUsers("GG");
                l_BW.ReportProgress(0, "Read wordlist");
                m_Controller.readWordListTFIDF("GG");
                l_BW.ReportProgress(0, "Remove regular users");
                m_Controller.removeRegularUsers("GG", 50);
                l_BW.ReportProgress(0, "Create expertise vector users");
                m_Controller.createExpertiseVectorUsersTFIDF("GG");
                l_BW.ReportProgress(0, "Analyse topic expertise");
                m_Controller.analyseTopicExpertise("GG","TFIDF", 50);
                l_BW.ReportProgress(0, "Done");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonTFIDF.Enabled = true;
                buttonCustom.Enabled = true;

                if (m_Controller.SelectedTrainingSet != "10")
                {
                    comboBoxTrainingSet.SelectedIndex++;
                    buttonTFIDF.PerformClick();
                }
                else
                {
                    //Show number of empty document vectors
                    //toolStripStatusLabel1.Text = "Empty: " + m_Controller.EmptyVectorCount +
                    //    " No expert: " + m_Controller.NoExpertsCount +
                    //    " No expert/not empty: " + m_Controller.NoExpertsCountExcludingEmptyVector +
                    //    " Unknown user: " + m_Controller.AnsweredByUnknownUser +
                    //    " Unknown: " + m_Controller.AnalysedQuestionCount +
                    //    " Total: " + m_Controller.Total;
                    //Reset
                    m_Controller.EmptyVectorCount = 0;
                    m_Controller.NoExpertsCount = 0;
                    m_Controller.NoExpertsCountExcludingEmptyVector = 0;
                    m_Controller.AnsweredByUnknownUser = 0;
                    m_Controller.AnalysedQuestionCount = 0;
                    m_Controller.Total = 0;
                }
            };
            l_BW.RunWorkerAsync();
        }

        private void buttonTFIDF_SO_Click(object sender, EventArgs e)
        {
            m_Controller.reset();
            buttonTFIDF_SO.Enabled = false;
            buttonCustom_SO.Enabled = false;
            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                l_BW.ReportProgress(0, "Read userlist");
                m_Controller.readUsers("SO");
                l_BW.ReportProgress(0, "Read wordlist");
                m_Controller.readWordListTFIDF("SO");
                l_BW.ReportProgress(0, "Remove regular users");
                m_Controller.removeRegularUsers("SO", 50);
                l_BW.ReportProgress(0, "Create expertise vector users");
                m_Controller.createExpertiseVectorUsersTFIDF("SO");
                l_BW.ReportProgress(0, "Analyse topic expertise");
                m_Controller.analyseTopicExpertise("SO", "TFIDF", 50);
                l_BW.ReportProgress(0, "Done");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonTFIDF_SO.Enabled = true;
                buttonCustom_SO.Enabled = true;

                if (m_Controller.SelectedTrainingSet != "10")
                {
                    comboBoxTrainingSet.SelectedIndex++;
                    buttonTFIDF_SO.PerformClick();
                }
                else
                {
                    //Show number of empty document vectors
                    //toolStripStatusLabel1.Text = "Empty: " + m_Controller.EmptyVectorCount +
                    //    " No expert: " + m_Controller.NoExpertsCount +
                    //    " No expert/not empty: " + m_Controller.NoExpertsCountExcludingEmptyVector +
                    //    " Unknown user: " + m_Controller.AnsweredByUnknownUser +
                    //    " Analysed: " + m_Controller.AnalysedQuestionCount +
                    //    " Total: " + m_Controller.Total;
                    //Reset
                    m_Controller.EmptyVectorCount = 0;
                    m_Controller.NoExpertsCount = 0;
                    m_Controller.NoExpertsCountExcludingEmptyVector = 0;
                    m_Controller.AnsweredByUnknownUser = 0;
                    m_Controller.AnalysedQuestionCount = 0;
                    m_Controller.Total = 0;
                }
            };
            l_BW.RunWorkerAsync();
        }

        private void buttonCustom_Click(object sender, EventArgs e)
        {
            m_Controller.reset();
            buttonTFIDF.Enabled = false;
            buttonCustom.Enabled = false;
            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                l_BW.ReportProgress(0, "Read userlist");
                m_Controller.readUsers("GG");
                l_BW.ReportProgress(0, "Read wordlist");
                m_Controller.readWordListCustom();
                l_BW.ReportProgress(0, "Remove regular users");
                m_Controller.removeRegularUsers("GG", 50);
                l_BW.ReportProgress(0, "Create expertise vector users");
                m_Controller.createExpertiseVectorUsersCustom("GG");
                l_BW.ReportProgress(0, "Analyse topic expertise");
                m_Controller.analyseTopicExpertise("GG", "Custom", 50);
                l_BW.ReportProgress(0, "Done");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonTFIDF.Enabled = true;
                buttonCustom.Enabled = true;

                if (m_Controller.SelectedTrainingSet != "10")
                {
                    comboBoxTrainingSet.SelectedIndex++;
                    buttonCustom.PerformClick();
                }
                else
                {
                    //Show number of empty document vectors
                    //toolStripStatusLabel1.Text = "Empty: " + m_Controller.EmptyVectorCount +
                    //    " No expert: " + m_Controller.NoExpertsCount +
                    //    " No expert/not empty: " + m_Controller.NoExpertsCountExcludingEmptyVector +
                    //    " Unknown user: " + m_Controller.AnsweredByUnknownUser +
                    //    " Unknown: " + m_Controller.AnalysedQuestionCount +
                    //    " Total: " + m_Controller.Total;
                    //Reset
                    m_Controller.EmptyVectorCount = 0;
                    m_Controller.NoExpertsCount = 0;
                    m_Controller.NoExpertsCountExcludingEmptyVector = 0;
                    m_Controller.AnsweredByUnknownUser = 0;
                    m_Controller.AnalysedQuestionCount = 0;
                    m_Controller.Total = 0;
                }
            };
            l_BW.RunWorkerAsync();
        }

        private void buttonCustom_SO_Click(object sender, EventArgs e)
        {
            m_Controller.reset();
            buttonTFIDF_SO.Enabled = false;
            buttonCustom_SO.Enabled = false;
            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                l_BW.ReportProgress(0, "Read userlist");
                m_Controller.readUsers("SO");
                l_BW.ReportProgress(0, "Read wordlist");
                m_Controller.readWordListCustom();
                l_BW.ReportProgress(0, "Remove regular users");
                m_Controller.removeRegularUsers("SO", 50);
                l_BW.ReportProgress(0, "Create expertise vector users");
                m_Controller.createExpertiseVectorUsersCustom("SO");
                l_BW.ReportProgress(0, "Analyse topic expertise");
                m_Controller.analyseTopicExpertise("SO", "Custom", 50);
                l_BW.ReportProgress(0, "Done");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonTFIDF_SO.Enabled = true;
                buttonCustom_SO.Enabled = true;

                if (m_Controller.SelectedTrainingSet != "10")
                {
                    comboBoxTrainingSet.SelectedIndex++;
                    buttonCustom_SO.PerformClick();
                }
                else
                {
                    //Show number of empty document vectors
                    //toolStripStatusLabel1.Text = "Empty: " + m_Controller.EmptyVectorCount +
                    //    " No expert: " + m_Controller.NoExpertsCount +
                    //    " No expert/not empty: " + m_Controller.NoExpertsCountExcludingEmptyVector +
                    //    " Unknown user: " + m_Controller.AnsweredByUnknownUser +
                    //    " Unknown: " + m_Controller.AnalysedQuestionCount +
                    //    " Total: " + m_Controller.Total;
                    //Reset
                    m_Controller.EmptyVectorCount = 0;
                    m_Controller.NoExpertsCount = 0;
                    m_Controller.NoExpertsCountExcludingEmptyVector = 0;
                    m_Controller.AnsweredByUnknownUser = 0;
                    m_Controller.AnalysedQuestionCount = 0;
                    m_Controller.Total = 0;
                }
            };
            l_BW.RunWorkerAsync();
        }

        private void buttonSurvey_Click(object sender, EventArgs e)
        {
            m_Controller.reset();
            buttonSurvey.Enabled = false;
            buttonSurveyGH.Enabled = false;
            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                l_BW.ReportProgress(0, "Read userlist");
                m_Controller.readUsers("GG");
                l_BW.ReportProgress(0, "Read wordlist");
                m_Controller.readWordListCustom();
                l_BW.ReportProgress(0, "Remove regular users");
                m_Controller.removeRegularUsers("GG", 50);
                l_BW.ReportProgress(0, "Create expertise vector users");
                m_Controller.createExpertiseVectorUsersCustomSurvey("GG");
                l_BW.ReportProgress(0, "Prepare survey data");
                m_Controller.surveyTopicExpertise(textBoxSurvey.Text);
                l_BW.ReportProgress(0, "Done");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonSurvey.Enabled = true;
                buttonSurveyGH.Enabled = true;
            };
            l_BW.RunWorkerAsync();
        }

        private void buttonSurveyGH_Click(object sender, EventArgs e)
        {
            buttonSurveyGH.Enabled = false;
            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                //l_BW.ReportProgress(0, "Read userlist");
                //m_Controller.readUsersGG();
                //l_BW.ReportProgress(0, "Read wordlist");
                //m_Controller.readWordListCustom();
                //l_BW.ReportProgress(0, "Remove regular users");
                //m_Controller.removeRegularUsers(50);
                //l_BW.ReportProgress(0, "Create expertise vector users");
                //m_Controller.createExpertiseVectorUsersCustom();
                l_BW.ReportProgress(0, "Prepare survey data");
                m_Controller.surveyTopicExpertiseGH(textBoxSurveyGH.Text);
                l_BW.ReportProgress(0, "Done");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonSurveyGH.Enabled = true;
            };
            l_BW.RunWorkerAsync();
        }

        private int getUnansweredQuestionCount(string p_Community)
        {
            int l_Unanswered = 0;
            if (p_Community.Equals("SO"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Stack Overflow/SO_Posts_filter.xml");
                string l_XPath = "//posts/row[@AnswerCount='0']";
                XmlNodeList l_Nodes = xmlDoc.SelectNodes(l_XPath);
                l_Unanswered = l_Nodes.Count;
            }
            else if (p_Community.Equals("GG"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Google Groups/Output/GG_topics.xml");
                string l_XPath = "//Topics/Topic/Messages";
                XmlNodeList l_Nodes = xmlDoc.SelectNodes(l_XPath);
                foreach (XmlNode l_Node in l_Nodes)
                {
                    if (l_Node.ChildNodes.Count == 1)
                    {
                        l_Unanswered++;
                    }
                }
            }
            return l_Unanswered;
        }

        private void textBoxSurvey_TextChanged(object sender, EventArgs e)
        {
            textBoxSurveyGH.Text = textBoxSurvey.Text;
        }

        private void textBoxSurveyGH_TextChanged(object sender, EventArgs e)
        {
            textBoxSurvey.Text = textBoxSurveyGH.Text;
        }

        private void m_Controller_statusBarUpdated(object sender, CustomArgs ca)
        {
            //Create new thread
            new Thread(new ParameterizedThreadStart(setStatusBarCrossThread)).Start(ca.getMessage());
        }

        private void setStatusBarCrossThread(object p_Message)
        {
            try
            {
                if (statusStrip1.InvokeRequired)
                {
                    SetStatusBarCallback l_Delegate = new SetStatusBarCallback(setStatusBar);
                    this.Invoke(l_Delegate, new object[] { (string)p_Message });
                }
                else
                {
                    setStatusBar(p_Message);
                }
            }
            catch (Exception)
            {
                //No statusbar update this time :(
            }
        }

        /*
         * Set a message on the statusbar
         */
        private void setStatusBar(object p_Message)
        {
            toolStripStatusLabel1.Text = p_Message.ToString();
        }

        private void comboBoxTrainingSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Controller.SelectedTrainingSet = comboBoxTrainingSet.Text;
        }

//--> TESTING

        /*private void buttonTest_Click(object sender, EventArgs e)
        {
            m_Controller.printTermFrequency();


            //m_Controller.printUserExpertise("Ron Crump");
            //m_Controller.printUserExpertise("Louis Reynolds");

            /*List<int> l_MedianTest = new List<int>();
            l_MedianTest.Add(1);
            l_MedianTest.Add(2);
            l_MedianTest.Add(3);
            l_MedianTest.Add(4);
            System.Console.WriteLine(l_MedianTest.Median());*/


            /*double l_CosineSimilarity = 0.0;
            double l_DotProduct = 0.0;
            double l_MagV1 = 0.0;//Question vector
            double l_MagV2 = 0.0;//User vector

            //Question
            List<double> p_V1 = new List<double>();
            p_V1.Add(1.0);
            p_V1.Add(0.0);
            p_V1.Add(0.0);
            p_V1.Add(1.0);
            //p_V1.Add(1.0);
            //User
            List<double> l_V2 = new List<double>();
            l_V2.Add(0.0);
            l_V2.Add(0.0);
            l_V2.Add(0.0);
            l_V2.Add(0.0);
            //l_V2.Add(0.0);

            l_DotProduct = 0.0;
            l_MagV1 = 0.0;
            l_MagV2 = 0.0;
            for (int i = 0; i < p_V1.Count; i++)
            {
                l_DotProduct += p_V1[i] * l_V2[i];
                l_MagV1 += Math.Pow(p_V1[i], 2);
                l_MagV2 += Math.Pow(l_V2[i], 2);
            }
            l_CosineSimilarity = l_DotProduct / (Math.Sqrt(l_MagV1) * Math.Sqrt(l_MagV2));

            System.Console.WriteLine(l_CosineSimilarity);
        }*/

        /*private void button1_Click(object sender, EventArgs e)
        {
            double l_CosineSimilarity = 0.0;
            double l_DotProduct = 0.0;
            double l_MagV1 = 0.0;//Question vector
            double l_MagV2 = 0.0;//User vector

            //Question
            List<double> p_V1 = new List<double>();
            p_V1.Add(1.0);
            p_V1.Add(0.0);
            p_V1.Add(0.0);
            p_V1.Add(1.0);
            //p_V1.Add(1.0);
            //User
            List<double> l_V2 = new List<double>();
            l_V2.Add(1.5);
            l_V2.Add(0.0);
            l_V2.Add(0.0);
            l_V2.Add(1.0);
            //l_V2.Add(0.0);

            l_DotProduct = 0.0;
            l_MagV1 = 0.0;
            l_MagV2 = 0.0;
            for (int i = 0; i < p_V1.Count; i++)
            {
                l_DotProduct += p_V1[i] * l_V2[i];
                l_MagV1 += Math.Pow(p_V1[i], 2);
                l_MagV2 += Math.Pow(l_V2[i], 2);
            }
            l_CosineSimilarity = l_DotProduct / (Math.Sqrt(l_MagV1) * Math.Sqrt(l_MagV2));

            System.Console.WriteLine(l_CosineSimilarity);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double l_Number;
            string l_Value = "1.234567";

            try
            {
                l_Number = Double.Parse(l_Value, CultureInfo.InvariantCulture);
                Console.WriteLine("Converted '{0}' using {1} to {2}.",
                                  l_Value, CultureInfo.InvariantCulture.ToString(), l_Number);
            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to parse '{0}' with styles {1}.",
                                  l_Value, CultureInfo.InvariantCulture.ToString());
            }
            Console.WriteLine();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int p_QuestionIndex = 1;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Stack Overflow/SO_Posts_filter.xml");
            string l_XPath = "//row[@PostTypeId='1']";
            XmlNodeList l_ItemNodes = xmlDoc.SelectNodes(l_XPath);

            Console.WriteLine(l_ItemNodes[p_QuestionIndex - 1].Attributes["Id"].Value);
        }*/
    }
}
