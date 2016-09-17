using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IBatisNet.Common
{
    public interface IConnectionAdapter
    {
        IDbConnection Adapt(IDbConnection cn);
    }
}
