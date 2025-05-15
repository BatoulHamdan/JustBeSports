using JustBeSports.Core.Models;

namespace JustBeSports.Core.Interfaces
{
    public interface IAdminService
    {
        #region Methods

        List<AdminModel> GetAllAdmins();

        AdminModel GetAdminById(int id);

        void AddAdmin(string username, string passwordHash, string role);

        void UpdateAdmin(int id, string username, string passwordHash, string role);

        void DeleteAdmin(int id);

        #endregion
    }
}
