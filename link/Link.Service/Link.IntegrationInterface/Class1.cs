﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.IntegrationInterface
{
    public interface IIntegration
    {
        string Connect();
        string Authorize(string code);
        string Publish();
    }
}
