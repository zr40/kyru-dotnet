﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kyru.Core
{
    class Config
    {
        internal readonly string storeDirectory;

        Config() {
            storeDirectory = Path.Combine(System.Windows.Forms.Application.UserAppDataPath, "kyru" ,"backups");
        }
    }
}