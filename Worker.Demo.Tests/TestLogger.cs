﻿using System;

namespace Worker.Demo.Tests
{
    public class TestLogger : ILogger
    {
        public void Write(string entry)
        {
            Console.WriteLine(entry);
        }
    }
}