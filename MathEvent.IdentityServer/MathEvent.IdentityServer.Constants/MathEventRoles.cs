namespace MathEvent.IdentityServer.Constants
{
    /// <summary>
    /// Роли сервера MathEvent
    /// </summary>
    public static class MathEventRoles
    {
        public const string Administrator = "mathevent.admin";
        public const string Moderator = "mathevent.moderator";

        public const string Executive = Administrator + "," + Moderator;
    }
}
