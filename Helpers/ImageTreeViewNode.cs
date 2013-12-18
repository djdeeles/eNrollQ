using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enroll.WebParts
{
    public class ImageTreeViewNode : TreeNode
    {
        private List<TreeNodeIcon> oIcons;


        public List<TreeNodeIcon> Icons
        {
            get { return oIcons; }
            set { oIcons = value; }
        }

        protected override void RenderPostText(HtmlTextWriter writer)
        {
            if (oIcons != null)
            {
                foreach (TreeNodeIcon icon in oIcons)
                {
                    writer.AddAttribute("href", icon.Url);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);

                    writer.AddAttribute("src", icon.ImageUrl);
                    writer.AddAttribute("title", icon.ToolTip);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
            }
            base.RenderPostText(writer);
        }
    }

    public class TreeNodeIcon
    {
        public String ImageUrl = null;
        public String ToolTip = null;
        public String Url = null;
    }
}