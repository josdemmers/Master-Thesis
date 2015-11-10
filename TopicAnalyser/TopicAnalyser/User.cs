using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicAnalyser
{
    class User
    {
        private string m_Name = "";
        private string m_Id = "-1";
        private double m_Expertise = 0.0;
        private List<double> m_ExpertiseVector = new List<double>();

//--> Constructor

        public User(string p_Name)
        {
            m_Name = p_Name;
        }

        public User(string p_Name, string p_Id)
        {
            m_Name = p_Name;
            m_Id = p_Id;
        }

//--> Attributes

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public List<double> ExpertiseVector
        {
            get { return m_ExpertiseVector; }
            set { m_ExpertiseVector = value; }
        }

        public double Expertise
        {
            get { return m_Expertise; }
            set { m_Expertise = value; }
        }

//-->Methods

        public void updateExpertiseVector(string[] p_ExpertiseVector)
        {
            if (m_ExpertiseVector.Count == 0)
            {
                foreach (string l_Expertise in p_ExpertiseVector)
                {
                    m_ExpertiseVector.Add(double.Parse(l_Expertise, CultureInfo.InvariantCulture));
                }
            }
            else
            {
                for (int i = 0; i < p_ExpertiseVector.Length; i++)
                {
                    m_ExpertiseVector[i] += double.Parse(p_ExpertiseVector[i], CultureInfo.InvariantCulture);
                }
            }
        }

        public void initExpertiseVector(int p_Size)
        {
            for (int i = 0; i < p_Size; i++)
            {
                m_ExpertiseVector.Add(0.0);
            }
        }
    }
}
