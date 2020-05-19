namespace WUD
{
    using ICSharpCode.SharpZipLib.Zip;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    internal class UpdateListManager
    {
        public List<UpdateCategory> Categories;
        public List<UpdateFile> Files;
        public string Language;
        public string Platform;
        public string Product;
        public string ServicePack;
        public List<UpdateItem> Updates;

        public UpdateListManager()
        {
            this.Files = new List<UpdateFile>();
            this.Categories = new List<UpdateCategory>();
            this.Updates = new List<UpdateItem>();
            this.Product = "";
            this.ServicePack = "";
            this.Platform = "";
            this.Language = "";
        }

        public UpdateListManager(string path)
        {
            this.Files = new List<UpdateFile>();
            this.Categories = new List<UpdateCategory>();
            this.Updates = new List<UpdateItem>();
            this.Product = "";
            this.ServicePack = "";
            this.Platform = "";
            this.Language = "";
            DirectoryInfo info = new DirectoryInfo(path);
            foreach (FileInfo info2 in info.GetFiles("*.ul"))
            {
                try
                {
                    DataSet set = new DataSet();
                    set.ReadXml(info2.FullName);
                    DataTable table = set.Tables["updatelist"];
                    if (table.Columns.Contains("servicepack"))
                    {
                        this.Files.Add(new UpdateFile(table.Rows[0]["product"].ToString(), table.Rows[0]["servicepack"].ToString(), table.Rows[0]["platform"].ToString(), table.Rows[0]["language"].ToString(), new DateTime(Convert.ToInt32(table.Rows[0]["lastupdate"].ToString().Substring(0, 4)), Convert.ToInt32(table.Rows[0]["lastupdate"].ToString().Substring(5, 2)), Convert.ToInt32(table.Rows[0]["lastupdate"].ToString().Substring(8, 2))), info2.FullName));
                    }
                    else
                    {
                        this.Files.Add(new UpdateFile(table.Rows[0]["product"].ToString(), "any", table.Rows[0]["platform"].ToString(), table.Rows[0]["language"].ToString(), new DateTime(Convert.ToInt32(table.Rows[0]["lastupdate"].ToString().Substring(0, 4)), Convert.ToInt32(table.Rows[0]["lastupdate"].ToString().Substring(5, 2)), Convert.ToInt32(table.Rows[0]["lastupdate"].ToString().Substring(8, 2))), info2.FullName));
                    }
                }
                catch (XmlException exception)
                {
                    MessageBox.Show("The update list \"" + info2.Name + "\" is malformed. " + exception.Message, "Windows Updates Downloader", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            this.Files.Sort();
        }

        public int getCategoryIndex(int category)
        {
            int num = -1;
            for (int i = 0; i < this.Categories.Count; i++)
            {
                if (this.Categories[i].ID == category)
                {
                    num = i;
                }
            }
            return num;
        }

        public static void InstallCompressedUL(string ulz, string outpath)
        {
            new FastZip().ExtractZip(ulz, outpath, FastZip.Overwrite.Always, null, "", "", true);
        }

        public void Load(int index)
        {
            this.Load(this.Files[index].Filename);
        }

        public void Load(string filename)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            this.Product = document.DocumentElement.Attributes["product"].Value;
            this.ServicePack = "";
            if (document.DocumentElement.HasAttribute("servicepack"))
            {
                this.ServicePack = document.DocumentElement.Attributes["servicepack"].Value;
            }
            this.Platform = document.DocumentElement.Attributes["platform"].Value;
            this.Language = document.DocumentElement.Attributes["language"].Value;
            document = null;
            DataSet set = new DataSet();
            set.ReadXml(filename);
            DataTable table = set.Tables["category"];
            DataTable table2 = set.Tables["update"];
            this.Categories.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                this.Categories.Add(new UpdateCategory(Convert.ToInt32(table.Rows[i]["id"]), table.Rows[i]["category_Text"].ToString()));
            }
            this.Categories.Sort();
            this.Updates.Clear();
            for (int j = 0; j < table2.Rows.Count; j++)
            {
                string id = table2.Rows[j]["id"].ToString();
                int category = Convert.ToInt32(table2.Rows[j]["category"]);
                DateTime publishdate = new DateTime(Convert.ToInt32(table2.Rows[j]["publishdate"].ToString().Substring(0, 4)), Convert.ToInt32(table2.Rows[j]["publishdate"].ToString().Substring(5, 2)), Convert.ToInt32(table2.Rows[j]["publishdate"].ToString().Substring(8, 2)));
                string title = table2.Rows[j]["title"].ToString();
                string description = table2.Rows[j]["description"].ToString();
                string str4 = table2.Rows[j]["filename"].ToString();
                string url = table2.Rows[j]["url"].ToString();
                string article = "";
                if (table2.Columns.Contains("article"))
                {
                    article = table2.Rows[j]["article"].ToString();
                }
                this.Updates.Add(new UpdateItem(id, category, publishdate, title, description, article, str4, url));
            }
            this.Updates.Sort();
            table.Dispose();
            table2.Dispose();
            set.Dispose();
        }
    }
}

