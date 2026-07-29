using AIPortfolio.Application.Abstractions;
using AIPortfolio.Application.DTOs.Portfolio;
using Ardalis.Result;
using MediatR;
using PortfolioEntity = AIPortfolio.Domain.Entites.Portfolio.Portfolio;

namespace AIPortfolio.Application.Features.Portfolio.Commands.UpsertProfile;

internal sealed class UpsertProfileCommandHandler : IRequestHandler<UpsertProfileCommand, Result<UpsertProfileResponse>>
{
    private readonly IPortfolioRepository _repository;
    private readonly IUserInfoAccessor _userInfoAccessor;

    public UpsertProfileCommandHandler(IPortfolioRepository repository, IUserInfoAccessor userInfoAccessor)
    {
        _repository = repository;
        _userInfoAccessor = userInfoAccessor;
    }

    public async Task<Result<UpsertProfileResponse>> Handle(UpsertProfileCommand command,
        CancellationToken cancellationToken)
    {
        if (!_userInfoAccessor.IsAuthenticated) return Result.Unauthorized();

        var profile = new PortfolioEntity
        {
            UserId = _userInfoAccessor.UserId,
            CustomSlug = command.CustomSlug ?? "user-" + _userInfoAccessor.UserId,
            ProfileHeadline = command.ProfileHeadline,
            ProfileSummary = command.ProfileSummary,
            ContactEmail = command.ContactEmail,
            ContactPhone = command.ContactPhone,
            Address = command.Address,
            ProfilePictureUrl = command.ProfilePictureUrl,
            BannerPictureUrl = command.BannerPictureUrl,
            ThemeName = command.ThemeName,
            ThemeConfigJson = command.ThemeConfigJson,
            SEOTitle = command.SEOTitle,
            SEODescription = command.SEODescription,
            SEOKeywords = command.SEOKeywords,
            SelectedThemeId = command.SelectedThemeId,
            IsPublic = command.IsPublic,
            FullName = command.FullName,
            SocialLinks = command.SocialLinks ?? [],
            CreatedBy = _userInfoAccessor.Email ?? "system",
            CreatedFromIp = _userInfoAccessor.GetRemoteIp() ?? "127.0.0.1",
            RowVersion = new byte[8]
        };

        await _repository.UpsertProfileAsync(profile);
        return new UpsertProfileResponse("Profile updated successfully.");
    }
}