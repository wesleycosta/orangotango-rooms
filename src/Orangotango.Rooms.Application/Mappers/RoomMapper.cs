﻿using Orangotango.Rooms.Application.Abstractions;
using Orangotango.Rooms.Application.Results;
using Orangotango.Rooms.Domain.Rooms;

namespace Orangotango.Rooms.Application.Mappers;

internal sealed class RoomMapper : IRoomMapper
{
    public RoomBasicResult MapToRoomResult(Room room)
        => new()
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            CategoryId = room.CategoryId,
        };

    public RoomResult MapToRoomResultFull(Room room)
        => new()
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            CategoryId = room.CategoryId,
            CategoryName = room.Category.Name,
        };
}
