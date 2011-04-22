using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
namespace eTerm.ASyncActiveX
{
    public class SyntaxReader
    {
        // Fields
        private Color _commentColor = Color.Green;
        private Color _compareColor = Color.PaleVioletRed;
        private Color _defaultColor = Color.Black;
        private int _differencialPercentage = 0x65;
        private bool _hideStartPage = true;
        private Color _keyWordColor = Color.Blue;
        private int _oleCommentColor = 0x8000;
        private int _oleCompareColor = 0x9370db;
        private int _oleDefaultColor = -9999997;
        private int _oleKeyWordColor = 0xff0000;
        private int _oleOperatorColor = 0x808080;
        private int _oleStringColor = 0xff;
        private Color _operatorColor = Color.Gray;
        private bool _runWithIOStatistics = true;
        private Settings _settings;
        private bool _showFrmDocumentHeader = true;
        private Color _stringColor = Color.Red;
        private ArrayList Compares = new ArrayList();
        public Font EditorFont;
        private ArrayList Functions = new ArrayList();
        private ArrayList Keywords = new ArrayList();
        private ArrayList Operands = new ArrayList();
        public string ReservedWordsRegExPath = @"QUERYCOMMANDER\s|";
        private Hashtable sqlReservedWords = new Hashtable();
        public XmlNodeList xmlNodeList;
        public XmlDocument xmlReservedWords;

        // Methods
        public SyntaxReader()
        {
            this.LoadXMLDocuments();
        }

        public void FillArrays()
        {
        }

        public Color GetColor(string word)
        {
            if (this.sqlReservedWords.Contains(word))
            {
                switch (this.sqlReservedWords[word].ToString())
                {
                    case "keyword":
                        return this._keyWordColor;

                    case "operator":
                        return this._operatorColor;

                    case "compare":
                        return this._compareColor;
                }
            }
            return Color.Black;
        }

        public int GetColorRef(string word)
        {
            if (!this.sqlReservedWords.Contains(word))
            {
                return -9999997;
            }
            switch (this.sqlReservedWords[word].ToString())
            {
                case "keyword":
                    return this._oleKeyWordColor;

                case "operator":
                    return this._oleOperatorColor;

                case "compare":
                    return this._oleCompareColor;
            }
            return this._oleDefaultColor;
        }

        public bool IsFunction(string s)
        {
            return (this.Functions.BinarySearch(s) >= 0);
        }

        public bool IsKeyword(string s)
        {
            return (this.Keywords.BinarySearch(s) >= 0);
        }

        public bool IsReservedWord(string word)
        {
            if (word == null)
            {
                return false;
            }
            return this.sqlReservedWords.Contains(word.ToUpper());
        }

        private void LoadXMLDocuments()
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("eTerm.ASyncActiveX.SQLReservedWords.xml");
            this.xmlReservedWords = new XmlDocument();
            this.xmlReservedWords.Load(manifestResourceStream);
            this.xmlNodeList = this.xmlReservedWords.GetElementsByTagName("SQLReservedWords");
            ArrayList list = new ArrayList();
            foreach (XmlNode node in this.xmlNodeList[0].ChildNodes)
            {
                if (this.sqlReservedWords.Contains(node.Name))
                {
                    list.Add(node.Name);
                }
                else
                {
                    this.sqlReservedWords.Add(node.Name, node.Attributes["type"].Value);
                    this.ReservedWordsRegExPath = this.ReservedWordsRegExPath + node.Name + @"\s|";
                }
            }
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextReader textReader = new StreamReader(path);
            this._settings = (Settings)serializer.Deserialize(textReader);
            textReader.Close();
            this._hideStartPage = this._settings.HideStartPage;
            this._keyWordColor = Color.FromName(this._settings.keyWordColor);
            this._commentColor = Color.FromName(this._settings.commentColor);
            this._compareColor = Color.FromName(this._settings.compareColor);
            this._defaultColor = Color.FromName(this._settings.defaultColor);
            this._operatorColor = Color.FromName(this._settings.operatorColor);
            this._stringColor = Color.FromName(this._settings.stringColor);
            this._oleCommentColor = this._settings.Ole_commentColor;
            this._oleCompareColor = this._settings.Ole_compareColor;
            this._oleDefaultColor = this._settings.Ole_defaultColor;
            this._oleKeyWordColor = this._settings.Ole_keyWordColor;
            this._oleOperatorColor = this._settings.Ole_operatorColor;
            this._oleStringColor = this._settings.Ole_stringColor;
            this.EditorFont = new Font(this._settings.fontFamily, this._settings.fontSize, this._settings.fontStyle, this._settings.fontGraphicsUnit);
            this._differencialPercentage = this._settings.DifferencialPercentage;
            this._runWithIOStatistics = this._settings.RunWithIOStatistics;
            this._showFrmDocumentHeader = this._settings.ShowFrmDocumentHeader;
            if (!((this._differencialPercentage != 0) || this._runWithIOStatistics))
            {
                this._runWithIOStatistics = true;
                this._differencialPercentage = 0x65;
            }
        }

        public void Save()
        {
            this._settings.keyWordColor = this._keyWordColor.Name;
            this._settings.operatorColor = this._operatorColor.Name;
            this._settings.compareColor = this._compareColor.Name;
            this._settings.commentColor = this._commentColor.Name;
            this._settings.stringColor = this._stringColor.Name;
            this._settings.defaultColor = this._defaultColor.Name;
            this._settings.Ole_commentColor = this._oleCommentColor;
            this._settings.Ole_compareColor = this._oleCompareColor;
            this._settings.Ole_defaultColor = this._oleDefaultColor;
            this._settings.Ole_keyWordColor = this._oleKeyWordColor;
            this._settings.Ole_operatorColor = this._oleOperatorColor;
            this._settings.Ole_stringColor = this._oleStringColor;
            this._settings.RunWithIOStatistics = this._runWithIOStatistics;
            this._settings.DifferencialPercentage = this._differencialPercentage;
            this._settings.HideStartPage = this._hideStartPage;
            this._settings.ShowFrmDocumentHeader = this._showFrmDocumentHeader;
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, this._settings);
            textWriter.Close();
            this.LoadXMLDocuments();
        }

        public void Save(Font f)
        {
            this._settings.keyWordColor = this._keyWordColor.Name;
            this._settings.operatorColor = this._operatorColor.Name;
            this._settings.compareColor = this._compareColor.Name;
            this._settings.commentColor = this._commentColor.Name;
            this._settings.stringColor = this._stringColor.Name;
            this._settings.defaultColor = this._defaultColor.Name;
            this._settings.Ole_commentColor = this._oleCommentColor;
            this._settings.Ole_compareColor = this._oleCompareColor;
            this._settings.Ole_defaultColor = this._oleDefaultColor;
            this._settings.Ole_keyWordColor = this._oleKeyWordColor;
            this._settings.Ole_operatorColor = this._oleOperatorColor;
            this._settings.Ole_stringColor = this._oleStringColor;
            this._settings.fontFamily = f.FontFamily.Name;
            this._settings.fontGraphicsUnit = f.Unit;
            this._settings.fontSize = f.Size;
            this._settings.fontStyle = f.Style;
            this._settings.RunWithIOStatistics = this._runWithIOStatistics;
            this._settings.DifferencialPercentage = this._differencialPercentage;
            this._settings.HideStartPage = this._hideStartPage;
            this._settings.ShowFrmDocumentHeader = this._showFrmDocumentHeader;
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "EditorSettings.config");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, this._settings);
            textWriter.Close();
            this.LoadXMLDocuments();
        }

        // Properties
        public int color_comment
        {
            get
            {
                return this._oleCommentColor;
            }
            set
            {
                this._oleCommentColor = value;
            }
        }

        public int color_compare
        {
            get
            {
                return this._oleCompareColor;
            }
            set
            {
                this._oleCompareColor = value;
            }
        }

        public int color_default
        {
            get
            {
                return this._oleDefaultColor;
            }
            set
            {
                this._oleDefaultColor = value;
            }
        }

        public int color_keyword
        {
            get
            {
                return this._oleKeyWordColor;
            }
            set
            {
                this._oleKeyWordColor = value;
            }
        }

        public int color_operator
        {
            get
            {
                return this._oleOperatorColor;
            }
            set
            {
                this._oleOperatorColor = value;
            }
        }

        public int color_string
        {
            get
            {
                return this._oleStringColor;
            }
            set
            {
                this._oleStringColor = value;
            }
        }

        public Color CommentColor
        {
            get
            {
                return this._commentColor;
            }
            set
            {
                this._commentColor = value;
            }
        }

        public Color CompareColor
        {
            get
            {
                return this._compareColor;
            }
            set
            {
                this._compareColor = value;
            }
        }

        public Color DefaultColor
        {
            get
            {
                return this._defaultColor;
            }
            set
            {
                this._defaultColor = value;
            }
        }

        public int DifferencialPercentage
        {
            get
            {
                return this._differencialPercentage;
            }
            set
            {
                this._differencialPercentage = value;
            }
        }

        public bool HideStartPage
        {
            get
            {
                return this._hideStartPage;
            }
            set
            {
                this._hideStartPage = value;
            }
        }

        public Color KeyWordColor
        {
            get
            {
                return this._keyWordColor;
            }
            set
            {
                this._keyWordColor = value;
            }
        }

        public Color OperatorColor
        {
            get
            {
                return this._operatorColor;
            }
            set
            {
                this._operatorColor = value;
            }
        }

        public bool RunWithIOStatistics
        {
            get
            {
                return this._runWithIOStatistics;
            }
            set
            {
                this._runWithIOStatistics = value;
            }
        }

        public bool ShowFrmDocumentHeader
        {
            get
            {
                return this._showFrmDocumentHeader;
            }
            set
            {
                this._showFrmDocumentHeader = value;
            }
        }

        public Color StringColor
        {
            get
            {
                return this._stringColor;
            }
            set
            {
                this._stringColor = value;
            }
        }

        // Nested Types
        [Serializable]
        public class Settings
        {
            // Fields
            public string commentColor;
            public string compareColor;
            public string defaultColor;
            public int DifferencialPercentage;
            public string fontFamily;
            public GraphicsUnit fontGraphicsUnit;
            public float fontSize;
            public FontStyle fontStyle;
            public bool HideStartPage;
            public string keyWordColor;
            public int Ole_commentColor;
            public int Ole_compareColor;
            public int Ole_defaultColor;
            public int Ole_keyWordColor;
            public int Ole_operatorColor;
            public int Ole_stringColor;
            public string operatorColor;
            public bool RunWithIOStatistics;
            public bool ShowFrmDocumentHeader;
            public string stringColor;
        }
    }


}
