using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Groups_Crawler
{
    class Message
    {
        int m_UserIndex = -1;
        string m_UserName = "";
        string m_Avatar = "";
        string m_Email = "";
        string m_Date = "";
        string m_Content = "";

//-->Constructors

        public Message()
        {

        }

        //public Message(string p_UserName, string p_Avatar, string p_Date, string p_Content)
        //{
        //    m_UserName = p_UserName;
        //    m_Avatar = p_Avatar;
        //    m_Date = p_Date;
        //    m_Content = p_Content;
        //}

        public Message(string p_UserName, string p_Avatar, string p_Date, string p_Content, string p_Email)
        {
            m_UserName = p_UserName;
            m_Avatar = p_Avatar;
            m_Date = p_Date;
            m_Content = p_Content;
            m_Email = p_Email;
        }

//-->Attributes

        public int UserIndex
        {
            get { return m_UserIndex; }
            set { m_UserIndex = value; }
        }

        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        public string Avatar
        {
            get { return m_Avatar; }
            set { m_Avatar = value; }
        }

        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }

        public string Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }

        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }
    }
}
