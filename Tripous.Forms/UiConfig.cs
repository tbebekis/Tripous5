/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
 

namespace Tripous.Forms
{


    /// <summary>
    /// The tripous form system configuration and settings
    /// </summary>
    static public class UiConfig
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static UiConfig()
        {
            UiExists = Environment.UserInteractive;
            FormsNestedMode = true; 
            
            AllowCulturesInLogin = true; 
            AlternativeRowsBackColor = Color.WhiteSmoke; 

            AlwaysShowSelectActiveProfileDialog = false;
        }

 
 


        /// <summary>
        /// When false then there is no UI (Windows.Forms or ASP or anything else).
        /// <para>WARNING: The initial value for this settings comes from Environment.UserInteractive,
        /// meaning it is false for Window Service processes and IIS processes.
        /// Initially it is true for Service processes running in console mode.</para>
        /// </summary>
        static public bool UiExists { get; set; }
        /// <summary>
        /// Indicates the ui type (In this version should always return true)
        /// <para>Defaults to true.</para>
        /// </summary>
        static public bool FormsNestedMode { get; set; }
 
 
        /// <summary>
        /// When true then a Windows Forms application displays a close question box when closing.
        /// <para>Defaults to false.</para>
        /// </summary>
        static public bool DisplayCloseQuestionBox { get; set; }
        /// <summary>
        /// When true then the Ui uses repeatable inserts, that is goes from to insert mode again
        /// after a succesfull pair of insert-commit
        /// </summary>
        static public bool RepeatableInserts { get; set; }
        /// <summary>
        /// When true then the Ui uses repeatable edits, that is goes from to edit mode again
        /// after a succesfull pair of edit-commit
        /// </summary>
        static public bool RepeatableEdits { get; set; }
        /// <summary>
        /// When true, then the dialog for selecting the active profile, 
        /// is displayed before the login dialog.
        /// <para>Defautls to false.</para>
        /// </summary>
        static public bool AlwaysShowSelectActiveProfileDialog { get; set; }
        /// <summary>
        /// When true, the user may choose the application culture in the login dialog box
        /// <para>Defaults to true.</para>
        /// </summary>
        static public bool AllowCulturesInLogin { get; set; }
 

        /// <summary>
        /// The background color of the odd-numbered rows in grids.
        /// <para>Defaults to Color.WhiteSmoke</para>
        /// </summary>
        static public Color AlternativeRowsBackColor { get; set; }
 
 
 
 
    }
}
