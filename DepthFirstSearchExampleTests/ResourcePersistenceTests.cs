using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DepthFirstSearchExample;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DepthFirstSearchExampleTests
{
    public class ResourcePersistenceTests : IDisposable
    {
        private readonly AppDbContext context;

        public ResourcePersistenceTests()
        {
            this.context = AppDbContextFactory.Create(typeof(ResourcePersistenceTests));
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Can_Create_Resource_Tree()
        {
            await this.context.Resources.AddAsync(CreateResourceTree());
            await this.context.SaveChangesAsync();

            var nasima = await this.context.Resources
                .Include(r => r.InverseParentResource)
                .SingleAsync(r => r.Name == "nasima");

            nasima.Name.Should().Be("nasima");

            var farooq = nasima.InverseParentResource.SingleOrDefault(r => r.Name == "farooq");
            farooq.Should().NotBeNull();

            var sofia = nasima.InverseParentResource.SingleOrDefault(r => r.Name == "sofia");
            sofia.Should().NotBeNull();

            var noor = farooq.InverseParentResource.SingleOrDefault(r => r.Name == "noor");
            noor.Should().NotBeNull();

            var yasin = farooq.InverseParentResource.SingleOrDefault(r => r.Name == "yasin");
            yasin.Should().NotBeNull();

        }

        [Fact]
        public async Task Can_Map_Entity_To_Dto()
        {
            await this.context.Resources.AddAsync(CreateResourceTree());
            await this.context.SaveChangesAsync();

            var rootEntity = await this.context.Resources
                .Include(r => r.InverseParentResource)
                .SingleAsync(r => r.Name == "nasima");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ResourceEntity, ResourceDto>().ConvertUsing<ResourceDtoTypeConverter>();
            });

             config.AssertConfigurationIsValid();

             var mapper = new Mapper(config);
             var rootDto = mapper.Map<ResourceEntity, ResourceDto>(rootEntity);

             rootDto.Name.Should().Be("nasima");

             var farooq = rootDto.Children.Single(r => r.Name == "farooq");
             farooq.Parent.Id.Should().Be(rootDto.Id);

             var noor = farooq.Children.Single(r => r.Name == "noor");
             noor.Parent.Id.Should().Be(farooq.Id);

             var yasin = farooq.Children.Single(r => r.Name == "yasin");
             yasin.Parent.Id.Should().Be(farooq.Id);

            var sofia = rootDto.Children.Single(r => r.Name == "sofia");
             sofia.Parent.Id.Should().Be(rootDto.Id);

        }

        public void Dispose()
        {
            context?.Dispose();
        }

        private static ResourceEntity CreateResourceTree()
        {
            return new()
            {
                Name = "nasima",
                Path = "/nasima",
                InverseParentResource = new List<ResourceEntity>
                {
                    new()
                    {
                        Name = "farooq",
                        Path = "/nasima/farooq",
                        InverseParentResource = new List<ResourceEntity>
                        {
                            new()
                            {
                                Name = "noor",
                                Path = "/naisma/farooq/noor"
                            },
                            new()
                            {
                                Name = "yasin",
                                Path = "/naisma/farooq/yasin"
                            }
                        }
                    },

                    new()
                    {
                        Name = "sofia",
                        Path = "/nasima/sofia"
                    },
                }
            };
        }
    }
}
