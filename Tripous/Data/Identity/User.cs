namespace Tripous.Identity
{
    /// <summary>
    /// The user entity
    /// </summary>
    public class User: IdentityEntity
    {
        /* private */
        const string SDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        protected override void SetPropertyValue(PropertyInfo Prop, object Value)
        {
            if (string.Compare("LockoutEndUtc", Prop.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                if (!Sys.IsNull(Value) && !string.IsNullOrWhiteSpace(Value.ToString()))
                {
                    DateTime V = DateTime.ParseExact(Value.ToString(), SDateTimeFormat, CultureInfo.InvariantCulture);
                    this.LockoutEndUtc = V;
                }
                else
                {
                    this.LockoutEndUtc = DateTime.MinValue;
                }
            }
            else
            {
                base.SetPropertyValue(Prop, Value);
            }
            
        }
        /// <summary>
        /// Sets the value of a parameter that is going to be used with Sql statemement execution.
        /// <para>In the specified dictionary there should be an entry for each property of this instance.</para>
        /// </summary>
        protected override void SetDictionaryParam(PropertyInfo Prop, Dictionary<string, object> Params, object Value)
        {
            if (string.Compare("LockoutEndUtc", Prop.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                DateTime? V = (DateTime?)Value;
                Value = V.HasValue ? V.Value.ToString(SDateTimeFormat) : "";

                Params[Prop.Name] = Value;
            }
            else
            {
                base.SetDictionaryParam(Prop, Params, Value);
            }           
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public User()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public User(DataRow Row)
        {
            Assign(Row);
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return UserName;
        }
        /// <summary>
        /// It is called just before this instance is save to the database
        /// </summary>
        public override void BeforeSave(bool IsInsert)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new ApplicationException("UserName not defined");
            }

            if (string.IsNullOrWhiteSpace(NormalizedUserName))
            {
                NormalizedUserName = UserName.Normalize().ToUpperInvariant();
                IdDb.CheckUniqueStringField(this.TableName, "NormalizedUserName", NormalizedUserName, this.Id);
            }                

            if (!string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(NormalizedEmail))
            {
                NormalizedEmail = Email.Normalize().ToUpperInvariant();
                IdDb.CheckUniqueStringField(this.TableName, "NormalizedEmail", NormalizedEmail, this.Id);
            }
 
        }

        /* properties */
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Normalized UserName
        /// </summary>
        public string NormalizedUserName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Normalized Email
        /// </summary>
        public string NormalizedEmail { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// True when email is confirmed
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// True when phone is confirmed
        /// </summary>
        public bool PhoneConfirmed { get; set; }
        /// <summary>
        /// Failure counter of access attempts
        /// </summary>
        public int AccessFailedCount { get; set; }
        /// <summary>
        /// True when the lockout of the user is active
        /// </summary>
        public bool LockoutEnabled { get; set; }
        /// <summary>
        /// Indicates the date and time where user lockout expires
        /// </summary>
        public DateTime LockoutEndUtc { get; set; } = DateTime.MinValue;
        /// <summary>
        /// True when Two Factor Authentication is enabled for this user
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
 
    }
}
