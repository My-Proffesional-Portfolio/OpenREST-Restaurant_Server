using Microsoft.EntityFrameworkCore.Storage;
using OpenRestRestaurant_data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Utils
{
    public class TransactionManager
    {
        private readonly OpenRestRestaurantDbContext _context;
        public TransactionManager()
        {

        }
        public TransactionManager(OpenRestRestaurantDbContext context)
        {
            _context = context;
        }


        public virtual async Task RunTransaction(Action function)
        {

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    function();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Exception on transaction: " + ex.Message);
                }
            }
        }
    }
}
