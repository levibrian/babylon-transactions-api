﻿using AutoMapper;
using Ivas.Transactions.Domain.Dtos;
using Ivas.Transactions.Domain.Objects;
using Ivas.Transactions.Domain.Responses;

namespace Ivas.Transactions.Domain.Mappers
{
    public class PositionSummaryProfile : Profile
    {
        public PositionSummaryProfile()
        {
            CreateMap<PositionSummary, PositionSummaryGetResponse>();
        }
    }
}