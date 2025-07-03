using ECommerce.Core.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IUserRepository: IGenericRepository<User, string>
    {
        IEnumerable<User> GetCustomers();   
    }
}
