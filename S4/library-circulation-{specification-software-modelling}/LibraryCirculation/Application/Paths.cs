namespace LibraryCirculation.Application
{
    public abstract class Paths
    {
        private const string ResourceRoot = "../../../Data/";

        public const string Memberships = ResourceRoot + "memberships.csv";
        public const string Librarians = ResourceRoot + "librarians.csv";
        public const string Readers = ResourceRoot + "readers.csv";

        public const string InventoryBooks = ResourceRoot + "inventory_books.csv";
        public const string Publishers = ResourceRoot + "publishers.csv";
        public const string Branches = ResourceRoot + "branches.csv";
        public const string Authors = ResourceRoot + "authors.csv";
        public const string Copies = ResourceRoot + "copies.csv";
        public const string Books = ResourceRoot + "books.csv";

        public const string Reservations = ResourceRoot + "reservations.csv";
        public const string Payments = ResourceRoot + "payments.csv";
        public const string Rentals = ResourceRoot + "rentals.csv";
    }
}