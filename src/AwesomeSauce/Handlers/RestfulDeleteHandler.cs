using AwesomeSauce.Configuration.Storage;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace AwesomeSauce.Handlers
{
    public class RestfulDeleteHandler<TEntity> where TEntity : class
    {
        private readonly MongoSession _session;

        public RestfulDeleteHandler(MongoSession session)
        {
            _session = session;
        }

        public RestfulDeleteModel<TEntity> Execute(RestfulDeleteRequest<TEntity> request)
        {
            var collection = _session.Session.GetCollection<TEntity>(typeof (TEntity).Name.ToLowerInvariant());
            BsonValue id = new BsonObjectId(request.Id);
            var query = Query.EQ("_id", id);
            TEntity entity = collection.FindOne(query);
            return new RestfulDeleteModel<TEntity>() { };
        }
    }

    public class RestfulDeleteModel<T>
    {
    }

    public class RestfulDeleteRequest<T> : IRequestById
    {
        public string Id { get; set; }
    }
}