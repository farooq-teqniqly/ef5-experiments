﻿using System;
using System.Collections.Generic;

namespace DepthFirstSearchExample
{
    public class ResourceDto
    {
        public Guid Id { get; set; }

        public string Path { get; set; }
        
        public ResourceDto Parent { get; set; }

        public IEnumerable<ResourceDto> Children { get; set; }
    }
}