﻿using System;
using Ivas.Transactions.Domain.Dtos;
using Ivas.Transactions.Domain.Enums;

namespace Ivas.Transactions.Domain.Objects
{
    public class TransactionCreate : Transaction
    {
        public override string TransactionId => string.IsNullOrEmpty(_transactionRequest.TransactionId)
            ? Guid.NewGuid().ToString()
            : _transactionRequest.TransactionId;

        public override long UserId => _transactionRequest.UserId;

        public override string Ticker => _transactionRequest.Ticker.ToUpperInvariant();

        public override DateTime Date => _transactionRequest.Date == new DateTime()
            ? DateTime.Now
            : _transactionRequest.Date;

        public override decimal Units => _transactionRequest.Units;

        public override decimal PricePerUnit => _transactionRequest.PricePerUnit;

        public override TransactionTypeEnum TransactionType => _transactionRequest.TransactionType;
        
        private readonly TransactionCreateDto _transactionRequest;
        
        public TransactionCreate(TransactionCreateDto transactionCreateDto)
        {
            _transactionRequest = transactionCreateDto ?? throw new ArgumentNullException(nameof(transactionCreateDto));
        }
    }
}