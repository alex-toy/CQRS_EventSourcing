using Post.Common.DTOs;
using Post.Query.Api.Dtos;
using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Api.DTOs;

public class OrdersDto : BaseResponse
{
    public List<OrderDto> Orders { get; set; }
}