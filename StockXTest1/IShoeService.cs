using System.ServiceModel;
using System.ServiceModel.Web;

namespace StockXTest1
{
    [ServiceContract]
    public interface IShoeService
    {
        [OperationContract]
        [WebGet]
        bool AddShoeSize(string name, int size);

        [OperationContract]
        [WebGet]
        double GetAverageSize(string name);
    }
}
