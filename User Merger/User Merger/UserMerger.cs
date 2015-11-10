using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace User_Merger
{
    public partial class UserMerger : Form
    {
        public UserMerger()
        {
            InitializeComponent();
        }

        private void buttonStackOverflow_Click(object sender, EventArgs e)
        {
            buttonStackOverflow.Enabled = false;
            //openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //openFileDialog1.RestoreDirectory = true;
            //DialogResult l_Result = openFileDialog1.ShowDialog();
            //if (l_Result == DialogResult.OK)
            //{
                TextWriter l_TwUsers = new StreamWriter("Users_merged_SO.xml", false);
                l_TwUsers.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                l_TwUsers.WriteLine("<users>");

                try
                {
                    int l_ID = 0;
                    //open SO_Users_filter_hash.xml
                    //using (XmlReader l_Reader = XmlReader.Create(openFileDialog1.FileName))
                    using (XmlReader l_Reader_Ori = XmlReader.Create("Stack Overflow/SO_Users_filter_hash.xml"))
                    {
                        while (l_Reader_Ori.Read())
                        {
                            if (l_Reader_Ori.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader_Ori.Name == "row")
                                {
                                    XElement l_Element_Ori = XNode.ReadFrom(l_Reader_Ori) as XElement;
                                    if (l_Element_Ori != null)
                                    {
                                        XElement l_Element = new XElement("row");
                                        l_Element.SetAttributeValue("Id", l_ID.ToString());
                                        l_ID++;
                                        l_Element.SetAttributeValue("Id_SO", l_Element_Ori.Attribute("Id").Value);
                                        if(l_Element_Ori.Attribute("EmailHash") != null)
                                            l_Element.SetAttributeValue("EmailHash_SO", l_Element_Ori.Attribute("EmailHash").Value);
                                        if (l_Element_Ori.Attribute("ProfileImageUrl") != null)
                                            l_Element.SetAttributeValue("ProfileImageUrl_SO", l_Element_Ori.Attribute("ProfileImageUrl").Value);

                                        //Add user to xml
                                        l_TwUsers.WriteLine(l_Element.ToString());
                                    }
                                }
                            }
                        }
                    }

                    l_TwUsers.WriteLine("</users>");
                    l_TwUsers.Close();
                    buttonStackOverflow.Enabled = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("---> " + ex.Message);
                }
            //}
        }

        private void buttonGoogleGroups_Click(object sender, EventArgs e)
        {
            buttonGoogleGroups.Enabled = false;
            //openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //openFileDialog1.RestoreDirectory = true;
            //DialogResult l_Result = openFileDialog1.ShowDialog();
            //if (l_Result == DialogResult.OK)
            //{

            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                TextWriter l_TwUsers = new StreamWriter("Users_merged_SO_GG.xml", false);
                l_TwUsers.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                l_TwUsers.WriteLine("<users>");

                try
                {
                    //Get newest id
                    XDocument l_XDoc = XDocument.Load("Users_merged_SO.xml");
                    int l_ID = l_XDoc.Descendants("row").Max(x => (int)x.Attribute("Id"));
                    l_ID++;

                    string l_Hash = "";
                    bool l_HashOK = false;
                    List<string> l_AddedUsers = new List<string>();

                    //using (XmlReader l_Reader = XmlReader.Create(openFileDialog1.FileName))
                    using (XmlReader l_Reader_Ori = XmlReader.Create("Users_merged_SO.xml"))
                    {
                        while (l_Reader_Ori.Read())
                        {
                            if (l_Reader_Ori.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader_Ori.Name == "row")
                                {
                                    XElement l_Element_Ori = XNode.ReadFrom(l_Reader_Ori) as XElement;
                                    if (l_Element_Ori != null)
                                    {
                                        XElement l_ElementNew = new XElement("row");

                                        if (l_Element_Ori.Attribute("EmailHash_SO") != null)
                                        {
                                            //Open Google Groups user list to search for matching email address
                                            using (XmlReader l_Reader = XmlReader.Create("Google Groups/Output/GG_users.xml"))
                                            {
                                                while (l_Reader.Read())
                                                {
                                                    if (l_Reader.NodeType == XmlNodeType.Element)
                                                    {
                                                        if (l_Reader.Name == "User")
                                                        {
                                                            XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                                            if (l_Element != null)
                                                            {
                                                                if (l_Element.Attribute("Email") != null)
                                                                {
                                                                    string[] l_EmailList = l_Element.Attribute("Email").Value.Split(';');
                                                                    for (int i = 0; i < l_EmailList.Length; i++)
                                                                    {
                                                                        using (MD5 md5Hash = MD5.Create())
                                                                        {
                                                                            l_Hash = GetMd5Hash(md5Hash, l_EmailList[i]);
                                                                        }
                                                                        if (l_Element_Ori.Attribute("EmailHash_SO").Value == l_Hash)
                                                                            l_HashOK = true;
                                                                        if (l_Element_Ori.Attribute("ProfileImageUrl_SO") != null)
                                                                        {
                                                                            if (l_Element_Ori.Attribute("ProfileImageUrl_SO").Value.Contains(l_Hash))
                                                                                l_HashOK = true;
                                                                        }
                                                                    }
                                                                    if (l_HashOK)
                                                                    {
                                                                        l_BW.ReportProgress(0, "SO:" + l_Element_Ori.Attribute("Id_SO").Value + " matches GG:" + l_Element.Attribute("Id").Value);
                                                                        //Check if email matches hash
                                                                        l_ElementNew = l_Element_Ori;
                                                                        l_ElementNew.SetAttributeValue("Id_GG", l_Element.Attribute("Id").Value);
                                                                        l_ElementNew.SetAttributeValue("Email_GG", l_Element.Attribute("Email").Value);
                                                                        //Add user to xml
                                                                        l_TwUsers.WriteLine(l_ElementNew.ToString());
                                                                        //Keep track of added users
                                                                        l_AddedUsers.Add(l_Element.Attribute("Id").Value);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }//End of google groups read
                                        }
                                        if (!l_HashOK)
                                        {
                                            //Add user to xml
                                            l_TwUsers.WriteLine(l_Element_Ori.ToString());
                                        }
                                        //reset hash check
                                        l_HashOK = false;
                                    }
                                }
                            }
                        }
                    }
                    //Add remaining Google Groups IDs. i.e. Google Groups users with no matching email or no email at all.
                    using (XmlReader l_Reader = XmlReader.Create("Google Groups/Output/GG_users.xml"))
                    {
                        while (l_Reader.Read())
                        {
                            if (l_Reader.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader.Name == "User")
                                {
                                    XElement l_ElementNew = new XElement("row");
                                    XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                    if (l_Element != null)
                                    {
                                        if (!l_AddedUsers.Contains(l_Element.Attribute("Id").Value))
                                        {
                                            l_ElementNew.SetAttributeValue("Id", l_ID.ToString());
                                            l_ID++;
                                            l_ElementNew.SetAttributeValue("Id_GG", l_Element.Attribute("Id").Value);
                                            if (l_Element.Attribute("Email") != null)
                                                l_ElementNew.SetAttributeValue("Email_GG", l_Element.Attribute("Email").Value);
                                            //Add user to xml
                                            l_TwUsers.WriteLine(l_ElementNew.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }//End of google groups read


                    l_TwUsers.WriteLine("</users>");
                    l_TwUsers.Close();
                }
                catch (Exception ex)
                {
                    l_TwUsers.WriteLine("</users>");
                    l_TwUsers.Close();
                    Console.WriteLine("---> " + ex.Message);
                }
                //}
                l_BW.ReportProgress(0,"Ready");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonGoogleGroups.Enabled = true;
            };
            l_BW.RunWorkerAsync();
        }

        private void buttonGitHub_Click(object sender, EventArgs e)
        {
            buttonGitHub.Enabled = false;

            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {

                TextWriter l_TwUsers = new StreamWriter("Users_merged_SO_GG_GH.xml", false);
                l_TwUsers.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                l_TwUsers.WriteLine("<users>");

                try
                {
                    //Get newest id
                    XDocument l_XDoc = XDocument.Load("Users_merged_SO_GG.xml");
                    int l_ID = l_XDoc.Descendants("row").Max(x => (int)x.Attribute("Id"));
                    l_ID++;

                    string l_Hash = "";
                    bool l_MatchFound = false;
                    List<string> l_AddedUsers = new List<string>();

                    using (XmlReader l_Reader_Ori = XmlReader.Create("Users_merged_SO_GG.xml"))
                    {
                        while (l_Reader_Ori.Read())
                        {
                            if (l_Reader_Ori.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader_Ori.Name == "row")
                                {
                                    XElement l_Element_Ori = XNode.ReadFrom(l_Reader_Ori) as XElement;
                                    if (l_Element_Ori != null)
                                    {
                                        XElement l_ElementNew = new XElement("row");

                                        if (l_Element_Ori.Attribute("EmailHash_SO") != null || l_Element_Ori.Attribute("Email_GG") != null || l_Element_Ori.Attribute("ProfileImageUrl_SO") != null)
                                        {
                                            //Open GitHub user list to search for matching email address and/or hash
                                            using (XmlReader l_Reader = XmlReader.Create("GitHub/GH_users.xml"))
                                            {
                                                while (l_Reader.Read())
                                                {
                                                    if (l_Reader.NodeType == XmlNodeType.Element)
                                                    {
                                                        if (l_Reader.Name == "row")
                                                        {
                                                            XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                                            if (l_Element != null)
                                                            {
                                                                //Check Google Groups email for matches
                                                                if (l_Element_Ori.Attribute("Email_GG") != null)
                                                                {
                                                                    //GH email -> GG email
                                                                    if (l_Element.Attribute("Email") != null)
                                                                    {
                                                                        string[] l_EmailList = l_Element_Ori.Attribute("Email_GG").Value.Split(';');
                                                                        for (int i = 0; i < l_EmailList.Length; i++)
                                                                        {
                                                                            if (l_EmailList[i] == l_Element.Attribute("Email").Value)
                                                                                l_MatchFound = true;
                                                                        }
                                                                    }
                                                                    //GH gravatar -> GG email (hashed)
                                                                    if (!l_MatchFound)
                                                                    {
                                                                        if (l_Element.Attribute("GravatarId") != null)
                                                                        {
                                                                            string[] l_EmailList = l_Element_Ori.Attribute("Email_GG").Value.Split(';');
                                                                            for (int i = 0; i < l_EmailList.Length; i++)
                                                                            {
                                                                                using (MD5 md5Hash = MD5.Create())
                                                                                {
                                                                                    l_Hash = GetMd5Hash(md5Hash, l_EmailList[i]);
                                                                                }
                                                                                if (l_Element.Attribute("GravatarId").Value == l_Hash)
                                                                                    l_MatchFound = true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //Check Stack Overflow hash for matches
                                                                if (l_Element_Ori.Attribute("EmailHash_SO") != null)
                                                                {
                                                                    //GH email -> SO hash
                                                                    if (!l_MatchFound)
                                                                    {
                                                                        if (l_Element.Attribute("Email") != null)
                                                                        {
                                                                            using (MD5 md5Hash = MD5.Create())
                                                                            {
                                                                                l_Hash = GetMd5Hash(md5Hash, l_Element.Attribute("Email").Value);
                                                                            }
                                                                            if (l_Element_Ori.Attribute("EmailHash_SO").Value == l_Hash)
                                                                                l_MatchFound = true;
                                                                        }
                                                                    }
                                                                    //GH gravatar -> SO hash
                                                                    if (!l_MatchFound)
                                                                    {
                                                                        if (l_Element.Attribute("GravatarId") != null)
                                                                        {
                                                                            if (l_Element.Attribute("GravatarId").Value == l_Element_Ori.Attribute("EmailHash_SO").Value)
                                                                                l_MatchFound = true;
                                                                        }
                                                                    }
                                                                }
                                                                //Check Stack Overflow ProfileImageUrl for matching hash
                                                                if (l_Element_Ori.Attribute("ProfileImageUrl_SO") != null)
                                                                {
                                                                    //GH email -> SO ProfileImageUrl
                                                                    if (!l_MatchFound)
                                                                    {
                                                                        if (l_Element.Attribute("Email") != null)
                                                                        {
                                                                            using (MD5 md5Hash = MD5.Create())
                                                                            {
                                                                                l_Hash = GetMd5Hash(md5Hash, l_Element.Attribute("Email").Value);
                                                                            }
                                                                            if (l_Element_Ori.Attribute("ProfileImageUrl_SO").Value.Contains(l_Hash))
                                                                                l_MatchFound = true;
                                                                        }
                                                                    }
                                                                    //GH gravatar -> SO ProfileImageUrl
                                                                    if (!l_MatchFound)
                                                                    {
                                                                        if (l_Element.Attribute("GravatarId") != null)
                                                                        {
                                                                            if (l_Element_Ori.Attribute("ProfileImageUrl_SO").Value.Contains(l_Element.Attribute("GravatarId").Value))
                                                                                l_MatchFound = true;
                                                                        }
                                                                    }
                                                                }
                                                                if (l_MatchFound)
                                                                {
                                                                    l_BW.ReportProgress(0, "Match found for GH user: " + l_Element.Attribute("Id").Value);
                                                                    //Check if email matches email
                                                                    l_ElementNew = l_Element_Ori;
                                                                    if (l_Element.Attribute("Id") != null)
                                                                        l_ElementNew.SetAttributeValue("Id_GH", l_Element.Attribute("Id").Value);
                                                                    if (l_Element.Attribute("Email") != null)
                                                                        l_ElementNew.SetAttributeValue("Email_GH", l_Element.Attribute("Email").Value);
                                                                    if (l_Element.Attribute("GravatarId") != null)
                                                                        l_ElementNew.SetAttributeValue("GravatarId_GH", l_Element.Attribute("GravatarId").Value);
                                                                    //Add user to xml
                                                                    l_TwUsers.WriteLine(l_ElementNew.ToString());
                                                                    //Keep track of added users
                                                                    l_AddedUsers.Add(l_Element.Attribute("Id").Value);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }//End of GitHub read
                                        }
                                        if (!l_MatchFound)
                                        {
                                            //Add user to xml
                                            l_TwUsers.WriteLine(l_Element_Ori.ToString());
                                        }
                                        //reset match check
                                        l_MatchFound = false;
                                    }
                                }
                            }
                        }
                    }
                    //Add remaining GitHub users. i.e. GitHub users with no matching email or no email at all.
                    using (XmlReader l_Reader = XmlReader.Create("GitHub/GH_users.xml"))
                    {
                        while (l_Reader.Read())
                        {
                            if (l_Reader.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader.Name == "row")
                                {
                                    XElement l_ElementNew = new XElement("row");
                                    XElement l_Element = XNode.ReadFrom(l_Reader) as XElement;
                                    if (l_Element != null)
                                    {
                                        if (!l_AddedUsers.Contains(l_Element.Attribute("Id").Value))
                                        {
                                            l_ElementNew.SetAttributeValue("Id", l_ID.ToString());
                                            l_ID++;
                                            l_ElementNew.SetAttributeValue("Id_GH", l_Element.Attribute("Id").Value);
                                            if (l_Element.Attribute("Email") != null)
                                                l_ElementNew.SetAttributeValue("Email_GH", l_Element.Attribute("Email").Value);
                                            if (l_Element.Attribute("GravatarId") != null)
                                                l_ElementNew.SetAttributeValue("GravatarId_GH", l_Element.Attribute("GravatarId").Value);
                                            //Add user to xml
                                            l_TwUsers.WriteLine(l_ElementNew.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }//End of GitHub read


                    l_TwUsers.WriteLine("</users>");
                    l_TwUsers.Close();
                }
                catch (Exception ex)
                {
                    l_TwUsers.WriteLine("</users>");
                    l_TwUsers.Close();
                    Console.WriteLine("---> " + ex.Message);
                }
                l_BW.ReportProgress(0, "Ready");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonGitHub.Enabled = true;
            };
            l_BW.RunWorkerAsync();
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {
            buttonStats.Enabled = false;

            var l_BW = new BackgroundWorker();
            l_BW.WorkerReportsProgress = true;
            l_BW.DoWork += delegate
            {
                string l_ZDegree = "";
                string l_ZDegreeTemp = "";
                TextWriter l_TwUsers = new StreamWriter("Users_merged_SO_GG_GH_Stats.xml", false);
                l_TwUsers.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                l_TwUsers.WriteLine("<users>");

                try
                {
                    //Users_merged_SO_GG_GH.xml
                    //Stack Overflow/SO_stats.xml
                    //Google Groups/Output/GG_stats.xml

                    using (XmlReader l_Reader = XmlReader.Create("Users_merged_SO_GG_GH.xml"))
                    {
                        while (l_Reader.Read())
                        {
                            if (l_Reader.NodeType == XmlNodeType.Element)
                            {
                                if (l_Reader.Name == "row")
                                {
                                    XElement l_ElementOri = XNode.ReadFrom(l_Reader) as XElement;
                                    l_BW.ReportProgress(0, "Adding ZDegree for: " + l_ElementOri.Attribute("Id").Value);
                                    if (l_ElementOri != null)
                                    {
                                        XElement l_ElementNew = new XElement("row");
                                        //Stack Overflow
                                        if (l_ElementOri.Attribute("Id_SO") != null)
                                        {
                                            XElement l_Root = XElement.Load("Stack Overflow/SO_stats.xml");
                                            IEnumerable<XElement> l_Query =
                                                from l_Element in l_Root.Elements("User")
                                                where (string)l_Element.Attribute("Id") == (string)l_ElementOri.Attribute("Id_SO")
                                                select l_Element;

                                            foreach (XElement l_Element in l_Query)
                                            {
                                                l_ZDegree = l_Element.Attribute("ZDegree").Value;
                                            }
                                            l_ZDegree = l_ZDegree.Replace(',', '.');
                                        }

                                        //Google Groups
                                        if (l_ElementOri.Attribute("Id_GG") != null)
                                        {
                                            XElement l_Root = XElement.Load("Google Groups/Output/GG_stats.xml");
                                            IEnumerable<XElement> l_Query =
                                                from l_Element in l_Root.Elements("User")
                                                where (string)l_Element.Attribute("Id") == (string)l_ElementOri.Attribute("Id_GG")
                                                select l_Element;

                                            foreach (XElement l_Element in l_Query)
                                            {
                                                l_ZDegreeTemp = l_Element.Attribute("ZDegree").Value;
                                            }
                                            l_ZDegreeTemp = l_ZDegreeTemp.Replace(',', '.');
                                            if (l_ZDegree.Length == 0)
                                                l_ZDegree = l_ZDegreeTemp;
                                            else
                                            {
                                                if (double.Parse(l_ZDegreeTemp, CultureInfo.InvariantCulture) > double.Parse(l_ZDegree, CultureInfo.InvariantCulture))
                                                {
                                                    l_ZDegree = l_ZDegreeTemp;
                                                }
                                            }
                                        }

                                        //Set
                                        l_ElementNew = l_ElementOri;
                                        if (l_ZDegree.Length > 0)
                                            l_ElementNew.SetAttributeValue("ZDegree", l_ZDegree);
                                        l_TwUsers.WriteLine(l_ElementNew.ToString());
                                        l_ZDegree = "";
                                        l_ZDegreeTemp = "";
                                    }
                                }
                            }
                        }
                    }

                    l_TwUsers.WriteLine("</users>");
                    l_TwUsers.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("---> " + ex.Message);
                }
                l_BW.ReportProgress(0, "Ready");
            };
            l_BW.ProgressChanged += delegate(object p_Sender, ProgressChangedEventArgs p_E)
            {
                toolStripStatusLabel1.Text = p_E.UserState.ToString();
            };
            l_BW.RunWorkerCompleted += delegate
            {
                buttonStats.Enabled = true;
            };
            l_BW.RunWorkerAsync();
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
