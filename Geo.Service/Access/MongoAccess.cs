using Geo.Model;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Geo.Service
{
    public abstract class MongoAccess<E> where E : IEntity
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _dataBase;
        private readonly IMongoCollection<E> _entity;
        public IMongoDatabase CurrentMongoConnection { get { return _dataBase; } }
        public IMongoCollection<E> CurrentMongoColletion { get { return _entity; } }
        public MongoAccess(string connectionString, string databaseName, string collectionName)
        {
            this._client = new MongoClient(connectionString);
            this._dataBase = this._client.GetDatabase(databaseName);
            this._entity = this._dataBase.GetCollection<E>(collectionName);
        }
        public async Task<E> FindById(string id)
        {
            return await (await this._entity.FindAsync(x => x.Id.Equals(id))).FirstOrDefaultAsync();
        }
        public async Task<E> FindOne(Expression<Func<E, bool>> filter)
        {
            return await (await this._entity.FindAsync(filter)).FirstOrDefaultAsync();
        }
        public async Task<UpdateResult> UpdateFieldAndValue<TCampo>(E e, Expression<Func<E, TCampo>> field, TCampo value)
        {
            var filter = Builders<E>.Filter.Eq(d => d.Id, e.Id);
            return await this._entity.UpdateOneAsync(filter, Builders<E>.Update.Set(field, value));
        }
        public async Task<UpdateResult> UpdateSetMany(IList<E> es, UpdateDefinition<E> updateSet)
        {
            var filter = Builders<E>.Filter.In(d => d.Id, es.Select(a => a.Id));
            return await this._entity.UpdateManyAsync(filter, updateSet);
        }
        public async Task<IList<E>> All()
        {
            return await (await this._entity.FindAsync(_ => true, new FindOptions<E> { Limit = 200 })).ToListAsync();
        }
        public async Task<E> Add(E e)
        {
            await this._entity.InsertOneAsync(e);
            return e;
        }
        public async Task AddMany(IList<E> es)
        {
            await this._entity.InsertManyAsync(es);
        }
        public async Task<ReplaceOneResult> Upsert(E e, bool upSert)
        {
            return await this._entity.ReplaceOneAsync(x => x.Id.Equals(e.Id), e, new ReplaceOptions() { IsUpsert = upSert });
        }
        public async Task<BulkWriteResult> UpdateMany(IList<E> es)
        {
            List<WriteModel<E>> commands = [];

            foreach (E e in es)
            {
                var filter = Builders<E>.Filter.Eq(d => d.Id, e.Id);
                var command = new ReplaceOneModel<E>(filter, e);

                commands.Add(command);
            }

            return await this._entity.BulkWriteAsync(commands);
        }
        public async Task<UpdateResult> UpdateSetFieldAndValue<TCampo>(Expression<Func<E, bool>> filter, Expression<Func<E, TCampo>> field, TCampo value)
        {
            return await this._entity.UpdateOneAsync(filter, Builders<E>.Update.Set(field, value));
        }
        public async Task<E> FindOneAndUpdate(Expression<Func<E, bool>> filter, Expression<Func<E, long>> field, long add)
        {
            return await this._entity.FindOneAndUpdateAsync(filter, Builders<E>.Update.Inc(field, add), new FindOneAndUpdateOptions<E> { IsUpsert = true, ReturnDocument = ReturnDocument.After });
        }
        public async Task<DeleteResult> Remove(E e)
        {
            return await this._entity.DeleteOneAsync(x => x.Id.Equals(e.Id));
        }
        public async Task<BulkWriteResult> RemoveMany(IList<E> es)
        {
            List<WriteModel<E>> commands = [];
            foreach (E e in es)
            {
                var filter = Builders<E>.Filter.Eq(d => d.Id, e.Id);
                var command = new DeleteOneModel<E>(filter);

                commands.Add(command);
            }

            return await this._entity.BulkWriteAsync(commands);
        }
        public async Task<ReplaceOneResult> Update(E e)
        {
            return await this._entity.ReplaceOneAsync(x => x.Id.Equals(e.Id), e);
        }
        public async Task<UpdateResult> UpdateSet(E e, UpdateDefinition<E> updateSet)
        {
            var filter = Builders<E>.Filter.Eq(d => d.Id, e.Id);
            return await this._entity.UpdateOneAsync(filter, updateSet);
        }

        public async Task<IList<E>> Find(Expression<Func<E, bool>> filter)
        {
            return await (await this._entity.FindAsync(filter)).ToListAsync();
        }
        public IList<E> FindWithLimit(Expression<Func<E, bool>> filter, int limit)
        {
            return this._entity.Find(filter).Skip(0).Limit(limit).ToList();
        }
        public IList<E> FindWithSortBy(Expression<Func<E, bool>> filter, Expression<Func<E, object>> sortBy)
        {
            return this._entity.Find(filter).SortBy(sortBy).Skip(0).ToList();
        }
        public IList<E> FindWithSkipLimit(Expression<Func<E, bool>> filter, int skip, int limit)
        {
            return this._entity.Find(filter).Skip(skip).Limit(limit).ToList();
        }
        public IList<E> FindWithProjectionSkipLimit(Expression<Func<E, bool>> filter, List<Expression<Func<E, object>>> projectionFields, int skip, int limit)
        {
            var projection = Builders<E>.Projection.Include(projectionFields.First());

            foreach (var projectionField in projectionFields.Skip(1))
            {
                projection = projection.Include(projectionField);
            }

            return this._entity.Find(filter).Project<E>(projection).Skip(skip).Limit(limit).ToList();
        }
        public IList<E> FindWithProjection(Expression<Func<E, bool>> filter, List<Expression<Func<E, object>>> projectionFields)
        {
            var projection = Builders<E>.Projection.Include(projectionFields.First());

            foreach (var projectionField in projectionFields.Skip(1))
            {
                projection = projection.Include(projectionField);
            }

            return this._entity.Find(filter).Project<E>(projection).ToList();
        }
        public IList<E> FindWithProjectionLimit(Expression<Func<E, bool>> filter, List<Expression<Func<E, object>>> projectionFields, int limit)
        {
            var projection = Builders<E>.Projection.Include(projectionFields.First());

            foreach (var projectionField in projectionFields.Skip(1))
            {
                projection = projection.Include(projectionField);
            }

            return this._entity.Find(filter).Project<E>(projection).Limit(limit).ToList();
        }
    }
}
