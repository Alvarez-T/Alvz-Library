namespace Alvz.Data.Extensions;

public interface IConnectionStringProvider
{
    public string DatabaseName { get; set; }
    public string ConnectionString { get; }
    public string DatabasePath { get; }
}
