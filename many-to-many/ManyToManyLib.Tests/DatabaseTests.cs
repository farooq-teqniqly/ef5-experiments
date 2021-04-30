using Xunit;

namespace ManyToManyLib.Tests
{
    public class DatabaseTests : IClassFixture<DatabaseTestFixture>
    {
        private readonly DatabaseTestFixture fixture;

        public DatabaseTests(DatabaseTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Insert_People_And_Addresses()
        {
            using (var context = this.fixture.CreateContext())
            {
                using (context.Database.BeginTransaction())
                {
                    var person = new Person
                    {
                        FirstName = "Jeremy",
                        LastName = "Likness"
                    };

                    person.Addresses.Add(new Address
                    {
                        Street = "123 Main St."
                    });

                    person.Addresses.Add(new Address
                    {
                        Street = "999 First Ave"
                    });

                    context.People.Add(person);
                    context.SaveChanges();

                }
            }
        }
    }
}
