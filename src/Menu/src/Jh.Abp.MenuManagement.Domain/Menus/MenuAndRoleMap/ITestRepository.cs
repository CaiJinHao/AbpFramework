using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jh.Abp.MenuManagement.Menus
{
    public interface ITestRepository
    {
        Task<IEnumerable<string>> GetEmailsAsync();
    }
}
