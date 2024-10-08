﻿using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Orders;

public class UpdateOrderCommand : BaseCommand
{
    public string Author { get; set; }
    public string Address { get; set; }
    public bool IsEmergency { get; set; }
}
