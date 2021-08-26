using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tripous.Data
{
#warning TODO: Rename it WhereSql

    /// <summary>
    /// Helper for constructing a WHERE clause when SELECTing an Entity.
    /// </summary>
    public class EntityFilter
    {

        /// <summary>
        /// Boolean operators
        /// </summary>
        [Flags]
        public enum Bool
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// And
            /// </summary>
            And = 1,
            /// <summary>
            /// Or
            /// </summary>
            Or = 2,
            /// <summary>
            /// AndNot
            /// </summary>
            AndNot = 4,
            /// <summary>
            /// OrNot
            /// </summary>
            OrNot = 8
        }


        /// <summary>
        /// Conditions
        /// </summary>
        [Flags]
        public enum Condition
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Equal
            /// </summary>
            Equal = 1,
            /// <summary>
            /// Greater
            /// </summary>
            Greater = 2,
            /// <summary>
            /// GreaterOrEqual
            /// </summary>
            GreaterOrEqual = 4,
            /// <summary>
            /// Less 
            /// </summary>
            Less = 8,
            /// <summary>
            /// LessOrEqual
            /// </summary>
            LessOrEqual = 0x10,
            /// <summary>
            /// Like
            /// </summary>
            Like = 0x20,
            /// <summary>
            /// Between
            /// </summary>
            Between = 0x40,
            /// <summary>
            /// In
            /// </summary>
            In = 0x80,
            /// <summary>
            /// Null
            /// </summary>
            Null = 0x100
        }


        /// <summary>
        /// Throws an exception if a filter item is both a condition and a condition group.
        /// </summary>
        protected void Check()
        {
            if (IsCondition && IsGroup)
                throw new ApplicationException("A query filter item can not be condition and condition group at the same time");
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityFilter()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityFilter(Bool BoolOp, Condition ConditionOp, string FieldName, string Value)
        {
            this.BoolOp = BoolOp;
            this.ConditionOp = ConditionOp;
            this.FieldName = FieldName;
            this.Value = Value;
        }


        /* static */
        /// <summary>
        /// Creates the root filter
        /// </summary>
        static public EntityFilter CreateFilter()
        {
            return new EntityFilter();
        }

        /* public */
        /// <summary>
        /// Adds a group to the filter.
        /// <para>A group is used in sourrounding moultiple conditions in parenthesis, e.g. (condition0 OR condition1 AND NOT condition2)</para>
        /// </summary>
        public EntityFilter AddGroup(Bool BoolOp)
        {
            EntityFilter Result = new EntityFilter();
            Result.BoolOp = BoolOp;
            Items.Add(Result);
            return Result;
        }
        /// <summary>
        /// Adds a condition to the filter.
        /// </summary>
        public EntityFilter AddCondition(Bool BoolOp, Condition ConditionOp, string FieldName, string Value, string Value2 = "")
        {
            if (string.IsNullOrWhiteSpace(FieldName))
                throw new ApplicationException("No field name");

            if (ConditionOp == Condition.Between && string.IsNullOrWhiteSpace(Value2))
                throw new ApplicationException("BETWEEN requires two values");

            EntityFilter Result = new EntityFilter();
            Result.BoolOp = BoolOp;
            Result.ConditionOp = ConditionOp;
            Result.FieldName = FieldName;
            Result.Value = Value;
            Result.Value2 = Value2;

            Items.Add(Result);
            return Result;
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (IsGroup)
                return $"group: {BoolOp}";

            return $"condition: {BoolOp} {ConditionText}";
        }

        /* properties */
        /// <summary>
        /// The boolean operator to use
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Bool BoolOp { get; set; }
        /// <summary>
        /// The conditional operator to use
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Condition ConditionOp { get; set; }
        /// <summary>
        /// The field name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// The field value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// The second field value. Used with BETWEEN only
        /// </summary>
        public string Value2 { get; set; }
        /// <summary>
        /// Used when the item is a group. The list of conditions of a group.
        /// </summary>
        public List<EntityFilter> Items { get; } = new List<EntityFilter>();


        /// <summary>
        /// Generates and returns the filter text
        /// </summary>
        [JsonIgnore]
        public string SqlText
        {
            get
            {
                if (IsCondition)
                    return BoolText + ConditionText;
                else
                    return GroupText;
            }
        }
        /// <summary>
        /// True when is group
        /// </summary>
        [JsonIgnore]
        public bool IsGroup { get { return !IsCondition; } }
        /// <summary>
        /// True when is condition
        /// </summary>
        [JsonIgnore]
        public bool IsCondition { get { return !string.IsNullOrWhiteSpace(FieldName); } }
        /// <summary>
        /// Gets the boolean operator text
        /// </summary>
        [JsonIgnore]
        public string BoolText
        {
            get
            {
                Check();
                string Text = string.Empty;

                switch (BoolOp)
                {
                    case Bool.And: Text = "and "; break;
                    case Bool.Or: Text = "or "; break;
                    case Bool.AndNot: Text = "and not "; break;
                    case Bool.OrNot: Text = "or not "; break;
                }

                return Text;
            }
        }
        /// <summary>
        /// Gets the condition text
        /// </summary>
        [JsonIgnore]
        public string ConditionText
        {
            get
            {
                Check();
                string Text = string.Empty;

                switch (ConditionOp)
                {
                    case Condition.Equal: Text += $"{FieldName} = {Value}"; break;
                    case Condition.Greater: Text += $"{FieldName} > {Value}"; break;
                    case Condition.GreaterOrEqual: Text += $"{FieldName} >= {Value}"; break;
                    case Condition.Less: Text += $"{FieldName} < {Value}"; break;
                    case Condition.LessOrEqual: Text += $"{FieldName} <= {Value}"; break;
                    case Condition.Like: Text += $"{FieldName} like {Value}"; break;
                    case Condition.Between: Text += $"{FieldName} between {Value} and {Value2}"; break;
                    case Condition.In: Text += $"{FieldName} in ({Value})"; break;
                    case Condition.Null: Text += $"{FieldName} is null"; break;
                }

                return Text;
            }
        }
        /// <summary>
        /// Used when this is a group. Returns the group text
        /// </summary>
        [JsonIgnore]
        public string GroupText
        {
            get
            {
                Check();
                StringBuilder SB = new StringBuilder();

                bool IsFirst = true;
                string ItemText;

                foreach (var Item in Items)
                {
                    ItemText = Item.IsCondition ? Item.ConditionText : $"({Item.GroupText.Trim()})";

                    if (!IsFirst)
                    {
                        ItemText = Item.BoolText + ItemText;
                    }
                    IsFirst = false;

                    ItemText += " ";

                    if (Item.IsGroup)
                        SB.AppendLine(ItemText);
                    else
                        SB.Append(ItemText);
                }

                return SB.ToString();
            }
        }

    }

}
