using AutoMapper;
using BSAccountDetailsServiceReference;
using Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using FundsTransferRequestHeaderType = BSAccountFundsTransferServiceReference.RequestHeaderType;
using FundsTransferResponseHeaderType = BSAccountFundsTransferServiceReference.ResponseHeaderType;
using FundsTransferCredentialsType = BSAccountFundsTransferServiceReference.CredentialsType;
using FundsTransferStatusMessagesType = BSAccountFundsTransferServiceReference.StatusMessagesType;
using BSAccountFundsTransferServiceReference;
using RequestHeaderType = BSAccountDetailsServiceReference.RequestHeaderType;
using CredentialsType = BSAccountDetailsServiceReference.CredentialsType;
using ResponseHeaderType = BSAccountDetailsServiceReference.ResponseHeaderType;
using StatusMessagesType = BSAccountDetailsServiceReference.StatusMessagesType;

namespace Cashmere.Finacle.Integration.CQRS.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountDetailsRequestType, AccountDetailsRequestTypeDto>().ReverseMap();
            CreateMap<AccountDetailsResponseType, AccountDetailsResponseTypeDto>().ReverseMap();
            CreateMap<CredentialsType, CredentialsTypeDto>().ReverseMap();
            CreateMap<RequestHeaderType, RequestHeaderTypeDto>().ReverseMap();
            CreateMap<ResponseHeaderType, ResponseHeaderTypeDto>().ReverseMap();
            CreateMap<StatusMessagesType, StatusMessagesTypeDto>().ReverseMap();
            CreateMap<operationOutput, ValidateAccountResponseDto>().ReverseMap();

            CreateMap<PostRequest, FundsTransferResponseDto>().ReverseMap();
            CreateMap<FundsTransferType, FundsTransferTypeDto>().ReverseMap();
            CreateMap<FundsTransferCredentialsType, CredentialsTypeDto>().ReverseMap();
            CreateMap<FundsTransferRequestHeaderType, RequestHeaderTypeDto>().ReverseMap();
            CreateMap<FundsTransferResponseHeaderType, ResponseHeaderTypeDto>().ReverseMap();
            CreateMap<FundsTransferStatusMessagesType, StatusMessagesTypeDto>().ReverseMap();
            CreateMap<PostResponse, FundsTransferResponseDto>().ReverseMap();
         
        }
    }
}
