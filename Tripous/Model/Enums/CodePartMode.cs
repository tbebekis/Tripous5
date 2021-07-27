/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tripous.Model
{
    /// <summary>
    /// Indicates the type of the Text of a <see cref="CodePart"/>
    /// </summary>
    [TypeStoreItem]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodePartMode
    {
        /// <summary>
        /// Indicates that the Text is a FieldName which belongs to a table passed to <see cref="CodeProducer.MainTableName"/>.
        /// <para>Setting that value forces CodeProducer to construct and execute a SQL statement similar to</para>
        /// <para><c>select max(FieldName) as RESULT from MainTableName</c></para>
        /// <para>WARNING: If a PrefixPart is defined, then the CodeProducer assumes that the FieldName
        /// given as Text is the field where the produced code is going to be stored and it is a string field.</para>
        /// </summary>
        FieldName,
        /// <summary>
        /// Indicates that the Text is a SELECT statement which returns a single value.
        /// <para>Could be something as <c>  select max(CODE) from :@TABLE_NAME </c></para>
        /// <para>In this mode the Text may contain the :@TABLE_NAME placeholder, which is then replaced by 
        /// the value of the <see cref="CodeProducer.MainTableName"/>.</para>
        /// <para></para>
        /// <example> Here is a Text example. The CUSTOMER_ID value comes form the  <see cref="CodeProducer.CurrentRow"/> DataRow.
        /// <code>
        /// select CODE from CUSTOMER where ID = :CUSTOMER_ID
        /// </code>
        /// </example>
        /// <example> Here is another example
        /// <code>
        /// select CODE from :@TABLE_NAME where ID = :CUSTOMER_ID
        /// </code>
        /// </example>
        /// </summary>
        LookUpSql,
        /// <summary>
        /// Indicates that the Text is the Name of a Firebird Generator or 
        /// Oracle Sequencer which already exists. 
        /// <para>WARNING: The CodeProducer gets the next value of the generator/sequencer.</para>
        /// </summary>
        Sequencer,
        /// <summary>
        /// Indicates that the Text is a literal string.
        /// <para>A Literal text may include the following placeholders</para>
        ///  <list type="bullet"> 
        ///    <description> YY        two digit year </description>
        ///    <description> YYYY      four digit year </description>
        ///    <description> MM        two digit month </description>
        ///    <description> DD        two digit day of the month </description>
        ///  </list>
        /// <para>All other characters are considered literals  </para>      
        /// </summary>
        Literal,
    }
}
