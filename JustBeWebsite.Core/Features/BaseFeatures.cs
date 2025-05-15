using JustBeSports.Core.Context;

namespace JustBeSports.Core.Features
{
    public class BaseFeatures
    {
        #region Fields

        protected readonly JustBeSportsDbContext _context;

        #endregion

        #region Constructors

        public BaseFeatures(JustBeSportsDbContext context)
        {
            _context = context;
        }

        #endregion

        #region PublicMethods

        #endregion
    }
}
