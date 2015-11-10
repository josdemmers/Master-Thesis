using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace GitHub_Parser
{
    public partial class GitHubParser : Form
    {
        private List<string> m_UserIds = new List<string>();
        private Group m_Group = new Group();

        public GitHubParser()
        {
            InitializeComponent();
        }

        private void buttonGetUserIDs_Click(object sender, EventArgs e)
        {
            buttonGetUserIDs.Enabled = false;
            //Get IDs from issues
            string l_Text = System.IO.File.ReadAllText("github_ggplot2_issues_json.txt");
            int l_Index = 0;
            string l_Search = "\"user\" : {";
            string l_ID = "";
            l_Index = l_Text.IndexOf(l_Search, 0);
            while (l_Index != -1)
            {
                l_Search = "\"id\" : ";
                l_Index = l_Text.IndexOf(l_Search, l_Index);
                l_ID = l_Text.Substring(l_Index + l_Search.Length, l_Text.IndexOf(",", l_Index + l_Search.Length) - (l_Index + l_Search.Length));
                m_UserIds.Add(l_ID);
                //Search next
                l_Search = "\"user\" : {";
                l_Index = l_Text.IndexOf(l_Search, l_Index);
            }

            //Get IDs from issue_comments
            l_Text = System.IO.File.ReadAllText("github_ggplot2_issue-comments_json.txt");
            l_Search = "\"user\" : {";
            l_Index = l_Text.IndexOf(l_Search, 0);
            while (l_Index != -1)
            {
                l_Search = "\"id\" : ";
                l_Index = l_Text.IndexOf(l_Search, l_Index);
                l_ID = l_Text.Substring(l_Index + l_Search.Length, l_Text.IndexOf(",", l_Index + l_Search.Length) - (l_Index + l_Search.Length));
                //remove new line
                //if(l_ID.Contains("}"))
                //    l_ID = l_ID.Substring(0, l_ID.IndexOf("\r\n"));
                if (l_ID.Contains(" "))
                    l_ID = l_ID.Substring(0, l_ID.IndexOf(" "));
                m_UserIds.Add(l_ID);
                //Search next
                l_Search = "\"user\" : {";
                l_Index = l_Text.IndexOf(l_Search, l_Index);
            }
            m_UserIds = m_UserIds.Distinct().ToList();
            m_UserIds = m_UserIds.OrderBy(a => int.Parse(a)).ToList();
            buttonGetUserIDs.Enabled = true;
            buttonCreateQuery.Enabled = true;
        }

        private void buttonCreateQuery_Click(object sender, EventArgs e)
        {
            buttonCreateQuery.Enabled = false;
            TextWriter l_TwUsers = new StreamWriter("GH_users_json_query.txt", false);
            string l_Ids = "";

            //{ id:
            //{ { $in: [4196, 463252 ] }
            //}

            l_TwUsers.WriteLine("{ id: {$in: [");
            for (int i = 0; i < m_UserIds.Count; i++)
            {
                if(i + 1 < m_UserIds.Count)
                    l_Ids += m_UserIds[i] + ",";
                else
                    l_Ids += m_UserIds[i];
            }
            l_TwUsers.WriteLine(l_Ids);
            l_TwUsers.WriteLine("]}}");

            l_TwUsers.Close();
            buttonCreateQuery.Enabled = true;
        }

        private void buttonFixJSONFormat_Click(object sender, EventArgs e)
        {
            buttonFixJSONFormat.Enabled = false;

            TextWriter l_TwUsers = new StreamWriter("GH_users_json_fixedformat-temp.txt", false);
            string l_Json = "";

            //First step - users
            using (StreamReader l_Reader = new StreamReader("github_ggplot2_users_json.txt"))
            {
                while (true)
                {
                    string l_Line = l_Reader.ReadLine();
                    if (l_Line == null)
                    {
                        break;
                    }
                    else
                    {
                        l_TwUsers.WriteLine(l_Line + ",");
                    }
                }
            }
            l_TwUsers.Close();

            //Second step - users
            l_TwUsers = new StreamWriter("GH_users_json_fixedformat.txt", false);
            //using (StreamReader l_Reader = new StreamReader("GH_users_json_fixedformat-temp.txt"))
            using (StreamReader l_Reader = new StreamReader("GH_users_json_fixedformat-temp.txt"))
            {
                l_Json = l_Reader.ReadToEnd();
                l_Json = "[" + Environment.NewLine + l_Json + Environment.NewLine + "]";
                l_TwUsers.Write(l_Json);
            }
            l_TwUsers.Close();

            //First step - issues
            l_TwUsers = new StreamWriter("GH_issues_json_fixedformat-temp.txt", false);
            using (StreamReader l_Reader = new StreamReader("github_ggplot2_issues_json.txt"))
            {
                while (true)
                {
                    string l_Line = l_Reader.ReadLine();
                    if (l_Line == null)
                    {
                        break;
                    }
                    else
                    {
                        l_TwUsers.WriteLine(l_Line + ",");
                    }
                }
            }
            l_TwUsers.Close();

            //Second step - issues
            l_TwUsers = new StreamWriter("GH_issues_json_fixedformat.txt", false);
            using (StreamReader l_Reader = new StreamReader("GH_issues_json_fixedformat-temp.txt"))
            {
                l_Json = l_Reader.ReadToEnd();
                l_Json = "[" + Environment.NewLine + l_Json + Environment.NewLine + "]";
                l_TwUsers.Write(l_Json);
            }
            l_TwUsers.Close();

            //First step - issues comments
            l_TwUsers = new StreamWriter("GH_issue-comments_json_fixedformat-temp.txt", false);
            using (StreamReader l_Reader = new StreamReader("github_ggplot2_issue-comments_json.txt"))
            {
                while (true)
                {
                    string l_Line = l_Reader.ReadLine();
                    if (l_Line == null)
                    {
                        break;
                    }
                    else
                    {
                        l_TwUsers.WriteLine(l_Line + ",");
                    }
                }
            }
            l_TwUsers.Close();

            //Second step - issues
            l_TwUsers = new StreamWriter("GH_issue-comments_json_fixedformat.txt", false);
            using (StreamReader l_Reader = new StreamReader("GH_issue-comments_json_fixedformat-temp.txt"))
            {
                l_Json = l_Reader.ReadToEnd();
                l_Json = "[" + Environment.NewLine + l_Json + Environment.NewLine + "]";
                l_TwUsers.Write(l_Json);
            }
            l_TwUsers.Close();

            buttonFixJSONFormat.Enabled = true;
        }

        private void buttonUsersJSONtoXML_Click(object sender, EventArgs e)
        {
            buttonUsersJSONtoXML.Enabled = false;

            string l_Json = "";
            TextWriter l_TwUsers = new StreamWriter("GH_users.xml", false);
            l_TwUsers.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            l_TwUsers.WriteLine("<users>");

            using (StreamReader l_Reader = new StreamReader("GH_users_json_fixedformat.txt"))
            {
                l_Json = l_Reader.ReadToEnd();
                dynamic l_Array = JsonConvert.DeserializeObject(l_Json);
                foreach (var l_Item in l_Array)
                {
                    XElement l_Element = new XElement("row");
                    l_Element.SetAttributeValue("Id", l_Item.id);// checkElementAttribute(l_Item.id));
                    l_Element.SetAttributeValue("Login", checkElementAttribute(l_Item.login));
                    l_Element.SetAttributeValue("Name", checkElementAttribute(l_Item.name));
                    l_Element.SetAttributeValue("GravatarId", checkElementAttribute(l_Item.gravatar_id));
                    l_Element.SetAttributeValue("Email", checkElementAttribute(l_Item.email));
                    //Add user to xml
                    l_TwUsers.WriteLine(l_Element.ToString());
                    //Console.WriteLine("{0} {1} {2} {3} {4}", l_Item.id, l_Item.login, l_Item.name, l_Item.gravatar_id, l_Item.email);
                }
            }

            l_TwUsers.WriteLine("</users>");
            l_TwUsers.Close();

            buttonUsersJSONtoXML.Enabled = true;
        }

        private dynamic checkElementAttribute(dynamic p_Item)
        {
            if (string.IsNullOrEmpty((string)p_Item))
                return null;
            else
                return p_Item;
        }

        private void buttonPostsJSONtoXML_Click(object sender, EventArgs e)
        {
            buttonPostsJSONtoXML.Enabled = false;

            int l_Index = 0;
            string l_Json = "";
            XmlTextWriter l_Writer = new XmlTextWriter("GH_posts.xml", Encoding.UTF8);
            l_Writer.Formatting = System.Xml.Formatting.Indented;
            
            using (StreamReader l_Reader = new StreamReader("GH_issues_json_fixedformat.txt"))
            {
                l_Json = l_Reader.ReadToEnd();
                dynamic l_Array = JsonConvert.DeserializeObject(l_Json);
                foreach (var l_Item in l_Array)
                {
                    m_Group.Topics.Add(new Topic((string)l_Item.title, (string)l_Item.number));
                    l_Index = m_Group.Topics.FindIndex(item => item.Id == (string)l_Item.number);
                    m_Group.Topics[l_Index].Messages.Add(new Message((string)l_Item.user.id, (string)l_Item.created_at, (string)l_Item.body, "0", (string)l_Item.number));
                    //Add if new user
                    if (m_Group.Users.FindIndex(item => item.Id == (string)l_Item.user.id) < 0)
                    {
                        m_Group.Users.Add(new User((string)l_Item.user.id));
                    }
                }
            }

            using (StreamReader l_Reader = new StreamReader("GH_issue-comments_json_fixedformat.txt"))
            {
                l_Json = l_Reader.ReadToEnd();
                dynamic l_Array = JsonConvert.DeserializeObject(l_Json);
                foreach (var l_Item in l_Array)
                {
                    l_Index = m_Group.Topics.FindIndex(item => item.Id == (string)l_Item.issue_id);
                    m_Group.Topics[l_Index].Messages.Add(new Message((string)l_Item.user.id, (string)l_Item.created_at, (string)l_Item.body, (string)l_Item.id, (string)l_Item.issue_id));
                    //Add if new user
                    if (m_Group.Users.FindIndex(item => item.Id == (string)l_Item.user.id) < 0)
                    {
                        m_Group.Users.Add(new User((string)l_Item.user.id));
                    }
                }
            }

            //Write xml
            l_Writer.WriteStartDocument();
            l_Writer.WriteComment("ggplot2 " + m_Group.Topics.Count + " issues.");
            l_Writer.WriteStartElement("Topics");

            for (int i = 0; i < m_Group.Topics.Count; i++)
            {
                l_Writer.WriteStartElement("Topic");
                l_Writer.WriteAttributeString("Id", m_Group.Topics[i].Id);
                l_Writer.WriteAttributeString("Title", m_Group.Topics[i].Title);
                //l_Writer.WriteStartElement("Title");
                //l_Writer.WriteString(m_Group.Topics[i].Title);
                //l_Writer.WriteEndElement();
                //l_Writer.WriteStartElement("Id");
                //l_Writer.WriteString(m_Group.Topics[i].Id);
                //l_Writer.WriteEndElement();
                l_Writer.WriteStartElement("Messages");

                for (int j = 0; j < m_Group.Topics[i].Messages.Count; j++)
                {
                    l_Writer.WriteStartElement("Message");
                    l_Writer.WriteAttributeString("Id", m_Group.Topics[i].Messages[j].Id);
                    l_Writer.WriteAttributeString("ParentId", m_Group.Topics[i].Messages[j].ParentId);
                    l_Writer.WriteAttributeString("UserId", m_Group.Topics[i].Messages[j].UserId);
                    l_Writer.WriteAttributeString("Date", m_Group.Topics[i].Messages[j].Date);
                    l_Writer.WriteAttributeString("Content", m_Group.Topics[i].Messages[j].Content);
                    //l_Writer.WriteStartElement("Id");
                    //l_Writer.WriteString(m_Group.Topics[i].Messages[j].Id);
                    //l_Writer.WriteEndElement();
                    //l_Writer.WriteStartElement("ParentId");
                    //l_Writer.WriteString(m_Group.Topics[i].Messages[j].ParentId);
                    //l_Writer.WriteEndElement();
                    //l_Writer.WriteStartElement("UserId");
                    //l_Writer.WriteString(m_Group.Topics[i].Messages[j].UserId);
                    //l_Writer.WriteEndElement();
                    //l_Writer.WriteStartElement("Date");
                    //l_Writer.WriteString(m_Group.Topics[i].Messages[j].Date);
                    //l_Writer.WriteEndElement();
                    //l_Writer.WriteStartElement("Content");
                    //l_Writer.WriteString(m_Group.Topics[i].Messages[j].Content);
                    //l_Writer.WriteEndElement();
                    l_Writer.WriteEndElement();//~Message
                }

                l_Writer.WriteEndElement();//~Messages
                l_Writer.WriteEndElement();//~Topic
            }

            l_Writer.WriteEndElement();//~Topics
            l_Writer.WriteEndDocument();
            l_Writer.Flush();
            l_Writer.Close();

            buttonPostsJSONtoXML.Enabled = true;
            buttonStats.Enabled = true;
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {
            IOHandler l_IOHandler = IOHandler.Instance;
            buttonStats.Enabled = false;

            //Calc stats
            m_Group.resetUserStats();
            m_Group.countUserAnswers();
            m_Group.countUserQuestions();
            m_Group.calcUserZScore();
            m_Group.calcInDegree();
            m_Group.calcOutDegree();
            m_Group.calcUserZDegree();//Call after in-, out-degree
            //m_Group.calcExpertiseRank();//Call after in-, out-degree
            m_Group.calcExpertiseRank2();//Call after in-, out-degree

            l_IOHandler.exportStats(m_Group.Users);

            buttonStats.Enabled = true;
        }

        
    }
}
