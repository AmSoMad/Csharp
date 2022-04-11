        private void ShowChildForm(System.Type type, string title, Size size)
        {
            Cursor.Current = Cursors.WaitCursor;
            Control control = (Control)System.Activator.CreateInstance(type);

            Form form = findChildForm(title);
            if (form == null)
            {
                form = new ViewForm(control, title);
                form.MdiParent = this;
            }

            form.Size = size;
            form.Show();
            form.Select();
            Cursor.Current = Cursors.Default;
        }
        
        private void ShowChildForm(Control control, string title, Size size, bool toolBar)
        {
            Cursor.Current = Cursors.WaitCursor;
            Form form = findChildForm(title);
            if (form == null)
            {
                form = new ViewForm(control, title);
                form.MdiParent = this;
            }
            form.Size = size;
            form.Show();
            form.Select();

            Cursor.Current = Cursors.Default;
        }
        
        
        private void MenuItem_Click(object sender, EventArgs e)
        {
            ShowChildForm(typeof(menu.cs), "menu.cs", new Size(1300, 700));

        }
