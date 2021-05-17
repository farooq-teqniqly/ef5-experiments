using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepthFirstSearchExample
{
    public class ResourceEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ParentResourceId { get; set; }

        public string Path { get; set; }

        
        public ResourceEntity ParentResource { get; set; }

        public ICollection<ResourceEntity> InverseParentResource { get; set; }
    }
}
