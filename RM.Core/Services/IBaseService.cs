using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Core.Services
{
    public interface IBaseService
    {
        FileStreamResult GetPathOfResource(string fileName);
    }
}
