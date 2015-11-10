using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Overflow_Parser
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

        public void countUserAnswers()
        {
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_IndexAsker = -1;
            int l_Index = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_IndexAsker = getUserIndex(m_Topics[i].Messages[0].UserID);
                    for (int j = 0; j < m_Topics[i].Messages.Count; j++)
                    {
                        l_Index = getUserIndex(m_Topics[i].Messages[j].UserID);
                        if (l_Index != l_IndexAsker)
                            m_Users[l_Index].Answers += 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("---> Exception in countUserAnswers(): " + e.Message);
            }
        }

        public void countUserQuestions()
        {
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_Index = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_Index = getUserIndex(m_Topics[i].Messages[0].UserID);
                    m_Users[l_Index].Questions += 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("---> Exception in countUserQuestions(): " + e.Message);
            }
        }

        public void calcUserZScore()
        {
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
                Console.WriteLine("---> Exception in calcUserZScore(): " + e.Message);
            }
        }

        public void calcUserZDegree()
        {
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
                Console.WriteLine("---> Exception in calcUserZScore(): " + e.Message);
            }
        }

        public void calcInDegree()
        {
            //How many people a user replied to
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_IndexAsker = -1;
            int l_IndexAnswerer = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_IndexAsker = getUserIndex(m_Topics[i].Messages[0].UserID);
                    for (int j = 0; j < m_Topics[i].Messages.Count; j++)
                    {
                        l_IndexAnswerer = getUserIndex(m_Topics[i].Messages[j].UserID);
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
                Console.WriteLine("---> Exception in calcInDegree(): " + e.Message);
            }
        }

        public void calcOutDegree()
        {
            //How many people replied to a user
            IOHandler l_IOHandler = IOHandler.Instance;
            int l_IndexAsker = -1;
            int l_IndexAnswerer = -1;

            try
            {
                for (int i = 0; i < m_Topics.Count; i++)
                {
                    l_IndexAsker = getUserIndex(m_Topics[i].Messages[0].UserID);
                    for (int j = 0; j < m_Topics[i].Messages.Count; j++)
                    {
                        l_IndexAnswerer = getUserIndex(m_Topics[i].Messages[j].UserID);
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
                Console.WriteLine("---> Exception in calcOutDegree(): " + e.Message);
            }
        }

        public void calcExpertiseRank()
        {//Iterative power algorithm
            //LastExpertiseRank and ExpertiseRank are initialized at 1
            int l_Iteration = 0;
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

            foreach (User l_U in m_Users) // for every node on the graph
            {
                Console.WriteLine("--> " + l_U.Id + ": " + l_U.ExpertiseRank);
            }
            Console.WriteLine("--> Iterations done: " + l_Iteration);
        }

        /*public void calcExpertiseRank()
        {
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
                Console.WriteLine("---> Exception in calcExpertiseRank(): " + e.Message);
            }
        }*/

        /*private double expertiseRank(int p_Index, List<int> p_Exclude)
        {
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
                Console.WriteLine("---> Exception in expertiseRank(): " + e.Message);
                return -1;
            }

            return l_ExpertiseRank;
        }*/

        private int getUserIndex(string p_Id)
        {
            int l_Index = -1;

            for (int i = 0; i < Users.Count; i++)
            {
                if (p_Id.Equals(Users[i].Id))
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

        public void sortUsersById()
        {
            m_Users.Sort((x, y) => string.Compare(x.Id, y.Id));
            //var l_Users = m_Users.OrderBy(x=>x.Name).ToList();
        }
    }
}
