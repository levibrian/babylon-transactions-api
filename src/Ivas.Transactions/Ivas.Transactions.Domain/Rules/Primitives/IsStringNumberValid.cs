﻿using Ivas.Transactions.Domain.Enums;
using Ivas.Transactions.Shared.Notifications;
using Ivas.Transactions.Shared.Specifications.Interfaces;

namespace Ivas.Transactions.Domain.Rules.Primitives
{
    public class IsStringNumberValid : ISpecification<string>
    {
        public bool IsPrimitiveSatisfiedBy(string stringToEvaluate)
        {
            var expression = 
                !string.IsNullOrWhiteSpace(stringToEvaluate) && 
                int.TryParse(stringToEvaluate, out var convertedInt) &&
                convertedInt > 0;

            return expression;
        }
    }
}