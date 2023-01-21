﻿using System.Collections.Generic;
using Babylon.Investments.Domain.Abstractions.Requests;
using Babylon.Investments.Domain.Objects.Base;
using Babylon.Investments.Domain.Rules;
using Babylon.Investments.Shared.Extensions;
using Babylon.Investments.Shared.Notifications;

namespace Babylon.Investments.Domain.Validators
{
    public interface IOperationValidator
    {
        Result Validate(TransactionPostRequest request, ICollection<Transaction> transactionHistory);
    }
    
    public class SellOperationValidator : IOperationValidator
    {
        public Result Validate(TransactionPostRequest request, ICollection<Transaction> transactionHistory)
        {
            var transactionHistoryRules =
                new IsTransactionHistoryAny()
                    .And(new AreUnitsGreaterThanUnitsInHistory(request.Units));

            return transactionHistoryRules.IsSatisfiedBy(transactionHistory);
        }
    }
}