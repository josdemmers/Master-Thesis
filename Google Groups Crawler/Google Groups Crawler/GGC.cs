using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

/*
 * !! Important !!
 * When class names of HTML elements at Google Groups change
 * the variables in readTopic() need to be updated.
 *       
 */
namespace Google_Groups_Crawler
{
    public partial class GGC : Form
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);
        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();

        private Timer m_TimerTopicList = new Timer();
        private Timer m_TimerTopics = new Timer();
        private Timer m_TimerIdle = new Timer();
        private Stopwatch m_StopwatchIdle = new Stopwatch();
        private bool m_TimerTopicListRunning = false;
        private bool m_TimerTopicsRunning = false;
        private string m_State = "";
        private int m_CurrentTopicIndex = 0;
        private ToolTip m_Tooltip = new ToolTip();
        //Custom classes
        private Group m_Group = new Group();

//-->Constructor

        public GGC()
        {
            InitializeComponent();
            initEventHandlers();
            initTimers();
            initFolders();

            m_Tooltip.SetToolTip(buttonLoadList, "Load topics from browser.");
            m_Tooltip.SetToolTip(buttonFile, "Load topics from \\Output\\GG_topiclist.txt.");
        }

//-->Methods

        private void initBrowser()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                string l_UserAgent = "";
                string l_Js = @"<script type='text/javascript'>function getUserAgent(){document.write(navigator.userAgent)}</script>";
                webBrowserGoogleGroups.Document.Write(l_Js);
                webBrowserGoogleGroups.Document.InvokeScript("getUserAgent");
                l_UserAgent = webBrowserGoogleGroups.DocumentText.Substring(l_Js.Length);
                //l_Settings.UserAgent = l_UserAgent;
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                {
                    l_IOHandler.debug("Exception in initBrowser(): " + e.Message);
                }
            }
        }

        private void initEventHandlers()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                m_TimerTopicList.Tick += m_TimerTopicList_Tick;
                m_TimerTopics.Tick += m_TimerTopics_Tick;
                m_TimerIdle.Tick += m_TimerIdle_Tick;
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in initEventHandlers(): " + e.Message);
            }
        }

        private void initTimers()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                m_TimerTopicList.Interval = l_Settings.DelayTopics;
                m_TimerTopics.Interval = l_Settings.DelayTopics;
                m_TimerIdle.Interval = 500;
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in initTimers(): " + e.Message);
            }
        }

        private void initFolders()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                Directory.CreateDirectory(l_Settings.DebugDir);
                Directory.CreateDirectory(l_Settings.OutputDir);
                Directory.CreateDirectory(l_Settings.TopicsDir);
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in initFolders(): " + e.Message);
            }
        }

        private void loadMoreTopics()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;
            bool l_TopicListCompleted = true;

            try
            {
                if (webBrowserGoogleGroups.IsBusy)
                {
                    m_TimerTopicList.Start();
                }
                else
                {
                    HtmlElementCollection l_As = webBrowserGoogleGroups.Document.GetElementsByTagName("a");

                    foreach (HtmlElement l_A in l_As)
                    {
                        if (l_A.OuterHtml != null)
                        {
                            //<a title="Box plot based dashboard." href="https://groups.google.com/d/topic/ggplot2/ZbgnsPMCJa8">Box plot based dashboard.</a>
                            string l_Title = "";
                            string l_ID = "";
                            string l_Search = "/topic/" + textBoxGroupID.Text + "/";
                            int l_Index = -1;

                            //Check if real topic link
                            if (l_A.OuterHtml.Contains(l_Search))
                            {
                                l_Title = l_A.OuterText;
                                l_Index = l_A.OuterHtml.IndexOf(l_Search);
                                l_ID = l_A.OuterHtml.Substring(l_Index + l_Search.Length, l_A.OuterHtml.IndexOf("\"", l_Index + l_Search.Length) - (l_Index + l_Search.Length));
                                //Add topic
                                m_Group.addTopic(l_Title, l_ID);
                            }
                            else
                            {
                                //Goto next page
                                l_TopicListCompleted = false;
                                if (m_TimerTopicListRunning)
                                {
                                    l_A.InvokeMember("Click");
                                }
                            }
                        }
                    }

                    if (l_TopicListCompleted)
                    {
                        m_TimerTopicListRunning = false;
                        m_TimerTopicList.Stop();
                        buttonLoadList.Enabled = false;
                        m_TimerIdle.Stop();
                        m_State = "";
                        l_IOHandler.exportTopicList(m_Group.Topics);
                    }
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in loadMoreTopics(): " + e.Message);
            }
        }

        /*
         * Main method for extracting topics
         * 
         * Change local variables when class names of HTML elements change!
         * 
         */ 
        private void readTopic()
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            /* !! The following variables need to be updated when the class names of Google Groups change !! */
            //l_Class_ShowHideButton is the classname of the span element containing the show/hide button. (In hide state)
            string l_Class_ShowHideButton = "NYOQRLB-a-C NYOQRLB-a-s";
            //l_Class_Title is the classname of the span element containing the title
            string l_Class_Title = "NYOQRLB-h-f";
            //l_Class_Date is the classname of the span element containing the date
            string l_Class_Date = "NYOQRLB-w-f";
            //l_Class_UserName is the classname of the span element containing the user name
            string l_Class_UserName = "NYOQRLB-w-c";
            //l_Class_Avatar is the classname of the img element containing the avatar
            string l_Class_Avatar = "NYOQRLB-w-d";
            //l_Class_Content is the classname of the img element containing the content
            string l_Class_Content = "NYOQRLB-w-j";

            try
            {
                string l_Title = "";
                string l_ID = "";
                string l_AvatarRaw = "";
                string l_Search = "";
                int l_Index = -1;
                List<string> l_Names = new List<string>();
                List<string> l_Dates = new List<string>();
                List<string> l_Avatars = new List<string>();
                List<string> l_Contents = new List<string>();

                if (webBrowserGoogleGroups.IsBusy)
                {
                    m_TimerTopics.Start();
                }
                else if (webBrowserGoogleGroups.DocumentText.Contains(l_Class_ShowHideButton))
                {
                    HtmlElementCollection spans = webBrowserGoogleGroups.Document.GetElementsByTagName("span");
                    foreach (HtmlElement span in spans)
                    {
                        //l_Class_ShowHideButton is the classname of the span element containing the show/hide button.
                        if (span.OuterHtml != null)
                            if (span.OuterHtml.Contains(l_Class_ShowHideButton) && countStringOccurrences(span.OuterHtml, "<span") == 1 && span.Style == null)
                            {
                                if (m_TimerTopicsRunning)
                                {
                                    span.InvokeMember("Click");
                                    m_TimerTopics.Start();
                                    break;
                                }
                            }
                    }
                }
                else
                {
                    //Title, Date, and User Name
                    HtmlElementCollection spans = webBrowserGoogleGroups.Document.GetElementsByTagName("span");
                    foreach (HtmlElement span in spans)
                    {

                        if (span.OuterText != null)
                        {
                            if (span.OuterHtml.Contains(l_Class_Title) && countStringOccurrences(span.OuterHtml, "<span") == 1)
                            {//l_Class_Title is the classname of the span element containing the title
                                l_Title = span.OuterText;
                            }
                            else if (span.OuterHtml.Contains(l_Class_Date) && countStringOccurrences(span.OuterHtml, "<span") == 1)
                            {//l_Class_Date is the classname of the span element containing the date
                                l_Dates.Add(span.OuterText);
                            }
                            else if (span.OuterHtml.Contains(l_Class_UserName) && countStringOccurrences(span.OuterHtml, "<span") == 1)
                            {//l_Class_UserName is the classname of the span element containing the user name
                                l_Names.Add(span.OuterText);
                            }
                        }
                    }

                    //Avatar
                    HtmlElementCollection imgs = webBrowserGoogleGroups.Document.GetElementsByTagName("img");
                    foreach (HtmlElement img in imgs)
                    {

                        if (img.OuterHtml != null)
                        {
                            if (img.OuterHtml.Contains(l_Class_Avatar) && countStringOccurrences(img.OuterHtml, "<img") == 1)
                            {//l_Class_Avatar is the classname of the img element containing the avatar
                                l_AvatarRaw = img.OuterHtml;
                                l_Search = "src=\"";
                                l_Index = l_AvatarRaw.IndexOf(l_Search);
                                l_AvatarRaw = l_AvatarRaw.Substring(l_Index + l_Search.Length, l_AvatarRaw.IndexOf("\">", l_Index + l_Search.Length) - (l_Index + l_Search.Length));
                                l_Avatars.Add(l_AvatarRaw);
                            }
                        }
                    }

                    //Content
                    HtmlElementCollection divs = webBrowserGoogleGroups.Document.GetElementsByTagName("div");
                    foreach (HtmlElement div in divs)
                    {
                        if (div.OuterHtml != null)
                        {
                            if (div.OuterHtml.Contains(l_Class_Content) && div.OuterHtml.StartsWith("<div class=\"" + l_Class_Content + "\">"))
                            {//l_Class_Content is the classname of the div element containing the content
                                if (div.OuterText == null)
                                {
                                    //Safety check in case reply is empty (For example topic OiczAISRMXo of ggplot2)
                                    l_Contents.Add("");
                                }
                                else
                                {
                                    l_Contents.Add(div.OuterText);
                                }
                            }
                        }
                    }

                    //Save data
                    if (webBrowserGoogleGroups.Url.AbsoluteUri.StartsWith("https://groups.google.com/forum/m/#!topic/ggplot2/"))
                    {
                        l_ID = webBrowserGoogleGroups.Url.AbsoluteUri.Split('/')[webBrowserGoogleGroups.Url.AbsoluteUri.Split('/').Length - 1];
                        l_Index = m_Group.getTopicIndex(l_ID);

                        /*for (int i = 0; i < l_Names.Count; i++)
                        {
                            m_Group.Topics[l_Index].Messages.Add(new Message(l_Names[i], l_Avatars[i], l_Dates[i], l_Contents[i]));
                        }

                        for (int i = 0; i < l_Names.Count; i++)
                        {
                            if (m_Group.isUniqueUser(l_Names[i], l_Avatars[i]))
                                m_Group.Users.Add(new User(l_Names[i], l_Avatars[i]));
                        }*/

                        Topic l_Topic = m_Group.Topics[l_Index];
                        for (int i = 0; i < l_Names.Count; i++)
                        {
                            l_Topic.Messages.Add(new Message(l_Names[i], l_Avatars[i], l_Dates[i], System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(l_Contents[i])),""));
                        }
                        l_IOHandler.exportTopic(l_ID, l_Topic);
                        m_Tooltip.SetToolTip(buttonReadTopics, m_CurrentTopicIndex.ToString());
                    }

                    //Goto next topic
                    m_CurrentTopicIndex += 1;
                    if (m_CurrentTopicIndex < m_Group.Topics.Count && m_TimerTopicsRunning)
                    {
                        string l_Url = "https://groups.google.com/forum/m/#!topic/ggplot2/" + m_Group.Topics[m_CurrentTopicIndex].ID;
                        webBrowserGoogleGroups.Navigate(l_Url);
                    }
                    else
                    {
                        m_TimerIdle.Stop();
                        buttonReadTopics.Enabled = false;
                        m_TimerTopicsRunning = false;
                        m_State = "";
                    }
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                {
                    l_IOHandler.debug("Exception in readTopic(): " + e.Message);
                    l_IOHandler.saveServerResponse("readTopic", webBrowserGoogleGroups.DocumentText);
                }
            }
        }

        private int countStringOccurrences(string p_Text, string p_Pattern)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            int l_Count = 0;
            int l_Index = 0;

            try
            {
                while ((l_Index = p_Text.IndexOf(p_Pattern, l_Index)) != -1)
                {
                    l_Index += p_Pattern.Length;
                    l_Count++;
                }
            }
            catch (Exception e)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in countStringOccurrences(): " + e.Message);
            }

            return l_Count;
        }

//-->Events

        private void buttonLoadList_Click(object sender, EventArgs e)
        {
            if (m_TimerTopicListRunning)
            {
                m_State = "";
                m_TimerTopicListRunning = false;
                m_TimerTopicList.Stop();
                m_TimerIdle.Stop();
            }
            else
            {
                m_State = "loadmoretopics";
                m_TimerTopicListRunning = true;
                string l_Url = "https://groups.google.com/forum/?_escaped_fragment_=forum/" + textBoxGroupID.Text + "[1-100-false]";
                webBrowserGoogleGroups.Navigate(l_Url);
            }
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                buttonLoadList.Enabled = false;
                buttonFile.Enabled = false;

                //load GG_topiclist.txt
                TextReader l_TrTopiclist = new StreamReader(l_Settings.OutputDir + l_Settings.TopicListFile);
                string l_Topic = "";
                string l_ID = "";
                string l_Title = "";

                while ((l_Topic = l_TrTopiclist.ReadLine()) != null)
                {
                    l_ID = l_Topic.Split(';')[0];
                    l_Title = l_Topic.Substring(l_ID.Length + 1);
                    //Add topic
                    m_Group.addTopic(l_Title, l_ID);
                }
                l_TrTopiclist.Close();
                this.Text = "Google Groups Crawler - " + m_CurrentTopicIndex.ToString() + "/" + m_Group.Topics.Count.ToString();
            }
            catch (Exception ex)
            {
                buttonLoadList.Enabled = true;
                buttonFile.Enabled = true;
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in buttonFile_Click(): " + ex.Message);
            }
        }

        private void buttonReadTopics_Click(object sender, EventArgs e)
        {
            //Alternatives for reading topic data
            //https://groups.google.com/forum/?_escaped_fragment_=topic/ggplot2/ZbgnsPMCJa8 <-- Very compact, nice date/time format, no avatars
            //https://groups.google.com/forum/m/#!topic/ggplot2/ZbgnsPMCJa8 <-- Compact, annoying date/time format, avatars
            //https://groups.google.com/forum/?fromgroups#!topic/ggplot2/ZbgnsPMCJa8 <-- Not compact at all, annoying date/time format, avatars

            if (m_TimerTopicsRunning)
            {
                m_State = "";
                m_TimerTopicsRunning = false;
                m_TimerTopics.Stop();
                m_TimerIdle.Stop();
            }
            else
            {
                if (m_Group.Topics.Count > 0)
                {
                    m_State = "readtopic";
                    m_TimerTopicsRunning = true;
                    m_CurrentTopicIndex = (int)numericUpDownStartAt.Value;
                    string l_Url = "https://groups.google.com/forum/m/#!topic/ggplot2/" + m_Group.Topics[m_CurrentTopicIndex].ID;
                    webBrowserGoogleGroups.Navigate(l_Url);
                }
            }
        }

        private void numericUpDownDelay_ValueChanged(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;

            l_Settings.DelayTopics = (int)numericUpDownDelay.Value;
            initTimers();
        }

        private void webBrowserGoogleGroups_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                m_TimerIdle.Stop();
                toolStripStatusLabel1.Text = webBrowserGoogleGroups.Url.AbsoluteUri;
                if (m_State.Equals("loadmoretopics") && m_TimerTopicListRunning)
                {
                    m_StopwatchIdle.Restart();
                    m_TimerTopicList.Start();
                    m_TimerIdle.Start();
                }
                else if (m_State.Equals("readtopic") && m_TimerTopicsRunning)
                {
                    m_StopwatchIdle.Restart();
                    m_TimerTopics.Start();
                    m_TimerIdle.Start();
                }

                IntPtr pHandle = GetCurrentProcess();
                SetProcessWorkingSetSize(pHandle, -1, -1);
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in webBrowserGoogleGroups_DocumentCompleted(): " + ex.Message);
            }
        }

        /*
         * Timer used by "Load List" button
         */ 
        private void m_TimerTopicList_Tick(object sender, EventArgs e)
        {
            m_TimerTopicList.Stop();
            loadMoreTopics();
        }

        /*
         * Timer used by "Read Topics" button
         */ 
        private void m_TimerTopics_Tick(object sender, EventArgs e)
        {
            m_TimerTopics.Stop();
            readTopic();
        }

        private void m_TimerIdle_Tick(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                long l_IdleTime = m_StopwatchIdle.ElapsedMilliseconds;
                m_Tooltip.SetToolTip(labelDelay, l_IdleTime.ToString());
                this.Text = "Google Groups Crawler - " + m_CurrentTopicIndex.ToString() + "/" + m_Group.Topics.Count.ToString();
                if (l_IdleTime > 60 * 1000)
                {
                    m_StopwatchIdle.Restart();
                    webBrowserGoogleGroups.Stop();
                    webBrowserGoogleGroups.Refresh();
                    if (l_Settings.DebugMode)
                        l_IOHandler.debug("Browser refreshed: stuck for >60 seconds at " + m_State);
                }
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in m_TimerIdle_Tick(): " + ex.Message);
            }
        }

        private void buttonEditPostsID_Click(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                buttonEditPostsID.Enabled = false;

                //reset topics/users
                m_Group.Topics.Clear();
                m_Group.Users.Clear();

                //load GG_topiclist.txt
                TextReader l_TrTopiclist = new StreamReader(l_Settings.OutputDir + l_Settings.TopicListFile);
                string l_Topic = "";
                string l_ID = "";
                string l_Title = "";

                while ((l_Topic = l_TrTopiclist.ReadLine()) != null)
                {
                    l_ID = l_Topic.Split(';')[0];
                    l_Title = l_Topic.Substring(l_ID.Length + 1);
                    //Add topic
                    m_Group.addTopic(l_Title, l_ID);
                }
                l_TrTopiclist.Close();

                string[] l_Topicpaths = Directory.GetFiles(l_Settings.TopicsDir);
                for (int i = 0; i < l_Topicpaths.Length; i++)
                {
                    XmlDocument l_Doc = new XmlDocument();
                    l_Doc.Load(l_Topicpaths[i]);
                    IEnumerator l_IE = l_Doc.SelectNodes("/Topic/Messages/Message/User").GetEnumerator();

                    while (l_IE.MoveNext())
                    {
                        XmlNode l_XmlNode = l_IE.Current as XmlNode;
                        //name, avatar, email
                        if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "Hadley Wickham")
                            (l_IE.Current as XmlNode).ChildNodes[1].InnerText = "https://www.google.com/s2/photos/public/AIbEiAIAAABDCKzxuL65jIC3FyILdmNhcmRfcGhvdG8qKGM4M2ZlMDRjYWY2ZDZjZWMyN2U1ZDUyMjFiNTk4NzZhNjE4ZDE5MWQwAYjBlBmzwtjc-VbJmyY6bJRYefD9?authuser=0&amp;amp;sz=32";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "Brandon Hurr")
                            (l_IE.Current as XmlNode).ChildNodes[1].InnerText = "https://www.google.com/s2/photos/public/AIbEiAIAAABECLi4kL2rk_q94gEiC3ZjYXJkX3Bob3RvKig5ZWQwMzA2MjgwMzBiNThmNTVhODQ2ZmVmYzU4YTk5NTJkYjQ5YzJhMAEic3goxKc3sq6OwGuceOWb6QEBRA?authuser=0&amp;amp;sz=32";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "baptiste")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Baptiste Auguie";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "baptiste auguie")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Baptiste Auguie";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "baptiste Auguié")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Baptiste Auguie";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "Ista Zahn")
                            (l_IE.Current as XmlNode).ChildNodes[1].InnerText = "https://www.google.com/s2/photos/public/AIbEiAIAAABDCKOZ9JyQ5vyWECILdmNhcmRfcGhvdG8qKDc1MDY2ZGIxN2Y1NzBmYzY3NGVmYTBlMmYxMjYwYWI3ODc1MjgzYmMwAVDby7fzhdz_Dy-AgpVrQ9XfNjU1?authuser=0&amp;amp;sz=32";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "Jean-Olivier Irisson")
                            (l_IE.Current as XmlNode).ChildNodes[1].InnerText = "https://www.google.com/s2/photos/public/AIbEiAIAAABDCL7MxvKlt9-1ICILdmNhcmRfcGhvdG8qKGY0NzU3NjI1Yzg0N2M2ZWU5OTczZDk1YjRjZTgzNmMwNTg0OWZkYzMwAf3wZyzVffxxJ7VzPcQ0IYTDU0bJ?authuser=0&amp;amp;sz=32";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "pradip...@samhsa.hhs.gov")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Pradip";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "july")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Antje Niederlein";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "Prof. Bryan Hanson")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Bryan Hanson";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "Sean")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Sean T Ma";
                        else if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == "UkrBuckeye")
                            (l_IE.Current as XmlNode).ChildNodes[0].InnerText = "Yuliia Aloshycheva";
                        //Second iteration - removing special chars from names
                        //Note: Not needed anymore...

                    }

                    l_Doc.Save(l_Topicpaths[i]);
                }
                buttonEditPostsID.Enabled = true;
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in buttonEditPosts_Click(): " + ex.Message);
            }
        }

        private void buttonEditPostsEmail_Click(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                buttonEditPostsEmail.Enabled = false;

                //reset topics/users
                m_Group.Topics.Clear();
                m_Group.Users.Clear();

                //load GG_topiclist.txt
                TextReader l_TrTopiclist = new StreamReader(l_Settings.OutputDir + l_Settings.TopicListFile);
                string l_Topic = "";
                string l_ID = "";
                string l_Title = "";

                while ((l_Topic = l_TrTopiclist.ReadLine()) != null)
                {
                    l_ID = l_Topic.Split(';')[0];
                    l_Title = l_Topic.Substring(l_ID.Length + 1);
                    //Add topic
                    m_Group.addTopic(l_Title, l_ID);
                }
                l_TrTopiclist.Close();

                string[] l_Topicpaths = Directory.GetFiles(l_Settings.TopicsDir);
                for (int i = 0; i < l_Topicpaths.Length; i++)
                {
                    XmlDocument l_Doc = new XmlDocument();
                    l_Doc.Load(l_Topicpaths[i]);
                    IEnumerator l_IE = l_Doc.SelectNodes("/Topic/Messages/Message/User").GetEnumerator();

                    while (l_IE.MoveNext())
                    {
                        //load GG_useremaillist.txt
                        TextReader l_TrEmaillist = new StreamReader(l_Settings.OutputDir + l_Settings.Useremaillist);
                        string l_Name = "";
                        string l_Email = "";

                        XmlNode l_XmlNode = l_IE.Current as XmlNode;

                        while ((l_Topic = l_TrEmaillist.ReadLine()) != null)
                        {
                            l_Name = l_Topic.Split(';')[0];
                            l_Email = l_Topic.Substring(l_Name.Length + 1);

                            if ((l_IE.Current as XmlNode).ChildNodes[0].InnerText == l_Name)
                            {
                                (l_IE.Current as XmlNode).ChildNodes[2].InnerText = l_Email;
                                break;
                            }
                        }
                        l_TrEmaillist.Close();
                    }

                    l_Doc.Save(l_Topicpaths[i]);
                }
                buttonEditPostsEmail.Enabled = true;
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in buttonEditPostsEmail_Click(): " + ex.Message);
            }
        }

        private void buttonEditPostsContent_Click(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                buttonEditPostsContent.Enabled = false;

                //reset topics/users
                m_Group.Topics.Clear();
                m_Group.Users.Clear();

                //load GG_topiclist.txt
                TextReader l_TrTopiclist = new StreamReader(l_Settings.OutputDir + l_Settings.TopicListFile);
                string l_Topic = "";
                string l_ID = "";
                string l_Title = "";

                while ((l_Topic = l_TrTopiclist.ReadLine()) != null)
                {
                    l_ID = l_Topic.Split(';')[0];
                    l_Title = l_Topic.Substring(l_ID.Length + 1);
                    //Add topic
                    m_Group.addTopic(l_Title, l_ID);
                }
                l_TrTopiclist.Close();

                string[] l_Topicpaths = Directory.GetFiles(l_Settings.TopicsDir);
                for (int i = 0; i < l_Topicpaths.Length; i++)
                {
                    XmlDocument l_Doc = new XmlDocument();
                    l_Doc.Load(l_Topicpaths[i]);
                    IEnumerator l_IE = l_Doc.SelectNodes("/Topic/Messages/Message/content").GetEnumerator();

                    while (l_IE.MoveNext())
                    {
                        //XmlNode l_XmlNode = l_IE.Current as XmlNode;
                        string l_Content = (l_IE.Current as XmlNode).InnerText;
                        string l_ContentNew = "";
                        string l_Line = "";
                        int l_Index = 0;

                        //Decode base64
                        l_Content = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(l_Content));

                        //Remove quotes
                        l_Index = l_Content.IndexOf("- show quoted text -");
                        if (l_Index != -1)
                            l_Content = l_Content.Substring(0, l_Index);
                        l_Index = l_Content.IndexOf("- Show quoted text -");
                        if (l_Index != -1)
                            l_Content = l_Content.Substring(0, l_Index);

                        //Line-by-line check, removes remaining quoted text
                        StringReader l_StringReader = new StringReader(l_Content);
                        while (true)
                        {
                            l_Line = l_StringReader.ReadLine();
                            if (l_Line != null)
                            {
                                if (!l_Line.StartsWith(">")
                                    && !l_Line.EndsWith("wrote: "))
                                    l_ContentNew += l_Line + Environment.NewLine;
                            }
                            else
                            {
                                break;
                            }
                        }

                        //Encode base64
                        l_ContentNew = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(l_ContentNew));
                        //Set changes
                        (l_IE.Current as XmlNode).InnerText = l_ContentNew;
                    }

                    l_Doc.Save(l_Topicpaths[i]);
                }

                //Remove conflicts
                for (int i = 0; i < l_Topicpaths.Length; i++)
                {
                    XmlDocument l_Doc = new XmlDocument();
                    l_Doc.Load(l_Topicpaths[i]);
                    IEnumerator l_IE = l_Doc.SelectNodes("/Topic/Messages/Message/content").GetEnumerator();

                    while (l_IE.MoveNext())
                    {
                        //XmlNode l_XmlNode = l_IE.Current as XmlNode;
                        string l_Content = (l_IE.Current as XmlNode).InnerText;

                        //Decode base64
                        l_Content = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(l_Content));

                        //Replace chars
                        l_Content = CleanInvalidXmlChars(l_Content);

                        //Encode base64
                        l_Content = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(l_Content));

                        //Set changes
                        (l_IE.Current as XmlNode).InnerText = l_Content;
                    }

                    l_Doc.Save(l_Topicpaths[i]);
                }
                buttonEditPostsContent.Enabled = true;
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in buttonPostsContent_Click(): " + ex.Message);
            }
        }

        private void buttonEditPostsDate_Click(object sender, EventArgs e)
        {
            Settings l_Settings = Settings.Instance;
            IOHandler l_IOHandler = IOHandler.Instance;

            try
            {
                buttonEditPostsDate.Enabled = false;

                //reset topics/users
                m_Group.Topics.Clear();
                m_Group.Users.Clear();

                //load GG_topiclist.txt
                TextReader l_TrTopiclist = new StreamReader(l_Settings.OutputDir + l_Settings.TopicListFile);
                string l_Topic = "";
                string l_ID = "";
                string l_Title = "";

                while ((l_Topic = l_TrTopiclist.ReadLine()) != null)
                {
                    l_ID = l_Topic.Split(';')[0];
                    l_Title = l_Topic.Substring(l_ID.Length + 1);
                    //Add topic
                    m_Group.addTopic(l_Title, l_ID);
                }
                l_TrTopiclist.Close();

                string[] l_Topicpaths = Directory.GetFiles(l_Settings.TopicsDir);
                for (int i = 0; i < l_Topicpaths.Length; i++)
                {
                    XmlDocument l_Doc = new XmlDocument();
                    l_Doc.Load(l_Topicpaths[i]);
                    IEnumerator l_IE = l_Doc.SelectNodes("/Topic/Messages/Message/date").GetEnumerator();

                    while (l_IE.MoveNext())
                    {
                        //XmlNode l_XmlNode = l_IE.Current as XmlNode;
                        string l_Date = (l_IE.Current as XmlNode).InnerText;
                        string l_DateNew = "";

                        l_DateNew = convertDate(l_Date);
                        if (!l_DateNew.Equals("unknown"))
                        {
                            //Set changes
                            (l_IE.Current as XmlNode).InnerText = l_DateNew;
                        }
                    }

                    l_Doc.Save(l_Topicpaths[i]);
                }

                buttonEditPostsDate.Enabled = true;
            }
            catch (Exception ex)
            {
                if (l_Settings.DebugMode)
                    l_IOHandler.debug("Exception in buttonEditPostsDate_Click(): " + ex.Message);
            }
        }

        public static string CleanInvalidXmlChars(string text)
        {
            // From xml spec valid chars: 
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
            string re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";
            return Regex.Replace(text, re, "");
        }

        private string convertDate(string p_Date)
        {
            string l_Date = "unknown";
            string l_Day = "";
            int l_Month = 0;
            int l_Index = -1;
            string l_Year = "15";

            //Month
            if (p_Date.Contains("Aug"))
                l_Month = 8;
            else if (p_Date.Contains("Jul"))
                l_Month = 7;
            else if (p_Date.Contains("Jun"))
                l_Month = 6;
            else if (p_Date.Contains("May"))
                l_Month = 5;
            else if (p_Date.Contains("Apr"))
                l_Month = 4;
            else if (p_Date.Contains("Mar"))
                l_Month = 3;
            else if (p_Date.Contains("Feb"))
                l_Month = 2;
            else if (p_Date.Contains("Jan"))
                l_Month = 1;

            //Day
            l_Index = p_Date.IndexOf(" ", 0);
            if (l_Index >= 0)
            {
                l_Index += 1;
                l_Day = p_Date.Substring(p_Date.IndexOf(" ", 0));
            }

            //Date
            if (l_Index >= 0 && l_Month > 0)
            {
                l_Date = l_Month + "/" + l_Day + "/" + l_Year;
            }

            return l_Date;
        }

        
    }
}