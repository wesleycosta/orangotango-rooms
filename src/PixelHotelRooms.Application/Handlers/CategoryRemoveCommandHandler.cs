﻿using FluentValidation;
using PixelHotel.Core.Abstractions;
using PixelHotel.Core.Services;
using PixelHotel.Events.Rooms.Category;
using PixelHotelRooms.Application.Abstractions;
using PixelHotelRooms.Domain.CategoryAggregate;
using PixelHotelRooms.Domain.CategoryAggregate.Commands;

namespace PixelHotelRooms.Application.Handlers;

internal sealed class CategoryRemoveCommandHandler : CommandHandlerBase<CategoryRemoveCommand>
{
    private readonly ICategoryMapper _mapper;
    private readonly ICategoryRepository _repository;
    private readonly ICategoryPublisher _publisher;

    public CategoryRemoveCommandHandler(IUnitOfWork unitOfWork,
        IValidator<CategoryRemoveCommand> validator,
        ICategoryMapper mapper,
        ICategoryRepository repository,
        ICategoryPublisher publisher) : base(unitOfWork, validator)
    {
        _mapper = mapper;
        _repository = repository;
        _publisher = publisher;
    }

    public override async Task<Result> Handle(CategoryRemoveCommand command, CancellationToken cancellationToken)
    {
        if (!await Validate(command))
        {
            return BadResult();
        }

        var category = await _repository.GetById(command.Id);
        _repository.SoftDelete(category);

        if (await Commit())
        {
            await _publisher.PublishRemovedEvent(new CategoryRemovedEvent(category.Id));
            return SuccessfulResult(_mapper.MapToCategoryResult(category));
        }

        return BadResult();
    }
}