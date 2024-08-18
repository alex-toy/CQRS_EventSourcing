using Post.Common.DTOs;
using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Api.DTOs;

public class OrderDto : BaseResponse
{
    public List<OrderDb> Orders { get; set; }
}