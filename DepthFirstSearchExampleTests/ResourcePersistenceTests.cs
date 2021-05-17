using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepthFirstSearchExample;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            var tree = new ResourceEntity
            {
                Name = "nasima",
                Path = "/nasima",
                InverseParentResource = new List<ResourceEntity>
                {
                    new ResourceEntity
                    {
                        Name = "farooq",
                        Path = "/nasima/farooq",
                        InverseParentResource = new List<ResourceEntity>
                        {
                            new ResourceEntity
                            {
                                Name = "noor",
                                Path = "/naisma/farooq/noor"
                            },
                            new ResourceEntity
                            {
                                Name = "yasin",
                                Path = "/naisma/farooq/yasin"
                            }
                        }
                    },

                    new ResourceEntity
                    {
                        Name = "sofia",
                        Path = "/nasima/sofia"
                    },
                }
            };

            await this.context.Resources.AddAsync(tree);
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

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}
