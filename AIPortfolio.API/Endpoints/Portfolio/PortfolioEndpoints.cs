using AIPortfolio.API.Extensions;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteAchievement;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteCertification;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteCustomSection;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteEducation;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteExperience;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteLanguage;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteProject;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteSkill;
using AIPortfolio.Application.Features.Portfolio.Commands.DeleteSocialLink;
using AIPortfolio.Application.Features.Portfolio.Commands.SendContactMessage;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertAchievement;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertCertification;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertCustomSection;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertEducation;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertExperience;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertLanguage;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertProfile;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertProject;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertSkill;
using AIPortfolio.Application.Features.Portfolio.Commands.UpsertSocialLink;
using AIPortfolio.Application.Features.Portfolio.Queries.GetAchievements;
using AIPortfolio.Application.Features.Portfolio.Queries.GetCertifications;
using AIPortfolio.Application.Features.Portfolio.Queries.GetCustomSections;
using AIPortfolio.Application.Features.Portfolio.Queries.GetEducations;
using AIPortfolio.Application.Features.Portfolio.Queries.GetExperiences;
using AIPortfolio.Application.Features.Portfolio.Queries.GetLanguages;
using AIPortfolio.Application.Features.Portfolio.Queries.GetProfile;
using AIPortfolio.Application.Features.Portfolio.Queries.GetProjects;
using AIPortfolio.Application.Features.Portfolio.Queries.GetPublicPortfolio;
using AIPortfolio.Application.Features.Portfolio.Queries.GetSkills;
using AIPortfolio.Application.Features.Portfolio.Queries.GetSocialLinks;
using MediatR;

namespace AIPortfolio.API.Endpoints.Portfolio;

internal static class PortfolioEndpoints
{
    // PROFILE
    public static async Task<IResult> GetProfile(ISender mediator)
    {
        var result = await mediator.Send(new GetProfileQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertProfile(
        UpsertProfileCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    // EDUCATION
    public static async Task<IResult> GetEducation(ISender mediator)
    {
        var result = await mediator.Send(new GetEducationsQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertEducation(
        UpsertEducationCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteEducation(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteEducationCommand(id));
        return result.ToApiResult();
    }

    // EXPERIENCE
    public static async Task<IResult> GetExperience(ISender mediator)
    {
        var result = await mediator.Send(new GetExperiencesQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertExperience(
        UpsertExperienceCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteExperience(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteExperienceCommand(id));
        return result.ToApiResult();
    }

    // PROJECTS
    public static async Task<IResult> GetProjects(ISender mediator)
    {
        var result = await mediator.Send(new GetProjectsQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertProject(
        UpsertProjectCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteProject(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteProjectCommand(id));
        return result.ToApiResult();
    }

    // SKILLS
    public static async Task<IResult> GetSkills(ISender mediator)
    {
        var result = await mediator.Send(new GetSkillsQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertSkill(
        UpsertSkillCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteSkill(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteSkillCommand(id));
        return result.ToApiResult();
    }

    // CERTIFICATIONS
    public static async Task<IResult> GetCertifications(ISender mediator)
    {
        var result = await mediator.Send(new GetCertificationsQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertCertification(
        UpsertCertificationCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteCertification(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteCertificationCommand(id));
        return result.ToApiResult();
    }

    // ACHIEVEMENTS
    public static async Task<IResult> GetAchievements(ISender mediator)
    {
        var result = await mediator.Send(new GetAchievementsQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertAchievement(
        UpsertAchievementCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteAchievement(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteAchievementCommand(id));
        return result.ToApiResult();
    }

    // LANGUAGES
    public static async Task<IResult> GetLanguages(ISender mediator)
    {
        var result = await mediator.Send(new GetLanguagesQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertLanguage(
        UpsertLanguageCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteLanguage(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteLanguageCommand(id));
        return result.ToApiResult();
    }

    // SOCIAL LINKS
    public static async Task<IResult> GetSocialLinks(ISender mediator)
    {
        var result = await mediator.Send(new GetSocialLinksQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertSocialLink(
        UpsertSocialLinkCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteSocialLink(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteSocialLinkCommand(id));
        return result.ToApiResult();
    }

    // CUSTOM SECTIONS
    public static async Task<IResult> GetCustomSections(ISender mediator)
    {
        var result = await mediator.Send(new GetCustomSectionsQuery());
        return result.ToApiResult();
    }

    public static async Task<IResult> UpsertCustomSection(
        UpsertCustomSectionCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }

    public static async Task<IResult> DeleteCustomSection(
        long id,
        ISender mediator)
    {
        var result = await mediator.Send(new DeleteCustomSectionCommand(id));
        return result.ToApiResult();
    }

    // PUBLIC PORTFOLIO
    public static async Task<IResult> GetPublicPortfolio(
        string slug,
        ISender mediator)
    {
        var result = await mediator.Send(new GetPublicPortfolioQuery(slug));
        return result.ToApiResult();
    }

    public static async Task<IResult> SendPortfolioMessage(
        SendContactMessageCommand command,
        ISender mediator)
    {
        var result = await mediator.Send(command);
        return result.ToApiResult();
    }
}