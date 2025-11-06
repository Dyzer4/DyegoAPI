namespace TIVIT.CIPA.Api.Domain.Model.Results
{
    public class PermissionResult
    {
        public User User { get; private set; }
        public Profile Role { get; private set; }
        public IEnumerable<Action> Actions { get; private set; }

        public static PermissionResult Create(User user, Profile role, IEnumerable<Action> actions)
        {
            return new PermissionResult
            {
                User = user,
                Role = role,
                Actions = actions
            };
        }
    }
}
