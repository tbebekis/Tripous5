using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tripous.Tokenizing;
using Tripous.Parsing;

namespace WinApp.Demos
{
    public partial class Tokenizer1 : UserControl
    {
        public DemoTokenizer1 Demo;

        void AnyClick(object sender, EventArgs ea)
        {
            if (btnExecute == sender)
                Demo.Execute(edtEditor.Text);
        }

        void ControlInitialize()
        {
            btnExecute.Click += AnyClick;
        }

        public void Clear()
        {
            edtLog.Clear();
        }
        public void Append(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                edtLog.AppendText(Text);
            }
        }
        public void AppendLine(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                edtLog.AppendText(Text + Environment.NewLine);
            }
        }
        public void Log(string Text = null)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Clear();
            }
            else
            {
                AppendLine(Text);
            }
        }

        public Tokenizer1()
        {
            InitializeComponent();

            if (!DesignMode)
                ControlInitialize();
        }
    }

      

    public class DemoTokenizer1 : IDemo
    {
        Tokenizer1 fControl;

        public void ShowUi(TabPage Page)
        {
            if (fControl == null)
            {
                fControl = new Tokenizer1();
                fControl.Parent = Page;
                fControl.Dock = DockStyle.Fill;

                Page.Text = this.Title;
                fControl.Demo = this;
            }
        }
        void AppendLine(string Text)
        {
            fControl.AppendLine(Text);
        }
        void AppendSplitLine()
        {
            fControl.AppendLine("----------------------------------------------");
        }

   

        void Use_Tokenizer(string Text)
        {
            AppendLine(">> Tokenizer.NextToken()");
             
            TokenString TS = new TokenString(Text);
            //AppendLine($"TokenString Length: {TS.Length()}");
            //AppendLine($"{TS.ToString()}");

            //Text = "a b c";

            Tokenizer Tokenizer = new Tokenizer();
            Tokenizer.SetString(Text);

            Token T;

            while (true)
            {
                T = Tokenizer.NextToken();
                //AppendLine(!string.IsNullOrWhiteSpace(T.Kind.Name)? T.Kind.Name: "<UNKNOWN TOKEN>");
                AppendLine(T.DisplayText);

                if (T.Kind == Token.TT_EOF)
                {     
                    break;
                }                 
            }

            AppendSplitLine();
        }
        void Use_Assembly_NextElement(string Text)
        {
            // Text: Let's 'rock and roll'!
            Text =  "aa bb cc yy ; 123 a";
            AppendLine(">> TokenAssembly.NextElement()");
            TokenAssembly A = new TokenAssembly(Text);
            AppendLine(A.ToString());   // []^aa/bb/cc
            
            //while (A.HasMoreElements)
            //    AppendLine(A.NextElement());

            foreach (var item in A)
                AppendLine(item);
               
 
            AppendSplitLine();
        }
        void Use_TerminalParserWithTokenAssemply(string Text)
        {

            // Text: Let's 'rock and roll'!
            AppendLine(">> WordTerminalParser with a TokenAssembly");

            Parser Parser = new WordTerminalParser();
            Assembly A = new TokenAssembly(Text);

            A = Parser.BestMatch(A);
            if (A != null)
                AppendLine(A.ToString());
            else
                AppendLine("[no match]");

            AppendSplitLine();
        }
        void Use_QuotedStringParserWithTokenAssemply(string Text)
        {
            // Text: "steaming hot coffee"
            AppendLine(">> QuotedStringTerminalParser with a TokenAssembly");

            Parser Parser = new QuotedStringTerminalParser();
            Assembly A = new TokenAssembly(Text);

            A = Parser.BestMatch(A);
            if (A != null)
                AppendLine(A.ToString());
            else
                AppendLine("[no match]");

            AppendSplitLine();
        }
        void Use_RepetitionParserWithWordTerminalParser(string Text)
        {
            // Text: steaming hot coffee
            // [[]^steaming/hot/coffee, [steaming]steaming^hot/coffee, [steaming, hot]steaming/hot^coffee, [steaming, hot, coffee]steaming/hot/coffee^]
            // []^steaming/hot/coffee   [steaming]steaming^hot/coffee  [hot,steaming]steaming/hot^coffee   [coffee,hot,steaming]steaming/hot/coffee^
            Text = "steaming hot coffee";
            AppendLine(">> RepetitionParser with a WordTerminalParser sub-parser");

            TokenAssembly A = new TokenAssembly(Text);
            Parser SubParser = new WordTerminalParser();
            Parser Parser = new RepetitionParser(SubParser);
            ArrayList List = new ArrayList();
            List.Add(A);

            List = Parser.Match(List);

            if (List != null)
                AppendLine(Tokenizer.ToString(List));
            else
                AppendLine("[no match]");

            AppendSplitLine();
        }
        void Use_CompositeParsers(string Text)
        {
            // Text: hot hot steaming hot coffee
            Text = "hot hot steaming hot coffee";
            AppendLine(">> SequenceCollectionParser with composite parsers: Alternation, Sequence and Repetition parser");

            AlternationCollectionParser adjective = new AlternationCollectionParser();
            adjective.Add(new LiteralTerminalParser("steaming"));
            adjective.Add(new LiteralTerminalParser("hot"));

            SequenceCollectionParser good = new SequenceCollectionParser();
            good.Add(new RepetitionParser(adjective));
            good.Add(new LiteralTerminalParser("coffee"));

            Assembly A = new TokenAssembly(Text);
            A = good.BestMatch(A);

            if (A != null)
                AppendLine(A.ToString());
            else
                AppendLine("[no match]");
        }
        
        
        public void Execute(string Text)
        {
            fControl.Clear();
            Use_Tokenizer(Text);
            //Use_Assembly_NextElement(Text);
            //Use_TerminalParserWithTokenAssemply(Text);
            //Use_QuotedStringParserWithTokenAssemply(Text);
            //Use_RepetitionParserWithWordTerminalParser(Text);
            //Use_CompositeParsers(Text);
        }

        public bool Singleton => false;
        public string Title => "Tokenizer 1";
        public string Description => "TokenAssembly demo. Uses the NextElement() ";
    }



}
