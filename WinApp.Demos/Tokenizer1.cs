﻿using System;
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

   
        void Test()
        {
            string Text = "12 12.34 .1234 1234e-2"; // @"a(1)";
            AppendLine($">> Text: {Text}");

            TokenAssembly A = new TokenAssembly(Text);
            AppendLine(A.ToString());

            foreach (string Element in A)
                AppendLine(Element);
        }
        void Test_Tokenizer()
        {
            AppendLine($">> Tokenizer.NextToken()");

            string Text = @"Let's 'rock and roll'!";            
            AppendLine($">> Text: {Text}");
            AppendSplitLine();

            Tokenizer Tokenizer = new Tokenizer();
            Tokenizer.SetString(Text);

            Token T;

            while (true)
            {
                T = Tokenizer.NextToken();
                AppendLine(T.DisplayText);

                if (T.Kind == Token.TT_EOF)
                {     
                    break;
                }                 
            }

            AppendSplitLine();
        }
        void Test_AssemplyDisplay()
        {
            AppendLine($">> TokenAssembly.ToString()");

            // 1.
            string Text = "Congress admitted Colorado in 1876."; // @"1.2"; // Congress admitted Colorado in 1876.
            AppendLine($">> Text: {Text}");
            AppendSplitLine();

            TokenAssembly A = new TokenAssembly(Text);
            AppendLine(A.ToString());

            AppendSplitLine();

            // 2.
            Text = @"admitted(colorado, 1876) ";
            AppendLine($">> Text: {Text}");

            A = new TokenAssembly(Text);
            AppendLine(A.ToString());

            AppendSplitLine();
        }
        void Test_Assembly_NextElement()
        {
            AppendLine(">> TokenAssembly.NextElement()");

            string Text =  "aa bb cc yy ; 123 a";
            AppendLine($">> Text: {Text}");
            AppendSplitLine();

            TokenAssembly A = new TokenAssembly(Text);
            AppendLine(A.ToString());   
            
            //while (A.HasMoreElements)
            //    AppendLine(A.NextElement());

            foreach (var item in A)
                AppendLine(item);
               
 
            AppendSplitLine();
        }
        void Test_TerminalParserWithTokenAssemply()
        {
            // 2.5.1 Using Terminals
            AppendLine(">> WordTerminalParser with a TokenAssembly");
            string Text = "steaming hot coffee";
            AppendLine($">> Text: {Text}");
            AppendSplitLine();

            Parser Parser = new WordTerminalParser();
            Assembly A = new TokenAssembly(Text);

            while (true)
            {
                A = Parser.BestMatch(A);
                if (A == null)
                    break;
                AppendLine(A.ToString());
            }

            AppendSplitLine();
        }
        void Test_QuotedStringParserWithTokenAssemply()
        {
            // 2.5.7 Quoted Strings
            AppendLine(">> QuotedStringTerminalParser with a TokenAssembly");
            string Text = "\"Clark Kent\"";
            AppendLine($">> Text: {Text}");
            AppendSplitLine();

            Parser Parser = new QuotedStringTerminalParser();
            Assembly A = new TokenAssembly(Text);

            A = Parser.BestMatch(A);
            if (A != null)
                AppendLine(A.ToString());
            else
                AppendLine("[no match]");

            AppendSplitLine();
        }
        void Test_RepetitionParserWithWordTerminalParser()
        {
            // [[]^steaming/hot/coffee, [steaming]steaming^hot/coffee, [steaming, hot]steaming/hot^coffee, [steaming, hot, coffee]steaming/hot/coffee^]

            // 2.6.1 Repetition
            AppendLine(">> RepetitionParser with a WordTerminalParser sub-parser");
            string Text = "steaming hot coffee";
            AppendLine($">> Text: {Text}");
            AppendSplitLine();

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
        void Test_CompositeParsers()
        {
            // 2.6.2 Alternation and Sequence
            AppendLine(">> SequenceCollectionParser with composite parsers: Alternation, Sequence and Repetition parser");
            string Text = "hot hot steaming hot coffee";
            AppendLine($">> Text: {Text}");
            AppendSplitLine();

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

            //Test();

            //Test_Tokenizer();
            //Test_AssemplyDisplay();
            //Test_Assembly_NextElement();
            //Test_TerminalParserWithTokenAssemply();
            //Test_QuotedStringParserWithTokenAssemply();
            //Test_RepetitionParserWithWordTerminalParser();

            Test_CompositeParsers();
        }

        public bool Singleton => false;
        public string Title => "Tokenizer 1";
        public string Description => "Tokenizing and parsing demo";
    }



}
