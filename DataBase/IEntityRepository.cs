using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebPlayer.DataBase
{
    public interface IEntityRepository<T>
    {
        Task Insert(T entity);
        T Find(string objectId);
        ReplaceOneResult Update(T entity);
        DeleteResult Delete(string id);
    }
}