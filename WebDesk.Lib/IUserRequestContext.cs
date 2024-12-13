namespace WebLib
{
    /// <summary>
    /// A user request context (with cookies authentication)
    /// </summary>
    public interface IUserRequestContext : IRequestContext
    {
        /// <summary>
        /// Sign-in. Authenticates a specified, already validated, Visitor
        /// </summary>
        Task SignInAsync(Requestor V, bool IsPersistent, bool IsImpersonation);
        /// <summary>
        /// Sign-out.
        /// </summary>
        Task SignOutAsync();

        /// <summary>
        /// True when the user has loged-in usin a SuperUserPassword
        /// </summary>
        bool IsImpersonation { get; }
    }
}
