using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Parser
{
    class Message
    {
        //int m_UserIndex = -1;
        private string m_UserId = "";
        //string m_Avatar = "";
        //string m_Email = "";
        private string m_Date = "";
        private string m_Content = "";
        private string m_Id = "";
        private string m_ParentId = "";

//-->Constructors

        public Message()
        {

        }

        public Message(string p_UserId, string p_Date, string p_Content, string p_Id, string p_ParentId)
        {
            m_UserId = p_UserId;
            //m_Avatar = p_Avatar;
            m_Date = p_Date;
            m_Content = p_Content;
            m_Id = p_Id;
            m_ParentId = p_ParentId;
        }

//-->Attributes

        //public int UserIndex
        //{
        //    get { return m_UserIndex; }
        //    set { m_UserIndex = value; }
        //}

        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

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

        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }
    }
}
