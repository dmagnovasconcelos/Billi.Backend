﻿using Billi.Backend.CrossCutting.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Billi.Backend.CrossCutting.UnitsOfWork
{
    public abstract class UnitOfWork(BaseDbContext context) : IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public async Task Start()
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await context.SaveChangesAsync();
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                context.Dispose();
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}