using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;

namespace JustBeSports.Core.Services
{
    public class AdminService : IAdminService
    {
        #region Fields

        private readonly AdminFeatures _adminFeatures;

        #endregion

        #region Constructors

        public AdminService(AdminFeatures adminFeatures)
        {
            _adminFeatures = adminFeatures;
        }

        #endregion

        #region Methods

        public List<AdminModel> GetAllAdmins()
        {
            return _adminFeatures.GetAll()
                .Select(x => (AdminModel)x)
                .ToList();
        }

        public AdminModel GetAdminById(int id)
        {
            var admin = _adminFeatures.GetById(id);
            return admin != null ? (AdminModel)admin : null;
        }

        public void AddAdmin(string username, string passwordHash, string role)
        {
            var existingAdmin = _adminFeatures.GetAll()
                .FirstOrDefault(a => a.Username == username);

            if (existingAdmin != null)
            {
                throw new InvalidOperationException("Admin with this username already exists.");
            }

            var admin = new AdminModel
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = role
            };

            _adminFeatures.Insert(admin);
        }

        public void UpdateAdmin(int id, string username, string passwordHash, string role)
        {
            var admin = _adminFeatures.GetById(id);
            if (admin != null)
            {
                admin.Username = username;
                admin.PasswordHash = passwordHash;
                admin.Role = role;
                _adminFeatures.Update(admin);
            }
        }

        public void DeleteAdmin(int id)
        {
            var admin = _adminFeatures.GetById(id);
            if (admin != null)
            {
                _adminFeatures.Delete(id);
            }
        }

        #endregion
    }
}
