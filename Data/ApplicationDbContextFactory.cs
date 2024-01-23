using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using MyfirstBlackMetalAlbum.com.Models;

// you need to keep this class in the same folder as your dbContext
// the main function of this class is to help you generate controllers with db scaffolding using entity framework
//without this class you will not be able to generate those controllers and you will keep getting errors.

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<MyFirstBlackMetalAlbumContext>
{
    public MyFirstBlackMetalAlbumContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyFirstBlackMetalAlbumContext>();
        optionsBuilder.UseSqlServer("Server=HP-PC\\SQLEXPRESS;Database=MyFirstBlackMetalAlbum;Integrated Security=True;TrustServerCertificate=true");

        return new MyFirstBlackMetalAlbumContext(optionsBuilder.Options);
    }
}