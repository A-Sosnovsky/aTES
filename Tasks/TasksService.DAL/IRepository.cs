﻿using System.Linq;
using System.Threading;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.DAL;

public interface IRepository
{
    IQueryable<T> Query<T>() where T : class, IDbEntity;
    Task InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IDbEntity;
    void Update<T>(T entity, CancellationToken cancellationToken = default) where T : class, IDbEntity;
}