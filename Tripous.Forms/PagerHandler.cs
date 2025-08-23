namespace Tripous.Forms
{
    /// <summary>
    /// Handles drag n drop and closing <see cref="TabPage"/> items using the middle mouse button.
    /// </summary>
    public class PagerHandler
    {
        protected TabControl Pager;
        protected Type TabPageType;
        protected Font BoldFont;
        //protected SolidBrush BlackBrush = new SolidBrush(Color.Black);
        //protected SolidBrush NormalBrush = new SolidBrush(TabControl.DefaultBackColor);

        protected virtual TabPage FindTabPageUnderMouse(Point Location)
        {
            for (var i = 0; i < Pager.TabCount; i++)
            {
                if (!Pager.GetTabRect(i).Contains(Location))
                    continue;

                return Pager.TabPages[i];
            }

            return null;
        }
        protected virtual void ArrangeTabPages(TabPage Source, TabPage Dest)
        {
            //int SourceIndex = Pager.TabPages.IndexOf(Source);
            int DestIndex = Pager.TabPages.IndexOf(Dest);
            Pager.TabPages.Remove(Source);
            Pager.Refresh();
            Pager.TabPages.Insert(DestIndex, Source);
            Pager.Refresh();
        }

        protected virtual void Pager_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Left)
            {
                TabPage TabPage = FindTabPageUnderMouse(e.Location);

                if (TabPage != null)
                {
                    if (e.Button == MouseButtons.Middle)
                    {
                        IPanel Panel = TabPage.Tag as IPanel;
                        if (Panel != null)
                            Panel.Close();
                    }
                    else if (e.Button == MouseButtons.Left)
                    {
                        Pager.AllowDrop = true;
                        Pager.DoDragDrop(TabPage, DragDropEffects.All);
                        Application.DoEvents();
                    }
                }
            }
        }
        protected virtual void Pager_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(TabPageType))
            {
                TabPage TabPage = e.Data.GetData(TabPageType) as TabPage;
                if (Pager.TabPages.Contains(TabPage))
                {
                    e.Effect = DragDropEffects.Move;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }
        protected virtual void Pager_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(TabPageType))
            {
                TabPage SourceTabPage = e.Data.GetData(TabPageType) as TabPage;
                if (Pager.TabPages.Contains(SourceTabPage))
                {
                    Point Location = new Point(e.X, e.Y);
                    Location = Pager.PointToClient(Location);

                    TabPage DestTabPage = FindTabPageUnderMouse(Location);
                    if (SourceTabPage != DestTabPage)
                    {
                        ArrangeTabPages(SourceTabPage, DestTabPage);
                    }
                }

            }


        }
        protected virtual void Pager_DragLeave(object sender, EventArgs e)
        {
            // nothing
        }

        protected virtual void TabControlOnDrawItem2(object sender, DrawItemEventArgs e)
        {
            var Pager = (TabControl)sender;

            var TitleText = Pager.TabPages[e.Index].Text;

            e.Graphics
             .DrawString(TitleText
                         , e.Index == Pager.SelectedIndex ? BoldFont : Pager.Font
                         , Brushes.Black
                         , e.Bounds
                         , new StringFormat
                         {
                             Alignment = StringAlignment.Center,
                             LineAlignment = StringAlignment.Center
                         });
        }
        protected virtual void TabControlOnDrawItem(object sender, DrawItemEventArgs e)
        {
            var Pager = (TabControl)sender;
            TabPage page = Pager.TabPages[e.Index];
 
            using (SolidBrush Brush = (e.State == DrawItemState.Selected) ? new SolidBrush(SelectedTabColor) : new SolidBrush(TabControl.DefaultBackColor))
            {
                e.Graphics.FillRectangle(Brush, e.Bounds);
            }

            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -1 : 1;
            paddedBounds.Offset(1, yOffset);
            Color FontColor = (e.State == DrawItemState.Selected) ? SelectedFontColor : Color.Black;
            TextRenderer.DrawText(e.Graphics, page.Text, e.Font, paddedBounds, FontColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        protected virtual TabPage CreateTabPage(Type UserControlType, string PageId, object Info)
        {
            TabPage TabPage = new TabPage();

            Pager.Controls.Add(TabPage);
            Pager.SelectedTab = TabPage;

            UserControl UC = Activator.CreateInstance(UserControlType) as UserControl;
            IPanel Panel = UC as IPanel;
            Panel.Id = PageId;
            Panel.Info = Info;

            TabPage.Tag = UC;
            TabPage.Controls.Add(UC);

            UC.Location = new System.Drawing.Point(1, 1);
            UC.Dock = DockStyle.Fill;
            UC.TabIndex = 1;

            return TabPage;
        }

        // ● construction
        public PagerHandler(TabControl Pager, Type TabPageType)
        {
            this.Pager = Pager;
            this.TabPageType = TabPageType;
            Pager.AllowDrop = true;

            Pager.MouseDown += Pager_MouseDown;
            Pager.DragEnter += Pager_DragEnter;
            Pager.DragDrop += Pager_DragDrop;

            Pager.DragLeave += Pager_DragLeave;
            BoldFont = new Font(Pager.Font.FontFamily, Pager.Font.Size, FontStyle.Bold);

            Pager.DrawMode = TabDrawMode.OwnerDrawFixed;
            Pager.DrawItem += TabControlOnDrawItem;
        }

        // ● public
        public virtual TabPage FindTabPage(string PageId)
        {
            foreach (TabPage TabPage in Pager.TabPages)
            {
                IPanel Panel = TabPage.Tag as IPanel;
                if ((Panel != null) && Panel.Id.IsSameText(PageId))
                    return TabPage;
            }

            return null;
        }
        public virtual void ClosePage(string PageId)
        {
            TabPage TabPage = FindTabPage(PageId);
            if (TabPage != null)
            {
                IPanel Panel = TabPage.Tag as IPanel;
                if (Panel != null)
                    Panel.Close();
            }
        }
        public virtual TabPage ShowPage(Type UserControlType, string PageId = "", object Info = null)
        {
            if (string.IsNullOrWhiteSpace(PageId))
                PageId = UserControlType.FullName;

            TabPage TabPage = FindTabPage(PageId);
            if (TabPage == null)
            {
                TabPage = CreateTabPage(UserControlType, PageId, Info);
            }
            else
            {
                Pager.SelectedTab = TabPage;
            }

            return TabPage;
        }

        public virtual void CloseAll()
        {
            while (Pager.TabPages.Count > 0)
            {
                IPanel Panel = Pager.TabPages[0].Tag as IPanel;
                if (Panel != null)
                    Panel.Close();
                else
                    Pager.TabPages.RemoveAt(0);
            }
        }


        public Color SelectedTabColor { get; set; } = Color.Black;
        public Color SelectedFontColor { get; set; } = Color.Yellow;

    }
}
