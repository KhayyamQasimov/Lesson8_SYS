using Sys_Lesson8.DbContexts;
using Sys_Lesson8.Models;
using Sys_Lesson8.UnitOfWorks;

class Program
{
    static async Task Main(string[] args)
    {
        await using IUnitOfWork uow = new UnitOfWork(new LibraryContext());


        var allStudent = await uow.Students.GetAllStudentWithGroupNameAsync();
        foreach (var student in allStudent)
            Console.WriteLine($"{student.Name} {student.Surname} => {student.GroupName}");


        await uow.Categories.AddAsync(new Category { Name = "Test Category" });
        await uow.SaveChangesAsync();

        var category = await uow.Categories.GetByNameAsync("Test Category");
        if (category != null)
        {
            category.Name = "Updated Category";
            await uow.Categories.UpdateAsync(category);
            await uow.SaveChangesAsync();
        }


        foreach (var a in await uow.Authors.GetAuthorsWithBooksCountAsync())
            Console.WriteLine($"{a.Name} {a.Surname} => {a.BooksCount} kitab");

        foreach (var g in await uow.Groups.GetGroupsWithStudentsCountAsync())
            Console.WriteLine($"{g.Name} => {g.StudentsCount} tələbə");


        var ok = await uow.Groups.TransferStudentsAsync(fromGroupId: 1, toGroupId: 2, deleteSourceGroup: false);
        Console.WriteLine(ok ? "Transfer uğurlu oldu" : "Transfer olmadı");
    }
}