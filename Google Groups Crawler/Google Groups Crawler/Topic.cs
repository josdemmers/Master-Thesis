using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Groups_Crawler
{
    class Topic
    {
        private string m_Title = "";
        private string m_ID = "";

        //Custom classes
        private List<Message> m_Messages = new List<Message>();

//-->Contructor

        public Topic()
        {

        }

        public Topic(string p_Title, string p_ID)
        {
            m_Title = p_Title;
            m_ID = p_ID;
        }

//-->Attributes

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public List<Message> Messages
        {
            get { return m_Messages; }
            set { m_Messages = value; }
        }

//-->Methods

    }
}
