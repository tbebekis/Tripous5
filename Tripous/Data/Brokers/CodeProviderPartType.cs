using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Tripous.Data;

namespace Tripous.Data
{
    /// <summary>
    /// <para>The <see cref="CodeProviderDef"/> decriptor provides information regarding the parts that make up a Code, passing a definition text to a <see cref="CodeProvider"/>.</para>
    /// <para>The text that is passed to a <see cref="CodeProvider"/> by its <see cref="CodeProviderDef"/> descriptor comprises of parts, separated by the pipe | character.</para>     
    /// <para>This <see cref="CodeProviderPartType"/> enum type indicates how to handle a part of that text.</para>
    /// <para>CAUTION: The last part could be a <see cref="CodeProviderPartType.NumericSelect"/>, a <see cref="CodeProviderPartType.Sequencer"/> or a <see cref="CodeProviderPartType.Pivot"/> part. </para>
    /// <para>CAUTION: The <see cref="CodeProviderPartType.Pivot"/> part value is incremented by the code producer. </para>
    /// <para>CAUTION: No more than a single <see cref="CodeProviderPartType.Pivot"/> part is allowed. </para>
    /// <para>Examples of text:</para>
    /// <list type="bullet" >
    /// <item><term>SO|XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/> (Sales Order) and <see cref="CodeProviderPartType.Pivot"/></description></item>
    /// <item><term>SO|select Code from Country where Id = :CountryId|XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/>, with <see cref="CodeProviderPartType.NumericSelect"/> and <see cref="CodeProviderPartType.Pivot"/>. 
    /// The <c>:CountryId</c> parameter comes from the <see cref="DataRow"/> passed to code producer.</description></item>
    /// <item><term>SI|Sequencer;MySequencer;XXX-XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/> (Sales Invoice) and <see cref="CodeProviderPartType.Sequencer"/> with format.</description></item>
    /// <item><term>PO|YYYY-MM-DD|XXX-XXX</term><description><see cref="CodeProviderPartType.Literal"/> (Purchases Order), <see cref="CodeProviderPartType.Date"/> and <see cref="CodeProviderPartType.Pivot"/></description></item>
    /// </list>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodeProviderPartType
    {

        /// <summary>
        /// Indicates that the specified text is a literal string.
        /// <para>This part may include any character</para>
        /// <para>NOTE: This part has no format.</para>
        /// <para>Examples:</para>
        /// <list type="bullet" >
        /// <item><term>SO</term><description>Sales Order</description></item>
        /// <item><term>SI</term><description>Sales Invoice</description></item>
        /// </list>
        /// </summary>
        Literal,
        /// <summary>
        /// Indicates that the specified text is any combination of YYYY, YY, MM, DD and any valid separator character such as <c>'.', '-', '\\', '/', ' '</c>.
        /// <para>NOTE: This part has no format.</para>
        /// <para>Examples:</para>
        /// <list type="bullet" >
        /// <item><term>DD.MM.YYYY</term><description>Part with 4-digit year and dot separator.</description></item>
        /// <item><term>YYYY-MM-DD</term><description>Part with 4-digit year and minus separator</description></item>
        /// </list>
        /// </summary>
        Date,
        /// <summary>
        /// Indicates that the specified text is a SELECT statement which returns a single numeric value or a string value where all characters are numbers.
        /// <para>Could be something as <c>  select FIELD_NAME from @TABLE_NAME </c></para>
        /// <para>The text of the this part may contain the @TABLE_NAME placeholder, 
        /// which is then replaced by the value of a specified TableName passed to the code producer.</para>
        /// <para>NOTE: This part always has a format.</para>
        /// <para>NOTE: This part always starts with the word SELECT.</para>
        /// <para>Examples:</para>
        /// <list type="bullet" >
        /// <item><term>select max(NumberField) from @TABLE_NAME;XXX-XXX</term><description>Part with format.</description></item>
        /// <item><term>select max(NumberLikeField) from @TABLE_NAME;XXX-XXX</term><description>Part with format.</description></item>
        /// </list>
        /// </summary>
        NumericSelect,
        /// <summary>
        /// Indicates that the specified text is the Name of a Firebird Generator or Oracle Sequencer which already exists. 
        /// <para>WARNING: Returns the next value of the generator/sequencer.</para>
        /// <para>NOTE: This part always has a format.</para>
        /// <para>NOTE: This part always starts with the word SEQUENCER.</para>
        /// <para>Examples:</para>
        /// <list type="bullet" >
        /// <item><term>Sequencer;MySequencer;XXX-XXX-XXX</term><description>Part with format.</description></item>
        /// </list>
        /// </summary>
        Sequencer,
        /// <summary>
        /// Indicates that the specified text is any combination of the character X and any valid separator character such as <c>'.', '-', '\\', '/', ' '</c>.
        /// <para>NOTE: This part has no format. It is itself a format.</para>
        /// <para>NOTE: This part should always start with the capital letter X.</para>
        /// <para>Examples:</para>
        /// <list type="bullet" >
        /// <item><term>XXXXXX</term><description>Part without separators.</description></item>
        /// <item><term>XXX-XXX</term><description>Part with separators.</description></item>
        /// <item><term>XXX.XXX</term><description>Part with separators.</description></item>
        /// </list>
        /// </summary>
        Pivot,
    }
}
