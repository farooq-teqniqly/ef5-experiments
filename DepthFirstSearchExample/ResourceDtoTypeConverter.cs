using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DepthFirstSearchExample
{
    public class ResourceDtoTypeConverter : ITypeConverter<ResourceEntity, ResourceDto>
    {
        public ResourceDto Convert(ResourceEntity source, ResourceDto destination, ResolutionContext context)
        {
            var rootDto = new ResourceDto
            {
                Id = source.Id,
                Path = source.Path,
                Name = source.Name
            };

            MapChildren(rootDto, source.InverseParentResource);

            return rootDto;
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
                    Parent = parentDto,
                    Name = childEntity.Name
                };

                ((List<ResourceDto>)parentDto.Children).Add(childDto);

                MapChildren(childDto, childEntity.InverseParentResource);
            }
        }
    }
}
