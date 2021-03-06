﻿using Autofac;
using RS3Bot.Abstractions.Interfaces;
using System;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface ITaskHandler : IDisposable, IStartable
    {
    }
}