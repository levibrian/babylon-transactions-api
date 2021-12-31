﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ivas.Transactions.Domain.Abstractions.Services;
using Ivas.Transactions.Domain.Contracts.Repositories;
using Ivas.Transactions.Domain.Dtos;
using Ivas.Transactions.Domain.Objects;
using Ivas.Transactions.Domain.Validators;
using Ivas.Transactions.Shared.Exceptions.Custom;
using Ivas.Transactions.Shared.Notifications;
using Microsoft.Extensions.Logging;

namespace Ivas.Transactions.Domain.Services
{
    public interface ITransactionsInBulkService : 
        ICreatableAsyncService<IEnumerable<TransactionPostDto>>,
        IDeletableAsyncService<IEnumerable<TransactionDeleteDto>>
    {
        
    }
    
    public class TransactionsInBulkService : ITransactionsInBulkService
    {
        private readonly ITransactionRepository _transactionRepository;

        private readonly ITransactionValidator _transactionValidator;

        private readonly ILogger<TransactionsInBulkService> _logger;
        
        public TransactionsInBulkService(
            ITransactionRepository transactionRepository,
            ITransactionValidator transactionValidator,
            ILogger<TransactionsInBulkService> logger)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _transactionValidator = transactionValidator ?? throw new ArgumentNullException(nameof(transactionValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Result> CreateAsync(IEnumerable<TransactionPostDto> entity)
        {
            var validationResults = _transactionValidator.Validate(entity);
            
            if (validationResults.Any(x => x.IsFailure)) 
                throw new IvasException(
                    string.Join(
                        ", ", 
                        validationResults
                            .Where(x => x.Errors.Any())
                            .Select(x => x.Errors.Select(x => x.Message))));

            var domainEntitiesToInsert = entity
                .Select(transaction => new TransactionCreate(transaction))
                .ToList();

            await _transactionRepository.InsertInBulk(domainEntitiesToInsert);

            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(IEnumerable<TransactionDeleteDto> transactionsToDelete)
        {
            var validationResults =
                _transactionValidator
                    .ValidateDelete(transactionsToDelete)
                    .ToList();
            
            if (validationResults.Any(x => x.IsFailure)) 
                throw new IvasException(
                    string.Join(
                        ", ", 
                        validationResults
                            .Where(x => x.Errors.Any())
                            .SelectMany(x => x.Errors.Select(x => x.Message))));

            var entitiesToDelete = new List<Transaction>();
            
            foreach (var transaction in transactionsToDelete)
            {
                var fetchedTransactionFromDb =
                    await _transactionRepository.GetByIdAsync(transaction.ClientIdentifier, transaction.TransactionId);
                
                if (fetchedTransactionFromDb == null) 
                    _logger.LogWarning($"TransactionsInBulkService - Delete In Bulk...Transaction with ClientIdentifier: {transaction.ClientIdentifier} and with TransactionId: {transaction.TransactionId} not found..");
                else 
                    entitiesToDelete.Add(fetchedTransactionFromDb);
            }

            await _transactionRepository.DeleteInBulk(entitiesToDelete);
            
            return Result.Ok();
        }
    }
}