using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepthFirstSearchExample
{
    public class ResourceEntityToDtoMapper
    {
        public static ResourceDto Map(ResourceEntity entity)
        {
            // Setup the root DTO.
            var dto = new ResourceDto
            {
                Id = entity.Id,
                Path = entity.Path
            };

            MapChildren(dto, entity.InverseParentResource);

            return dto;
        }

        private static void MapChildren(ResourceDto parentDto, ICollection<ResourceEntity> childEntities)
        {
            if (childEntities == null)
            {
                return;
            }

            foreach (var childEntity in childEntities)
            {
                var childDto = new ResourceDto
                {
                    Id = childEntity.Id,
                    Path = childEntity.Path,
                    Parent = parentDto
                };

                ((List<ResourceDto>)parentDto.Children).Add(childDto);

                MapChildren(childDto, childEntity.InverseParentResource);
            }
        }
    }
}
