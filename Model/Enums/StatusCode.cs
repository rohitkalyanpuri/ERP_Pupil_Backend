﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Model
{
    public enum StatusCode
    {
        SystemException=-1,
        Unauthorized = -2,
        Ok=0,
        NoContent=1,
        AlreadyExists=2,
        PartiallyImported=3,
        NotImported=4,

    }
}
