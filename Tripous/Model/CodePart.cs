﻿/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Model
{
    /// <summary>
    /// Represents a part of a string Code which is automatically generated by a <see cref="CodeProducer"/>.
    /// </summary>
    public class CodePart : IAssignable
    {
 
        /// <summary>
        /// Constructor
        /// </summary>
        public CodePart()
        {
        }

        /// <summary>
        /// Clear this instance
        /// </summary>
        public virtual void Clear()
        {
        }
        /// <summary>
        /// Assignes Source to this instance.
        /// </summary>
        public virtual void Assign(object Source)
        {
            if (Source is CodePart)
            {
                Mode = (Source as CodePart).Mode;
                Text = (Source as CodePart).Text;
                Format = (Source as CodePart).Format;
            }

            if (Assigned != null)
                Assigned(this, EventArgs.Empty);
        }

        /// <summary>
        /// Clones this instance
        /// </summary>
        public object Clone()
        {
            CodePart Result = new CodePart();
            Result.Assign(this);
            return Result;
        }

        /// <summary>
        /// Gets or sets Mode which determines the meaning of the Text property. 
        /// </summary>
        public CodePartMode Mode { get; set; } = CodePartMode.FieldName;

        /// <summary>
        /// Gets or sets the Text.
        /// <para>The meaning of the Text is determined by the value of the <see cref="Mode"/>.</para>
        /// <para>When the mode is <see cref="CodePartMode.LookUpSql"/> the Text may contain
        /// the :@TABLE_NAME placeholder which is then replaced by the value of the <see cref="CodeProducer.MainTableName"/></para>
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Format text. The Format is used in formatting the code part, after
        /// retreiving its value.
        /// <para> Could be something as <c>XX-XX.</c> or  <c>XXXX.</c> or <c>YY-XX-XX. </c></para>
        /// <para>The Format can NOT contain literal characters.</para>
        /// <para>The only valid characters are the following</para>
        /// <list type="bullet"> 
        ///   <description> <c>X                  = digit of the Part</c>  </description><para></para>
        ///   <description> <c>., -, \, /, ' '    = digit separators of the Part</c>  </description><para></para>
        /// </list>     
        ///  for instance XXX-XXX or XX.XX.XXX      
        /// <para></para>     
        /// <para>The length of the Format is calculated by counting the literal and the X characters. </para> 
        /// <example>Example
        /// <code>    XX-XX    total length 5, length 4      </code>
        ///  </example>  
        /// </summary>
        public string Format { get; set; } = "";

        /// <summary>
        /// Occurs after a successful call to Assign()
        /// </summary>
        public event EventHandler Assigned;
    }
}
