using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Stack_Overflow_Parser
{
    public static class MyExtensions
    {
        public static IEnumerable<PostExtension> Posts(this XmlReader p_Source)
        {
            while (p_Source.Read())
            {
                if (p_Source.NodeType == XmlNodeType.Element && p_Source.Name == "row")
                {
                    int l_PostTypeId;
                    int.TryParse(p_Source.GetAttribute("PostTypeId"), out l_PostTypeId);
                    yield return new PostExtension
                    {
                        PostTypeId = l_PostTypeId,
                        Tags = p_Source.GetAttribute("Tags")
                    };
                }
            }
        }

        public static IEnumerable<UserExtension> Users(this XmlReader p_Source)
        {
            while (p_Source.Read())
            {
                if (p_Source.NodeType == XmlNodeType.Element && p_Source.Name == "row")
                {
                    yield return new UserExtension
                    {
                        Id = p_Source.GetAttribute("Id"),
                        EmailHash = p_Source.GetAttribute("EmailHash")
                    };
                }
            }
        }
    }
}
