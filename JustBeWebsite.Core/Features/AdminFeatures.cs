using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;

namespace JustBeSports.Core.Features
{
    public class AdminFeatures : BaseFeatures
    {
        #region Constructors

        public AdminFeatures(JustBeSportsDbContext context) : base(context)
        {
        }

        #endregion

        #region Methods

        public void Insert(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public Admin GetById(int id)
        {
            return _context.Admins
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Admin> GetAll()
        {
            return _context.Admins.ToList();
        }

        public void Update(Admin updatedAdmin)
        {
            var admin = _context.Admins
                .FirstOrDefault(x => x.Id == updatedAdmin.Id);

            if (admin != null)
            {
                admin.Username = updatedAdmin.Username;
                admin.PasswordHash = updatedAdmin.PasswordHash;

                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var admin = _context.Admins
                .FirstOrDefault(x => x.Id == id);

            if (admin != null)
            {
                _context.Admins.Remove(admin);
                _context.SaveChanges();
            }
        }

        #endregion
    }
}
