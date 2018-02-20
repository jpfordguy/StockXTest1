
namespace StockXTest1
{
    public class ShoeService : IShoeService
    {
        public bool AddShoeSize(string name, int size)
        {
            if(DatabaseHelper.DatabaseInterface == null)
            {
                return false;
            }
            if(name == null)
            {
                return false;
            }
            if(name.Trim() == "")
            {
                return false;
            }
            if(size < 1 || size > 5)
            {
                return false;
            }
            if(!DatabaseHelper.DatabaseInterface.Update(name,size))
            {
                return false;
            }
            return true;
        }

        public double GetAverageSize(string name)
        {
            if (DatabaseHelper.DatabaseInterface == null)
            {
                return -1;
            }
            if (name == null)
            {
                return -1;
            }
            if (name.Trim() == "")
            {
                return -1;
            }
            return DatabaseHelper.DatabaseInterface.GetSize(name);
        }
    }
}
