﻿using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.UnitTests.Common;
public abstract class BaseFixture
{
    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }

    public Faker Faker { get;  set; }
}
