using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TopicAnalyser
{
    class Controller
    {
        private List<string> m_Words = new List<string>();
        private List<string[]> m_Values = new List<string[]>();
        private List<string> m_UserList = new List<string>();
        private List<string> m_UserListTraining = new List<string>();
        private List<string> m_UserListTesting = new List<string>();
        private List<User> m_Users = new List<User>();
        private string m_SelectedTrainingSet = "1";

        private int m_EmptyVectorCount = 0;
        private int m_NoExpertsCount = 0;
        private int m_NoExpertsCountExcludingEmptyVector = 0;
        private int m_AnsweredByUnknownUser = 0;
        private int m_AnalysedQuestionCount = 0;
        private int m_Total = 0;

        //Events
        public delegate void SetStatusBarHandler(object sender, CustomArgs ca);
        public event SetStatusBarHandler statusBarUpdated;

//--> Constructor

        public Controller()
        {

        }

//--> Properties

        public string SelectedTrainingSet
        {
            get { return m_SelectedTrainingSet; }
            set { m_SelectedTrainingSet = value; }
        }

        public int EmptyVectorCount
        {
            get { return m_EmptyVectorCount; }
            set { m_EmptyVectorCount = value; }
        }

        public int NoExpertsCount
        {
            get { return m_NoExpertsCount; }
            set { m_NoExpertsCount = value; }
        }

        public int NoExpertsCountExcludingEmptyVector
        {
            get { return m_NoExpertsCountExcludingEmptyVector; }
            set { m_NoExpertsCountExcludingEmptyVector = value; }
        }

        public int AnsweredByUnknownUser
        {
            get { return m_AnsweredByUnknownUser; }
            set { m_AnsweredByUnknownUser = value; }
        }

        public int AnalysedQuestionCount
        {
            get { return m_AnalysedQuestionCount; }
            set { m_AnalysedQuestionCount = value; }
        }

        public int Total
        {
            get { return m_Total; }
            set { m_Total = value; }
        }

//-->Methods

        public void reset()
        {
            m_Words.Clear();
            m_Values.Clear();
            m_UserList.Clear();
            m_UserListTraining.Clear();
            m_UserListTesting.Clear();
            m_Users.Clear();
        }

        private List<double> createVector(string p_Text)
        {
            List<double> l_Vector = new List<double>();
            foreach (string l_Word in m_Words)
            {
                //if (p_Text.Contains(l_Word) & (p_Text.Contains(" " + l_Word) || p_Text.Contains(l_Word + " ")))
                //Extra check above could help with words being part of another word. For example "rel" can easily be part of another word.
                if (p_Text.Contains(l_Word))
                {
                    l_Vector.Add(1.0);
                }
                else
                {
                    l_Vector.Add(0.0);
                }
            }
            return l_Vector;
        }

        /*
         * Default:
         * No frequencies
         */ 
        private void compareVectors(List<double> p_V1)
        {
            double l_CosineSimilarity = 0.0;
            double l_DotProduct = 0.0;
            double l_MagV1 = 0.0;//Question vector
            double l_MagV2 = 0.0;//User vector

            for (int j = 0; j < m_Users.Count; j++)
            {
                List<double> l_V2 = m_Users[j].ExpertiseVector;
                //Cap vector at 1 (Ignore frequencies)
                for (int i = 0; i < l_V2.Count; i++)
                {
                    if (l_V2[i] > 0.0)
                       l_V2[i] = 1.0;
                }
                //Ignore not relevant expertise
                //for (int i = 0; i < l_V2.Count; i++)
                //{
                //    if (l_V2[i] > 0.0 && p_V1[i] == 0.0)
                //        l_V2[i] = 0.0;
                //}

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
                m_Users[j].Expertise = l_CosineSimilarity;
            }
        }

        /*
         * Default:
         * No frequencies
         * No irrelevant terms
         */ 
        private void compareVectors2(List<double> p_V1)
        {
            double l_CosineSimilarity = 0.0;
            double l_DotProduct = 0.0;
            double l_MagV1 = 0.0;//Question vector
            double l_MagV2 = 0.0;//User vector

            for (int j = 0; j < m_Users.Count; j++)
            {
                List<double> l_V2 = m_Users[j].ExpertiseVector.ToList();
                //Ignore not relevant expertise
                for (int i = 0; i < l_V2.Count; i++)
                {
                    if (l_V2[i] == 1.0 && p_V1[i] == 0.0)
                        l_V2[i] = 0.0;
                }

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
                m_Users[j].Expertise = l_CosineSimilarity;
            }
        }

        public void readUsers(string p_Community)
        {
            string l_UserName = "";

            //GG_userlist.csv used to link users with GG_wordlist_tfidf.csv
            foreach (string l_User in File.ReadLines("R/" + p_Community + "_userlist.csv"))
            {
                l_UserName = l_User.Replace("\"", "");
                m_UserList.Add(l_UserName);
            }
            //GG_userlist_training.csv used to link users with GG_wordlist_tfidf.csv
            foreach (string l_User in File.ReadLines("R/" + p_Community + "_userlist_training_" + m_SelectedTrainingSet + ".csv"))
            {
                l_UserName = l_User.Replace("\"", "");
                m_UserListTraining.Add(l_UserName);
            }
            //GG_userlist_testing.csv used to link users with GG_wordlist_tfidf.csv
            foreach (string l_User in File.ReadLines("R/" + p_Community + "_userlist_testing_" + m_SelectedTrainingSet + ".csv"))
            {
                l_UserName = l_User.Replace("\"", "");
                m_UserListTesting.Add(l_UserName);
            }

            //List of unique users
            if (p_Community.Equals("GG"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Google Groups/Output/GG_users.xml");
                string l_XPath = "//Users/User";
                XmlNodeList l_Nodes = xmlDoc.SelectNodes(l_XPath);
                foreach (XmlNode l_Node in l_Nodes)
                {
                    string l_Name = l_Node.Attributes.GetNamedItem("Name").InnerText;
                    string l_Id = l_Node.Attributes.GetNamedItem("Id").InnerText;
                    if (m_Users.FindIndex(item => item.Name == l_Name) < 0)
                    {
                        m_Users.Add(new User(l_Name, l_Id));
                    }
                }
            }
            else//Stack Overflow
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Stack Overflow/SO_Users_filter_hash.xml");
                string l_XPath = "//users/row";
                XmlNodeList l_Nodes = xmlDoc.SelectNodes(l_XPath);
                foreach (XmlNode l_Node in l_Nodes)
                {
                    string l_Name = l_Node.Attributes.GetNamedItem("DisplayName").InnerText;
                    string l_Id = l_Node.Attributes.GetNamedItem("Id").InnerText;
                    if (m_Users.FindIndex(item => item.Id == l_Id) < 0)
                    {
                        m_Users.Add(new User(l_Name, l_Id));
                    }
                }
            }
        }

        public void readWordListTFIDF(string p_Community)
        {
            string l_Path = "";
            if (p_Community.Equals("GG"))
                l_Path = "R/GG_wordlist_tfidf.csv";
            else
                l_Path = "R/SO_wordlist_tfidf.csv";

            //Read column names only (words)
            foreach (string l_Line in File.ReadLines(l_Path))
            {
                m_Words = l_Line.Replace("\"", "").Split(',').ToList();
                break;
            }
        }

        public void readWordListCustom()
        {
            //Read custom word list
            m_Words.Clear();
            foreach (string l_Line in File.ReadLines("R/ggplot2_wordlist.csv"))
            {
                m_Words.Add(l_Line.Substring(1,l_Line.Length-2));
            }
            m_Words.RemoveAt(0);//Remove column
        }

        public void createExpertiseVectorUsersTFIDF(string p_Community)
        {
            bool l_Init = true;
            int l_Counter = 0;
            int l_Index = 0;

            foreach (string l_Line in File.ReadLines("R/" + p_Community + "_wordlist_tfidf_training_" + m_SelectedTrainingSet + ".csv"))
            {
                if (l_Init)
                {
                    //Skip col name csv file
                    l_Init = false;
                }
                else
                {
                    if (p_Community.Equals("GG"))
                    {
                        //Find index of user
                        l_Index = m_Users.FindIndex(item => item.Name == m_UserListTraining[l_Counter]);
                        if (l_Index != -1)
                        {
                            //Update expertise vector
                            m_Users[l_Index].updateExpertiseVector(l_Line.Split(','));
                        }
                    }
                    else
                    {
                        //Find index of user
                        l_Index = m_Users.FindIndex(item => item.Id == m_UserListTraining[l_Counter]);
                        if (l_Index != -1)
                        {
                            //Update expertise vector
                            m_Users[l_Index].updateExpertiseVector(l_Line.Split(','));
                        }
                    }
                    l_Counter++;//Not increased when reading first line (column names)
                }
            }

            //Check for empty expertise vectors
            for (int i = 0; i < m_Users.Count; i++)
            {
                if (m_Users[i].ExpertiseVector.Count == 0)
                {
                    m_Users[i].initExpertiseVector(m_Words.Count);
                }
            }
        }

        public void createExpertiseVectorUsersCustom(string p_Community)
        {
            //int l_Count = 0;
            //Create expertise vectors
            for (int i = 0; i < m_Users.Count; i++)
            {
                string l_ContentAll = "";
                string l_OtherCommunityContent = "";
                XmlDocument doc = new XmlDocument();
                if (p_Community.Equals("GG"))
                {
                    doc.Load("Google Groups/Output/GG_topics_training_" + m_SelectedTrainingSet + ".xml");

                    foreach (XmlNode l_Content in doc.SelectNodes("//Message[User/Name = \"" + m_Users[i].Name + "\"]/content"))
                    {
                        l_ContentAll += l_Content.InnerText;
                    }
                    l_OtherCommunityContent = getPostContentOtherCommunity("GG", "SO", m_Users[i].Id);
                    //if (l_OtherCommunityContent.Length > 0)
                    //    l_Count++;
                    m_Users[i].ExpertiseVector = createVector(l_ContentAll + " " + l_OtherCommunityContent);
                }
                else
                {
                    doc.Load("Stack Overflow/SO_Posts_filter_training_" + m_SelectedTrainingSet + ".xml");

                    foreach (XmlNode l_Node in doc.SelectNodes("//row[@OwnerUserId='" + m_Users[i].Id + "']"))
                    {
                        l_ContentAll += l_Node.Attributes["Body"].Value;
                    }
                    l_OtherCommunityContent = getPostContentOtherCommunity("SO", "GG", m_Users[i].Id);
                    //if (l_OtherCommunityContent.Length > 0)
                    //    l_Count++;
                    m_Users[i].ExpertiseVector = createVector(l_ContentAll + " " + l_OtherCommunityContent);
                }
                //reset
                l_OtherCommunityContent = "";
            }

            //Check for empty expertise vectors
            for (int i = 0; i < m_Users.Count; i++)
            {
                if (m_Users[i].ExpertiseVector.Count == 0)
                {
                    m_Users[i].initExpertiseVector(m_Words.Count);
                }
            }
        }

        public void createExpertiseVectorUsersCustomSurvey(string p_Community)
        {
            //int l_Count = 0;
            //Create expertise vectors
            for (int i = 0; i < m_Users.Count; i++)
            {
                string l_ContentAll = "";
                string l_OtherCommunityContent = "";
                XmlDocument doc = new XmlDocument();
                if (p_Community.Equals("GG"))
                {
                    doc.Load("Google Groups/Output/GG_topics.xml");

                    foreach (XmlNode l_Content in doc.SelectNodes("//Message[User/Name = \"" + m_Users[i].Name + "\"]/content"))
                    {
                        l_ContentAll += l_Content.InnerText;
                    }
                    l_OtherCommunityContent = getPostContentOtherCommunity("GG", "SO", m_Users[i].Id);
                    //if (l_OtherCommunityContent.Length > 0)
                    //    l_Count++;
                    m_Users[i].ExpertiseVector = createVector(l_ContentAll + " " + l_OtherCommunityContent);
                }
                else
                {
                    doc.Load("Stack Overflow/SO_Posts_filter.xml");

                    foreach (XmlNode l_Node in doc.SelectNodes("//row[@OwnerUserId='" + m_Users[i].Id + "']"))
                    {
                        l_ContentAll += l_Node.Attributes["Body"].Value;
                    }
                    l_OtherCommunityContent = getPostContentOtherCommunity("SO", "GG", m_Users[i].Id);
                    //if (l_OtherCommunityContent.Length > 0)
                    //    l_Count++;
                    m_Users[i].ExpertiseVector = createVector(l_ContentAll + " " + l_OtherCommunityContent);
                }
                //reset
                l_OtherCommunityContent = "";
            }

            //Check for empty expertise vectors
            for (int i = 0; i < m_Users.Count; i++)
            {
                if (m_Users[i].ExpertiseVector.Count == 0)
                {
                    m_Users[i].initExpertiseVector(m_Words.Count);
                }
            }
        }

        private string getPostContentOtherCommunity(string p_CommunityORI, string p_CommunityTARGET, string p_Id)
        {
            XmlDocument doc = new XmlDocument();
            string l_Content = "";
            string l_IdOther = "";
            string l_Name = "";

            if (p_CommunityORI.Equals("GG"))
            {
                XElement l_Root = XElement.Load("Users_merged_SO_GG_GH_Stats.xml");
                IEnumerable<XElement> l_Query =
                    from l_Element in l_Root.Elements("row")
                    where (string)l_Element.Attribute("Id_GG") == p_Id
                    select l_Element;

                foreach (XElement l_Element in l_Query)
                {
                    if (l_Element.Attribute("Id_SO") != null)
                    {
                        l_IdOther = l_Element.Attribute("Id_SO").Value;
                    }
                }
                if (l_IdOther.Length > 0)
                {
                    doc.Load("Stack Overflow/SO_Posts_filter.xml");
                    foreach (XmlNode l_Node in doc.SelectNodes("//row[@OwnerUserId='" + l_IdOther + "']"))
                    {
                        l_Content += l_Node.Attributes["Body"].Value;
                    }
                }
            }
            else//Stack Overflow
            {
                XElement l_Root = XElement.Load("Users_merged_SO_GG_GH_Stats.xml");
                IEnumerable<XElement> l_Query =
                    from l_Element in l_Root.Elements("row")
                    where (string)l_Element.Attribute("Id_SO") == p_Id
                    select l_Element;

                foreach (XElement l_Element in l_Query)
                {
                    if (l_Element.Attribute("Id_GG") != null)
                    {
                        l_IdOther = l_Element.Attribute("Id_GG").Value;
                    }
                }
                if (l_IdOther.Length > 0)
                {
                    l_Name = getNameByIdGGFromFile(l_IdOther);
                    doc.Load("Google Groups/Output/GG_topics.xml");
                    foreach (XmlNode l_ContentNode in doc.SelectNodes("//Message[User/Name = \"" + l_Name + "\"]/content"))
                    {
                        l_Content += l_ContentNode.InnerText;
                    }
                }
            }

            return l_Content;
        }

        private string getNameByIdGGFromFile(string p_Id)
        {
            string l_Name = "";

            XElement l_Root = XElement.Load("Google Groups/Output/GG_users.xml");
            IEnumerable<XElement> l_Query =
                from l_Element in l_Root.Elements("User")
                where (string)l_Element.Attribute("Id") == p_Id
                select l_Element;

            foreach (XElement l_Element in l_Query)
            {
                if (l_Element.Attribute("Name") != null)
                {
                    l_Name = l_Element.Attribute("Name").Value;
                }
            }
            return l_Name;
        }

        /*public void removeRegularUsers(string p_Community, int p_Rank)
        {
            string l_Path = "";
            if (p_Community.Equals("GG"))
                l_Path = "Google Groups/Output/GG_stats.xml";
            else
                l_Path = "Stack Overflow/SO_stats.xml";

            //Use ZDegree ranking to remove regular users. We are only interested in experts (Top *p_Rank*)
            XElement l_Xelement = XElement.Load(l_Path);
            IEnumerable<XElement> l_Users = l_Xelement.Elements();
            var l_UsersOrdered = l_Users.OrderByDescending(x => double.Parse(x.Attribute("ZDegree").Value));
            var l_UserRegulars = l_UsersOrdered.Skip(p_Rank);
            foreach (XElement l_UserRegular in l_UserRegulars)
            {
                //if (!l_UserRegular.Attribute("Id").Value.Equals("1187"))
                    m_Users.RemoveAll(x => x.Id == l_UserRegular.Attribute("Id").Value);
            }
        }*/

        public void removeRegularUsers(string p_Community, int p_Rank)
        {
            string l_Path = "Users_merged_SO_GG_GH_Stats.xml";
            string l_Community = "Id_" + p_Community;
            XElement l_Root = XElement.Load(l_Path);
            IEnumerable<XElement> l_Query =
                from l_Xelement in l_Root.Elements("row")
                where l_Xelement.Attribute(l_Community) != null
                select l_Xelement;

            var l_UsersOrdered = l_Query.OrderByDescending(x => double.Parse(x.Attribute("ZDegree").Value, CultureInfo.InvariantCulture));
            var l_UserRegulars = l_UsersOrdered.Skip(p_Rank);
            foreach (XElement l_UserRegular in l_UserRegulars)
            {
                m_Users.RemoveAll(x => x.Id == l_UserRegular.Attribute(l_Community).Value);
            }
        }

        public void analyseTopicExpertise(string p_Community, string p_Method, int p_Rank)
        {
            StreamWriter l_SW_NaN = new StreamWriter(p_Community + "_" + p_Method + "_avg_rank_NaN_" + m_SelectedTrainingSet + ".csv");
            StreamWriter l_SW = new StreamWriter(p_Community + "_" + p_Method + "_avg_rank_" + m_SelectedTrainingSet + ".csv");
            List<double> l_QuestionVector = new List<double>();
            List<int> l_MedianRank = new List<int>();
            string l_Question = "";
            int l_QuestionCounter = 0;
            int l_AnswerCounter = 0;
            double l_Rank = 0.0;
            double l_Ranking = 0.0;

            //Process questions one by one
            l_SW_NaN.WriteLine(p_Method + "_avg_rank;top" + p_Rank + ";all");
            l_SW.WriteLine(p_Method + "_avg_rank;top" + p_Rank + ";all");
            //IEnumerable<XElement> l_Topics = l_Xelement.Elements();
            foreach (string l_Line in File.ReadLines("R/" + p_Community + "_questionlist_testing_" + m_SelectedTrainingSet + ".csv"))
            {
                l_QuestionCounter++;//Start with index 1 because XPath is 1-based
                setStatusBarEvent("Analyse topic expertise: " + l_QuestionCounter);

                if (p_Method.Equals("TFIDF"))
                {
                    //Create expertise vector
                    //GG_TFIDF_avg_rank.csv
                    //GG_TFIDF_avg_rank_NaN.csv
                    //l_Question = getTopicContentTesting(l_QuestionCounter.ToString());
                    //l_QuestionVector = createVector(l_Question);
                    l_QuestionVector = createVector(l_Line);
                    compareVectors(l_QuestionVector);
                }
                else//if (p_Method.Equals("Custom"))
                {
                    //Create expertise vector
                    //GG_Custom_avg_rank_regular.csv - Uses compareVectors
                    //GG_Custom_avg_rank.csv - Uses compareVectors2
                    //GG_Custom_avg_rank_distinct.csv - Uses compareVectors2
                    l_Question = getTopicContentTesting(p_Community, l_QuestionCounter.ToString());
                    l_QuestionVector = createVector(l_Question);
                    //compareVectors(l_QuestionVector);
                    compareVectors2(l_QuestionVector);
                }
                //Sort users by expertise
                m_Users = m_Users.OrderByDescending(x => x.Expertise).ToList();
                List<double> l_ExpertiseList = new List<double>();
                for (int i = 0; i < m_Users.Count; i++)
                {
                    l_ExpertiseList.Add(m_Users[i].Expertise);
                    //Console.WriteLine(m_Users[i].Name + ": " + m_Users[i].Expertise);
                }

                List<string> l_UserList = new List<string>();
                //Google Groups
                if (p_Community.Equals("GG"))
                {
                    //Calc avg rank of all users that answered the question
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load("Google Groups/Output/GG_topics_testing_" + m_SelectedTrainingSet + ".xml");
                    string l_XPath = "//Topic[" + l_QuestionCounter + "]/Messages/Message/User/Name";
                    XmlNodeList itemNodes = xmlDoc.SelectNodes(l_XPath);
                    //Create distinct list of users active in topic
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        if (l_UserList.FindIndex(item => item == itemNode.InnerText) < 0)
                        {
                            l_UserList.Add(itemNode.InnerText);
                        }
                    }
                    //Calc avg rank
                    //- skipping topic starter
                    //- skipping NaN values
                    List<User> l_Users = new List<User>();
                    for (int i = 0; i < m_Users.Count; i++)
                    {
                        if (m_Users[i].Expertise == m_Users[i].Expertise)//Removes NaN values
                        {
                            l_Users.Add(m_Users[i]);
                        }
                    }

                    if (l_Users.Count > 0)
                    {
                        for (int i = 1; i < l_UserList.Count; i++)//Topic starter is at index 0
                        {
                            int l_Index = l_Users.FindIndex(item => item.Name == l_UserList[i]);
                            if (l_Index >= 0)
                            {
                                l_AnswerCounter++;
                                //Default approach
                                l_MedianRank.Add(l_Index);
                                l_Rank += l_Index;
                                //Distinct approach
                                //l_MedianRank.Add(l_Users.Select(x => x.Expertise).Distinct().ToList().IndexOf(l_Users[l_Index].Expertise));
                                //l_Rank += l_Users.Select(x => x.Expertise).Distinct().ToList().IndexOf(l_Users[l_Index].Expertise);
                            }
                        }
                        if (l_AnswerCounter == 0 && l_UserList.Count > 1 && !isEmptyDocumentVector(l_QuestionVector))
                        {
                            AnsweredByUnknownUser++;
                        }
                    }
                    else
                    {
                        //No experts found for this question
                        m_NoExpertsCount++;
                        if (!isEmptyDocumentVector(l_QuestionVector))
                            m_NoExpertsCountExcludingEmptyVector++;
                    }
                }
                else//Stack Overflow
                {
                    //Calc avg rank of all users that answered the question
                    string l_PostId = getQuestionIdSOTesting(l_QuestionCounter);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load("Stack Overflow/SO_Posts_filter_testing_" + m_SelectedTrainingSet + ".xml");
                    string l_XPath = "//row[@ParentId='" + l_PostId + "']";
                    XmlNodeList l_ItemNodes = xmlDoc.SelectNodes(l_XPath);

                    //Create distinct list of users active in topic
                    foreach (XmlNode itemNode in l_ItemNodes)
                    {
                        if (l_UserList.FindIndex(item => item == itemNode.Attributes["OwnerUserId"].Value) < 0)
                        {
                            l_UserList.Add(itemNode.Attributes["OwnerUserId"].Value);
                        }
                    }
                    //Calc avg rank
                    //- skipping NaN values
                    List<User> l_Users = new List<User>();
                    for (int i = 0; i < m_Users.Count; i++)
                    {
                        if (m_Users[i].Expertise == m_Users[i].Expertise)//Removes NaN values
                        {
                            l_Users.Add(m_Users[i]);
                        }
                    }

                    if (l_Users.Count > 0)
                    {
                        for (int i = 0; i < l_UserList.Count; i++)
                        {
                            int l_Index = l_Users.FindIndex(item => item.Id == l_UserList[i]);
                            if (l_Index >= 0)
                            {
                                l_AnswerCounter++;
                                //Default approach
                                l_MedianRank.Add(l_Index);
                                l_Rank += l_Index;
                                //Distinct approach
                                //l_MedianRank.Add(l_Users.Select(x => x.Expertise).Distinct().ToList().IndexOf(l_Users[l_Index].Expertise));
                                //l_Rank += l_Users.Select(x => x.Expertise).Distinct().ToList().IndexOf(l_Users[l_Index].Expertise);
                            }
                        }
                        if (l_AnswerCounter == 0 && l_UserList.Count > 1 && !isEmptyDocumentVector(l_QuestionVector))
                        {
                            AnsweredByUnknownUser++;
                        }
                    }
                    else
                    {
                        //No experts found for this question
                        m_NoExpertsCount++;
                        if (!isEmptyDocumentVector(l_QuestionVector))
                            m_NoExpertsCountExcludingEmptyVector++;
                    }
                }

                if (l_MedianRank.Count > 0)
                {
                    l_Ranking = (double)l_MedianRank.Median();
                }
                else
                {
                    l_Ranking = double.NaN;
                }

                //l_SW_NaN.WriteLine(l_Rank / l_AnswerCounter + ";" + l_AnswerCounter + ";" + l_UserList.Count);
                //if (l_AnswerCounter > 0)
                //    l_SW.WriteLine(l_Rank / l_AnswerCounter + ";" + l_AnswerCounter + ";" + l_UserList.Count);
                l_SW_NaN.WriteLine(l_Ranking + ";" + l_AnswerCounter + ";" + l_UserList.Count);
                if (l_AnswerCounter > 0)
                    l_SW.WriteLine(l_Ranking + ";" + l_AnswerCounter + ";" + l_UserList.Count);

                if (isEmptyDocumentVector(l_QuestionVector))
                {
                    m_EmptyVectorCount++;
                }
                else
                {
                    if (l_AnswerCounter > 0)
                        m_AnalysedQuestionCount++;
                }

                m_Total++;

                //Prepare for next question
                l_AnswerCounter = 0;
                l_Rank = 0.0;
                l_Ranking = 0.0;
                l_MedianRank.Clear();
            }
            l_SW_NaN.Close();
            l_SW.Close();
        }

        public void surveyTopicExpertise(string p_UserId)
        {
            StreamWriter l_SW_Survey = new StreamWriter("GG_Survey_FExpertise_" + p_UserId + ".csv");
            List<double> l_QuestionVector = new List<double>();
            string l_Question = "";
            int l_QuestionCounter = 0;
            int l_Index = -1;
            
            foreach (string l_Line in File.ReadLines("R/GG_questionlist.csv"))
            {
                l_QuestionCounter++;
                setStatusBarEvent("Prepare survey data: " + l_QuestionCounter);

                l_Question = getTopicContentALL("GG", l_QuestionCounter.ToString());
                //Create expertise vector
                //l_QuestionVector = createVector(l_Line);
                l_QuestionVector = createVector(l_Question);
                //compareVectors(l_QuestionVector);
                compareVectors2(l_QuestionVector);
                //Sort users by expertise
                m_Users = m_Users.OrderByDescending(x => x.Expertise).ToList();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Google Groups/Output/GG_topics.xml");
                string l_XPath = "//Topic[" + l_QuestionCounter + "]/Messages/Message/User/Name";
                XmlNodeList itemNodes = xmlDoc.SelectNodes(l_XPath);
                List<string> l_UserList = new List<string>();
                //Create distinct list of users active in topic
                foreach (XmlNode itemNode in itemNodes)
                {
                    if (l_UserList.FindIndex(item => item == itemNode.InnerText) < 0)
                    {
                        l_UserList.Add(itemNode.InnerText);
                    }
                }
                //Category 1 - Participant (l_UserList) and recommended (m_Users) by algorithm
                l_Index = m_Users.FindIndex(item => item.Name == p_UserId);
                if (l_UserList.Contains(p_UserId))
                {
                    if (l_Index >= 0 && (m_Users[l_Index].Expertise == m_Users[l_Index].Expertise))
                    {
                        l_SW_Survey.WriteLine("1;" + getTopicId(l_QuestionCounter.ToString()) + ";" + m_Users[l_Index].Expertise.ToString());
                    }
                }
                //Category 2 - Participant (l_UserList) and NOT recommended (m_Users) by algorithm
                if (l_UserList.Contains(p_UserId))
                {
                    if (l_Index == -1 || (m_Users[l_Index].Expertise != m_Users[l_Index].Expertise))//NAN != NAN == true
                    {
                        l_SW_Survey.WriteLine("2;" + getTopicId(l_QuestionCounter.ToString()) + ";" + m_Users[l_Index].Expertise.ToString());
                        if(l_QuestionVector.Contains(1.0))
                            l_SW_Survey.WriteLine("2+;" + getTopicId(l_QuestionCounter.ToString()) + ";" + m_Users[l_Index].Expertise.ToString());
                    }
                }
                //Category 3 - NOT Participant (l_UserList) and recommended (m_Users) by algorithm
                if (!l_UserList.Contains(p_UserId))
                {
                    if (l_Index >= 0 && (m_Users[l_Index].Expertise == m_Users[l_Index].Expertise))
                    {
                        l_SW_Survey.WriteLine("3;" + getTopicId(l_QuestionCounter.ToString()) + ";" + m_Users[l_Index].Expertise.ToString());
                    }
                }
                //Category 4 - NOT Participant (l_UserList) and NOT recommended (m_Users) by algorithm
                if (!l_UserList.Contains(p_UserId))
                {
                    if (l_Index == -1 || (m_Users[l_Index].Expertise != m_Users[l_Index].Expertise))//NAN != NAN == true
                    {
                        l_SW_Survey.WriteLine("4;" + getTopicId(l_QuestionCounter.ToString()) + ";" + m_Users[l_Index].Expertise.ToString());
                        if (l_QuestionVector.Contains(1.0))
                            l_SW_Survey.WriteLine("4+;" + getTopicId(l_QuestionCounter.ToString()) + ";" + m_Users[l_Index].Expertise.ToString());
                    }
                }
            }
            l_SW_Survey.Close();
        }

        public void surveyTopicExpertiseGH(string p_UserId)
        {
            StreamWriter l_SW_Survey = new StreamWriter("GH_Survey_FExpertise_" + p_UserId + ".csv");
            List<double> l_QuestionVector = new List<double>();
            string l_Question = "";
            int l_QuestionCounter = 0;
            int l_Index = -1;

            foreach (string l_Line in File.ReadLines("R/GH_issuelist.csv"))
            {
                l_QuestionCounter++;
                setStatusBarEvent("Prepare survey data: " + l_QuestionCounter);

                l_Question = getIssueContentALL(l_QuestionCounter.ToString());
                //Create expertise vector
                //l_QuestionVector = createVector(l_Line);
                l_QuestionVector = createVector(l_Question);
                //compareVectors(l_QuestionVector);
                compareVectors2(l_QuestionVector);
                //Sort users by expertise
                m_Users = m_Users.OrderByDescending(x => x.Expertise).ToList();

                //Category 5 - GitHub issue
                l_Index = m_Users.FindIndex(item => item.Name == p_UserId);
                if (l_Index >= 0 && (m_Users[l_Index].Expertise == m_Users[l_Index].Expertise))//NAN == NAN == false
                {
                    l_SW_Survey.WriteLine("5;" + getIssueIdALL(l_QuestionCounter.ToString()) + ";" + m_Users[l_Index].Expertise.ToString());
                }
            }
            l_SW_Survey.Close();
        }

        private string getTopicId(string p_Index)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Google Groups/Output/GG_topics.xml");
            string l_XPath = "//Topic[" + p_Index + "]/ID";
            XmlNode l_Node = xmlDoc.SelectSingleNode(l_XPath);
            return l_Node.InnerText;
        }

        private string getQuestionIdSO(int p_QuestionIndex)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Stack Overflow/SO_Posts_filter.xml");
            string l_XPath = "//row[@PostTypeId='1']";
            XmlNodeList l_ItemNodes = xmlDoc.SelectNodes(l_XPath);

            return l_ItemNodes[p_QuestionIndex - 1].Attributes["Id"].Value;
        }

        private string getQuestionIdSOTesting(int p_QuestionIndex)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Stack Overflow/SO_Posts_filter_testing_" + m_SelectedTrainingSet + ".xml");
            string l_XPath = "//row[@PostTypeId='1']";
            XmlNodeList l_ItemNodes = xmlDoc.SelectNodes(l_XPath);

            return l_ItemNodes[p_QuestionIndex - 1].Attributes["Id"].Value;
        }

        /*
         * Get content of question
         */ 
        private string getTopicContentALL(string p_Community, string p_Index)
        {
            if (p_Community.Equals("GG"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Google Groups/Output/GG_topics.xml");
                string l_XPath = "//Topic[" + p_Index + "]/Messages/Message[1]/content";
                XmlNode l_Node = xmlDoc.SelectSingleNode(l_XPath);
                return l_Node.InnerText;
            }
            else//Stack Overflow
            {
                string l_PostId = getQuestionIdSO(int.Parse(p_Index));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Stack Overflow/SO_Posts_filter.xml");
                string l_XPath = "//row[@Id='" + l_PostId + "']";
                XmlNode l_ItemNode = xmlDoc.SelectSingleNode(l_XPath);
                return l_ItemNode.Attributes["Body"].Value;
            }
        }

        /*
         * Get content of question
         */
        private string getTopicContentTesting(string p_Community, string p_Index)
        {
            if (p_Community.Equals("GG"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Google Groups/Output/GG_topics_testing_" + m_SelectedTrainingSet + ".xml");
                string l_XPath = "//Topic[" + p_Index + "]/Messages/Message[1]/content";
                XmlNode l_Node = xmlDoc.SelectSingleNode(l_XPath);
                return l_Node.InnerText;
            }
            else//Stack Overflow
            {
                string l_PostId = getQuestionIdSOTesting(int.Parse(p_Index));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("Stack Overflow/SO_Posts_filter_testing_" + m_SelectedTrainingSet + ".xml");
                string l_XPath = "//row[@Id='" + l_PostId + "']";
                XmlNode l_ItemNode = xmlDoc.SelectSingleNode(l_XPath);
                return l_ItemNode.Attributes["Body"].Value;
            }
        }

        private string getIssueIdALL(string p_Index)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("GitHub/GH_posts.xml");
            string l_XPath = "//Topic[" + p_Index + "]";
            XmlNode l_Node = xmlDoc.SelectSingleNode(l_XPath);
            l_Node = l_Node.Attributes.GetNamedItem("Id");
            return l_Node.InnerText;
        }

        private string getIssueContentALL(string p_Index)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("GitHub/GH_posts.xml");
            string l_XPath = "//Topic[" + p_Index + "]/Messages/Message[1]";
            XmlNode l_Node = xmlDoc.SelectSingleNode(l_XPath);
            l_Node = l_Node.Attributes.GetNamedItem("Content");
            return l_Node.InnerText;
        }

        private bool isEmptyDocumentVector(List<double> p_QuestionVector)
        {
            bool l_IsEmpty = true;

            for (int i = 0; i < p_QuestionVector.Count; i++)
            {
                if (p_QuestionVector[i] > 0)
                {
                    l_IsEmpty = false;
                    break;
                }
            }
            return l_IsEmpty;
        }



        /*
         * Used for testing and debug purposes only
         */
        public void printUserExpertise(string p_UserId)
        {
            readUsers("GG");
            readWordListCustom();
            removeRegularUsers("GG", 50);
            createExpertiseVectorUsersCustom("GG");

            int l_Index = m_Users.FindIndex(item => item.Name == p_UserId);
            for (int i = 0; i < m_Words.Count; i++)
            {
                if (m_Users[l_Index].ExpertiseVector[i].Equals(1.0))
                    Console.WriteLine(m_Words[i]);
            }
        }

        /*
         * Used for testing and debug purposes only
         */
        public void printTermFrequency()
        {
            readWordListCustom();
            int l_QuestionCounter = 0;
            List<double> l_QuestionVectorTotal = new List<double>(new double[m_Words.Count]);

            foreach (string l_Line in File.ReadLines("R/" + "GG" + "_questionlist.csv"))
            {
                l_QuestionCounter++;
                string l_Question = getTopicContentALL("GG", l_QuestionCounter.ToString());
                List<double> l_QuestionVector = createVector(l_Question);

                for (int i = 0; i < l_QuestionVectorTotal.Count; i++)
                {
                    l_QuestionVectorTotal[i] += l_QuestionVector[i];
                }
            }

            for (int i = 0; i < l_QuestionVectorTotal.Count; i++)
            {
                Console.WriteLine(l_QuestionVectorTotal[i]);
            }
        }

//--> Events

        private void setStatusBarEvent(string p_Message)
        {
            CustomArgs l_CustomArgs = new CustomArgs(p_Message);
            statusBarUpdated(this, l_CustomArgs);
        }

    }
}
