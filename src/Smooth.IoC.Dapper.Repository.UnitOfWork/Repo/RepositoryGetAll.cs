﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.FastCrud;
using Smooth.IoC.Dapper.Repository.UnitOfWork.Data;
using static Dapper.FastCrud.Sql;

namespace Smooth.IoC.Dapper.Repository.UnitOfWork.Repo
{
    public abstract partial class Repository<TEntity, TPk>
        where TEntity : class
    {
        public IEnumerable<TEntity> GetAll(ISession session)
        {
            var enumerable = session.Query<TEntity>($"SELECT * FROM {Table<TEntity>()}");
            return IsIEntity() ? 
                enumerable 
                : session.Find<TEntity>();
        }
        public IEnumerable<TEntity> GetAll(IUnitOfWork uow)
        {
            return IsIEntity() ?
                uow.Connection.Query<TEntity>($"SELECT * FROM {Table<TEntity>()}", transaction: uow.Transaction)
                : uow.Find<TEntity>();
        }

        public IEnumerable<TEntity> GetAll<TSesssion>() where TSesssion : class, ISession
        {
            using (var session = Factory.Create<TSesssion>())
            {
                return GetAll(session);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISession session)
        {
            return IsIEntity() ?
                await session.QueryAsync<TEntity>($"SELECT * FROM {Table<TEntity>()}")
                : await session.FindAsync<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IUnitOfWork uow)
        {
            return IsIEntity() ?
                await uow.Connection.QueryAsync<TEntity>($"SELECT * FROM {Table<TEntity>()}",transaction: uow.Transaction) 
                : await uow.FindAsync<TEntity>();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync<TSesssion>() where TSesssion : class, ISession
        {
            using (var session = Factory.Create<TSesssion>())
            {
                return GetAllAsync(session);
            }
        }
    }
}
