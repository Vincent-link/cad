using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using RegulatoryModel.Model;
using RegulatoryPlan.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Command
{
  public sealed  class CadHelper
    {
        public static string fileName = "";
        public static string cityName = "";
        static DerivedTypeEnum dp = DerivedTypeEnum.None;
        private static readonly CadHelper helperInstance = new CadHelper();

        static CadHelper() { }
        private CadHelper()
        {
           
        }
        public static CadHelper Instance
        {
            get { return helperInstance; }
        }

        public Document Doc { get => doc; private set => doc = value; }
        public Editor Editor
        {
            get
            {
                if (editor == null)
                {
                    editor = Doc.Editor;
                }
                return editor;
            }
            private set => editor = value;
        }
        public Database Database
        {
            get
            {
                if (database== null)
                {
                    database = Doc.Database;
                }
                return database;
            }
            private set =>database = value;
        }
        private Document doc;
        private Editor editor;
        private Database database;

        internal  void AutoOpenFile(string file, string city, DerivedTypeEnum derivedType)
        {
            try
            {
                fileName = file;
                cityName = city;
                dp = derivedType;
                num = 0;
                Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docAutoChange);
                Doc= Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file, false);
                Application.DocumentManager.DocumentActivated -= docChange;
                Application.DocumentManager.MdiActiveDocument.CloseAndDiscard();
            }
            catch { }
        }


        internal void OpenFile(string file, string city, DerivedTypeEnum derivedType)
        {
            try
            {
                fileName = file;
                cityName = city;
                dp = derivedType;
                num = 0;
                Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docChange);
                Doc= Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
                Application.DocumentManager.DocumentActivated -= docChange;
            }
            catch { }
        }
        public  void OpenFile(string file, string city)
        {
            fileName = file;
            cityName = city;
            num = 0;
            Application.DocumentManager.DocumentActivated += new DocumentCollectionEventHandler(docChange);
            Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(file);
            Application.DocumentManager.DocumentActivated -= docChange;
        }
        public  int num = 0;
        /// <summary>
        /// 文档发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void docChange(object sender, DocumentCollectionEventArgs e)
        {
            try
            {
                if (fileName == Application.DocumentManager.MdiActiveDocument.Name && num == 0)
                {
                    Doc = Application.DocumentManager.MdiActiveDocument;
                    num++;
                    MainForm mf = new MainForm(cityName, dp);
                    mf.Show();
                    // mf.Close();

                }
            }
            catch { }
        }

        /// <summary>
        /// 文档发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void docAutoChange(object sender, DocumentCollectionEventArgs e)
        {
            try
            {
                if (fileName == Application.DocumentManager.MdiActiveDocument.Name && num == 0)
                {
                    Doc = Application.DocumentManager.MdiActiveDocument;
                    num++;
                    MainForm mf = new MainForm(cityName, dp, true);
                    mf.ShowDialog();
                    // mf.Close();

                }
            }
            catch { }
        }

    }
}
