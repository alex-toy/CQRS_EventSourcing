using Post.Common.DTOs;
using Post.Query.Api.Dtos;

namespace Post.Query.Api.DTOs;

public class OrdersDto : BaseResponse
{
    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
}