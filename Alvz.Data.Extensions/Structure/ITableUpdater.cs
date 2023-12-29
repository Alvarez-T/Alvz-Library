namespace Alvz.Data.Extensions.Structure;

public interface ITableUpdater
{
    void UpdateTable();
    bool IsAlreadyUpdated();
    void UpdateData();

}
