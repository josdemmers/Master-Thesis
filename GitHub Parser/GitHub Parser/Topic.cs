using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Parser
{
    class Topic
    {
        private string m_Title = "";
        private string m_Id = "";

        //Custom classes
        private List<Message> m_Messages = new List<Message>();

//-->Contructor

        public Topic()
        {

        }

        public Topic(string p_Title, string p_Id)
        {
            m_Title = p_Title;
            m_Id = p_Id;
        }

//-->Attributes

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public List<Message> Messages
        {
            get { return m_Messages; }
            set { m_Messages = value; }
        }

//-->Methods

    }
}
