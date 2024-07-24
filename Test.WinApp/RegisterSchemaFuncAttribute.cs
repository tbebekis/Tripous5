 

namespace Test.WinApp
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class RegisterSchemaFuncAttribute: Attribute
    {
        public RegisterSchemaFuncAttribute(int Version)
        {
            this.Version = Version;
        }

        public int Version { get; set; } = 1;
    }
}
