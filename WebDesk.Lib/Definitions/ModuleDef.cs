using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk
{
    /// <summary>
    /// A module definition
    /// </summary>
    public class ModuleDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// A name unique among all instances of this type
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Resource string key
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Icon key
        /// </summary>
        public string IconKey { get; set; }

        /// <summary>
        /// The author (company, person, etc) who created this instance
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Creation date-time
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Modification date-time
        /// </summary>
        public DateTime ModifiedOn { get; set; }
    }

     
    /// <summary>
    /// Table definition
    /// </summary>
    public class DataTableDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// A name unique among all instances of this type
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Resource string key
        /// </summary>
        public string TitleKey { get; set; }

        /// <summary>
        /// Fields
        /// </summary>
        public List<DataFieldDef> Fields { get; set; } = new List<DataFieldDef>();
        /// <summary>
        /// UniqueConstraints
        /// </summary>
        public List<UniqueConstraintDef> UniqueConstraints { get; set; } = new List<UniqueConstraintDef>();
        /// <summary>
        /// ForeignKeys
        /// </summary>
        public List<ForeignKeyDef> ForeignKeys { get; set; } = new List<ForeignKeyDef>();
    }


    /// <summary>
    /// The data-type of a data field
    /// </summary>
    [Flags]
    public enum DataFieldType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// String (nvarchar, varchar)
        /// </summary>
        String = 1,
        /// <summary>
        /// Integer
        /// </summary>
        Integer = 2,
        /// <summary>
        /// Float (float, double precision, etc)
        /// </summary>
        Float = 4,
        /// <summary>
        /// Decimal (decimal(18, 4))
        /// </summary>
        Decimal = 8,
        /// <summary>
        /// Date (date)
        /// </summary>
        Date = 0x10,
        /// <summary>
        /// DateTime (datetime, timestamp, etc)
        /// </summary>
        DateTime = 0x20,
        /// <summary>
        /// Boolean (integer always, 1 = true, else false)
        /// </summary>
        Boolean = 0x40,
        /// <summary>
        /// Blob
        /// </summary>
        Blob = 0x80,
        /// <summary>
        /// Text Blob
        /// </summary>
        TextBlob = 0x100,
    }


    /// <summary>
    /// Field definition
    /// </summary>
    public class DataFieldDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// A name unique among all instances of this type
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Resource string key
        /// </summary>
        public string TitleKey { get; set; }

        /// <summary>
        /// True when the field is a primary key
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// The data-type of the field. One of the <see cref="DataFieldType"/> constants.
        /// </summary>
        public DataFieldType DataType { get; set; }
        /// <summary>
        /// Field length. Applicable to varchar fields only.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// True when the field is NOT nullable  
        /// </summary>
        public bool NotNull { get; set; }   // when true then produces 'not null'
        /// <summary>
        /// The default expression, if any. E.g. 0, or ''. Defaults to null.
        /// </summary>
        public string Default { get; set; } // e.g. produces default 0, or default ''
    }


    /// <summary>
    /// constraint UC_{TABLE_NAME}_00 unique (FIELD_NAME)
    /// </summary>
    public class UniqueConstraintDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The field name upon where the constraint is applied
        /// </summary>
        public string FieldName { get; set; }
    }

    /// <summary>
    /// constraint FC_{TABLE_NAME}_00 foreign key (FIELD_NAME) references FOREIGN_TABLE_NAME (FOREIGN_FIELD_NAME)
    /// </summary>
    public class ForeignKeyDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The field name upon where the constraint is applied
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// The foreign table name
        /// </summary>
        public string ForeignTableName { get; set; }
        /// <summary>
        /// The foreign field name  
        /// </summary>
        public string ForeignFieldName { get; set; }

    }


}
