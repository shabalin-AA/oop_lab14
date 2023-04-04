using System;
using lab10;
using lab12;
using System.Linq;



namespace lab14
{
    class Program
    {
        static Random rnd = new Random();


        // наименование всех книг вышедших не ранее указанного года
        static void SelectWithLinq(Queue<List<Printing>> library, int year)
        {
            var olderThanYear = from department in library select (
                from printing in department
                where printing is Book && printing.Year >= year
                select printing
            );

            foreach (var department in olderThanYear)
                foreach (var printing in department)
                    Console.WriteLine(printing);
        }

        static void SelectWithExtensionMethod(Queue<List<Printing>> library, int year)
        {
            var olderThanYear = library.Select(
                department => department.Where(p => p is Book && p.Year >= year)
            );

            foreach (var department in olderThanYear)
                foreach (var printing in department)
                    Console.WriteLine(printing);
        }


        // количество книг во всех отделах библиотеки
        static void CountWithLinq(Queue<List<Printing>> library)
        {
            var qBooks = (
                from department in library
                select (
                    from p in department
                    where p is Book
                    select p
                ).Count()
            ).Sum();
            Console.WriteLine(qBooks);
        }

        static void CountWithExtensionMethod(Queue<List<Printing>> library)
        {
            var qBooks = library.Sum(
                l => l.Count(p => p is Book)
            );
            Console.WriteLine(qBooks);
        }


        // наименование всех печатных изданий из двух самых маленьких отделов библиотеки 
        static void SetWithLinq(Queue<List<Printing>> library)
        {
            var sizeOrdereddepartments = from department in library orderby department.Count() select department;

            var unionBiggestdepartments = Enumerable.ElementAt<List<Printing>>(sizeOrdereddepartments, 0)
                .Union(Enumerable.ElementAt<List<Printing>>(sizeOrdereddepartments, 1));

            foreach (var p in unionBiggestdepartments)
                Console.WriteLine(p);
        }

        static void SetWithExtensionMethod(Queue<List<Printing>> library)
        {
            var sizeOrdereddepartments = library.OrderBy(l => l.Count());

            var unionBiggestdepartments = Enumerable.ElementAt<List<Printing>>(sizeOrdereddepartments, 0)
                .Union(Enumerable.ElementAt<List<Printing>>(sizeOrdereddepartments, 1));

            foreach (var p in unionBiggestdepartments)
                Console.WriteLine(p);
        }


        // суммарное количество учебников в библиотеке
        static void AggregateWithLinq(Queue<List<Printing>> library)
        {
            IEnumerable<int> countSchoolBooksIndepartments =
                from department in library select department.Count(el => el is SchoolBook);

            int qSchoolBooks = countSchoolBooksIndepartments.Sum();
            Console.WriteLine(qSchoolBooks);
        }

        static void AggregateWithExtensionMethod(Queue<List<Printing>> library)
        {
            var countSchoolBooksIndepartments = library.Select(
                l => l.Count(el => el is SchoolBook)
            );

            int qSchoolBooks = countSchoolBooksIndepartments.Aggregate((x, y) => x + y);
            Console.WriteLine(qSchoolBooks);
        }


        // наименование всех книг, чьи названия начинаются с указанной буквы
        static void GroupWithLinq(Queue<List<Printing>> library, char firstLetter)
        {
            var beginsWithLetter = from department in library select (
                from printing in (
                    from p in department
                    where p is Book
                    group p by p.Name[0]
                )
                where printing.Key == firstLetter
                select printing
            );

            foreach (var department in beginsWithLetter)
            {
                var departmentAsEnum = department.SelectMany(department => department);
                foreach (var printing in departmentAsEnum)
                    Console.WriteLine(printing);
            }
        }

        static void GroupWithExtensionMethod(Queue<List<Printing>> library, char firstLetter)
        {
            var beginsWithLetter = library.Select(
                department => department.Where(x => x.Name[0] == firstLetter)
            );

            foreach (var department in beginsWithLetter)
            {
                foreach (var printing in department)
                    Console.WriteLine(printing);
            }
        }


        static List<Printing> genList(int n)
        {
            var l = new List<Printing>();

            int qMagazines = rnd.Next(n);
            int qBooks = rnd.Next(n - qMagazines);
            int qSchoolBooks = (n - qMagazines - qBooks);

            for (int i = 0; i < qMagazines; i++)
                l.Add(new Magazine());

            for (int i = 0; i < qBooks; i++)
                l.Add(new Book());

            for (int i = 0; i < qSchoolBooks; i++)
                l.Add(new SchoolBook());

            return l;
        }


        static void Main()
        {
            var library = new Queue<List<Printing>>();
            for (int i = 0; i < 10; i++)
            {
                int n = rnd.Next(50);
                library.Enqueue(genList(n));
            }
            Console.Write("\n");

            Console.WriteLine("\nSelect: ");
            SelectWithLinq(library, 2005);
            SelectWithExtensionMethod(library, 2005);

            Console.WriteLine("\nCount: ");
            CountWithLinq(library);
            CountWithExtensionMethod(library);

            Console.WriteLine("\nSet: ");
            SetWithLinq(library);
            SetWithExtensionMethod(library);

            Console.WriteLine("\nAggregate: ");
            AggregateWithLinq(library);
            AggregateWithExtensionMethod(library);

            Console.WriteLine("\nGrouping: ");
            GroupWithLinq(library, 'A');
            GroupWithExtensionMethod(library, 'A');


            var t = new PrintingTree<string, Printing>();
            for (int i = 0; i < 5; i ++)
            {
                t.Add(i.ToString(), new Book());
            }

            Console.WriteLine("\nSelection with predicate");
            var filteredTree = t.Where(tn => tn.Key != "3");
            foreach (var tn in filteredTree)
            {
                Console.WriteLine(tn);
            }

            Console.WriteLine("\nAverage");
            Console.WriteLine(t.Average(tn => tn.Value.Year));

            Console.WriteLine("\nSorting");
            var sortedTree = t.Sort((x, y) => x.Value.Year.CompareTo(y.Value.Year));
            foreach(var tn in sortedTree)
            {
                Console.WriteLine(tn);
            }
        }
    }
}