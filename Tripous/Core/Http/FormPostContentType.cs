namespace Tripous
{
    /// <summary>
    /// Indicates the <c>Content-Type</c> used in a Fomr POST.
    /// <para><c>application/x-www-form-urlencoded</c> should be used in general, for simple text data, such as form fields. </para>
    /// <para> <c>multipart/form-data</c> should be used for binary data, such as files.</para>
    /// <para>In any case it is crucial what the server expects.</para>
    /// </summary>
    public enum FormPostContentType
    {
        /// <summary>
        /// <para><c>Content-Type</c> Header: <c>application/x-www-form-urlencoded</c></para>
        /// <para><c>application/x-www-form-urlencoded</c> is the default format used by HTML forms. </para>
        /// <para>In this format, the data is encoded as key-value pairs separated by &amp; characters, with the key and value separated by = characters. </para>
        /// <para>For example, key1=value1&amp;key2=value2 .</para>
        /// <para>This format is simple and efficient, but it has limitations in terms of the types of data that can be sent</para>
        /// </summary>
        FormUrlEncoded,
        /// <summary>
        /// <para><c>Content-Type</c> Header: <c>multipart/form-data</c></para>
        /// <para><c>multipart/form-data</c> is a more flexible format that can be used to send binary data, such as files, as well as text data. </para>
        /// <para>In this format, the data is divided into multiple parts, each with its own set of headers.</para>
        /// <para>Each part is separated by a boundary string, which is specified in the <c>Content-Type</c> header. </para>
        /// <para>This format is more complex than <c>application/x-www-form-urlencoded</c>, but it allows for more types of data to be sent.</para>
        /// </summary>
        MultipartFormData,

    }
}
 