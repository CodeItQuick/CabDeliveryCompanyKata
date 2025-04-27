using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Domain.CustomerList;

namespace Tests.CabDeliveryCompanyKata.Adapter.Console;

public class FakeRepositoryTestCustomerListRepository
{
    [Fact]
    public void Repository_CanCreateAnItem()
    {
        var customerListRepository = new FakeDatabaseRepository<Patron>();

        customerListRepository.Save(new Patron());

        var customerListDto = customerListRepository.Read();

        Assert.Single(customerListDto);
        Assert.Equal(1, customerListDto.First().Id);
    }
     [Fact]
     public void Repository_CanCreateAnItemWithDetails()
     {
         var customerListRepository = new FakeDatabaseRepository<Patron>();
         var firstCustomer = new Patron()
         {
             Id = 7,
             Name = "Hello World",
             Status = PatronStatus.Delivered,
             EndLocation = "2 Destination Lane",
             MenuId = 1,
             StartLocation = "1 Starting Drive"
         };
         customerListRepository.Save(firstCustomer);

         var customerListDto = customerListRepository.Read();

         Assert.Single(customerListDto);
         Assert.Equal(1, customerListDto.First().Id);
     }
     
     [Fact]
     public void Repository_CanAddTwoItems()
     {
         var customerListRepository = new FakeDatabaseRepository<Patron>();
         customerListRepository.Save(new Patron());
         customerListRepository.Save(new Patron());

         var customerListDto = customerListRepository.Read();

         Assert.Equal(2, customerListDto.Count);
     }

     [Fact]
     public void Repository_CanUpdateAnItem()
     {
         var customerListRepository = new FakeDatabaseRepository<Patron>();
         customerListRepository.Save(new Patron());
         customerListRepository.Save(new Patron());

         customerListRepository.Update(2, 
                 new() { Id = 1, Name = "hello world"});

         var customerListDto = customerListRepository.Read();

         Assert.Equal(2, customerListDto.Count);
         Assert.Equal(2, customerListDto.Find(x => x.Id == 2).Id);
         Assert.Equal("hello world", customerListDto.Find(x => x.Id == 2).Name);
     }
//
     [Fact]
     public void Repository_CanDeleteAnItem()
     {
         var customerListRepository = new FakeDatabaseRepository<Patron>();
         customerListRepository.Save(new Patron());
         customerListRepository.Save(new Patron());
         customerListRepository.Save(new Patron());
         customerListRepository.Delete(2);

         var customerListDto = customerListRepository.Read();

         Assert.Equal(2, customerListDto.Count);
         Assert.Equal(1, customerListDto.First().Id);
         Assert.Equal(3, customerListDto.Skip(1).First().Id);
     }
     [Fact]
     public void Repository_CanSearchUsingFilters()
     {
         var customerListRepository = new FakeDatabaseRepository<Patron>();
         customerListRepository.Save(new Patron());
         customerListRepository.Save(new Patron());
         customerListRepository.Save(new Patron());
         customerListRepository.Delete(2);

         var customerListDto = customerListRepository
             .Search(x => x.Id == 3);

         Assert.Equal(1, customerListDto.Count);
         Assert.Equal(3, customerListDto.First().Id);
     }
}