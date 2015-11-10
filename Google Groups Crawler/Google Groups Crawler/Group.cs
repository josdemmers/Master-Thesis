using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Groups_Crawler
{
    class Group
    {
        //Custom classes
        private List<Topic> m_Topics = new List<Topic>();
        private List<User> m_Users = new List<User>();

//-->Contructor

        public Group()
        {

        }

//-->Attributes

        public List<Topic> Topics
        {
            get { return m_Topics; }
            set { m_Topics = value; }
        }

        public List<User> Users
        {
            get { return m_Users; }
            set { m_Users = value; }
        }

//-->Methods

        public void addTopic(string p_Title, string p_ID)
        {
            if (isUniqueTopic(p_ID))
            {
                m_Topics.Add(new Topic(p_Title, p_ID));
            }
        }

        public bool isUniqueUser(string p_User, string p_Avatar, string p_Email)
        {
            bool l_IsUnique = true;
            for (int i = 0; i < m_Users.Count; i++)
            {
                if (m_Users[i].Name.Equals(p_User) && m_Users[i].Avatar.Equals(p_Avatar) && m_Users[i].Email.Equals(p_Email))
                {
                    l_IsUnique = false;
                    break;
                }
            }
            return l_IsUnique;
        }

        public bool isUniqueTopic(string p_ID)
        {
            bool l_IsUnique = true;
            for (int i = 0; i < m_Topics.Count; i++)
            {
                if (m_Topics[i].ID.Equals(p_ID))
                {
                    l_IsUnique = false;
                    break;
                }
            }
            return l_IsUnique;
        }

        public void countUserAnswers()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_IndexAsker = -1;
            int l_Index = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_IndexAsker = getUserIndex(m_Topics[i].Messages[0].UserName, m_Topics[i].Messages[0].Avatar);
                    for (int j = 0; j < m_Topics[i].Messages.Count; j++)
                    {
                        l_Index = getUserIndex(m_Topics[i].Messages[j].UserName, m_Topics[i].Messages[j].Avatar);
                        if (l_Index != l_IndexAsker)
                            m_Users[l_Index].Answers += 1;
                    }
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in countUserAnswers(): " + e.Message);
            }
        }

        public void countUserQuestions()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_Index = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_Index = getUserIndex(m_Topics[i].Messages[0].UserName, m_Topics[i].Messages[0].Avatar);
                    m_Users[l_Index].Questions += 1;
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in countUserQuestions(): " + e.Message);
            }
        }

        public void calcUserZScore()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                for (int i = 0; i < Users.Count; i++)
                {
                    Users[i].ZScore = (Users[i].Answers - Users[i].Questions) / (Math.Sqrt(Users[i].Answers + Users[i].Questions));
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in calcUserZScore(): " + e.Message);
            }
        }

        public void calcUserZDegree()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                for (int i = 0; i < Users.Count; i++)
                {
                    Users[i].ZDegree = (Users[i].InDegreeList.Count - Users[i].OutDegreeList.Count) / (Math.Sqrt(Users[i].InDegreeList.Count + Users[i].OutDegreeList.Count));
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in calcUserZScore(): " + e.Message);
            }
        }

        public void calcInDegree()
        {
            //How many people user x helped (user x gives answer)
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_IndexAsker = -1;
            int l_IndexAnswerer = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_IndexAsker = getUserIndex(m_Topics[i].Messages[0].UserName, m_Topics[i].Messages[0].Avatar);
                    for (int j = 0; j < m_Topics[i].Messages.Count; j++)
                    {
                        l_IndexAnswerer = getUserIndex(m_Topics[i].Messages[j].UserName, m_Topics[i].Messages[j].Avatar);
                        if (l_IndexAnswerer != l_IndexAsker)
                        {
                            if (!m_Users[l_IndexAnswerer].InDegreeList.Contains(l_IndexAsker))
                                m_Users[l_IndexAnswerer].InDegreeList.Add(l_IndexAsker);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in calcInDegree(): " + e.Message);
            }
        }

        public void calcOutDegree()
        {
            //How many people helped user x (user x receives answer)
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_IndexAsker = -1;
            int l_IndexAnswerer = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_IndexAsker = getUserIndex(m_Topics[i].Messages[0].UserName, m_Topics[i].Messages[0].Avatar);
                    for (int j = 0; j < m_Topics[i].Messages.Count; j++)
                    {
                        l_IndexAnswerer = getUserIndex(m_Topics[i].Messages[j].UserName, m_Topics[i].Messages[j].Avatar);
                        if (l_IndexAnswerer != l_IndexAsker)
                        {
                            if (!m_Users[l_IndexAsker].OutDegreeList.Contains(l_IndexAnswerer))
                                m_Users[l_IndexAsker].OutDegreeList.Add(l_IndexAnswerer);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in calcOutDegree(): " + e.Message);
            }
        }

        public void calcExpertiseRank2()
        {//Iterative power algorithm
            //LastExpertiseRank and ExpertiseRank are initialized at 1
            int l_Iteration = 0;
            //double p = 0.5;// Damping factor
            double p = Math.Pow(0.85, 2);

            bool l_Converged = false;
            while (!l_Converged) // Loops until change between current and last expertise rank is insignificant
            {
                double l_Change = 0;
                foreach (User l_UserX in m_Users) // Foreach user active in the Google Groups group
                {
                    l_UserX.LastExpertiseRank = l_UserX.ExpertiseRank;
                    double l_Sum = 0;
                    foreach (int l_UserY in l_UserX.InDegreeList) // Foreach user helped by x
                    {
                        l_Sum += m_Users[l_UserY].ExpertiseRank / (m_Users[l_UserY].OutDegreeList.Count); // Expertise rank divided by the total amount of people who helped user y.
                    }
                    l_UserX.ExpertiseRank = (1.0 - p) + p * l_Sum; // The summation of users helped by x as calculated above is multiplied by the damping factor. This value is added to the minimal ExpertiseRank (1.0 - p)

                    l_Change += Math.Abs(l_UserX.ExpertiseRank - l_UserX.LastExpertiseRank);
                }

                if (l_Change < .00001)
                {
                    l_Converged = true;
                }

                l_Iteration++;
            }

            Console.WriteLine("--> Iteration: " + l_Iteration);

            foreach (User l_U in m_Users) // for every node on the graph
            {
                Console.WriteLine("--> " + l_U.Name + ": " + l_U.ExpertiseRank);
            }
        }

        public void calcExpertiseRank()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                for (int i = 0; i < m_Users.Count; i++)
                {
                    List<int> l_Exclude = new List<int>();
                    l_Exclude.Add(i);
                    m_Users[i].ExpertiseRank = expertiseRank(i, l_Exclude);
                    Console.WriteLine("--> " + m_Users[i].ExpertiseRank);
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in calcExpertiseRank(): " + e.Message);
            }
        }

        private double expertiseRank(int p_Index, List<int> p_Exclude)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            double l_ExpertiseRank = 0.0;
            double d = Math.Pow(0.85, 2); //damping factor            

            try
            {
                //Console.WriteLine(p_Index);

                //l_ExpertiseRank = (1 - d);
                for (int i = 0; i < m_Users[p_Index].InDegreeList.Count; i++)
                {
                    if (!p_Exclude.Contains(m_Users[p_Index].InDegreeList[i]))
                    {
                        p_Exclude.Add(m_Users[p_Index].InDegreeList[i]);
                        l_ExpertiseRank += (expertiseRank(m_Users[p_Index].InDegreeList[i], p_Exclude) / m_Users[m_Users[p_Index].InDegreeList[i]].OutDegreeList.Count);
                    }
                }
                //l_ExpertiseRank -= (1 - d);
                l_ExpertiseRank *= d;
                l_ExpertiseRank += (1 - d);
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in expertiseRank(): " + e.Message);
                return -1;
            }

            return l_ExpertiseRank;
        }

        public void calcExpertiseRankTEST()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_Iteration = 0;

            try
            {
                for (int i = 0; i < m_Users.Count; i++)
                {
                    List<int> l_Exclude = new List<int>();
                    l_Exclude.Add(i);
                    m_Users[i].ExpertiseRank = expertiseRankTEST(i, l_Exclude, l_Iteration);
                    Console.WriteLine("--> " + m_Users[i].ExpertiseRank);
                    //reset expertise
                    foreach (User l_User in m_Users)
                    {
                        l_User.ExpertiseRank = 0.0;
                    }
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in calcExpertiseRankTEST(): " + e.Message);
            }
        }

        private double expertiseRankTEST(int p_Index, List<int> p_Exclude, int p_Iteration)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            double l_ExpertiseRank = 0.0;
            double d = Math.Pow(0.85, 2); //damping factor     

            try
            {
                //Console.WriteLine(p_Index);

                //l_ExpertiseRank = (1 - d);
                for (int i = 0; i < m_Users[p_Index].InDegreeList.Count; i++)
                {
                    if (!p_Exclude.Contains(m_Users[p_Index].InDegreeList[i]))
                    //if (p_Iteration > 10)
                    {
                        p_Exclude.Add(m_Users[p_Index].InDegreeList[i]);
                        l_ExpertiseRank += (expertiseRankTEST(m_Users[p_Index].InDegreeList[i], p_Exclude, p_Iteration) / m_Users[m_Users[p_Index].InDegreeList[i]].OutDegreeList.Count);
                    }
                }
                //l_ExpertiseRank -= (1 - d);
                l_ExpertiseRank *= d;
                l_ExpertiseRank += (1 - d);

                p_Iteration++;
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in expertiseRankTEST(): " + e.Message);
                return -1;
            }

            return l_ExpertiseRank;
        }

        private int getUserIndex(string p_Name, string p_Avatar)
        {
            int l_Index = -1;

            for (int i = 0; i < Users.Count; i++)
            {
                if (p_Name.Equals(Users[i].Name) && p_Avatar.Equals(Users[i].Avatar))
                {
                    l_Index = i;
                    break;
                }
            }
            return l_Index;
        }

        public int getTopicIndex(string p_ID)
        {
            int l_Index = -1;

            for (int i = 0; i < Topics.Count; i++)
            {
                if (p_ID.Equals(Topics[i].ID))
                {
                    l_Index = i;
                    break;
                }
            }
            return l_Index;
        }

        public void resetUserStats()
        {
            for (int i = 0; i < Users.Count; i++)
            {
                Users[i].Answers = 0;
                Users[i].Questions = 0;
            }
        }

        public void sortUsersByActivity()
        {
            int l_Activity = 0, l_ActivityNext = 0;
            User l_TempUser = null;
            for (int j = 0; j < Users.Count; j++)
            {
                for (int i = 0; i < Users.Count - 1 - j; i++)
                {
                    l_Activity = Users[i].Answers + Users[i].Questions;
                    l_ActivityNext = Users[i + 1].Answers + Users[i + 1].Questions;
                    if (l_Activity < l_ActivityNext)
                    {
                        l_TempUser = Users[i];
                        Users[i] = Users[i + 1];
                        Users[i + 1] = l_TempUser;
                    }
                }
            }
        }

        public void sortUsersByZScore()
        {
            User l_TempUser = null;
            for (int j = 0; j < Users.Count; j++)
            {
                for (int i = 0; i < Users.Count - 1 - j; i++)
                {
                    if (Users[i].ZScore < Users[i + 1].ZScore)
                    {
                        l_TempUser = Users[i];
                        Users[i] = Users[i + 1];
                        Users[i + 1] = l_TempUser;
                    }
                }
            }
        }

        public void sortUsersByName()
        {
            m_Users.Sort((x, y) => string.Compare(x.Name, y.Name));
            //var l_Users = m_Users.OrderBy(x=>x.Name).ToList();
        }
    }
}
