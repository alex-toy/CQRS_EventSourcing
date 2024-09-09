using Post.Common.DTOs;

namespace Post.Query.Api.Dtos;

public class DeliveriesDto : BaseResponse
{
    public List<DeliveryDto> Deliveries { get; set; } = new List<DeliveryDto>();
}
