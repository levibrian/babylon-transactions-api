﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ivas.Transactions.Domain.Enums;

namespace Ivas.Transactions.Domain.Objects
{
    public class TransactionSummary
    {
        public long UserId => _transactions.FirstOrDefault().UserId;

        public string Ticker => _transactions.FirstOrDefault().Ticker;
        
        public decimal Shares => 
            BuyPositions
                .Sum(x => x.Units);

        public decimal TotalInvested => this.Shares * this.PricePerShare;

        public decimal RealizedDividends => CalculateRealizedDividends();

        public decimal PricePerShare => 
            BuyPositions
                .Average(x => x.PricePerUnit);

        private readonly IEnumerable<Transaction> _transactions;

        private IEnumerable<Transaction> BuyPositions =>
            _transactions
                .Where(x => 
                    x.TransactionType == TransactionTypeEnum.Buy);

        private IEnumerable<Transaction> DividendPositions =>
            _transactions
                .Where(x =>
                    x.TransactionType == TransactionTypeEnum.Dividend);

        public TransactionSummary()
        {
        }

        public TransactionSummary(IEnumerable<Transaction> transactions)
        {
            _transactions = transactions 
                            ?? throw new ArgumentNullException(nameof(transactions));
        }

        private decimal CalculateRealizedDividends()
        {
            return DividendPositions
                .Sum(dividendPosition => 
                    dividendPosition.Units * dividendPosition.PricePerUnit);
        }
    }
}