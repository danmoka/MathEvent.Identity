using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace MathEvent.IdentityServer.Authorization
{
    /// <summary>
    /// Описывает возможные CRUD операции
    /// </summary>
    public static class Operations
    {
        public static OperationAuthorizationRequirement Create
        {
            get
            {
                return new OperationAuthorizationRequirement { Name = nameof(Create) };
            }
            private set { }
        }

        public static OperationAuthorizationRequirement Read
        {
            get
            {
                return new OperationAuthorizationRequirement { Name = nameof(Read) };
            }
            private set { }
        }

        public static OperationAuthorizationRequirement Update
        {
            get
            {
                return new OperationAuthorizationRequirement { Name = nameof(Update) };
            }
            private set { }
        }

        public static OperationAuthorizationRequirement Delete
        {
            get
            {
                return new OperationAuthorizationRequirement { Name = nameof(Delete) };
            }
            private set { }
        }
    }
}
