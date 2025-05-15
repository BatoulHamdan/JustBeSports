using JustBeSports.Core.Models;

namespace JustBeSports.Core.Entities
{
    partial class Admin
    {
        public static implicit operator AdminModel(Admin item)
        {
            AdminModel retValue = null;
            if (item != null)
            {
                retValue = new AdminModel
                {
                    Id = item.Id,
                    Username = item.Username,
                    PasswordHash = item.PasswordHash,
                    Role = item.Role,
                };
            }
            return retValue;
        }

        public static implicit operator Admin(AdminModel item)
        {
            Admin retValue = null;
            if (item != null)
            {
                retValue = new Admin
                {
                    Id = item.Id,
                    Username = item.Username,
                    PasswordHash = item.PasswordHash,
                    Role= item.Role,
                };
            }
            return retValue;
        }
    }
}
