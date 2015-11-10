using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Parser
{
    class User
    {
        private string m_Id = "";
        //private string m_Name = "";
        //private string m_Avatar = "";
        //private string m_Email = "";
        private int m_Answers = 0;
        private int m_Questions = 0;
        private double m_ZScore = 0.0;
        private double m_ZDegree = 0.0;
        private double m_ExpertiseRank = 1.0;
        private double m_LastExpertiseRank = 1.0;
        private List<int> m_InDegreeList = new List<int>();
        private List<int> m_OutDegreeList = new List<int>();

//-->Constructors

        public User()
        {

        }

        public User(string p_Id)
        {
            m_Id = p_Id;
        }

//-->Attributes

        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        //public string Name
        //{
        //    get { return m_Name; }
        //    set { m_Name = value; }
        //}

        //public string Avatar
        //{
        //    get { return m_Avatar; }
        //    set { m_Avatar = value; }
        //}

        //public string Email
        //{
        //    get { return m_Email; }
        //    set { m_Email = value; }
        //}

        public int Answers
        {
            get { return m_Answers; }
            set { m_Answers = value; }
        }

        public int Questions
        {
            get { return m_Questions; }
            set { m_Questions = value; }
        }

        public double ZScore
        {
            get { return m_ZScore; }
            set { m_ZScore = value; }
        }

        public double ZDegree
        {
            get { return m_ZDegree; }
            set { m_ZDegree = value; }
        }

        public double ExpertiseRank
        {
            get { return m_ExpertiseRank; }
            set { m_ExpertiseRank = value; }
        }

        public double LastExpertiseRank
        {
            get { return m_LastExpertiseRank; }
            set { m_LastExpertiseRank = value; }
        }

        public List<int> OutDegreeList
        {
            get { return m_OutDegreeList; }
            set { m_OutDegreeList = value; }
        }

        public List<int> InDegreeList
        {
            get { return m_InDegreeList; }
            set { m_InDegreeList = value; }
        }

//-->Methods

    }
}
