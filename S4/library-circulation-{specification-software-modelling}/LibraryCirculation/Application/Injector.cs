using System;
using System.Collections.Generic;
using LibraryCirculation.Core;
using LibraryCirculation.Core.Finance;
using LibraryCirculation.Core.Fund;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.Core.Fund.Organization;
using LibraryCirculation.Core.Renting;
using LibraryCirculation.Core.Users;
using LibraryCirculation.Core.Users.Librarians;
using LibraryCirculation.Core.Users.Readers;
using LibraryCirculation.DataManagement.KeyGenerators;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Application
{
    public static class Injector
    {
        private static readonly Dictionary<Type, object> Services = new()
        {
            {
                typeof(InventoryBookService),
                new InventoryBookService(GetRepository<InventoryBook>(Paths.InventoryBooks))
            },
            {
                typeof(ReservationService), new ReservationService(GetRepository<Reservation>(Paths.Reservations))
            },
            {
                typeof(MembershipService), new MembershipService(GetRepository<Membership>(Paths.Memberships, false))
            },
            {
                typeof(LibrarianService), new LibrarianService(GetRepository<Librarian>(Paths.Librarians, false))
            },
            {
                typeof(PublisherService), new PublisherService(GetRepository<Publisher>(Paths.Publishers))
            },
            {
                typeof(PaymentService), new PaymentService(GetRepository<Payment>(Paths.Payments))
            },
            {
                typeof(BranchService), new BranchService(GetRepository<Branch>(Paths.Branches))
            },
            {
                typeof(AuthorService), new AuthorService(GetRepository<Author>(Paths.Authors))
            },
            {
                typeof(RentalService), new RentalService(GetRepository<Rental>(Paths.Rentals))
            },
            {
                typeof(ReaderService), new ReaderService(GetRepository<Reader>(Paths.Readers, false))
            },
            {
                typeof(LoginService), new LoginService
                (
                    GetRepository<Reader>(Paths.Readers, false),
                    GetRepository<Librarian>(Paths.Librarians, false)
                )
            },
            {
                typeof(CopyService), new CopyService(GetRepository<Copy>(Paths.Copies))
            },
            {
                typeof(BookService), new BookService(GetRepository<Book>(Paths.Books, false))
            }
        };

        static Injector()
        {
            Services[typeof(SystemUpdaterService)] = new SystemUpdaterService();
            Services[typeof(BookReportService)] = new BookReportService();
        }

        public static T GetService<T>()
        {
            var type = typeof(T);
            if (Services.TryGetValue(type, out var service))
                return (T)service;

            throw new ArgumentException($"No implementation found for type '{type}'");
        }

        public static FileRepository<T> GetRepository<T>(string path, bool numeric = true)
            where T : IKey, ISerializable, new()
        {
            IKeyGenerator keyGen = numeric ? new NumericKeyGenerator() : new DefaultKeyGenerator();
            return new FileRepository<T>(path, keyGen);
        }
    }
}